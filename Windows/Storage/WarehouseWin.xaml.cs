using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Storage;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace KirillPolyanskiy.CustomBrokerWpf
{
	public partial class WarehousesWin : Window
	{
		private DataModelClassLibrary.BindingDischarger mydischanger;
		private WarehouseViewCommander mycmd;

		public WarehousesWin()
		{
			InitializeComponent();
			mydischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { this.mainDataGrid });
		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			mycmd = new WarehouseViewCommander();
			mycmd.EndEdit = mydischanger.EndEdit;
			mycmd.CancelEdit = mydischanger.CancelEdit;
			this.DataContext = mycmd;
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!(mydischanger.EndEdit() && mycmd.SaveDataChanges()))
			{
				this.Activate();
				if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
				{
					e.Cancel = true;
				}
			}
			if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
		}
		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			WarehouseOpen(null);
		}
		private void MainDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
			if (e.Source is DataGrid && e.OriginalSource is TextBlock)
			{
				object item = (e.OriginalSource as TextBlock).GetBindingExpression(TextBlock.TextProperty).ResolvedSource;
				//string property = (e.OriginalSource as TextBlock).GetBindingExpression(TextBlock.TextProperty).ResolvedSourcePropertyName;
				if (item is WarehouseVM & mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
				{
					WarehouseOpen(item as WarehouseVM);
				}
			}
		}
		private void WarehouseOpen(WarehouseVM warehouse)
		{
			if (warehouse == null)
			{
				mycmd.Add.Execute(null);
				warehouse = mycmd.Items.CurrentItem as WarehouseVM;
			}

			Window ObjectWin = null;
			foreach (Window item in this.OwnedWindows)
			{
				if (item.Name == "winWarehouseItem" && (item.DataContext as WarehouseCommand).VModel.DomainObject == warehouse.DomainObject) ObjectWin = item;
			}
			if (ObjectWin == null)
			{
				WarehouseCommand cmd = new WarehouseCommand(warehouse, mycmd.Items);
                ObjectWin = new WarehouseItemWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = cmd;
                ObjectWin.Show();
            }
			else
			{
				ObjectWin.Activate();
				if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
			}
		}
		
		private void Delete_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.mainDataGrid.CanUserDeleteRows && this.mainDataGrid.SelectedItems.Count > 0;
			e.Handled = true;
		}
		private void Delete_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			mycmd.Delete.Execute(mainDataGrid.SelectedItem);
		}
    }
}
