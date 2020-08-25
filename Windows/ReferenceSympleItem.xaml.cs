using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace KirillPolyanskiy.CustomBrokerWpf
{
	public partial class ReferenceSympleItemWin : Window
	{
		private DataModelClassLibrary.BindingDischarger mydischanger;
		private DataModelClassLibrary.ReferenceCollectionSimpleItem mycollection;

		public ReferenceSympleItemWin()
		{
			InitializeComponent();
			mydischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { this.mainDataGrid });
		}

		internal bool CanAddRows
		{ set { this.mainDataGrid.CanUserAddRows = value; } get { return this.mainDataGrid.CanUserAddRows; } }
		internal bool CanDeleteRows
		{ set { this.mainDataGrid.CanUserDeleteRows = value; } get { return this.mainDataGrid.CanUserDeleteRows; } }

		internal void SetDataContext(DataModelClassLibrary.ReferenceCollectionSimpleItem collection, bool sortbyid)
		{
			mycollection = collection;
			ListCollectionView view = new ListCollectionView(mycollection);
			view.Filter = DataModelClassLibrary.ViewModelViewCommand.ViewFilterDefault;
			view.SortDescriptions.Add(new System.ComponentModel.SortDescription(sortbyid ? "Id" : "Name", System.ComponentModel.ListSortDirection.Ascending));
			this.DataContext = view;
		}
		private bool SaveChanges()
		{
			bool isSuccess = false;
			try
			{
				isSuccess = mydischanger.EndEdit() && mycollection.SaveDataChanges();
			}
			catch (Exception ex)
			{
				if (ex is System.Data.SqlClient.SqlException)
				{
					System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
					if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
					else
					{
						System.Text.StringBuilder errs = new System.Text.StringBuilder();
						foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
						{
							errs.Append(sqlerr.Message + "\n");
						}
						MessageBox.Show(errs.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				else
				{
					MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
				{
					isSuccess = SaveChanges();
				}
			}
			return isSuccess;
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!SaveChanges())
			{
				this.Activate();
				if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
				{
					e.Cancel = true;
				}
			}
			if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
		}
		private void Refresh_Click(object sender, RoutedEventArgs e)
		{
			if (SaveChanges() || MessageBox.Show("Изменения не сохранены и будут потеряны при обновлении. \n Отменить обновление?", "Обновление", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
				mycollection.Refresh();
		}
		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (SaveChanges())
				KirillPolyanskiy.Common.PopupCreator.GetPopup("Изменения сохранены", Brushes.LightGreen);
			else
				KirillPolyanskiy.Common.PopupCreator.GetPopup("Изменения не сохранены", Brushes.LightPink, Brushes.Red);
		}
		private void Delete_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.mainDataGrid.CanUserDeleteRows && this.mainDataGrid.SelectedItems.Count > 0;
			e.Handled = true;
		}
		private void Delete_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			ListCollectionView view = this.DataContext as ListCollectionView;
			System.Collections.Generic.List<DataModelClassLibrary.ReferenceSimpleItem> deleting = new System.Collections.Generic.List<DataModelClassLibrary.ReferenceSimpleItem>();
			foreach (object obj in this.mainDataGrid.SelectedItems)
				if (obj is DataModelClassLibrary.ReferenceSimpleItem)
					deleting.Add(obj as DataModelClassLibrary.ReferenceSimpleItem);
			foreach (DataModelClassLibrary.ReferenceSimpleItem item in deleting)
				if (item.DomainState == DataModelClassLibrary.DomainObjectState.Added)
					view.Remove(item);
				else
				{
					view.EditItem(item);
					item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
					view.CommitEdit();
				}
		}
		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
