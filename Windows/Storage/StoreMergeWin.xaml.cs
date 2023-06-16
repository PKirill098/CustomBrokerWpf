using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Storage;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class StoreMergeWin : Window
    {
        private StorageDataManager mymanager;
        private lib.BindingDischarger mydischarger;

        public StoreMergeWin()
        {
            InitializeComponent();
        }
        private void winStoreMerge_Loaded(object sender, RoutedEventArgs e)
        {
            mydischarger = new lib.BindingDischarger(this, new DataGrid[] { StorageDataDataGrid, StorageDateMathDataGrid });
            mymanager = new StorageDataManager();
            mymanager.EndEdit = mydischarger.EndEdit;
            mymanager.CancelEdit = mydischarger.CancelEdit;
            this.DataContext = mymanager;
            setFilterButtonImage();
        }

        private void LoadExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.storeComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Укажите склад!", "Обработка склада", MessageBoxButton.OK, MessageBoxImage.Stop);
                this.storeComboBox.Focus();
                (sender as Button).CommandParameter = false;
                e.Handled = true;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void winStoreMerge_Closing(object sender, CancelEventArgs e)
        {
            StorageDataManager cmd = this.DataContext as StorageDataManager;
            bool isdirty = !mydischarger.EndEdit();
            if (!isdirty)
                foreach (StorageDataVM item in cmd.Items)
                    if (item.IsDirty)
                    { isdirty = true; break; }
            if (!isdirty)
                foreach (StorageMathVM item in cmd.MathView.SourceCollection)
                    if (item.Request.IsDirty)
                    { isdirty = true; break; }
            if (!isdirty)
            {
                if (!cmd.SaveDataChanges())
                {
                    this.Activate();
                    if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        e.Cancel = true;
                    }
                    else
                        cmd.Reject.Execute(null);
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                {
                    cmd.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                mymanager.Filter.RemoveCurrentWhere();
            }
        }
        
		private void StoreDelete_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
		{
            e.CanExecute = mymanager.Delete.CanExecute((sender as DataGrid)?.SelectedItems);
            e.Handled = true;
        }
		private void StoreDelete_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
            mymanager.Delete.Execute((sender as DataGrid)?.SelectedItems);
        }
        private void RequestDelete_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
		{
            e.CanExecute = mymanager.RequestDelete.CanExecute((sender as DataGrid)?.SelectedItems);
            e.Handled = true;
        }
		private void RequestDelete_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
            mymanager.RequestDelete.Execute((sender as DataGrid)?.SelectedItems);
        }


        //private void ButtonMath_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveChanges();// Сохранить привязки
        //    MathRequest();
        //}
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.StorageDataDataGrid.SelectedIndex > -1)
        //    {
        //        if ((this.StorageDateMathDataGrid.SelectedIndex > -1) | (this.StorageDateMathDataGrid.Items.Count == 1))
        //        {
        //            StringBuilder strErr = new StringBuilder();
        //            StoreMergeDS.tableStorageDataRow storageRow = (this.StorageDataDataGrid.SelectedItem as DataRowView).Row as StoreMergeDS.tableStorageDataRow;
        //            if (storageRow.IsrequestIdNull())
        //            {
        //                StoreMergeDS.tableStorageDateMathRow mathRow;
        //                if (this.StorageDateMathDataGrid.SelectedIndex > -1)
        //                    mathRow = (this.StorageDateMathDataGrid.SelectedItem as DataRowView).Row as StoreMergeDS.tableStorageDateMathRow;
        //                else
        //                    mathRow = (this.StorageDateMathDataGrid.Items[0] as DataRowView).Row as StoreMergeDS.tableStorageDateMathRow;
        //                StoreMergeDS mergeDS = ((KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS)(this.FindResource("storeMergeDS")));
        //                try
        //                {
        //                    if (!mathRow.IscellNumberNull() && (storageRow.cellnumber != mathRow.cellNumber)) strErr.Append("Количество мест не совпадает\n");
        //                    if (!mathRow.IsvolumeNull() && storageRow.volume != mathRow.volume) strErr.Append("Объем не совпадает\n");
        //                    if (!mathRow.IsgoodValueNull() && !storageRow.IsgoodvalueNull() && storageRow.goodvalue != mathRow.goodValue) strErr.Append("Стоимость не совпадает\n");
        //                    if (!mathRow.IsofficialWeightNull() && storageRow.grossweight != mathRow.officialWeight) strErr.Append("Вес по документам не совпадает\n");
        //                    if (!mathRow.IsactualWeightNull() && storageRow.netweight != mathRow.actualWeight) strErr.Append("Вес фактический не совпадает\n");
        //                    if (strErr.Length > 0)
        //                    {
        //                        strErr.Append("\nСвязать склад и заявку?");
        //                        if (MessageBox.Show(strErr.ToString(), "Привязка", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
        //                            return;
        //                    }
        //                    mathRow.storagePoint = storageRow.storagePoint;
        //                    mathRow.storageDate = storageRow.storageDate;
        //                    mathRow.agentFullName = storageRow.agent;
        //                    if (mathRow.IscellNumberNull() || (storageRow.cellnumber != mathRow.cellNumber)) mathRow.cellNumber = storageRow.cellnumber;
        //                    if (mathRow.IsvolumeNull() || storageRow.volume != mathRow.volume) mathRow.volume = storageRow.volume;
        //                    if (!storageRow.IsgoodvalueNull() && (mathRow.IsgoodValueNull() || storageRow.goodvalue != mathRow.goodValue)) mathRow.goodValue = storageRow.goodvalue;
        //                    if (mathRow.IsofficialWeightNull() || storageRow.grossweight != mathRow.officialWeight) mathRow.officialWeight = storageRow.grossweight;
        //                    if (mathRow.IsactualWeightNull() || storageRow.netweight != mathRow.actualWeight) mathRow.actualWeight = storageRow.netweight;
        //                    if (!storageRow.IsstoragenoteNull()) mathRow.storageNote = storageRow.storagenote;
        //                    mathRow.storeid = storageRow.storeId; // установка склада
        //                    mathRow.storeName = storageRow.storeName;
        //                    mathRow.EndEdit();
        //                    KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter mathAdapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter();
        //                    mathAdapter.Adapter.UpdateCommand.Parameters["@customer"].Value = storageRow.customer;
        //                    mathAdapter.Connection.Open();
        //                    mathAdapter.Transaction = mathAdapter.Connection.BeginTransaction();
        //                    try
        //                    {
        //                        mathAdapter.Update(mathRow);
        //                        storageRow.requestId = mathRow.requestId;
        //                        storageRow.EndEdit();
        //                        StoreMergeDSTableAdapters.StorageDataAdapter storeageAdapter = new StoreMergeDSTableAdapters.StorageDataAdapter();
        //                        storeageAdapter.Update(storageRow);
        //                        mathAdapter.Transaction.Commit();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        mathAdapter.Transaction.Rollback();
        //                        mathRow.RejectChanges();
        //                        storageRow.SetrequestIdNull();//storageRow.Field<int>(storageRow.Table.Columns["requestId"],DataRowVersion.Original);
        //                        storageRow.EndEdit();
        //                        if (ex is System.Data.SqlClient.SqlException)
        //                        {
        //                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //                            if (err.Number > 49999)
        //                            {
        //                                MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            }
        //                            else
        //                            {
        //                                System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //                                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //                                {
        //                                    errs.Append(sqlerr.Message + "\n");
        //                                }
        //                                MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        }
        //                    }
        //                    if (!storageRow.IsrequestIdNull())
        //                    {
        //                        StoreMergeDS.tableStorageDateMathDataTable mathTable = (this.FindResource("storeMergeDS") as StoreMergeDS).tableStorageDateMath;
        //                        DataRow[] deleteRows = mathTable.Select("storage<>'" + mathRow.storagePoint + "' AND requestId=" + mathRow.requestId);
        //                        foreach (DataRow row in deleteRows)
        //                        {
        //                            row.Delete();
        //                        }
        //                        BindingListCollectionView mathview = CollectionViewSource.GetDefaultView(this.StorageDateMathDataGrid.ItemsSource) as BindingListCollectionView;
        //                        mathview.CustomFilter = "storage='" + storageRow.storagePoint + "' AND requestId=" + storageRow.requestId;
        //                        mergeButton.IsEnabled = false;
        //                        createButton.IsEnabled = false;
        //                        severButton.IsEnabled = true;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (ex is System.Data.SqlClient.SqlException)
        //                    {
        //                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //                        if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        else
        //                        {
        //                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //                            {
        //                                errs.Append(sqlerr.Message + "\n");
        //                            }
        //                            MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Привязка", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("Склад уже привязан к заявке!", "Привязка", MessageBoxButton.OK, MessageBoxImage.Stop);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Выдилите заявку", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Выдилите строку склада", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //    }
        //}
        //private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.StorageDataDataGrid.SelectedIndex > -1)
        //    {
        //        StorageDataDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
        //        BindingListCollectionView mathview = CollectionViewSource.GetDefaultView(StorageDateMathDataGrid.ItemsSource) as BindingListCollectionView;
        //        StoreMergeDS.tableStorageDataRow storageRow = (this.StorageDataDataGrid.SelectedItem as DataRowView).Row as StoreMergeDS.tableStorageDataRow;
        //        if (storageRow.IsrequestIdNull())
        //        {
        //            StoreMergeDS.tableStorageDateMathRow mathRow = ((mathview.SourceCollection as DataView).Table as StoreMergeDS.tableStorageDateMathDataTable).NewtableStorageDateMathRow();
        //            StoreMergeDS mergeDS = ((KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS)(this.FindResource("storeMergeDS")));
        //            KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter mathAdapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter();
        //            try
        //            {
        //                mathRow.storage = storageRow.storagePoint;
        //                mathRow.storagePoint = storageRow.storagePoint;
        //                mathRow.storageDate = storageRow.storageDate;
        //                mathRow.customerFullName = storageRow.customer;
        //                mathRow.agentFullName = storageRow.agent;
        //                mathRow.cellNumber = storageRow.cellnumber;
        //                mathRow.volume = storageRow.volume;
        //                if (!storageRow.IsgoodvalueNull()) mathRow.goodValue = storageRow.goodvalue;
        //                mathRow.officialWeight = storageRow.grossweight;
        //                if (!storageRow.IsnetweightNull()) mathRow.actualWeight = storageRow.netweight;
        //                if (!storageRow.IsstoragenoteNull()) mathRow.storageNote = storageRow.storagenote;
        //                mathRow.storeid = storageRow.storeId; // установка склада
        //                mathRow.storeName = storageRow.storeName;
        //                mathRow.EndEdit();
        //                mathRow.Table.Rows.Add(mathRow);
        //                mathAdapter.Connection.Open();
        //                mathAdapter.Transaction = mathAdapter.Connection.BeginTransaction();
        //                try
        //                {
        //                    mathAdapter.Update(mathRow);
        //                    storageRow.requestId = mathRow.requestId;
        //                    storageRow.EndEdit();
        //                    StoreMergeDSTableAdapters.StorageDataAdapter storeageAdapter = new StoreMergeDSTableAdapters.StorageDataAdapter();
        //                    storeageAdapter.Update(storageRow);
        //                    mathAdapter.Transaction.Commit();
        //                }
        //                catch (Exception ex)
        //                {
        //                    mathAdapter.Transaction.Rollback();
        //                    mathRow.Delete();
        //                    storageRow.SetrequestIdNull();
        //                    storageRow.EndEdit();
        //                    if (ex is System.Data.SqlClient.SqlException)
        //                    {
        //                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //                        if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        else
        //                        {
        //                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //                            {
        //                                errs.Append(sqlerr.Message + "\n");
        //                            }
        //                            MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    }
        //                }
        //                if (!storageRow.IsrequestIdNull())
        //                {
        //                    mathview.CustomFilter = "storage='" + storageRow.storagePoint + "' AND requestId=" + storageRow.requestId;
        //                    mergeButton.IsEnabled = false;
        //                    createButton.IsEnabled = false;
        //                    severButton.IsEnabled = true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message + "\n" + ex.Source, "Привязка", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Склад уже привязан к заявке!", "Привязка", MessageBoxButton.OK, MessageBoxImage.Stop);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Выдилите строку склада", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //    }
        //}
        //private void severButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.StorageDataDataGrid.SelectedIndex > -1)
        //    {
        //        if (this.StorageDateMathDataGrid.Items.Count > 0)
        //        {
        //            if (MessageBox.Show("Развязать заявку и информацию со склада?", "Привязка", MessageBoxButton.YesNo) != MessageBoxResult.No)
        //            {
        //                StoreMergeDS.tableStorageDataRow storageRow = (this.StorageDataDataGrid.SelectedItem as DataRowView).Row as StoreMergeDS.tableStorageDataRow;
        //                StoreMergeDS.tableStorageDateMathRow mathRow;
        //                mathRow = (this.StorageDateMathDataGrid.Items[0] as DataRowView).Row as StoreMergeDS.tableStorageDateMathRow;
        //                try
        //                {
        //                    storageRow.SetrequestIdNull();
        //                    storageRow.EndEdit();
        //                    mathRow.SetstoragePointNull();
        //                    mathRow.SetstorageDateNull();
        //                    mathRow.SetstoreidNull();
        //                    mathRow.SetstoreNameNull();
        //                    mathRow.SetstorageNoteNull();
        //                    mathRow.EndEdit();
        //                    KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter mathAdapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter();
        //                    mathAdapter.Adapter.UpdateCommand.Parameters["@customer"].Value = storageRow.customer;
        //                    mathAdapter.Connection.Open();
        //                    mathAdapter.Transaction = mathAdapter.Connection.BeginTransaction();
        //                    try
        //                    {
        //                        mathAdapter.Update(mathRow);
        //                        StoreMergeDSTableAdapters.StorageDataAdapter storeageAdapter = new StoreMergeDSTableAdapters.StorageDataAdapter();
        //                        storeageAdapter.Update(storageRow);
        //                        mathAdapter.Transaction.Commit();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        mathAdapter.Transaction.Rollback();
        //                        mathRow.RejectChanges();
        //                        storageRow.RejectChanges();
        //                        if (ex is System.Data.SqlClient.SqlException)
        //                        {
        //                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //                            if (err.Number > 49999)
        //                            {
        //                                MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            }
        //                            else
        //                            {
        //                                System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //                                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //                                {
        //                                    errs.Append(sqlerr.Message + "\n");
        //                                }
        //                                MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        }
        //                    }
        //                    if (storageRow.IsrequestIdNull())
        //                    {
        //                        BindingListCollectionView mathview = CollectionViewSource.GetDefaultView(this.StorageDateMathDataGrid.ItemsSource) as BindingListCollectionView;
        //                        mathview.CustomFilter = "storage='" + storageRow.storagePoint + "'";
        //                        mergeButton.IsEnabled = true;
        //                        createButton.IsEnabled = true;
        //                        severButton.IsEnabled = false;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (ex is System.Data.SqlClient.SqlException)
        //                    {
        //                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //                        if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        else
        //                        {
        //                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //                            {
        //                                errs.Append(sqlerr.Message + "\n");
        //                            }
        //                            MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Привязка", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Выполните подбор заявок", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Выдилите строку склада", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //    }
        //}
        public bool IsShowFilter
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        internal lib.SQLFilter.SQLFilter Filter
        {
            get { return mymanager.Filter; }
           //set
           //{
			//	if (!SaveChanges())
			//		MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			//	else
			//	{
			//		mythisfilter = value;
			//		DataRefresh();
			//	}
			//}
        }
		//internal void runFilter()
		//{
		//	if (!SaveChanges())
		//		MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		//	else
		//	{
		//		DataRefresh();
		//	}
		//}
		internal void setFilterButtonImage()
        {
            string uribitmap;
            if (mymanager.Filter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (FilterButton.Content as Image).Source = bi3;
        }

        //private void SaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SaveChanges())
        //    {
        //        PopupText.Text = "Изменения сохранены";
        //        popInf.IsOpen = true;
        //    }
        //}
        //private void RefreshButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
        //    {
        //        DataRefresh();
        //    }
        //}
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winStoreMergeFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new StoreMergeFilterWin();
                    ObjectWin.Owner = this;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
            else
            {
                if (ObjectWin != null)
                {
                    ObjectWin.Close();
                }
            }
        }

		private void StorageDateMathDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (e.OriginalSource is TextBlock && (StorageDateMathDataGrid.CurrentCell.Column?.SortMemberPath == "Request.Id" || StorageDateMathDataGrid.CurrentCell.Column?.SortMemberPath == "Request.RequestDate"))
			{
				if (mymanager.MathView.CurrentItem is StorageMathVM & StorageDateMathDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
				{
                    StorageMathVM math = mymanager.MathView.CurrentItem as StorageMathVM;
					OpenRequest(math.Request);
				}
				e.Handled = true;
			}
		}
		private void OpenRequest(Classes.Domain.RequestVM request)
		{
			RequestNewWin newWin = null;
            MainWindow mainwindow = App.Current.MainWindow as MainWindow;
			foreach (Window item in mainwindow.ListChildWindow)
			{
				if (item.Name == "winRequestNew")
				{
					if ((item.DataContext as Classes.Domain.RequestVMCommand).VModel.DomainObject.Equals(request.DomainObject))
						newWin = item as RequestNewWin;
				}
			}
			if (newWin == null)
			{
				newWin = new RequestNewWin();
				newWin.thisStoragePointValidationRule.RequestId = request.Id;
				Classes.Domain.RequestVMCommand cmd = new Classes.Domain.RequestVMCommand(request, null);
				cmd.EndEdit = newWin.BindingDischarger.EndEdit;
				cmd.CancelEdit = newWin.BindingDischarger.CancelEdit;
				newWin.DataContext = cmd;
				mainwindow.ListChildWindow.Add(newWin);
				newWin.Show();
			}
			else
			{
				newWin.Activate();
				if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
			}
		}
	}
}
