using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.IO;
using System.Data;
using System.ComponentModel;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ParcelListWin.xaml
    /// </summary>
    public partial class ParcelListWin : Window, ISQLFiltredWindow
    {
        ParcelDS parcelDS;
        public ParcelListWin()
        {
            parcelDS = new ParcelDS();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refDS.tableRequestStatus.Count == 0)
            {
                ReferenceDSTableAdapters.RequestStatusAdapter adapterStatus = new ReferenceDSTableAdapters.RequestStatusAdapter();
                adapterStatus.Fill(refDS.tableRequestStatus);
            }
            CollectionViewSource statusVS = this.FindResource("keyStatusVS") as CollectionViewSource;
            statusVS.Source = new System.Data.DataView(refDS.tableRequestStatus, "rowId>49", "rowId", System.Data.DataViewRowState.CurrentRows);
            if (refDS.tableParcelType.Count == 0)
            {
                ReferenceDSTableAdapters.ParcelTypeAdapter parceltypeAdapter = new ReferenceDSTableAdapters.ParcelTypeAdapter();
                parceltypeAdapter.Fill(refDS.tableParcelType);
            }
            CollectionViewSource typeVS = this.FindResource("keyTypeVS") as CollectionViewSource;
            typeVS.Source = new System.Data.DataView(refDS.tableParcelType);
            //if (refDS.tableGoodsType.Count == 0)
            //{
            //    ReferenceDSTableAdapters.GoodsTypeAdapter goodstypeadapter = new ReferenceDSTableAdapters.GoodsTypeAdapter();
            //    goodstypeadapter.Fill(refDS.tableGoodsType);
            //}
            CollectionViewSource goodsVS = this.FindResource("keyGoodsVS") as CollectionViewSource;
            ListCollectionView goods = new ListCollectionView(CustomBrokerWpf.References.GoodsTypesParcel);
            goods.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            goodsVS.Source = CustomBrokerWpf.References.GoodsTypesParcel;
            goodsVS.View.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            mainDataRefresh();
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
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                thisfilter.RemoveCurrentWhere();
            }
        }

        private void mainDataRefresh()
        {
            try
            {
                KirillPolyanskiy.CustomBrokerWpf.ParcelDSTableAdapters.ParcelAdapter parcelAdapter = new KirillPolyanskiy.CustomBrokerWpf.ParcelDSTableAdapters.ParcelAdapter();
                Window ObjectWin = null;
                foreach (Window item in this.OwnedWindows)
                {
                    if (item.Name == "winParcel") ObjectWin = item;
                }
                parcelDataGrid.ItemsSource = null;
                if (ObjectWin != null) (ObjectWin as ParcelWin).mainGrid.DataContext = null;
                parcelAdapter.Fill(parcelDS.tableParcel, thisfilter.FilterWhereId);
                parcelDS.tableParcel.DefaultView.Sort = "sortnumber DESC";
                parcelDataGrid.ItemsSource = parcelDS.tableParcel.DefaultView;
                if (ObjectWin != null) (ObjectWin as ParcelWin).mainGrid.DataContext = parcelDS.tableParcel.DefaultView;
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
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                if (parcelDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
                {
                    ParcelDSTableAdapters.ParcelAdapter parcelAdapter = new ParcelDSTableAdapters.ParcelAdapter();
                    DirectoryInfo dir = new DirectoryInfo("E:\\Счета");
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
                        }
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
                                        parceldir.MoveTo(dir.FullName + "\\" + prow.Field<string>("fullNumber", DataRowVersion.Current));//+ prow.docdirpath.Substring(prow.docdirpath.Length - 5);
                                    else
                                        dir.CreateSubdirectory(prow.fullNumber); //+ prow.docdirpath.Substring(prow.docdirpath.Length - 5)
                                    prow.docdirpath = prow.fullNumber; //+ prow.docdirpath.Substring(prow.docdirpath.Length - 5)
                                    prow.EndEdit();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Не удалось переименовать папку для документов Доставки!\n\n" + ex.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                            }
                        }
                        parcelAdapter.Update(rows);
                    }
                    parcelAdapter.Update(parcelDS.tableParcel);
                    isSuccess = true;
                }
                else
                {
                    DataGridRow item;
                    for (int i = 0; i < this.parcelDataGrid.Items.Count; i++)
                    {
                        item = (DataGridRow)this.parcelDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                        if ((item is DataGridRow) && Validation.GetHasError(item))
                        {
                            this.parcelDataGrid.ScrollIntoView(item.Item);
                            MessageBox.Show(Validation.GetErrors(item)[0].ErrorContent.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                    }
                }
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
                //if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                //{
                //    isSuccess = SaveChanges();
                //}
            }
            return isSuccess;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.parcelDataGrid.ItemsSource) as BindingListCollectionView;
            this.parcelDataGrid.CurrentItem = view.AddNew();
            ParcelItem_Click(this, new RoutedEventArgs());
        }
        private void ParcelItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.parcelDataGrid.CurrentItem != null)
            {
                parcelDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                if (!this.parcelDataGrid.CurrentCell.IsValid) //для обновления Grid
                {
                    if (!this.parcelDataGrid.IsFocused) this.parcelDataGrid.Focus();
                    this.parcelDataGrid.CurrentCell = new DataGridCellInfo(this.parcelDataGrid.CurrentItem, this.parcelDataGrid.Columns[0]);
                }
                Window ObjectWin = null;
                foreach (Window item in this.OwnedWindows)
                {
                    if (item.Name == "winParcel") ObjectWin = item;
                }
                if (ObjectWin == null)
                {
                    ObjectWin = new ParcelWin();
                    ObjectWin.Owner = this;
                    (ObjectWin as ParcelWin).DS = parcelDS;
                    (ObjectWin as ParcelWin).Filter = this.thisfilter;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }

                //ParcelItemWin newWin = new ParcelItemWin();
                //newWin.Owner = this;

                //newWin.mainGrid.DataContext = this.parcelDataGrid.CurrentItem;
                //newWin.Show();
            }
        }
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            bool isReject = false;
            if (this.parcelDataGrid.SelectedItem is DataRowView & this.parcelDataGrid.SelectedItems.Count == 1)
            {
                if (MessageBox.Show("Отменить несохраненные изменения в перевозке?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    if ((this.parcelDataGrid.SelectedItem as DataRowView).IsEdit | (this.parcelDataGrid.SelectedItem as DataRowView).IsNew)
                    {
                        this.parcelDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                        this.parcelDataGrid.CancelEdit(DataGridEditingUnit.Row);
                    }
                    else
                    {
                        ParcelDS.tableParcelRow prow = (this.parcelDataGrid.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow;
                        DataRow[] rrows = parcelDS.tableParcelRequest.Select("parcel=" + prow.parcelId.ToString());
                        foreach (DataRow rrow in rrows)
                            rrow.RejectChanges();
                        prow.RejectChanges();
                    }
                }
            }
            else
            {
                if (MessageBox.Show("Отменить все несохраненные изменения?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    this.parcelDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                    this.parcelDataGrid.CancelEdit(DataGridEditingUnit.Row);
                    parcelDS.tableParcel.RejectChanges();
                    parcelDS.tableParcelRequest.RejectChanges();
                }
            }
            if (isReject)
            {
                PopupText.Text = "Изменения отменены";
                popInf.PlacementTarget = sender as UIElement;
                popInf.IsOpen = true;
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
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) mainDataRefresh();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mainDataGrid_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Input.RoutedCommand com = e.Command as System.Windows.Input.RoutedCommand;
            if (com != null)
            {
                if (com == ApplicationCommands.Delete && this.parcelDataGrid.SelectedItems.Count > 0)
                {
                    e.Handled = !(MessageBox.Show("Удалить выделенные строки?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
                }
            }
        }
        private void mainDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
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
                if (!SaveChanges())
                    MessageBox.Show("Применение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    thisfilter = value;
                    mainDataRefresh();
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

    }
}
