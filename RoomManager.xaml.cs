#region Using Statements
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using Dale.ManageRooms.Common.Display.Rooms;
using Dale.ManageRooms.Wpf;
using Dale.Wpf.Controls;
#endregion

namespace Dale.ManageRooms.Common.Rooms
{
	/// <summary>
	/// Interaction logic for RoomManager.xaml
	/// </summary>
	public partial class RoomManager : AppWindowBase
	{
		#region Private Members
		private RoomManagerUxManager _uxManager;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public RoomManager()
			: base()
		{
			InitializeComponent();
		}
		#endregion

		#region Properties
		/// <summary>App Form Id</summary>
		public override AppFormIDEnum AppFormID
		{
			get { return AppFormIDEnum.RoomManager; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// OnSetup
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="userPreferences"></param>
		/// <returns></returns>
		protected override bool OnSetup(AppFormSettings settings, System.Xml.XmlDocument userPreferences)
		{
			_uxManager = new RoomManagerUxManager(this, Controller, DataManager);
			this.DataContext = _uxManager;
			this.Title = string.Format("Room Manager - {0}", base.DataManager.Year);
			return true;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Window loaded handler
		/// </summary>
		private void RoomManager_Loaded(object sender, RoutedEventArgs e)
		{
			_uxManager.OnInitialLoad();
		}

		/// <summary>
		/// Closing event handler
		/// </summary>
		private void RoomManager_Closing(object sender, CancelEventArgs e)
		{
			//check for unsaved changes.
			if (!_uxManager.ConfirmUnsavedChanges())
			{
				e.Cancel = true;
			}
		}

		#region Left-Side Panel and Buttons
		private void AddRoom_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.AddRoom();
		}

		private void RoomTypeCInstanceNumbers_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.ShowTypeCInstanceNumbers();
		}

		/// <summary>
		/// Focused row changing
		/// </summary>
		private void dgRooms_FocusedRowChanging(object sender, Dale.Wpf.Controls.DaleTableView.CanceledEventArgs e)
		{
			e.Cancel = !_uxManager.ConfirmUnsavedChanges();
		}

		#endregion

		#region General Tab
		private void LevelManagerTitleEdit_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.UpdateLevelManagerTitle();
		}

		private void UploadLevelManagerSignature_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.UploadLevelManagerSignature();
		}

		private void RemoveLevelManagerSignature_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.RemoveLevelManagerSignature();
		}

		private void ViewLevelManagerSignature_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.ViewLevelManagerSignature();
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.OnSaveClicked();
		}
		#endregion

		#region Rooms Tab

		/// <summary>
		/// Rooms key down handler
		/// </summary>
		private void dgvRooms_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				_uxManager.EditRoom();
			}
		}

		/// <summary>
		/// Edit Room button click handler
		/// </summary>
		private void EditRoom_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.EditRoom();
		}

		/// <summary>
		/// Set Owner button click handler
		/// </summary>
		private void SetOwner_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.MoveSelectedRoomsToOwner();
		}

		/// <summary>
		/// Move Rooms button click handler
		/// </summary>
		private void MoveRooms_Click(object sender, RoutedEventArgs e)
		{
			_uxManager.MoveSelectedRoomsToRoom();
		}

		/// <summary>
		/// Room datagrid double click
		/// </summary>
		private void dgvRooms_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			int rowHandle = (sender as DaleGridControl).View.GetRowHandleByMouseEventArgs(e);

			if (rowHandle != GridControl.InvalidRowHandle)
			{
				_uxManager.EditRoom();
			}
		}
		#endregion

		#region Shared Room Tab Events

		private void Grid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				EditRoom(sender);
			}
		}

		private void RoomsGrid_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			int rowHandle = (sender as DaleGridControl).View.GetRowHandleByMouseEventArgs(e);

			if (rowHandle != GridControl.InvalidRowHandle)
			{
				EditRoom(sender);
			}
		}

		private void MoveRooms_ButtonClick(object sender, RoutedEventArgs e)
		{
			RoomDetailRoomTabDisplay display = GetRoomDetailRoomTabDisplay(sender as DaleRibbonButton);
			if (display != null)
			{
				_uxManager.MoveSelectedRooms(display);
			}
		}

		private void ChangeBillingAccount_ButtonClick(object sender, RoutedEventArgs e)
		{
			RoomDetailRoomTabDisplay display = GetRoomDetailRoomTabDisplay(sender as DaleRibbonButton);
			if (display != null)
			{
				_uxManager.ChangeRoomsBillingAccount(display);
			}
		}

		private void ViewSelectedRoom_ButtonClick(object sender, RoutedEventArgs e)
		{
			RoomDetailRoomTabDisplay display = GetRoomDetailRoomTabDisplay(sender as DaleRibbonButton);
			if (display != null)
			{
				_uxManager.EditRoom(display);
			}
		}

		private void RefreshRooms_ButtonClick(object sender, RoutedEventArgs e)
		{
			RoomDetailRoomTabDisplay display = GetRoomDetailRoomTabDisplay(sender as DaleRibbonButton);
			if (display != null)
			{
				if (_uxManager.ConfirmNumberOfRooms(display))
				{
					_uxManager.RefreshRooms(display);
				}
			}
		}

		private RoomDetailRoomTabDisplay GetRoomDetailRoomTabDisplay(DaleRibbonButton sender)
		{
			RoomDetailRoomTabDisplay display = null;
			DaleRibbonButton senderElement = sender as DaleRibbonButton;

			if (senderElement != null)
			{
				display = senderElement.DataContext as RoomDetailRoomTabDisplay;
			}

			return display;
		}

		private void EditRoom(object sender)
		{
			RoomDetailRoomTabDisplay display = null;
			DaleGridControl senderElement = sender as DaleGridControl;

			if (senderElement != null)
			{
				display = senderElement.DataContext as RoomDetailRoomTabDisplay;
				if (display != null)
				{
					_uxManager.EditRoom(display);
				}
			}
		}
		#endregion

		#endregion
	}
}
