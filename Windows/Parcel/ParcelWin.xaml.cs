using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using excel = Microsoft.Office.Interop.Excel;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ParcelWin.xaml
    /// </summary>
    public partial class ParcelWin : Window, ISQLFiltredWindow
    {
        bool isRequestSave;
        DataView viewParcelRequest;
        DataView viewRequest;
        ParcelDS parcelDS;
        internal ParcelDS DS
        { get { return parcelDS; } set { parcelDS = value; } }
        private System.Collections.Generic.List<Classes.Domain.RequestItemViewCommand> ritemcmd;

        public ParcelWin()
        {
            InitializeComponent();
            isRequestSave = true;
            ritemcmd = new System.Collections.Generic.List<Classes.Domain.RequestItemViewCommand>();
        }

        private void winParcel_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refDS.tableRequestStatus.Count == 0)
            {
                ReferenceDSTableAdapters.RequestStatusAdapter adapterStatus = new ReferenceDSTableAdapters.RequestStatusAdapter();
                adapterStatus.Fill(refDS.tableRequestStatus);
            }
            statusComboBox.ItemsSource = new System.Data.DataView(refDS.tableRequestStatus, "rowId>49", "rowId", DataViewRowState.CurrentRows);
            if (refDS.tableParcelType.Count == 0)
            {
                ReferenceDSTableAdapters.ParcelTypeAdapter parceltypeAdapter = new ReferenceDSTableAdapters.ParcelTypeAdapter();
                parceltypeAdapter.Fill(refDS.tableParcelType);
            }
            parcelTypeComboBox.ItemsSource = refDS.tableParcelType.DefaultView;
            if (refDS.tableGoodsType.Count == 0)
            {
                ReferenceDSTableAdapters.GoodsTypeAdapter goodstypeadapter = new ReferenceDSTableAdapters.GoodsTypeAdapter();
                goodstypeadapter.Fill(refDS.tableGoodsType);
            }
            refDS.tableGoodsType.DefaultView.Sort = "Nameitem";
            goodstypeComboBox.ItemsSource = refDS.tableGoodsType.DefaultView;
            if (parcelDS == null)
            {
                parcelDS = new ParcelDS();
                mainDataRefresh();
            }
            else
            {
                this.ParcelNumberListTextBlock.Visibility = Visibility.Collapsed;
                this.ParcelNumberList.Visibility = Visibility.Collapsed;
                this.FilterButton.Visibility = Visibility.Collapsed;
                mainGrid.DataContext = parcelDS.tableParcel.DefaultView;
            }
            viewParcelRequest = new DataView(parcelDS.tableParcelRequest, "parcel Is Not Null", "pgroupsort", DataViewRowState.CurrentRows);
            viewRequest = new DataView(parcelDS.tableParcelRequest, "parcel Is Null", "pgroupsort", DataViewRowState.CurrentRows);
        }
        private void mainDataRefresh()
        {
            try
            {
                ParcelDSTableAdapters.ParcelAdapter parcelAdapter = new ParcelDSTableAdapters.ParcelAdapter();
                mainGrid.DataContext = null;
                if (this.Owner != null) (this.Owner as ParcelListWin).parcelDataGrid.ItemsSource = null;
                parcelAdapter.Fill(parcelDS.tableParcel, thisfilter.FilterWhereId);
                parcelDS.tableParcel.DefaultView.Sort = "sortnumber Desc";
                mainGrid.DataContext = parcelDS.tableParcel.DefaultView;
                if (this.Owner != null) (this.Owner as ParcelListWin).parcelDataGrid.ItemsSource = parcelDS.tableParcel.DefaultView;
                (CollectionViewSource.GetDefaultView(parcelDS.tableParcel.DefaultView) as BindingListCollectionView).MoveCurrentToFirst();
                setFilterButtonImage();
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
                if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    mainDataRefresh();
                }
            }
        }

        private void winParcel_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
                thisfilter.RemoveCurrentWhere();
            }
        }
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(parcelDS.tableParcel.DefaultView) as BindingListCollectionView;

                IInputElement fcontrol = FocusManager.GetFocusedElement(this);
                if (fcontrol is TextBox & view.CurrentItem != null)
                {
                    BindingExpression be;
                    be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                    if (be != null)
                    {
                        //DataRow row = (view.CurrentItem as DataRowView).Row;
                        //decimal d;
                        //DateTime dt;
                        //bool isDirty = false;
                        //switch (be.ParentBinding.Path.Path)
                        //{
                        //    case "lorryvolume":
                        //    case "lorrytonnage":
                        //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !decimal.TryParse((fcontrol as TextBox).Text, out d) || row.Field<Decimal>(be.ParentBinding.Path.Path) != d;
                        //        break;
                        //    case "shipplandate":
                        //    case "shipdate":
                        //    case "preparation":
                        //    case "borderdate":
                        //    case "terminalin":
                        //    case "terminalout":
                        //    case "unloaded":
                        //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !DateTime.TryParse((fcontrol as TextBox).Text, out dt) || row.Field<DateTime>(be.ParentBinding.Path.Path) != dt;
                        //        break;
                        //    case "shipnumber":
                        //    case "declaration":
                        //    case "carrier":
                        //    case "carrierperson":
                        //    case "carriertel":
                        //    case "trucker":
                        //    case "truckertel":
                        //    case "lorry":
                        //    case "lorryregnum":
                        //    case "lorryvin":
                        //    case "trailerregnum":
                        //    case "trailervin":
                        //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !(fcontrol as TextBox).Text.Equals(row.Field<string>(be.ParentBinding.Path.Path));
                        //        break;
                        //    default:
                        //        isDirty = true;
                        //        MessageBox.Show("Поле не добавлено в обработчик сохранения без потери фокуса!", "Сохранение изменений");
                        //        break;
                        //}
                        if (be.IsDirty) be.UpdateSource();
                        if (be.HasError) return false;
                    }
                }

                if (view.IsAddingNew) view.CommitNew();
                if (view.IsEditingItem) view.CommitEdit();
                this.ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                this.ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                DirectoryInfo dir = new DirectoryInfo("E:\\Счета");
                ParcelDSTableAdapters.ParcelRequestAdapter requestAdapter = new ParcelDSTableAdapters.ParcelRequestAdapter();
                ParcelDSTableAdapters.ParcelAdapter parcelAdapter = new ParcelDSTableAdapters.ParcelAdapter();
                DataRow[] rows = parcelDS.tableParcel.Select("", "", DataViewRowState.Added);
                if (rows.Length > 0)
                {
                    parcelAdapter.Update(rows);
                    foreach (DataRow row in rows)
                    {
                        ParcelDS.tableParcelRow prow = row as ParcelDS.tableParcelRow;
                        try
                        {
                            if (!prow.IsdocdirpathNull()) dir.CreateSubdirectory(prow.docdirpath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Не удалось создать папку для документов Доставки!\n" + ex.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        requestAdapter.Update(parcelDS.tableParcelRequest.Select("parcel=" + prow.parcelId.ToString()));
                    }
                    CheckGroup(rows);
                }
                rows = parcelDS.tableParcel.Select("", "", DataViewRowState.ModifiedCurrent);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        ParcelDS.tableParcelRow prow = row as ParcelDS.tableParcelRow;
                        if (prow.Field<string>("fullNumber", DataRowVersion.Original) != prow.Field<string>("fullNumber", DataRowVersion.Current))
                        {
                            try
                            {
                                DirectoryInfo parceldir = new DirectoryInfo(dir.FullName + "\\" + prow.docdirpath);
                                if (parceldir.Exists)
                                    parceldir.MoveTo(dir.FullName + "\\" + prow.Field<string>("fullNumber", DataRowVersion.Current));//+ prow.docdirpath.Substring(prow.docdirpath.Length - 5)
                                else
                                    dir.CreateSubdirectory(prow.fullNumber); //+ prow.docdirpath.Substring(prow.docdirpath.Length - 5)
                                prow.docdirpath = prow.fullNumber; //+ prow.docdirpath.Substring(prow.docdirpath.Length - 5) -год
                                prow.EndEdit();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Не удалось переименовать папку для документов Доставки!\n\n" + ex.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                        }
                        requestAdapter.Update(parcelDS.tableParcelRequest.Select("parcel=" + prow.parcelId.ToString()));
                    }
                    parcelAdapter.Update(rows);
                    CheckGroup(rows);
                }
                // обновление заявок
                requestAdapter.Update(parcelDS.tableParcelRequest);
                parcelAdapter.Update(parcelDS.tableParcel);
                foreach(Classes.Domain.RequestItemViewCommand item in ritemcmd)
                { item.SaveDataChanges(); }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.State == 2)
                    {
                        try
                        {
                            DataRow[] errrows;
                            errrows = parcelDS.tableParcel.GetErrors();
                            foreach (ParcelDS.tableParcelRow row in errrows)
                            {
                                System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                {
                                    errs.Append(sqlerr.Message + "\n");
                                }
                                MessageBox.Show(errs.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            ParcelRequestConflictResolution resolver = new ParcelRequestConflictResolution();
                            errrows = parcelDS.tableParcelRequest.GetErrors();
                            foreach (ParcelDS.tableParcelRequestRow row in errrows)
                            {
                                resolver.Row = row;
                                return SaveChanges();
                            }
                        }
                        catch (Exception ep)
                        {
                            MessageBox.Show(ep.Message + "\n" + ep.Source, "Разрешение конфликта записи", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
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
                }
                else if (ex is System.Data.NoNullAllowedException)
                {
                    if (parcelDS.tableParcel.HasErrors)
                        ParcelNumberList.SelectedItem = parcelDS.tableParcel.GetErrors()[0];
                    else
                    {
                        foreach (DataRowView viewrow in parcelDS.tableParcel.DefaultView)
                        {
                            if (viewrow.IsNew)
                            {
                                ParcelNumberList.SelectedItem = viewrow;
                            }
                        }
                    }
                    string msg = string.Empty;
                    if (((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).IsNull("parceltype")) msg = msg + " \"Тип\"";
                    if (((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).IsNull("shipplandate")) msg = " \"Дата отгрузки план\"";
                    MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля" + msg + " или удалите перевозку.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                //{
                //    isSuccess = SaveChanges();
                //}
            }
            return isSuccess;
        }
        private void CheckGroup(DataRow[] rows)
        {
            SqlCommand com = new SqlCommand();
            using (SqlConnection con = new SqlConnection(References.ConnectionString))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "ParcelGroupCheck_sp";
                com.Connection = con;
                SqlParameter parId = new SqlParameter();
                parId.ParameterName = "@parcelId";
                parId.SqlDbType = SqlDbType.Int;
                com.Parameters.Add(parId);
                SqlParameter parRez = new SqlParameter();
                parRez.Direction = ParameterDirection.Output;
                parRez.ParameterName = "@equals";
                parRez.SqlDbType = SqlDbType.TinyInt;
                com.Parameters.Add(parRez);
                con.Open();
                foreach (DataRow row in rows)
                {

                    parId.Value = (row as ParcelDS.tableParcelRow).parcelId;
                    com.ExecuteNonQuery();
                    if ((byte)parRez.Value != 0) MessageBox.Show("Не все группы заявок поставлены в загрузку " + (row as ParcelDS.tableParcelRow).fullNumber + " полностью!", "Группы заявок", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                con.Close();
            }
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParcelNumberList.SelectedItem is DataRowView)
            {
                if (MessageBox.Show("Отменить несохраненные изменения в перевозке?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    BindingListCollectionView view = CollectionViewSource.GetDefaultView(parcelDS.tableParcel.DefaultView) as BindingListCollectionView;
                    IInputElement fcontrol = FocusManager.GetFocusedElement(this);
                    if (fcontrol is TextBox & view.CurrentItem != null)
                    {
                        BindingExpression be;
                        be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                        if (be != null)
                        {
                            if (be.IsDirty) be.UpdateTarget();
                        }
                    }
                    ParcelRequestDataGrid.CancelEdit();
                    view.CancelEdit();
                    (ParcelNumberList.SelectedItem as DataRowView).Row.RejectChanges();
                    foreach (DataRowView viewrow in viewParcelRequest)
                        viewrow.Row.RejectChanges();
                    foreach (DataRowView viewrow in viewRequest)
                        viewrow.Row.RejectChanges();
                    foreach (Classes.Domain.RequestItemViewCommand item in ritemcmd)
                        item.Reject.Execute(null);

                    PopupText.Text = "Изменения отменены";
                    popInf.PlacementTarget = sender as UIElement;
                    popInf.IsOpen = true;
                }
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
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(parcelDS.tableParcel.DefaultView) as BindingListCollectionView;
                view.AddNew();
            }
            catch (NoNullAllowedException)
            {
                if (parcelDS.tableParcel.HasErrors)
                    ParcelNumberList.SelectedItem = parcelDS.tableParcel.GetErrors()[0];
                else
                {
                    foreach (DataRowView viewrow in parcelDS.tableParcel.DefaultView)
                    {
                        if (viewrow.IsNew)
                        {
                            ParcelNumberList.SelectedItem = viewrow;
                        }
                    }
                }
                string msg = string.Empty;
                if (((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).IsNull("parceltype")) msg = msg + " \"Тип\"";
                if (((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).IsNull("shipplandate")) msg = " \"Дата отгрузки план\"";
                MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля" + msg + " или удалите перевозку.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParcelRequestDataGrid.Items.Count > 0)
                MessageBox.Show("Нельзя удалить перевозку пока она содержит заявки!", "Удаление", MessageBoxButton.OK, MessageBoxImage.Stop);
            else if (MessageBox.Show("Удалить перевозку?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(parcelDS.tableParcel.DefaultView) as BindingListCollectionView;
                if (view.CurrentItem != null) (view.CurrentItem as DataRowView).Delete();
            }
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                isRequestSave = false;
                int parcelid = ParcelNumberList.SelectedValue != null ? (int)ParcelNumberList.SelectedValue : 0;
                mainDataRefresh();
                ParcelNumberList.SelectedValue = parcelid;
                parcelid = ParcelNumberList.SelectedValue != null ? (int)ParcelNumberList.SelectedValue : 0;
                loadParcelRequest(parcelid);
                isRequestSave = true;
            }
        }
        private void toExcelButton_Click(object sender, RoutedEventArgs e)
        {
            ExcelReport();
        }
        private void toDocButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ParcelNumberList.SelectedItem is DataRowView)
                {
                    ParcelDS.tableParcelRow prow = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow;
                    if (Directory.Exists("E:\\Счета\\" + prow.docdirpath))
                    {
                        System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
                    }
                    else if (Directory.Exists("E:\\Счета\\" + prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5)))
                    {
                        prow.docdirpath = prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5);
                        prow.EndEdit();
                        System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
                    }
                    else
                    {
                        if (MessageBox.Show("Не удалось найти папку отправки: E:\\Счета\\" + prow.docdirpath + "\nСоздать папку?", "Папка документов", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            System.IO.Directory.CreateDirectory("E:\\Счета\\" + prow.docdirpath);
                            System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Папка документов");
            }
        }
        private void MoveInformStore_Click(object sender, RoutedEventArgs e)
        {
            ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            for (int i = 0; i < ParcelRequestDataGrid.Items.Count; i++)
            {
                ParcelDS.tableParcelRequestRow row = (this.ParcelRequestDataGrid.Items[i] as DataRowView).Row as ParcelDS.tableParcelRequestRow;
                if (row.IsstorageInformNull())
                {
                    row.storageInform = DateTime.Today;
                    row.EndEdit();
                }
            }
        }
        private void MoveSpecification_Click(object sender, RoutedEventArgs e)
        {
            if (this.SaveChanges() && this.ParcelNumberList.SelectedIndex > -1 && ((this.ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).parceltype == 1)
            {
                FileInfo[] files;
                string num = ((this.ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).parcelnumber;
                DirectoryInfo dirIn = new DirectoryInfo(@"V:\Отправки");
                if (dirIn.Exists)
                {
                    if (dirIn.GetDirectories(num + "_*").Length > 0)
                    {
                        dirIn = dirIn.GetDirectories(num + "_*")[0];
                        DirectoryInfo dirOut = new DirectoryInfo(@"V:\Спецификации");
                        if (dirOut.Exists)
                        {
                            foreach (ParcelDS.tableParcelRequestRow row in parcelDS.tableParcelRequest)
                            {
                                if (row.IsparcelNull()) continue;
                                //if (!row.isspecification)
                                //{
                                files = dirOut.GetFiles("*" + row.storagePoint + "*");
                                if (files.Length > 0)
                                {
                                    try
                                    {
                                        if (File.Exists(dirIn.FullName + "\\" + files[0].Name))
                                            File.Delete(dirIn.FullName + "\\" + files[0].Name);
                                        files[0].MoveTo(dirIn.FullName + "\\" + files[0].Name);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "Ошибка доступа к файлу", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                if (dirIn.GetFiles("*" + row.storagePoint + "*").Length > 0)
                                {
                                    row.isspecification = true;
                                    row.EndEdit();
                                }
                                //}
                            }
                        }
                        else
                            MessageBox.Show(@"Папка 'V:\Спецификации' не найдена!", "Перенос спецификаций", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show(@"Папка 'V:\Отправки\" + num + "_...' не найдена!", "Перенос спецификаций", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show(@"Папка 'V:\Отправки' не найдена!", "Перенос спецификаций", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadSpecification_Click(object sender, RoutedEventArgs e)
        {
            if(this.SaveChanges() && this.ParcelNumberList.SelectedIndex > -1 && ((this.ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).parceltype == 1
               && MessageBox.Show("Загрузить спецификации ?","Загрузка спецификаций",MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                try
                {
                    string num = ((this.ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).parcelnumber;
                    DirectoryInfo dirIn = new DirectoryInfo(@"V:\Отправки");
                    if (dirIn.Exists)
                    {
                        if (dirIn.GetDirectories(num + "_*").Length > 0)
                        {
                            dirIn = dirIn.GetDirectories(num + "_*")[0];
                            if (mybw == null)
                            {
                                mybw = new System.ComponentModel.BackgroundWorker();
                                mybw.DoWork += BackgroundWorker_DoWork;
                                mybw.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                                mybw.WorkerReportsProgress = true;
                                mybw.ProgressChanged += BackgroundWorker_ProgressChanged;
                            }
                            if (!mybw.IsBusy)
                            {
                                if (myExcelImportWin != null && myExcelImportWin.IsVisible)
                                {
                                    myExcelImportWin.MessageTextBlock.Text = string.Empty;
                                    myExcelImportWin.ProgressBar1.Value = 0;
                                }
                                else
                                {
                                    myExcelImportWin = new ExcelImportWin();
                                    myExcelImportWin.Show();
                                }
                                string[] arg = { "true", dirIn.FullName, (System.Windows.MessageBox.Show("Пропускать уже имеющиеся позиции (по номеру)?\nИмеющиеся позиции не будут обновлены значениями из файла.", "Загрузка данных", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes).ToString() };
                                mybw.RunWorkerAsync(arg);
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
                            }
                        }
                        else
                            MessageBox.Show(@"Папка 'V:\Отправки\" + num + "_...' не найдена!", "Перенос спецификаций", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show(@"Папка 'V:\Отправки' не найдена!", "Перенос спецификаций", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Загрузка спецификаций");
                }
            }
        }

        private void RequestAddButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)this.ParcelNumberList.SelectedValue < 0)
            {
                try
                {
                    (this.ParcelNumberList.SelectedItem as DataRowView).EndEdit();
                    ParcelDSTableAdapters.ParcelAdapter parcelAdapter = new ParcelDSTableAdapters.ParcelAdapter();
                    parcelAdapter.Update((this.ParcelNumberList.SelectedItem as DataRowView).Row);
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
                    else if (ex is System.Data.NoNullAllowedException)
                    {
                        if (parcelDS.tableParcel.HasErrors)
                            ParcelNumberList.SelectedItem = parcelDS.tableParcel.GetErrors()[0];
                        else
                        {
                            foreach (DataRowView viewrow in parcelDS.tableParcel.DefaultView)
                            {
                                if (viewrow.IsNew)
                                {
                                    ParcelNumberList.SelectedItem = viewrow;
                                }
                            }
                        }
                        string msg = string.Empty;
                        if (((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).IsNull("parceltype")) msg = msg + " \"Тип\"";
                        if (((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).IsNull("shipplandate")) msg = " \"Дата отгрузки план\"";
                        MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля" + msg + " или удалите перевозку.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    return;
                }
            }
            if ((RequestDataGrid.SelectedIndex > -1) | (RequestDataGrid.Items.Count == 1))
            {
                if (RequestDataGrid.Items.Count == 1) this.RequestDataGrid.SelectedItems.Add(this.RequestDataGrid.Items[0]);

                ParcelDS.tableParcelRow parcelrow = (this.ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow;
                ParcelDS.tableParcelRequestRow[] rows = new ParcelDS.tableParcelRequestRow[RequestDataGrid.SelectedItems.Count];
                for (int i = 0; i < RequestDataGrid.SelectedItems.Count; i++)
                {
                    rows[i] = (this.RequestDataGrid.SelectedItems[i] as DataRowView).Row as ParcelDS.tableParcelRequestRow;
                }
                RequestDataGrid.SelectionChanged -= RequestDataGrid_SelectionChanged;
                foreach (ParcelDS.tableParcelRequestRow row in rows)
                {
                    row.parcel = parcelrow.parcelId;
                    row.statusId = (int)this.statusComboBox.SelectedValue;
                    row.status = ((this.statusComboBox.SelectedItem as DataRowView).Row as ReferenceDS.tableRequestStatusRow).name;
                    row.EndEdit();
                }
                RequestDataGrid.SelectionChanged += RequestDataGrid_SelectionChanged;
                //ParcelDS.tableParcelRequestRow row;
                //if (this.RequestDataGrid.SelectedIndex > -1)
                //    row = (this.RequestDataGrid.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRequestRow;
                //else
                //    row = (this.RequestDataGrid.Items[0] as DataRowView).Row as ParcelDS.tableParcelRequestRow;
                //row.parcel = (int)this.ParcelNumberList.SelectedValue;
                //row.statusId = (int)this.statusComboBox.SelectedValue;
                //row.status = ((this.statusComboBox.SelectedItem as DataRowView).Row as ReferenceDS.tableRequestStatusRow).name;
                //row.EndEdit();
                //if (!row.IsvolumeNull()) this.volumeTextBox.Text = (decimal.Parse(this.volumeTextBox.Text) + row.volume).ToString("N4");
                //if (!row.IsactualWeightNull()) this.actualWeightTextBox.Text = (decimal.Parse(this.actualWeightTextBox.Text) + row.actualWeight).ToString("N4");
                //decimal v = 0M, m = 0M;
                //decimal.TryParse(lorryvolumeTextBox.Text, out v);
                //decimal.TryParse(lorryWeightTextBox.Text, out m);
                //if (v < decimal.Parse(this.volumeTextBox.Text)) this.volumeTextBox.Foreground = Brushes.Red;
                //if (m < decimal.Parse(this.actualWeightTextBox.Text)) this.actualWeightTextBox.Foreground = Brushes.Red;
            }
            else
            {
                MessageBox.Show("Выделите строки в нижнем списке", "Постановка в загрузку", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void RequestOutButton_Click(object sender, RoutedEventArgs e)
        {
            if ((ParcelRequestDataGrid.SelectedIndex > -1) | (ParcelRequestDataGrid.Items.Count == 1))
            {
                if (ParcelRequestDataGrid.Items.Count == 1) this.ParcelRequestDataGrid.SelectedItems.Add(this.ParcelRequestDataGrid.Items[0]);
                ParcelDS.tableParcelRequestRow[] rows = new ParcelDS.tableParcelRequestRow[ParcelRequestDataGrid.SelectedItems.Count];
                for (int i = 0; i < ParcelRequestDataGrid.SelectedItems.Count; i++)
                {
                    rows[i] = (this.ParcelRequestDataGrid.SelectedItems[i] as DataRowView).Row as ParcelDS.tableParcelRequestRow;
                }
                int n = 0; decimal v = 0M, aw = 0M, ow = 0M, c = 0M;
                ReferenceDS.tableRequestStatusRow statusrow = (this.FindResource("keyReferenceDS") as ReferenceDS).tableRequestStatus.Select("rowId=40")[0] as ReferenceDS.tableRequestStatusRow;
                ParcelRequestDataGrid.SelectionChanged -= ParcelRequestDataGrid_SelectionChanged;
                foreach (ParcelDS.tableParcelRequestRow row in rows)
                {
                    row.SetparcelNull();
                    row.SetstorageInformNull();
                    row.statusId = statusrow.rowId;
                    row.status = statusrow.name;
                    row.EndEdit();
                    if (!row.IsvolumeNull()) v = v + row.volume;
                    if (!row.IsactualWeightNull()) aw = aw + row.actualWeight;
                    if (!row.IsofficialWeightNull()) ow = ow + row.officialWeight;
                    if (!row.IscellNumberNull()) n = n + row.cellNumber;
                    if (!row.IsgoodValueNull()) c = c + row.goodValue;
                }
                ParcelRequestDataGrid.SelectionChanged += ParcelRequestDataGrid_SelectionChanged;
                //ParcelDS.tableParcelRequestRow row;
                //if (this.ParcelRequestDataGrid.SelectedIndex > -1)
                //    row = (this.ParcelRequestDataGrid.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRequestRow;
                //else
                //    row = (this.ParcelRequestDataGrid.Items[0] as DataRowView).Row as ParcelDS.tableParcelRequestRow;
                //row.SetparcelNull();
                //ReferenceDS.tableRequestStatusRow statusrow=(this.FindResource("keyReferenceDS") as ReferenceDS).tableRequestStatus.Select("rowId=40")[0] as ReferenceDS.tableRequestStatusRow;
                //row.statusId = statusrow.rowId;
                //row.status = statusrow.name;
                //row.EndEdit();
                this.volumeTextBox.Text = (decimal.Parse(this.volumeTextBox.Text) - v).ToString("N4");
                this.volumeFreeTextBox.Text = (decimal.Parse(this.volumeFreeTextBox.Text) + v).ToString("N4");
                this.actualWeightTextBox.Text = (decimal.Parse(this.actualWeightTextBox.Text) - aw).ToString("N4");
                this.actualWeightFreeTextBox.Text = (decimal.Parse(this.actualWeightFreeTextBox.Text) + aw).ToString("N4");
                this.officialWeightTextBox.Text = (decimal.Parse(this.officialWeightTextBox.Text) - ow).ToString("N4");
                this.officialWeightFreeTextBox.Text = (decimal.Parse(this.officialWeightFreeTextBox.Text) + ow).ToString("N4");
                this.offactWeightTextBox.Text = (decimal.Parse(this.actualWeightTextBox.Text) - decimal.Parse(this.officialWeightTextBox.Text)).ToString("N4");
                this.offactWeightFreeTextBox.Text = (decimal.Parse(this.actualWeightFreeTextBox.Text) - decimal.Parse(this.officialWeightFreeTextBox.Text)).ToString("N4");
                this.goodValueTextBox.Text = (decimal.Parse(this.goodValueTextBox.Text) - c).ToString("N2");
                this.goodValueFreeTextBox.Text = (decimal.Parse(this.goodValueFreeTextBox.Text) + c).ToString("N2");
                this.cellNumberTextBox.Text = (Int16.Parse(this.cellNumberTextBox.Text) - n).ToString();
                this.cellNumberFreeTextBox.Text = (Int16.Parse(this.cellNumberFreeTextBox.Text) + n).ToString();
                decimal.TryParse(lorryvolumeTextBox.Text, out v);
                decimal.TryParse(lorryWeightTextBox.Text, out ow);
                if (!(v < decimal.Parse(this.volumeTextBox.Text))) this.volumeTextBox.Foreground = this.lorryvolumeTextBox.Foreground;
                if (!(ow < decimal.Parse(this.actualWeightTextBox.Text))) this.actualWeightTextBox.Foreground = this.lorryWeightTextBox.Foreground;
            }
            else
            {
                MessageBox.Show("Выделите строку в верхнем списке", "Снятие с загрузки", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ParcelNumberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int parcelid = ParcelNumberList.SelectedValue != null ? (int)ParcelNumberList.SelectedValue : 0;
            loadParcelRequest(parcelid);
        }
        private void loadParcelRequest(int parcelid)
        {
            ParcelRequestDataGrid.ItemsSource = null;
            RequestDataGrid.ItemsSource = null;
            if (parcelid != 0)
            {
                ParcelDSTableAdapters.ParcelRequestAdapter requestAdapter = new ParcelDSTableAdapters.ParcelRequestAdapter();
                try
                {
                    if (isRequestSave) requestAdapter.Update(parcelDS.tableParcelRequest);
                    requestAdapter.Fill(parcelDS.tableParcelRequest, parcelid);
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
                    else if (ex is System.Data.NoNullAllowedException)
                    {
                        MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля или удалите перевозку.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    return;
                }
                ParcelRequestDataGrid.ItemsSource = viewParcelRequest;
                RequestDataGrid.ItemsSource = viewRequest;
                RequestDataGrid.IsEnabled = true;
                StackPanel1.IsEnabled = true;
                this.WrapPanel1.IsEnabled = true;
                this.WrapPanel2.IsEnabled = true;
                this.WrapPanel3.IsEnabled = true;
                this.Grid1.IsEnabled = true;
                decimal v = 0M, am = 0M, om = 0M, c = 0M; int n = 0;
                foreach (DataRowView row in viewParcelRequest)
                {
                    ParcelDS.tableParcelRequestRow prrow = row.Row as ParcelDS.tableParcelRequestRow;
                    if (!prrow.IsvolumeNull()) v = v + prrow.volume;
                    if (!prrow.IsactualWeightNull()) am = am + prrow.actualWeight;
                    if (!prrow.IsofficialWeightNull()) om = om + prrow.officialWeight;
                    if (!prrow.IscellNumberNull()) n = n + prrow.cellNumber;
                    if (!prrow.IsgoodValueNull()) c = c + prrow.goodValue;
                }
                this.volumeTextBox.Text = v.ToString("N4");
                this.goodValueTextBox.Text = c.ToString("N2");
                this.actualWeightTextBox.Text = am.ToString("N4");
                this.officialWeightTextBox.Text = om.ToString("N4");
                this.offactWeightTextBox.Text = (am - om).ToString("N4");
                this.cellNumberTextBox.Text = n.ToString();
                if (ParcelNumberList.SelectedIndex < 0)
                {
                    v = 0; am = 0;
                }
                else
                {
                    ParcelDS.tableParcelRow parcelrow = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow;
                    v = parcelrow.IslorryvolumeNull() ? 0 : parcelrow.lorryvolume;
                    am = parcelrow.IslorrytonnageNull() ? 0 : parcelrow.lorrytonnage;
                }
                if (v < decimal.Parse(this.volumeTextBox.Text))
                    this.volumeTextBox.Foreground = Brushes.Red;
                else
                    this.volumeTextBox.Foreground = this.lorryvolumeTextBox.Foreground;
                if (am < decimal.Parse(this.actualWeightTextBox.Text))
                    this.actualWeightTextBox.Foreground = Brushes.Red;
                else
                    this.actualWeightTextBox.Foreground = lorryWeightTextBox.Foreground;
                v = 0M; am = 0M; om = 0M; c = 0M; n = 0;
                foreach (DataRowView row in viewRequest)
                {
                    ParcelDS.tableParcelRequestRow prrow = row.Row as ParcelDS.tableParcelRequestRow;
                    if (!prrow.IsvolumeNull()) v = v + prrow.volume;
                    if (!prrow.IsactualWeightNull()) am = am + prrow.actualWeight;
                    if (!prrow.IsofficialWeightNull()) om = om + prrow.officialWeight;
                    if (!prrow.IscellNumberNull()) n = n + prrow.cellNumber;
                    if (!prrow.IsgoodValueNull()) c = c + prrow.goodValue;
                }
                this.volumeFreeTextBox.Text = v.ToString("N4");
                this.goodValueFreeTextBox.Text = c.ToString("N2");
                this.actualWeightFreeTextBox.Text = am.ToString("N4");
                this.officialWeightFreeTextBox.Text = om.ToString("N4");
                this.offactWeightFreeTextBox.Text = (am - om).ToString("N4");
                this.cellNumberFreeTextBox.Text = n.ToString();
            }
            else
            {
                this.RequestDataGrid.IsEnabled = false;
                this.StackPanel1.IsEnabled = false;
                this.WrapPanel1.IsEnabled = false;
                this.WrapPanel2.IsEnabled = false;
                this.WrapPanel3.IsEnabled = false;
                this.Grid1.IsEnabled = false;
                this.volumeTextBox.Text = 0.ToString("N4");
                this.actualWeightTextBox.Text = this.volumeTextBox.Text;
                this.officialWeightTextBox.Text = this.volumeTextBox.Text;
                this.offactWeightTextBox.Text = this.volumeTextBox.Text;
                this.cellNumberTextBox.Text = this.volumeTextBox.Text;
                this.goodValueTextBox.Text = this.volumeTextBox.Text;
                this.volumeFreeTextBox.Text = this.volumeTextBox.Text;
                this.actualWeightFreeTextBox.Text = this.volumeTextBox.Text;
                this.officialWeightFreeTextBox.Text = this.volumeTextBox.Text;
                this.offactWeightFreeTextBox.Text = this.volumeTextBox.Text;
                this.cellNumberFreeTextBox.Text = this.volumeTextBox.Text;
                this.goodValueFreeTextBox.Text = this.volumeTextBox.Text;
            }
        }

        private void RequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridCellInfo cellinf;
            ParcelDS.tableParcelRequestRow row;
            ParcelDS.tableParcelRequestRow rowgroup;
            //int countnoready = 0;
            DataRowView[] noreadyrowview = new DataRowView[RequestDataGrid.Items.Count];
            foreach (DataRowView rowview in e.AddedItems)
            {
                if (!(rowview.Row is ParcelDS.tableParcelRequestRow)) break;
                row = rowview.Row as ParcelDS.tableParcelRequestRow;
                //if (row.IsspecificationNull())
                //{
                //    noreadyrowview[countnoready] = rowview;
                //    countnoready++;
                //    continue;
                //}
                if (!row.IsparcelgroupNull())
                {
                    foreach (DataRowView viewrow in rowview.DataView)
                    {
                        rowgroup = viewrow.Row as ParcelDS.tableParcelRequestRow;
                        if (!rowgroup.IsparcelgroupNull() && row.pgroupsort == rowgroup.pgroupsort && !RequestDataGrid.SelectedItems.Contains(viewrow))
                        {
                            //if (rowgroup.IsspecificationNull())
                            //{
                            //    noreadyrowview[countnoready] = viewrow;
                            //    countnoready++;
                            //    continue;
                            //}
                            RequestDataGrid.SelectedItems.Add(viewrow);
                            foreach (DataGridColumn colm in this.RequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(viewrow, colm);
                                if (!RequestDataGrid.SelectedCells.Contains(cellinf)) RequestDataGrid.SelectedCells.Add(cellinf);
                            }
                            break;
                        }
                    }
                }
                if (!row.IsvolumeNull())
                {
                    this.volumeTextBox.Text = (decimal.Parse(this.volumeTextBox.Text) + row.volume).ToString("N4");
                    this.volumeFreeTextBox.Text = (decimal.Parse(this.volumeFreeTextBox.Text) - row.volume).ToString("N4");
                }
                if (!row.IsactualWeightNull())
                {
                    this.actualWeightTextBox.Text = (decimal.Parse(this.actualWeightTextBox.Text) + row.actualWeight).ToString("N4");
                    this.actualWeightFreeTextBox.Text = (decimal.Parse(this.actualWeightFreeTextBox.Text) - row.actualWeight).ToString("N4");
                }
                if (!row.IsofficialWeightNull())
                {
                    this.officialWeightTextBox.Text = (decimal.Parse(this.officialWeightTextBox.Text) + row.officialWeight).ToString("N4");
                    this.officialWeightFreeTextBox.Text = (decimal.Parse(this.officialWeightFreeTextBox.Text) - row.officialWeight).ToString("N4");
                }
                this.offactWeightTextBox.Text = (decimal.Parse(this.actualWeightTextBox.Text) - decimal.Parse(this.officialWeightTextBox.Text)).ToString("N4");
                this.offactWeightFreeTextBox.Text = (decimal.Parse(this.actualWeightFreeTextBox.Text) - decimal.Parse(this.officialWeightFreeTextBox.Text)).ToString("N4");
                if (!row.IsgoodValueNull())
                {
                    this.goodValueTextBox.Text = (decimal.Parse(this.goodValueTextBox.Text) + row.goodValue).ToString("N2");
                    this.goodValueFreeTextBox.Text = (decimal.Parse(this.goodValueFreeTextBox.Text) - row.goodValue).ToString("N2");
                }
                if (!row.IscellNumberNull())
                {
                    this.cellNumberTextBox.Text = (Int16.Parse(this.cellNumberTextBox.Text) + row.cellNumber).ToString();
                    this.cellNumberFreeTextBox.Text = (Int16.Parse(this.cellNumberFreeTextBox.Text) - row.cellNumber).ToString();
                }
                decimal v = 0M, m = 0M;
                decimal.TryParse(lorryvolumeTextBox.Text, out v);
                decimal.TryParse(lorryWeightTextBox.Text, out m);
                if (v < decimal.Parse(this.volumeTextBox.Text)) this.volumeTextBox.Foreground = Brushes.Red;
                if (m < decimal.Parse(this.actualWeightTextBox.Text)) this.actualWeightTextBox.Foreground = Brushes.Red;

            }
            foreach (DataRowView rowview in e.RemovedItems)
            {
                if (!(rowview.Row is ParcelDS.tableParcelRequestRow)) break;
                row = rowview.Row as ParcelDS.tableParcelRequestRow;
                if (!row.IsparcelgroupNull())
                {
                    foreach (DataRowView viewrow in RequestDataGrid.SelectedItems)
                    {
                        rowgroup = viewrow.Row as ParcelDS.tableParcelRequestRow;
                        if (!rowgroup.IsparcelgroupNull() && row.pgroupsort == rowgroup.pgroupsort)
                        {
                            RequestDataGrid.SelectedItems.Remove(viewrow);
                            foreach (DataGridColumn colm in this.RequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(viewrow, colm);
                                if (RequestDataGrid.SelectedCells.Contains(cellinf)) RequestDataGrid.SelectedCells.Remove(cellinf);
                            }
                            break;
                        }
                    }
                }
                if (!row.IsvolumeNull())
                {
                    this.volumeTextBox.Text = (decimal.Parse(this.volumeTextBox.Text) - row.volume).ToString("N4");
                    this.volumeFreeTextBox.Text = (decimal.Parse(this.volumeFreeTextBox.Text) + row.volume).ToString("N4");
                }
                if (!row.IsactualWeightNull())
                {
                    this.actualWeightTextBox.Text = (decimal.Parse(this.actualWeightTextBox.Text) - row.actualWeight).ToString("N4");
                    this.actualWeightFreeTextBox.Text = (decimal.Parse(this.actualWeightFreeTextBox.Text) + row.actualWeight).ToString("N4");
                }
                if (!row.IsofficialWeightNull())
                {
                    this.officialWeightTextBox.Text = (decimal.Parse(this.officialWeightTextBox.Text) - row.officialWeight).ToString("N4");
                    this.officialWeightFreeTextBox.Text = (decimal.Parse(this.officialWeightFreeTextBox.Text) + row.officialWeight).ToString("N4");
                }
                this.offactWeightTextBox.Text = (decimal.Parse(this.actualWeightTextBox.Text) - decimal.Parse(this.officialWeightTextBox.Text)).ToString("N4");
                this.offactWeightFreeTextBox.Text = (decimal.Parse(this.actualWeightFreeTextBox.Text) - decimal.Parse(this.officialWeightFreeTextBox.Text)).ToString("N4");
                if (!row.IsgoodValueNull())
                {
                    this.goodValueTextBox.Text = (decimal.Parse(this.goodValueTextBox.Text) - row.goodValue).ToString("N2");
                    this.goodValueFreeTextBox.Text = (decimal.Parse(this.goodValueFreeTextBox.Text) + row.goodValue).ToString("N2");
                }
                if (!row.IscellNumberNull())
                {
                    this.cellNumberTextBox.Text = (Int16.Parse(this.cellNumberTextBox.Text) - row.cellNumber).ToString();
                    this.cellNumberFreeTextBox.Text = (Int16.Parse(this.cellNumberFreeTextBox.Text) + row.cellNumber).ToString();
                }
                decimal v = 0M, m = 0M;
                decimal.TryParse(lorryvolumeTextBox.Text, out v);
                decimal.TryParse(lorryWeightTextBox.Text, out m);
                if (!(v < decimal.Parse(this.volumeTextBox.Text))) this.volumeTextBox.Foreground = this.lorryvolumeTextBox.Foreground;
                if (!(m < decimal.Parse(this.actualWeightTextBox.Text))) this.actualWeightTextBox.Foreground = this.lorryWeightTextBox.Foreground;
            }
            //if (countnoready > 0)
            //{
            //    RequestDataGrid.SelectionChanged -= RequestDataGrid_SelectionChanged;
            //    for (int i = 0; i < countnoready; i++)
            //    {
            //        if (RequestDataGrid.SelectedItems.Contains(noreadyrowview[i])) RequestDataGrid.SelectedItems.Remove(noreadyrowview[i]);
            //        MessageBox.Show("Заявка " + (noreadyrowview[i].Row as ParcelDS.tableParcelRequestRow).requestId.ToString() + " не может быть поставлена в загрузку т.к. отсутствует спецификация!", "Постановка в загрузку", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    }
            //    RequestDataGrid.SelectionChanged += RequestDataGrid_SelectionChanged;
            //}
        }
        private void ParcelRequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridCellInfo cellinf;
            ParcelDS.tableParcelRequestRow row;
            ParcelDS.tableParcelRequestRow rowgroup;
            if (!(e.OriginalSource is DataGrid)) return;
            foreach (DataRowView rowview in e.AddedItems)
            {
                if (!(rowview.Row is ParcelDS.tableParcelRequestRow)) break;
                row = rowview.Row as ParcelDS.tableParcelRequestRow;
                if (!row.IsparcelgroupNull())
                {
                    foreach (DataRowView viewrow in rowview.DataView)
                    {
                        rowgroup = viewrow.Row as ParcelDS.tableParcelRequestRow;
                        if (!rowgroup.IsparcelgroupNull() && row.pgroupsort == rowgroup.pgroupsort && !ParcelRequestDataGrid.SelectedItems.Contains(viewrow))
                        {
                            ParcelRequestDataGrid.SelectedItems.Add(viewrow);
                            foreach (DataGridColumn colm in this.ParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(viewrow, colm);
                                if (!ParcelRequestDataGrid.SelectedCells.Contains(cellinf)) ParcelRequestDataGrid.SelectedCells.Add(cellinf);
                            }
                            break;
                        }
                    }
                }
            }
            foreach (DataRowView rowview in e.RemovedItems)
            {
                if (!(rowview.Row is ParcelDS.tableParcelRequestRow)) break;
                row = rowview.Row as ParcelDS.tableParcelRequestRow;
                if (!row.IsparcelgroupNull())
                {
                    foreach (DataRowView itemrow in ParcelRequestDataGrid.SelectedItems)
                    {
                        rowgroup = itemrow.Row as ParcelDS.tableParcelRequestRow;
                        if (!rowgroup.IsparcelgroupNull() && row.pgroupsort == rowgroup.pgroupsort)
                        {
                            ParcelRequestDataGrid.SelectedItems.Remove(itemrow);
                            foreach (DataGridColumn colm in this.ParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(itemrow, colm);
                                if (ParcelRequestDataGrid.SelectedCells.Contains(cellinf)) ParcelRequestDataGrid.SelectedCells.Remove(cellinf);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void RequestUpDown_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue) viewRequest.RowFilter = "parcel Is Null"; else viewRequest.RowFilter = "parcel=0";
        }

        private void RequestItem_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grid = null;
            if (ParcelRequestDataGrid.IsKeyboardFocusWithin)
                grid = ParcelRequestDataGrid;
            else if (RequestDataGrid.IsFocused)
                grid = RequestDataGrid;
            if (grid != null && grid.CurrentItem != null & grid.CommitEdit(DataGridEditingUnit.Row, true))
            {

                if (!grid.CurrentCell.IsValid) //для обновления Grid
                {
                    grid.CurrentCell = new DataGridCellInfo(grid.CurrentItem, grid.Columns[4]);
                }
                RequestItemWin newWin = null;
                foreach (Window item in this.OwnedWindows)
                {
                    if (item.Name == "winRequestItem")
                    {
                        if ((item as RequestItemWin).mainGrid.DataContext.Equals(grid.CurrentItem))
                            newWin = item as RequestItemWin;
                    }
                }
                if (newWin == null)
                {
                    newWin = new RequestItemWin();
                    newWin.Owner = this;
                    ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
                    newWin.statusComboBox.ItemsSource = new System.Data.DataView(refDS.tableRequestStatus, "rowId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
                    newWin.goodsComboBox.ItemsSource = new System.Data.DataView(refDS.tableGoodsType, "Iditem>0", string.Empty, System.Data.DataViewRowState.CurrentRows);

                    RequestDS requestDS = ((RequestDS)(this.FindResource("requestDS")));
                    newWin.customerComboBox.ItemsSource = new System.Data.DataView(requestDS.tableCustomerName, string.Empty, "customerName", System.Data.DataViewRowState.CurrentRows);
                    newWin.agentComboBox.ItemsSource = new System.Data.DataView(requestDS.tableAgentName, string.Empty, "agentName", System.Data.DataViewRowState.CurrentRows);
                    newWin.mainGrid.DataContext = grid.CurrentItem;
                    newWin.RequestItemViewCommand = new Classes.Domain.RequestItemViewCommand(((grid.CurrentItem as DataRowView).Row as CustomBrokerWpf.RequestDS.tableRequestRow).requestId);
                    newWin.thisStoragePointValidationRule.RequestId = ((grid.CurrentItem as DataRowView).Row as CustomBrokerWpf.RequestDS.tableRequestRow).requestId;
                    newWin.Show();
                }
                else
                {
                    newWin.Activate();
                    if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
                }
            }
        }

        private void ExcelReport()
        {
            bool isNew; int offset;
            offset = 2;
            isNew = MessageBox.Show("Перенести в Excel только новые заявки?", "в Excel", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            excel.Application exApp = new excel.Application();
            excel.Application exAppProt = new excel.Application();
            excel.Workbook exWb;
            try
            {
                ParcelDS.tableParcelRequestRow itemRow;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                excel.Worksheet exWh = exWb.Sheets[1];
                excel.Range r;
                exWh.Name = ParcelNumberList.Text;
                exWh.Cells[1, 1] = "Позиция по складу"; exWh.Cells[1, 2] = "Дата поступления"; exWh.Cells[1, 3] = "Клиент"; exWh.Cells[1, 4] = "Поставщик"; exWh.Cells[1, 5] = "Группа менеджеров";
                exWh.Cells[1, 6] = "Кол-во мест"; exWh.Cells[1, 7] = "Вес по док, кг"; exWh.Cells[1, 8] = "Вес факт, кг"; exWh.Cells[1, 9] = "Объем, м3"; exWh.Cells[1, 10] = "Примечание менеджера";
                r = exWh.Columns[6, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[7, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[8, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[9, Type.Missing]; r.NumberFormat = "#,##0.00";
                for (int i = 0; i < viewParcelRequest.Count; i++)
                {
                    itemRow = viewParcelRequest[i].Row as ParcelDS.tableParcelRequestRow;
                    if (isNew && !itemRow.IsstorageInformNull()) { offset--; continue; }
                    if (!itemRow.IsstoragePointNull()) exWh.Cells[offset + i, 1] = itemRow.storagePoint;
                    if (!itemRow.IsstorageDateNull()) exWh.Cells[offset + i, 2] = itemRow.storageDate;
                    if (!itemRow.IscustomerFullNameNull()) exWh.Cells[offset + i, 3] = itemRow.customerFullName;
                    if (!itemRow.IsagentFullNameNull()) exWh.Cells[offset + i, 4] = itemRow.agentFullName;
                    if (!itemRow.IsmanagerGroupNull()) exWh.Cells[offset + i, 5] = itemRow.managerGroup;
                    if (!itemRow.IscellNumberNull()) exWh.Cells[offset + i, 6] = itemRow.cellNumber;
                    if (!itemRow.IsofficialWeightNull()) exWh.Cells[offset + i, 7] = itemRow.officialWeight;
                    if (!itemRow.IsactualWeightNull()) exWh.Cells[offset + i, 8] = itemRow.actualWeight;
                    if (!itemRow.IsvolumeNull()) exWh.Cells[offset + i, 9] = itemRow.volume;
                    if (!itemRow.IsmanagerNoteNull()) exWh.Cells[offset + i, 10] = itemRow.managerNote;
                }
                exApp.Visible = true;
                exWh = null;
            }
            catch (Exception ex)
            {
                if (exApp != null)
                {
                    foreach (excel.Workbook itemBook in exApp.Workbooks)
                    {
                        itemBook.Close(false);
                    }
                    exApp.Quit();
                }
                MessageBox.Show(ex.Message, "Создание заявки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }

        #region Filter
        private CustomBrokerWpf.SQLFilter thisfilter = new SQLFilter("parcel", "AND");
        public bool IsShowFilter
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        public SQLFilter Filter
        {
            get { return thisfilter; }
            set
            {
                if (this.IsLoaded && !SaveChanges())
                    MessageBox.Show("Применение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    thisfilter.RemoveCurrentWhere();
                    thisfilter = value;
                    if (this.IsLoaded) mainDataRefresh();
                }
            }
        }
        public void RunFilter()
        {
            if (!SaveChanges())
                MessageBox.Show("Применение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                mainDataRefresh();
            }
        }
        private void setFilterButtonImage()
        {
            string uribitmap;
            if (thisfilter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (FilterButton.Content as Image).Source = bi3;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winParcelFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new ParcelFilterWin();
                    (ObjectWin as ParcelFilterWin).FilterOwner = this;
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
        #endregion

        private void mainValidation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action != ValidationErrorEventAction.Removed)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        private ExcelImportWin myExcelImportWin;
        private System.ComponentModel.BackgroundWorker mybw;
        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            excel.Application exApp = new excel.Application();
            excel.Application exAppProt = new excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            int totalcount=0;
            string[] args = e.Argument as string[];
            bool isclose = bool.Parse(args[0]);
            DirectoryInfo dirIn = new DirectoryInfo(args[1]);
            FileInfo[] files;
            try
            {
                int num = 1, count = viewParcelRequest.Count;
                System.Collections.Generic.List<string> loaded = new System.Collections.Generic.List<string>();
                ParcelDS.tableParcelRequestRow row;
                foreach (DataRowView viewrow in viewParcelRequest)
                {
                    row = viewrow.Row as ParcelDS.tableParcelRequestRow;
                    if (!row.IsparcelNull() & !row.IsstoragePointNull() && !string.IsNullOrEmpty(row.storagePoint) )
                    {
                        
                        files = dirIn.GetFiles("?"+row.storagePoint + "*");
                        if (!loaded.Contains(row.storagePoint) & files.Length > 0)
                        {
                            requestid = row.requestId;
                            Classes.Domain.RequestItemViewCommand cmd = App.Current.Dispatcher.Invoke<Classes.Domain.RequestItemViewCommand>(new Func<Classes.Domain.RequestItemViewCommand>(this.GetNewRequestItemCMD), System.Windows.Threading.DispatcherPriority.Normal);
                            totalcount += cmd.OnExcelImportReady(worker, exApp, files[0].FullName, bool.Parse(args[2]), row, num, count);
                            App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<Classes.Domain.RequestItemViewCommand>(ritemcmd.Add), cmd);
                            cmd = null;
                            loaded.Add(row.storagePoint);
                        }
                    }
                    else
                        worker.ReportProgress((int)100 * num / count);
                    num++;
                }
                e.Result = totalcount;
            }
            finally
            {
                if (exApp != null)
                {
                    if (isclose)
                    {
                        foreach (excel.Workbook itemBook in exApp.Workbooks)
                        {
                            itemBook.Close(false);
                        }
                        exApp.DisplayAlerts = true;
                        exApp.ScreenUpdating = true;
                        exApp.Quit();
                    }
                    else
                    {
                        exApp.Visible = true;
                        exApp.DisplayAlerts = true;
                        exApp.ScreenUpdating = true;
                    }
                    exApp = null;
                }
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                myExcelImportWin.MessageTextBlock.Text = "Обработка прервана из-за ошибки" + "\n" + e.Error.Message;
            }
            else
            {
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                myExcelImportWin.MessageTextBlock.Text = "Обработка выполнена успешно." + "\n" + e.Result.ToString() + " строк обработано";
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            myExcelImportWin.ProgressBar1.Value = e.ProgressPercentage;
        }
        private int requestid;
        private Classes.Domain.RequestItemViewCommand GetNewRequestItemCMD()
        { return new Classes.Domain.RequestItemViewCommand(requestid); }

        private void ColmarkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ParcelRequestDataGrid.SelectedItems.Count>0 & e.AddedItems.Count>0)
            {
                ParcelDS.tableParcelRequestRow row;
                foreach (DataRowView viewrow in ParcelRequestDataGrid.SelectedItems)
                {
                    if (viewrow != ParcelRequestDataGrid.CurrentItem)
                    {
                        row = viewrow.Row as ParcelDS.tableParcelRequestRow;
                        row.colmark = (e.AddedItems[0] as System.Windows.Shapes.Rectangle).Fill.ToString();
                        row.EndEdit();
                    }
                }
            }
        }

        private void customerButton_Click(object sender, RoutedEventArgs e)
        {

            CustomerOpen((string)(sender as Button).Tag);
        }
        private void CustomerOpen(string name)
        {
            if (name.Length > 0)
            {
                ClientWin win = new ClientWin();
                win.Show();
                win.CustomerNameList.Text = name;
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CustomerOpen((string)(sender as Label).Tag);
        }

        private void MailSMS_Click(object sender, RoutedEventArgs e)
        {
            MailSMSWin win = new MailSMSWin();
            int parcelid = ParcelNumberList.SelectedValue != null ? (int)ParcelNumberList.SelectedValue : 0;
            Classes.MailSMSCommand cmd = new Classes.MailSMSCommand(parcelid);
            win.DataContext = cmd;
            win.Owner = this;
            win.Show();
        }
    }

    internal class ParcelRequestConflictResolution
    {
        ConcurrencyManager myconcurrency;
        ParcelDS.tableParcelRequestRow myrow;
        internal ParcelDS.tableParcelRequestRow Row
        {
            set
            {
                myrow = value;
                myconcurrency.Clear();
                Resolve();
            }
        }

        internal ParcelRequestConflictResolution()
        { myconcurrency = new ConcurrencyManager(); }
        internal ParcelRequestConflictResolution(ParcelDS.tableParcelRequestRow row)
            : this()
        {
            myrow = row;
            Resolve();
        }

        private void Resolve()
        {
            Classes.Domain.Request rq = References.RequestStore.GetItem(myrow.requestId);

            PropertyConcurrencyManager<int?> statusres = new PropertyConcurrencyManager<int?>("Status", "Статус", rq.Status.Id, myrow.Field<int?>("statusId", DataRowVersion.Original), myrow.Field<int?>("statusId", DataRowVersion.Current), new Func<int?, int?, bool>((int? ser, int? cur) =>{ return (PropertyConcurrencyManager<int?>.CheckEquals(ser, cur) || (((ser == 30 & rq.Specification!=null) | ser == 40) & cur == 50) | (ser < 50 & ser != 20 & cur == 40)); }));
            PropertyConcurrencyManager<int?> parcelres = new PropertyConcurrencyManager<int?>("Parcel", "Отправка", rq.ParcelId, myrow.Field<int?>("parcel", DataRowVersion.Original), myrow.Field<int?>("parcel", DataRowVersion.Current));
            PropertyConcurrencyManager<bool> isspecificationres = new PropertyConcurrencyManager<bool>("IsSpecification",string.Empty, rq.IsSpecification, myrow.Field<bool>("isspecification", DataRowVersion.Original), myrow.Field<bool>("isspecification", DataRowVersion.Current));
            PropertyConcurrencyManager<DateTime?> storeinformres = new PropertyConcurrencyManager<DateTime?>("StoreInform", "Инфо", rq.StoreInform, myrow.Field<DateTime?>("storageInform", DataRowVersion.Original), myrow.Field<DateTime?>("storageInform", DataRowVersion.Current));
            PropertyConcurrencyManager<Int16?> cellnumberres = new PropertyConcurrencyManager<Int16?>("CellNumber", "Кол-во мест", rq.CellNumber, myrow.Field<Int16?>("cellNumber", DataRowVersion.Original), myrow.Field<Int16?>("cellNumber", DataRowVersion.Current));
            PropertyConcurrencyManager<decimal?> officialweightres = new PropertyConcurrencyManager<decimal?>("OfficialWeight", "Вес по док, кг", rq.OfficialWeight, myrow.Field<decimal?>("officialWeight", DataRowVersion.Original), myrow.Field<decimal?>("officialWeight", DataRowVersion.Current));
            PropertyConcurrencyManager<decimal?> volumeres = new PropertyConcurrencyManager<decimal?>("Volume", "Объем, м3", rq.Volume, myrow.Field<decimal?>("volume", DataRowVersion.Original), myrow.Field<decimal?>("volume", DataRowVersion.Current));
            PropertyConcurrencyManager<decimal?> actualweightres = new PropertyConcurrencyManager<decimal?>("ActualWeight", "Вес факт, кг", rq.ActualWeight, myrow.Field<decimal?>("actualWeight", DataRowVersion.Original), myrow.Field<decimal?>("actualWeight", DataRowVersion.Current));
            PropertyConcurrencyManager<decimal?> goodres = new PropertyConcurrencyManager<decimal?>("GoodValue", "Стоимость товара, Е", rq.GoodValue, myrow.Field<decimal?>("goodValue", DataRowVersion.Original), myrow.Field<decimal?>("goodValue", DataRowVersion.Current));
            PropertyConcurrencyManager<string> managernoteres = new PropertyConcurrencyManager<string>("ManagerNote", "Примечание менеджера", rq.ManagerNote, myrow.Field<string>("managerNote", DataRowVersion.Original), myrow.Field<string>("managerNote", DataRowVersion.Current));
            PropertyConcurrencyManager<string> colorres = new PropertyConcurrencyManager<string>("ColorMark", "Метка", rq.ColorMark, myrow.Field<string>("colmark", DataRowVersion.Original), myrow.Field<string>("colmark", DataRowVersion.Current));
            PropertyConcurrencyManager<string> whores = new PropertyConcurrencyManager<string>("UpdateWho", "Обновил", rq.UpdateWho, myrow.Field<string>("UpdateWho", DataRowVersion.Original), myrow.Field<string>("UpdateWho", DataRowVersion.Current), new Func<string, string, bool>((string s1, string s2) => { return true; }));
            PropertyConcurrencyManager<DateTime?> whereres = new PropertyConcurrencyManager<DateTime?>("UpdateWhen", "Обновлено", rq.UpdateWhen, myrow.Field<DateTime?>("UpdateWhen", DataRowVersion.Original), myrow.Field<DateTime?>("UpdateWhen", DataRowVersion.Current), new Func<DateTime?, DateTime?, bool>((DateTime? s1, DateTime? s2) => { return true; }));

            myconcurrency.AddCheckPropertyArray(new PropertyConcurrencyManager[] { statusres, parcelres, isspecificationres, storeinformres, cellnumberres, officialweightres, actualweightres, volumeres, goodres, managernoteres, colorres, whores, whereres });

            if (myconcurrency.isConflict)
            {
                throw new Exception("Изменение данных невозможно.Запись уже была отредактирована другим пользователем!" + "\n" + "Обновите данные и повторите попытку.");
            }
            else
            {
                myrow.BeginEdit();
                try
                {
                    if (rq.Stamp!=0) myrow.stamp = (int)rq.Stamp;
                    else throw new Exception("Неудалось получить версию сервера. Возможно запись была удалена!" + "\n" + "Обновите данные и повторите попытку.");
                    if (myconcurrency.isNeedUpdate)
                    {
                        foreach (PropertyConcurrencyManager item in myconcurrency.NeedUpdateProperties.Values)
                        {
                            switch (item.PropertyName)
                            {
                                case "Status":
                                    {
                                        PropertyConcurrencyManager<int?> itemT = item as PropertyConcurrencyManager<int?>;
                                        if (itemT.Current.HasValue) myrow.statusId = itemT.Current.Value;
                                        else myrow.SetstatusIdNull();
                                    }
                                    break;
                                case "Parcel":
                                    {
                                        PropertyConcurrencyManager<int?> itemT = item as PropertyConcurrencyManager<int?>;
                                        if (itemT.Current.HasValue) myrow.parcel = itemT.Current.Value;
                                        else myrow.SetparcelNull();
                                    }
                                    break;
                                case "IsSpecification":
                                    myrow.isspecification = (item as PropertyConcurrencyManager<bool>).Current;
                                    break;
                                case "StoreInform":
                                    {
                                        PropertyConcurrencyManager<DateTime?> itemT = item as PropertyConcurrencyManager<DateTime?>;
                                        if (itemT.Current.HasValue) myrow.storageInform = itemT.Current.Value;
                                        else myrow.SetstorageInformNull();
                                    }
                                    break;
                                case "CellNumber":
                                    {
                                        PropertyConcurrencyManager<Int16?> itemT = item as PropertyConcurrencyManager<Int16?>;
                                        if (itemT.Current.HasValue) myrow.cellNumber = itemT.Current.Value;
                                        else myrow.SetcellNumberNull();
                                    }
                                    break;
                                case "OfficialWeight":
                                    {
                                        PropertyConcurrencyManager<decimal?> itemT = item as PropertyConcurrencyManager<decimal?>;
                                        if (itemT.Current.HasValue) myrow.officialWeight = itemT.Current.Value;
                                        else myrow.SetofficialWeightNull();
                                    }
                                    break;
                                case "ActualWeight":
                                    {
                                        PropertyConcurrencyManager<decimal?> itemT = item as PropertyConcurrencyManager<decimal?>;
                                        if (itemT.Current.HasValue) myrow.actualWeight = itemT.Current.Value;
                                        else myrow.SetactualWeightNull();
                                    }
                                    break;
                                case "Volume":
                                    {
                                        PropertyConcurrencyManager<decimal?> itemT = item as PropertyConcurrencyManager<decimal?>;
                                        if (itemT.Current.HasValue) myrow.volume = itemT.Current.Value;
                                        else myrow.SetvolumeNull();
                                    }
                                    break;
                                case "GoodValue":
                                    {
                                        PropertyConcurrencyManager<decimal?> itemT = item as PropertyConcurrencyManager<decimal?>;
                                        if (itemT.Current.HasValue) myrow.goodValue = itemT.Current.Value;
                                        else myrow.SetgoodValueNull();
                                    }
                                    break;
                                case "ManagerNote":
                                    {
                                        PropertyConcurrencyManager<string> itemT = item as PropertyConcurrencyManager<string>;
                                        if (string.IsNullOrEmpty(itemT.Current)) myrow.SetmanagerNoteNull();
                                        else myrow.managerNote = itemT.Current;
                                    }
                                    break;
                                case "ColorMark":
                                    {
                                        PropertyConcurrencyManager<string> itemT = item as PropertyConcurrencyManager<string>;
                                        if (string.IsNullOrEmpty(itemT.Current)) myrow.SetcolmarkNull();
                                        else myrow.colmark = itemT.Current;
                                    }
                                    break;
                            }
                        }
                    }
                }
                finally { myrow.EndEdit(); }
            }
        }
    }
}
