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

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для StoreMergeWin.xaml
    /// </summary>
    public partial class StoreMergeWin : Window
    {
        private CustomBrokerWpf.SQLFilter mythisfilter = new SQLFilter("storage", "AND");
        public bool IsShowFilter
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        internal SQLFilter Filter
        {
            get { return mythisfilter; }
            set
            {
                if (!SaveChanges())
                    MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    mythisfilter = value;
                    DataRefresh();
                }
            }
        }
        internal void runFilter()
        {
            if (!SaveChanges())
                MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                DataRefresh();
            }
        }
        private void setFilterButtonImage()
        {
            string uribitmap;
            if (mythisfilter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (FilterButton.Content as Image).Source = bi3;
        }

        public StoreMergeWin()
        {
            InitializeComponent();
        }

        private void LoadExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.storeComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Укажите склад!", "Обработка склада", MessageBoxButton.OK, MessageBoxImage.Stop);
                this.storeComboBox.Focus();
            }
            else
            {
                //if ((int)this.storeComboBox.SelectedValue == 2)
                //{
                //    MessageBox.Show("Извините загрузка Склада 2 пока не готова :-(");
                //    return;
                //}
                Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
                fd.CheckPathExists = true;
                fd.CheckFileExists = true;
                fd.Multiselect = false;
                fd.Title = "Выбор файла с данными склада";
                fd.Filter = "Файл Excel |*.xls;*.xlsx";
                fd.ShowDialog();
                if (File.Exists(fd.FileName))
                {
                    BackgroundWorker bw = this.FindResource("keyBackgroundWorker") as BackgroundWorker;
                    if (!bw.IsBusy)
                    {
                        string[] arg = { this.storeComboBox.SelectedValue.ToString(), this.storeComboBox.Text, fd.FileName };
                        bw.RunWorkerAsync(arg);
                    }
                    else
                    {
                        MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка склада", MessageBoxButton.OK, MessageBoxImage.Hand);
                    }
                }
            }
        }
        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;

            try
            {
                int maxr,loadr=0, store;
                decimal dval; Int16 bval;
                string str;
                string[] args = e.Argument as string[];
                DateTime sdate;
                StoreMergeDS storeMergeDS = ((KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS)(this.FindResource("storeMergeDS")));
                StoreMergeDS.tableStorageDataRow row;
                StoreMergeDS.tableStorageDataDataTable tabl = storeMergeDS.tableStorageData;
                Excel.Workbook exWb = exApp.Workbooks.Open(args[2], false, true);
                Excel.Worksheet exWh = exWb.Sheets[1];
                // Задать форматы столбцов
                maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                store = int.Parse(args[0]);
                if (store == 1)
                {
                    for (int r = 1; r <= maxr; r++)
                    {
                        string storagePoint = (exWh.Cells[r, 1].Text as string).Trim();
                        if (storagePoint.Length > 6) storagePoint = storagePoint.Substring(0, 6);
                        if (storagePoint.Length > 0 && decimal.TryParse(storagePoint, out dval) && !(exWh.Cells[r, 17].Text as string).Trim().Equals("+", StringComparison.Ordinal))
                        {
                            str = (exWh.Cells[r, 2].Text as string).Trim();
                            if (DateTime.TryParseExact(str, new string[] { "yyyy.MM.dd", "dd.MM.yyyy", "dd.MM.yy" }, System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None, out sdate))
                            {
                                row = tabl.FindBystoragePointstorageDatestoreId(storagePoint, sdate, store);
                                if (row == null) row = tabl.NewtableStorageDataRow();
                                if (row.IsrequestIdNull())
                                {
                                    row.storagePoint = storagePoint;
                                    row.storageDate = sdate;
                                    str = (exWh.Cells[r, 3].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
                                    row.agent = str;
                                    str = (exWh.Cells[r, 4].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
                                    row.customer = str;
                                    if (decimal.TryParse(exWh.Cells[r, 5].Text, out dval))
                                    {
                                        row.grossweight = dval;
                                    }
                                    else
                                    {
                                        //if((exWh.Cells[r, 5].Text as String).IndexOf('.')>0)
                                        //    if (decimal.TryParse((exWh.Cells[r, 5].Text as String).Replace('.',','), out dval))
                                        //    {
                                        //        row.grossweight = dval;
                                        //    }
                                        //    else
                                        //    {
                                        //    }
                                        if (row.RowState == DataRowState.Detached) row.Delete();
                                        else row.CancelEdit();
                                        throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 5].Address(false, false) + " к формату числа!");
                                    }
                                    if (decimal.TryParse(exWh.Cells[r, 6].Text, out dval))
                                    {
                                        row.netweight = dval;
                                    }
                                    else
                                    {
                                        if (row.RowState == DataRowState.Detached) row.Delete();
                                        else row.CancelEdit();
                                        throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 6].Address(false, false) + " к формату числа!");
                                    }
                                    if (Int16.TryParse(exWh.Cells[r, 7].Text, out bval))
                                    {
                                        row.cellnumber = bval;
                                    }
                                    else
                                    {
                                        if (row.RowState == DataRowState.Detached) row.Delete();
                                        else row.CancelEdit();
                                        throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " к формату целого числа!");
                                    }
                                    if (decimal.TryParse(exWh.Cells[r, 8].Text, out dval))
                                    {
                                        row.volume = dval;
                                    }
                                    else
                                    {
                                        if (row.RowState == DataRowState.Detached) row.Delete();
                                        else row.CancelEdit();
                                        throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 8].Address(false, false) + " к формату числа!");
                                    }
                                    if (decimal.TryParse(exWh.Cells[r, 9].Text, out dval))
                                    {
                                        row.goodvalue = dval;
                                    }
                                    else
                                    {
                                        if (row.RowState == DataRowState.Detached) row.Delete();
                                        else row.CancelEdit();
                                        throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 9].Address(false, false) + " к формату числа!");
                                    }
                                    str = exWh.Cells[r, 10].Text;
                                    if (str.Length > 0)
                                        if (decimal.TryParse(str, out dval))
                                        {
                                            row.service = dval;
                                        }
                                        else
                                        {
                                            if (row.RowState == DataRowState.Detached) row.Delete();
                                            else row.CancelEdit();
                                            throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 10].Address(false, false) + " к формату числа!");
                                        }
                                    else row.freightcost = 0;
                                    str = exWh.Cells[r, 11].Text;
                                    if (str.Length > 0)
                                        if (decimal.TryParse(str, out dval))
                                        {
                                            row.forwarding = dval;
                                        }
                                        else
                                        {
                                            if (row.RowState == DataRowState.Detached) row.Delete();
                                            else row.CancelEdit();
                                            throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 11].Address(false, false) + " к формату числа!");
                                        }
                                    else row.freightcost = 0;
                                    str = (exWh.Cells[r, 12].Text as string).Trim(); if (str.Length > 6) str = str.Substring(0, 6);
                                    row.shipmentnumber = str;
                                    str = (exWh.Cells[r, 13].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
                                    row.storagenote = str;
                                    row.store = exWh.Cells[r, 14].Text;
                                    str = (exWh.Cells[r, 15].Text as string).Trim(); if (str.Length > 180) str = str.Substring(0, 180);
                                    row.doc = str;
                                    str = (exWh.Cells[r, 16].Text as string).Trim(); if (str.Length > 6) str = str.Substring(0, 6);
                                    row.freightnumber = str;
                                    str = exWh.Cells[r, 18].Text;
                                    if (str.Length > 0)
                                        if (decimal.TryParse(str, out dval))
                                        {
                                            row.freightcost = dval;
                                        }
                                        else
                                        {
                                            if (row.RowState == DataRowState.Detached) row.Delete();
                                            else row.CancelEdit();
                                            throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 18].Address(false, false) + " к формату числа!");
                                        }
                                    else row.freightcost = 0;
                                    row.storeId = store;
                                    row.storeName = args[1];
                                    if (row.RowState == DataRowState.Detached) tabl.AddtableStorageDataRow(row);
                                    else row.EndEdit();
                                }
                                loadr++;
                            }
                            else
                            {
                                throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 2].Address(false, false) + " к формату даты!");
                            }
                        }
                        worker.ReportProgress((int)(decimal.Divide(r, maxr) * 100));
                    }
                }
                else
                {
                    for (int r = 1; r <= maxr; r++)
                    {
                        string storagePoint = (exWh.Cells[r, 2].Text as string).Trim();
                        if (storagePoint.Length > 6) storagePoint = storagePoint.Substring(0, 6);
                        if (storagePoint.Length > 0 && decimal.TryParse(storagePoint, out dval))
                        {
                            str = (exWh.Cells[r, 3].Text as string).Trim();
                            if (DateTime.TryParseExact(str, new string[] { "yyyy.MM.dd", "dd.MM.yyyy", "dd.MM.yy" }, System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None, out sdate))
                            {
                                row = tabl.FindBystoragePointstorageDatestoreId(storagePoint, sdate, store);
                                if (row == null) row = tabl.NewtableStorageDataRow();
                                if (row.IsrequestIdNull())
                                {
                                    row.storagePoint = storagePoint;
                                    row.storageDate = sdate;
                                    str = (exWh.Cells[r, 5].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
                                    row.customer = str;
                                    str = (exWh.Cells[r, 7].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
                                    row.agent = str;
                                    str = (exWh.Cells[r, 8].Text as string).Trim();
                                    str = str.Substring(0, str.LastIndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, 0) + 1).Trim();
                                    if (Int16.TryParse(str, out bval))
                                    {
                                        row.cellnumber = bval;
                                    }
                                    else
                                    {
                                        if (row.RowState == DataRowState.Detached) row.Delete();
                                        else row.CancelEdit();
                                        throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 8].Address(false, false) + " к формату целого числа!");
                                    }
                                    if (decimal.TryParse(exWh.Cells[r, 9].Text, out dval))
                                    {
                                        row.grossweight = dval;
                                    }
                                    else
                                    {
                                        if (row.RowState == DataRowState.Detached) row.Delete();
                                        else row.CancelEdit();
                                        throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 9].Address(false, false) + " к формату числа!");
                                    }
                                    if (decimal.TryParse(exWh.Cells[r, 10].Text, out dval))
                                    {
                                        row.netweight = dval;
                                    }
                                    else
                                    {
                                        if (row.RowState == DataRowState.Detached) row.Delete();
                                        else row.CancelEdit();
                                        throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 10].Address(false, false) + " к формату числа!");
                                    }
                                    if (decimal.TryParse(exWh.Cells[r, 11].Text, out dval))
                                    {
                                        row.volume = dval;
                                    }
                                    else
                                    {
                                        if (row.RowState == DataRowState.Detached) row.Delete();
                                        else row.CancelEdit();
                                        throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 11].Address(false, false) + " к формату числа!");
                                    }
                                    str = (exWh.Cells[r, 12].Text as string).Trim(); if (str.Length > 180) str = str.Substring(0, 180);
                                    row.doc = str;
                                    str = (exWh.Cells[r, 6].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
                                    row.storagenote = str;
                                    str = (exWh.Cells[r, 13].Text as string).Trim(); if (str.Length + row.storagenote.Length > 100) str = str.Substring(0, 100 - row.storagenote.Length);
                                    row.storagenote = row.storagenote + str;
                                    row.storeId = store;
                                    row.storeName = args[1];
                                    if (row.RowState == DataRowState.Detached) tabl.AddtableStorageDataRow(row);
                                    else row.EndEdit();
                                }
                                loadr++;
                            }
                            else
                            {
                                throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 3].Address(false, false) + " к формату даты!");
                            }
                        }
                        worker.ReportProgress((int)(decimal.Divide(r, maxr) * 100));
                    }
                }
                e.Result = loadr;
                worker.ReportProgress(100);
                exWb.Close();
            }
            finally
            {
                if (exApp != null)
                {
                    foreach (Excel.Workbook itemBook in exApp.Workbooks)
                    {
                        itemBook.Close(false);
                    }
                    exApp.DisplayAlerts = true;
                    exApp.ScreenUpdating = true;
                    exApp.Quit();
                    exApp = null;
                }
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SaveChanges();
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Загрузка прервана из-за ошибки");
            }
            //else if (e.Cancelled)
            //{
            //    resultLabel.Text = "Canceled";
            //}
            else
            {
                //    // Finally, handle the case where the operation 
                //    // succeeded.
                //    resultLabel.Text = e.Result.ToString();
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.StorageDataDataGrid.ItemsSource) as BindingListCollectionView;
                view.Refresh();
                ;
                PopupText.Text = e.Result.ToString()+" строк загружено";
                popInf.IsOpen = true;
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void winStoreMerge_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refDS.tableStore.Count == 0)
            {
                ReferenceDSTableAdapters.StoreAdapter adapterStore = new ReferenceDSTableAdapters.StoreAdapter();
                adapterStore.Fill(refDS.tableStore);
            }
            CollectionViewSource storeVS = this.FindResource("keyStoreVS") as CollectionViewSource;
            storeVS.Source = new System.Data.DataView(refDS.tableStore);

            KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS storeMergeDS = ((KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS)(this.FindResource("storeMergeDS")));
            KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDataAdapter storeMergeDSStorageDataAdapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDataAdapter();
            storeMergeDSStorageDataAdapter.Fill(storeMergeDS.tableStorageData, mythisfilter.FilterWhereId);
        }

        private void ButtonMath_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();// Сохранить привязки
            MathRequest();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.StorageDataDataGrid.SelectedIndex > -1)
            {
                if ((this.StorageDateMathDataGrid.SelectedIndex > -1) | (this.StorageDateMathDataGrid.Items.Count == 1))
                {
                    StringBuilder strErr = new StringBuilder();
                    StoreMergeDS.tableStorageDataRow storageRow = (this.StorageDataDataGrid.SelectedItem as DataRowView).Row as StoreMergeDS.tableStorageDataRow;
                    if (storageRow.IsrequestIdNull())
                    {
                        StoreMergeDS.tableStorageDateMathRow mathRow;
                        if (this.StorageDateMathDataGrid.SelectedIndex > -1)
                            mathRow = (this.StorageDateMathDataGrid.SelectedItem as DataRowView).Row as StoreMergeDS.tableStorageDateMathRow;
                        else
                            mathRow = (this.StorageDateMathDataGrid.Items[0] as DataRowView).Row as StoreMergeDS.tableStorageDateMathRow;
                        StoreMergeDS mergeDS = ((KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS)(this.FindResource("storeMergeDS")));
                        try
                        {
                            if (!mathRow.IscellNumberNull() && (storageRow.cellnumber != mathRow.cellNumber)) strErr.Append("Количество мест не совпадает\n");
                            if (!mathRow.IsvolumeNull() && storageRow.volume != mathRow.volume) strErr.Append("Объем не совпадает\n");
                            if (!mathRow.IsgoodValueNull() && !storageRow.IsgoodvalueNull() && storageRow.goodvalue != mathRow.goodValue) strErr.Append("Стоимость не совпадает\n");
                            if (!mathRow.IsofficialWeightNull() && storageRow.grossweight != mathRow.officialWeight) strErr.Append("Вес по документам не совпадает\n");
                            if (!mathRow.IsactualWeightNull() && storageRow.netweight != mathRow.actualWeight) strErr.Append("Вес фактический не совпадает\n");
                            if (strErr.Length > 0)
                            {
                                strErr.Append("\nСвязать склад и заявку?");
                                if (MessageBox.Show(strErr.ToString(), "Привязка", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                                    return;
                            }
                            mathRow.storagePoint = storageRow.storagePoint;
                            mathRow.storageDate = storageRow.storageDate;
                            mathRow.agentFullName = storageRow.agent;
                            if (mathRow.IscellNumberNull() || (storageRow.cellnumber != mathRow.cellNumber)) mathRow.cellNumber = storageRow.cellnumber;
                            if (mathRow.IsvolumeNull() || storageRow.volume != mathRow.volume) mathRow.volume = storageRow.volume;
                            if (!storageRow.IsgoodvalueNull() && (mathRow.IsgoodValueNull() || storageRow.goodvalue != mathRow.goodValue)) mathRow.goodValue = storageRow.goodvalue;
                            if (mathRow.IsofficialWeightNull() || storageRow.grossweight != mathRow.officialWeight) mathRow.officialWeight = storageRow.grossweight;
                            if (mathRow.IsactualWeightNull() || storageRow.netweight != mathRow.actualWeight) mathRow.actualWeight = storageRow.netweight;
                            if (!storageRow.IsstoragenoteNull()) mathRow.storageNote = storageRow.storagenote;
                            mathRow.storeid = storageRow.storeId; // установка склада
                            mathRow.storeName = storageRow.storeName;
                            mathRow.EndEdit();
                            KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter mathAdapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter();
                            mathAdapter.Adapter.UpdateCommand.Parameters["@customer"].Value = storageRow.customer;
                            mathAdapter.Connection.Open();
                            mathAdapter.Transaction = mathAdapter.Connection.BeginTransaction();
                            try
                            {
                                mathAdapter.Update(mathRow);
                                storageRow.requestId = mathRow.requestId;
                                storageRow.EndEdit();
                                StoreMergeDSTableAdapters.StorageDataAdapter storeageAdapter = new StoreMergeDSTableAdapters.StorageDataAdapter();
                                storeageAdapter.Update(storageRow);
                                mathAdapter.Transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                mathAdapter.Transaction.Rollback();
                                mathRow.RejectChanges();
                                storageRow.SetrequestIdNull();//storageRow.Field<int>(storageRow.Table.Columns["requestId"],DataRowVersion.Original);
                                storageRow.EndEdit();
                                if (ex is System.Data.SqlClient.SqlException)
                                {
                                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                                    if (err.Number > 49999)
                                    {
                                        MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else
                                    {
                                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                        {
                                            errs.Append(sqlerr.Message + "\n");
                                        }
                                        MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            if (!storageRow.IsrequestIdNull())
                            {
                                StoreMergeDS.tableStorageDateMathDataTable mathTable = (this.FindResource("storeMergeDS") as StoreMergeDS).tableStorageDateMath;
                                DataRow[] deleteRows = mathTable.Select("storage<>'" + mathRow.storagePoint + "' AND requestId=" + mathRow.requestId);
                                foreach (DataRow row in deleteRows)
                                {
                                    row.Delete();
                                }
                                BindingListCollectionView mathview = CollectionViewSource.GetDefaultView(this.StorageDateMathDataGrid.ItemsSource) as BindingListCollectionView;
                                mathview.CustomFilter = "storage='" + storageRow.storagePoint + "' AND requestId=" + storageRow.requestId;
                                mergeButton.IsEnabled = false;
                                createButton.IsEnabled = false;
                                severButton.IsEnabled = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex is System.Data.SqlClient.SqlException)
                            {
                                System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                                if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                else
                                {
                                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                    {
                                        errs.Append(sqlerr.Message + "\n");
                                    }
                                    MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show(ex.Message + "\n" + ex.Source, "Привязка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Склад уже привязан к заявке!", "Привязка", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
                else
                {
                    MessageBox.Show("Выдилите заявку", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Выдилите строку склада", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (this.StorageDataDataGrid.SelectedIndex > -1)
            {
                StorageDataDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                BindingListCollectionView mathview = CollectionViewSource.GetDefaultView(StorageDateMathDataGrid.ItemsSource) as BindingListCollectionView;
                StoreMergeDS.tableStorageDataRow storageRow = (this.StorageDataDataGrid.SelectedItem as DataRowView).Row as StoreMergeDS.tableStorageDataRow;
                if (storageRow.IsrequestIdNull())
                {
                    StoreMergeDS.tableStorageDateMathRow mathRow = ((mathview.SourceCollection as DataView).Table as StoreMergeDS.tableStorageDateMathDataTable).NewtableStorageDateMathRow();
                    StoreMergeDS mergeDS = ((KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS)(this.FindResource("storeMergeDS")));
                    KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter mathAdapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter();
                    try
                    {
                        mathRow.storage = storageRow.storagePoint;
                        mathRow.storagePoint = storageRow.storagePoint;
                        mathRow.storageDate = storageRow.storageDate;
                        mathRow.customerFullName = storageRow.customer;
                        mathRow.agentFullName = storageRow.agent;
                        mathRow.cellNumber = storageRow.cellnumber;
                        mathRow.volume = storageRow.volume;
                        if (!storageRow.IsgoodvalueNull()) mathRow.goodValue = storageRow.goodvalue;
                        mathRow.officialWeight = storageRow.grossweight;
                        if (!storageRow.IsnetweightNull()) mathRow.actualWeight = storageRow.netweight;
                        if (!storageRow.IsstoragenoteNull()) mathRow.storageNote = storageRow.storagenote;
                        mathRow.storeid = storageRow.storeId; // установка склада
                        mathRow.storeName = storageRow.storeName;
                        mathRow.EndEdit();
                        mathRow.Table.Rows.Add(mathRow);
                        mathAdapter.Connection.Open();
                        mathAdapter.Transaction = mathAdapter.Connection.BeginTransaction();
                        try
                        {
                            mathAdapter.Update(mathRow);
                            storageRow.requestId = mathRow.requestId;
                            storageRow.EndEdit();
                            StoreMergeDSTableAdapters.StorageDataAdapter storeageAdapter = new StoreMergeDSTableAdapters.StorageDataAdapter();
                            storeageAdapter.Update(storageRow);
                            mathAdapter.Transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            mathAdapter.Transaction.Rollback();
                            mathRow.Delete();
                            storageRow.SetrequestIdNull();
                            storageRow.EndEdit();
                            if (ex is System.Data.SqlClient.SqlException)
                            {
                                System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                                if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                else
                                {
                                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                    {
                                        errs.Append(sqlerr.Message + "\n");
                                    }
                                    MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        if (!storageRow.IsrequestIdNull())
                        {
                            mathview.CustomFilter = "storage='" + storageRow.storagePoint + "' AND requestId=" + storageRow.requestId;
                            mergeButton.IsEnabled = false;
                            createButton.IsEnabled = false;
                            severButton.IsEnabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Привязка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Склад уже привязан к заявке!", "Привязка", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            else
            {
                MessageBox.Show("Выдилите строку склада", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void severButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.StorageDataDataGrid.SelectedIndex > -1)
            {
                if (this.StorageDateMathDataGrid.Items.Count > 0)
                {
                    if (MessageBox.Show("Развязать заявку и информацию со склада?", "Привязка", MessageBoxButton.YesNo) != MessageBoxResult.No)
                    {
                        StoreMergeDS.tableStorageDataRow storageRow = (this.StorageDataDataGrid.SelectedItem as DataRowView).Row as StoreMergeDS.tableStorageDataRow;
                        StoreMergeDS.tableStorageDateMathRow mathRow;
                        mathRow = (this.StorageDateMathDataGrid.Items[0] as DataRowView).Row as StoreMergeDS.tableStorageDateMathRow;
                        try
                        {
                            storageRow.SetrequestIdNull();
                            storageRow.EndEdit();
                            mathRow.SetstoragePointNull();
                            mathRow.SetstorageDateNull();
                            mathRow.SetstoreidNull();
                            mathRow.SetstoreNameNull();
                            mathRow.SetstorageNoteNull();
                            mathRow.EndEdit();
                            KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter mathAdapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter();
                            mathAdapter.Adapter.UpdateCommand.Parameters["@customer"].Value = storageRow.customer;
                            mathAdapter.Connection.Open();
                            mathAdapter.Transaction = mathAdapter.Connection.BeginTransaction();
                            try
                            {
                                mathAdapter.Update(mathRow);
                                StoreMergeDSTableAdapters.StorageDataAdapter storeageAdapter = new StoreMergeDSTableAdapters.StorageDataAdapter();
                                storeageAdapter.Update(storageRow);
                                mathAdapter.Transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                mathAdapter.Transaction.Rollback();
                                mathRow.RejectChanges();
                                storageRow.RejectChanges();
                                if (ex is System.Data.SqlClient.SqlException)
                                {
                                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                                    if (err.Number > 49999)
                                    {
                                        MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else
                                    {
                                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                        {
                                            errs.Append(sqlerr.Message + "\n");
                                        }
                                        MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            if (storageRow.IsrequestIdNull())
                            {
                                BindingListCollectionView mathview = CollectionViewSource.GetDefaultView(this.StorageDateMathDataGrid.ItemsSource) as BindingListCollectionView;
                                mathview.CustomFilter = "storage='" + storageRow.storagePoint + "'";
                                mergeButton.IsEnabled = true;
                                createButton.IsEnabled = true;
                                severButton.IsEnabled = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex is System.Data.SqlClient.SqlException)
                            {
                                System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                                if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                else
                                {
                                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                    {
                                        errs.Append(sqlerr.Message + "\n");
                                    }
                                    MessageBox.Show(errs.ToString(), "Сохранение привязки", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show(ex.Message + "\n" + ex.Source, "Привязка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выполните подбор заявок", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Выдилите строку склада", "Привязка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges())
            {
                PopupText.Text = "Изменения сохранены";
                popInf.IsOpen = true;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void winStoreMerge_Closing(object sender, CancelEventArgs e)
        {
            if (!SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                mythisfilter.RemoveCurrentWhere();
            }
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                DataRefresh();
            }
        }
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

        private bool SaveChanges()
        {
            bool isSuccess = false;
            StoreMergeDS mergeDS = ((KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS)(this.FindResource("storeMergeDS")));
            try
            {
                StorageDateMathDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                StorageDataDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter mathAdapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter();
                mathAdapter.Update(mergeDS.tableStorageDateMath);
                StoreMergeDSTableAdapters.StorageDataAdapter storeageAdapter = new StoreMergeDSTableAdapters.StorageDataAdapter();
                storeageAdapter.Update(mergeDS.tableStorageData);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999)
                    {
                        MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                        mergeDS.tableStorageData.RejectChanges();
                    }
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
        private void MathRequest()
        {
            try
            {
                KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS storeMergeDS = ((KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS)(this.FindResource("storeMergeDS")));
                storeMergeDS.tableStorageDateMath.Clear();
                CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter adapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDateMathAdapter();
                adapter.ClearBeforeFill = false;
                StoreMergeDS.tableStorageDataDataTable storagetable = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tableStorageDataViewSource"))).Source as StoreMergeDS.tableStorageDataDataTable;
                foreach (StoreMergeDS.tableStorageDataRow row in storagetable)
                {
                    //StoreMergeDS.tableStorageDateMathRow mathrow = row.Row as StoreMergeDS.tableStorageDateMathRow;
                    adapter.Fill(storeMergeDS.tableStorageDateMath, row.IsrequestIdNull() ? 0 : row.requestId, row.storagePoint, row.storeId, row.customer, row.agent);
                }
                BindingListCollectionView mathview = CollectionViewSource.GetDefaultView(this.StorageDateMathDataGrid.ItemsSource) as BindingListCollectionView;
                mathview.Refresh();

                //System.Windows.Data.CollectionViewSource tableStorageDateMathViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tableStorageDateMathViewSource")));
                //tableStorageDateMathViewSource.View.MoveCurrentToFirst();
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                    {
                        errs.Append(sqlerr.Message + "\n");
                    }
                    MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void DataRefresh()
        {
            KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS storeMergeDS = ((KirillPolyanskiy.CustomBrokerWpf.StoreMergeDS)(this.FindResource("storeMergeDS")));
            KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDataAdapter storeMergeDSStorageDataAdapter = new KirillPolyanskiy.CustomBrokerWpf.StoreMergeDSTableAdapters.StorageDataAdapter();
            //storeMergeDSStorageDataAdapter.ClearBeforeFill = false;
            CollectionViewSource StorageViewSource = this.FindResource("tableStorageDataViewSource") as CollectionViewSource;
            StorageViewSource.Source = null;
            storeMergeDSStorageDataAdapter.Fill(storeMergeDS.tableStorageData, mythisfilter.FilterWhereId);
            StorageViewSource.Source = storeMergeDS.tableStorageData;
            MathRequest();
            setFilterButtonImage();
        }
        private void tableStorageDataDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == StorageDataDataGrid & e.AddedItems.Count > 0)
            {
                BindingListCollectionView mathview = CollectionViewSource.GetDefaultView(this.StorageDateMathDataGrid.ItemsSource) as BindingListCollectionView;
                StoreMergeDS.tableStorageDataRow row = (e.AddedItems[0] as DataRowView).Row as StoreMergeDS.tableStorageDataRow;
                if (row.IsrequestIdNull())
                    mathview.CustomFilter = "storage='" + row.storagePoint + "'";
                else
                    mathview.CustomFilter = "storage='" + row.storagePoint + "' AND requestId=" + row.requestId;
                mergeButton.IsEnabled = row.IsrequestIdNull();
                createButton.IsEnabled = row.IsrequestIdNull();
                severButton.IsEnabled = !row.IsrequestIdNull();
            }
        }
        private void StorageDataGrid_PreviewExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            System.Windows.Input.RoutedCommand com = e.Command as System.Windows.Input.RoutedCommand;
            if (com != null)
            {
                if (com == System.Windows.Input.ApplicationCommands.Delete && this.StorageDataDataGrid.SelectedItems.Count > 0)
                {
                    e.Handled = !(MessageBox.Show("Удалить выделенные строки склада?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
                }
            }
        }
    }
}
