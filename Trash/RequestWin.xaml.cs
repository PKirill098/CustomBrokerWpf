using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.IO;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class RequestWin : Window, System.ComponentModel.INotifyPropertyChanged
    {
        decimal totalOldValue = 0;
        private CustomBrokerWpf.SQLFilter thisfilter = new SQLFilter("request", "AND");
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
            get { return thisfilter; }
            set
            {
                if (!SaveChanges())
                    MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    thisfilter = value;
                    mainDataRefresh();
                }
            }
        }
        internal void runFilter()
        {
            if (!SaveChanges())
                MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                if (item.Name == "winRequestFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new RequestFilterWin();
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

        public RequestWin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thisStoragePointValidationRule.CurrentDataGrid = this.mainDataGrid;
            DataLoad();
        }
        private void DataLoad()
        {
            try
            {
                KirillPolyanskiy.CustomBrokerWpf.RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.FindResource("requestDS")));
                RequestDSTableAdapters.tableAgentNameAdapter thisAgentNameAdapter = new RequestDSTableAdapters.tableAgentNameAdapter();
                thisAgentNameAdapter.Fill(requestDS.tableAgentName);
                RequestDSTableAdapters.tableCustomerNameAdapter thisCustomerNameAdapter = new RequestDSTableAdapters.tableCustomerNameAdapter();
                thisCustomerNameAdapter.Fill(requestDS.tableCustomerName);
                ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
                if (refDS.tableRequestStatus.Count == 0)
                {
                    ReferenceDSTableAdapters.RequestStatusAdapter adapterStatus = new ReferenceDSTableAdapters.RequestStatusAdapter();
                    adapterStatus.Fill(refDS.tableRequestStatus);
                }
                CollectionViewSource statusVS = this.FindResource("keyStatusVS") as CollectionViewSource;
                statusVS.Source = new System.Data.DataView(refDS.tableRequestStatus, "rowId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
                if (refDS.tableGoodsType.Count == 0)
                {
                    ReferenceDSTableAdapters.GoodsTypeAdapter adapterGoodsType = new ReferenceDSTableAdapters.GoodsTypeAdapter();
                    adapterGoodsType.Fill(refDS.tableGoodsType);
                }
                CollectionViewSource goodsVS = this.FindResource("keyGoodsTypeVS") as CollectionViewSource;
                goodsVS.Source = new System.Data.DataView(refDS.tableGoodsType, "Iditem>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
                CollectionViewSource storeVS = this.FindResource("keyStoreVS") as CollectionViewSource;
                storeVS.Source = new ListCollectionView(KirillPolyanskiy.CustomBrokerWpf.References.Stores);
                if (refDS.tableForwarder.Count == 0)
                {
                    ReferenceDSTableAdapters.ForwarderAdapter adapterStore = new ReferenceDSTableAdapters.ForwarderAdapter();
                    adapterStore.Fill(refDS.tableForwarder);
                }
                CollectionViewSource forwarderVS = this.FindResource("keyForwarderVS") as CollectionViewSource;
                forwarderVS.Source = new System.Data.DataView(refDS.tableForwarder, "itemId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
                if (refDS.tableParcelType.Count == 0) refDS.ParcelTypeRefresh();
                CollectionViewSource parceltypeVS = this.FindResource("keyParcelTypeVS") as CollectionViewSource;
                parceltypeVS.Source = new System.Data.DataView(refDS.tableParcelType);
                //KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest requestDSRequest_tbTableAdapter = new KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest();
                //requestDSRequest_tbTableAdapter.Fill(requestDS.tableRequest, thisfilter.FilterSQLID);
                //mainGrid.DataContext = requestDS.tableRequest.DefaultView;
                mainDataRefresh();
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
                    DataLoad();
                }
            }
        }
        private void mainDataRefresh()
        {
            try
            {
                BindingListCollectionView view;
                KirillPolyanskiy.CustomBrokerWpf.RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.FindResource("requestDS")));
                view = CollectionViewSource.GetDefaultView(requestDS.tableRequest.DefaultView) as BindingListCollectionView;
                System.ComponentModel.SortDescription[] sortColl = new System.ComponentModel.SortDescription[view.SortDescriptions.Count];
                view.SortDescriptions.CopyTo(sortColl, 0);
                KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest requestAdapter = new KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest();
                mainDataGrid.ItemsSource = null;
                requestAdapter.Fill(requestDS.tableRequest, thisfilter.FilterWhereId);
                mainDataGrid.ItemsSource = requestDS.tableRequest.DefaultView;
                using (view.DeferRefresh())
                {
                    foreach (System.ComponentModel.SortDescription itemsort in sortColl)
                    {
                        view.SortDescriptions.Add(itemsort);
                        foreach (DataGridColumn colmn in mainDataGrid.Columns)
                        {
                            if (colmn.SortMemberPath.Equals(itemsort.PropertyName))
                            {
                                colmn.SortDirection = itemsort.Direction;
                                break;
                            }
                        }
                    }
                }
                totalDataRefresh();
                string uribitmap;
                if (thisfilter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
                else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
                System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
                (FilterButton.Content as Image).Source = bi3;
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
                    DataLoad();
                }
            }
        }
        private void FilterLoad()
        {
            using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                try
                {
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "dbo.UserFilter_sp";
                    SqlParameter winname = new SqlParameter("@winName", this.Name);
                    com.Parameters.Add(winname);
                    System.Xml.XmlReader reader = com.ExecuteXmlReader();

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
                        DataLoad();
                    }
                }
                finally { con.Close(); }
            }
        }
        private bool SaveChanges()
        {
            bool isSuccess = false;
            RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.FindResource("requestDS")));
            try
            {
                mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest requestDSRequest_tbTableAdapter = new KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest();
                requestDSRequest_tbTableAdapter.Adapter.ContinueUpdateOnError = false;
                requestDSRequest_tbTableAdapter.Update(requestDS.tableRequest);
                //DataRow[] errrows = requestDS.tableRequest.GetErrors();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999)
                    {
                        switch (err.Number)
                        {
                            case 50000:
                                MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                            case 50001:
                                try
                                {
                                    DataRow[] errrows = requestDS.tableRequest.GetErrors();
                                    RequestDS.tableRequestRow requestrow = errrows[0] as RequestDS.tableRequestRow;
                                    RequestConflictResolution res = new RequestConflictResolution(requestrow);
                                    int newstamp = res.isCheckedRow();
                                    if (newstamp != 0)
                                    {
                                        requestrow.ClearErrors();
                                        requestrow.stamp = newstamp;
                                        requestrow.EndEdit();
                                        return SaveChanges();
                                    }
                                    else
                                    {
                                        MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                catch (Exception ep)
                                {
                                    MessageBox.Show(ep.Message + "\n" + ep.Source, "Разрешение конфликта записи", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                break;
                        }
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
                //if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                //{
                //    isSuccess = SaveChanges();
                //}
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
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                thisfilter.RemoveCurrentWhere();
            }
        }
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            bool isReject=false;
            KirillPolyanskiy.CustomBrokerWpf.RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.FindResource("requestDS")));
            if (this.mainDataGrid.SelectedItem is DataRowView & this.mainDataGrid.SelectedItems.Count == 1)
            {
                if (MessageBox.Show("Отменить несохраненные изменения в заявке?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    if ((this.mainDataGrid.SelectedItem as DataRowView).IsEdit | (this.mainDataGrid.SelectedItem as DataRowView).IsNew)
                    {
                        this.mainDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                        this.mainDataGrid.CancelEdit(DataGridEditingUnit.Row);
                    }
                    else (this.mainDataGrid.SelectedItem as DataRowView).Row.RejectChanges();
                }
            }
            else
            {
                if (MessageBox.Show("Отменить все несохраненные изменения?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    this.mainDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                    this.mainDataGrid.CancelEdit(DataGridEditingUnit.Row);
                    requestDS.tableRequest.RejectChanges();
                }
            }
            if (isReject)
            {
                    PopupText.Text = "Изменения отменены";
                    popInf.PlacementTarget = sender as UIElement;
                    popInf.IsOpen = true;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges())
            {
                PopupText.Text = "Изменения сохранены";
                popInf.PlacementTarget = sender as UIElement;
                popInf.IsOpen = true;
            }
        }

        private void HistoryOpen_Click(object sender, RoutedEventArgs e)
        {
            RequestHistoryWin newHistory = new RequestHistoryWin();
            if ((sender as Button).Tag is RequestVM)
            {
                Request request = ((sender as Button).Tag as RequestVM).DomainObject;
                RequestHistoryViewCommand cmd = new RequestHistoryViewCommand(request);
                newHistory.DataContext = cmd;
            }
            newHistory.Owner = this;
            newHistory.Show();
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

        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void FreightColumn_Click(object sender, RoutedEventArgs e)
        {
            mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            if (mainDataGrid.CurrentItem is DataRowView)
            {
                RequestDS.tableRequestRow row = (mainDataGrid.CurrentItem as DataRowView).Row as RequestDS.tableRequestRow;
                FreightWin winFreight = null;
                foreach (Window frwin in this.OwnedWindows)
                {
                    if (frwin.Name == "winFreight")
                    {
                        if ((frwin as FreightWin).RequestRow.requestId == row.requestId) winFreight = frwin as FreightWin;
                    }
                }
                if (winFreight == null)
                {
                    foreach (Window item in this.OwnedWindows)
                    {
                        if (item.Name == "winRequestItem")
                        {
                            if ((item as RequestItemWin).mainGrid.DataContext.Equals(this.mainDataGrid.CurrentItem))
                            {
                                foreach (Window frwin in item.OwnedWindows)
                                {
                                    if (frwin.Name == "winFreight")
                                    {
                                        if ((frwin as FreightWin).RequestRow.requestId == row.requestId) winFreight = frwin as FreightWin;
                                    }
                                }
                            }
                        }
                    }
                }
                if (winFreight == null)
                {
                    winFreight = new FreightWin();
                    if (row.isfreight) winFreight.FreightId = row.freight;
                    else winFreight.FreightId = 0;
                    RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.FindResource("requestDS")));
                    winFreight.agentComboBox.ItemsSource = new System.Data.DataView(requestDS.tableAgentName, string.Empty, "agentName", System.Data.DataViewRowState.CurrentRows);
                    if (!row.IsagentIdNull()) winFreight.agentComboBox.SelectedValue = row.agentId;
                    winFreight.RequestRow = row;
                    winFreight.Owner = this;
                    winFreight.Show();
                }
                else
                {
                    winFreight.Activate();
                    if (winFreight.WindowState == WindowState.Minimized) winFreight.WindowState = WindowState.Normal;
                }
            }
        }

        private void mainDataGrid_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Input.RoutedCommand com = e.Command as System.Windows.Input.RoutedCommand;
            if (com != null)
            {
                if (com == ApplicationCommands.Delete && this.mainDataGrid.SelectedItems.Count > 0)
                {
                    e.Handled = !(MessageBox.Show("Удалить выделенные строки?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
                }
            }
        }

        #region Data Grid Total Sum
        private void totalDataRefresh()
        {
            int totalCellNumber = 0, totalCount = 0;
            decimal totalVolume = 0, totalOfficialWeight = 0, totalActualWeight = 0, totalGoodValue = 0;
            if (this.mainDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < this.mainDataGrid.SelectedItems.Count; i++)
                {
                    if (this.mainDataGrid.SelectedItems[i] is DataRowView)
                    {
                        totalCount++;
                        RequestDS.tableRequestRow row = (this.mainDataGrid.SelectedItems[i] as DataRowView).Row as RequestDS.tableRequestRow;
                        if (!row.IscellNumberNull()) totalCellNumber = totalCellNumber + row.cellNumber;
                        if (!row.IsvolumeNull()) totalVolume = totalVolume + row.volume;
                        if (!row.IsofficialWeightNull()) totalOfficialWeight = totalOfficialWeight + row.officialWeight;
                        if (!row.IsactualWeightNull()) totalActualWeight = totalActualWeight + row.actualWeight;
                        if (!row.IsgoodValueNull()) totalGoodValue = totalGoodValue + row.goodValue;
                    }
                }
            }
            else
            {
                KirillPolyanskiy.CustomBrokerWpf.RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.FindResource("requestDS")));
                DataView view = requestDS.tableRequest.DefaultView;
                totalCount = view.Count;
                foreach (DataRowView viewrow in view)
                {
                    RequestDS.tableRequestRow row = viewrow.Row as RequestDS.tableRequestRow;
                    if (!row.IscellNumberNull()) totalCellNumber = totalCellNumber + row.cellNumber;
                    if (!row.IsvolumeNull()) totalVolume = totalVolume + row.volume;
                    if (!row.IsofficialWeightNull()) totalOfficialWeight = totalOfficialWeight + row.officialWeight;
                    if (!row.IsactualWeightNull()) totalActualWeight = totalActualWeight + row.actualWeight;
                    if (!row.IsgoodValueNull()) totalGoodValue = totalGoodValue + row.goodValue;
                }
            }
            TotalCountTextBox.Text = totalCount.ToString();
            TotalcellNumberTextBox.Text = totalCellNumber.ToString();
            TotalVolumeTextBox.Text = totalVolume.ToString("N4");
            TotalOfficialWeightTextBox.Text = totalOfficialWeight.ToString("N4");
            TotalActualWeightTextBox.Text = totalActualWeight.ToString("N4");
            TotalGoodValueTextBox.Text = totalGoodValue.ToString("N4");
        }
        private void mainDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string col = e.Column.Header.ToString();
            if (col == "Кол-во мест" | col == "Объем, м3" | col == "Вес по док, кг" | col == "Вес факт, кг" | col == "Стоимость товара, Е")
            {
                decimal.TryParse((e.Column.GetCellContent(e.Row) as TextBlock).Text, out totalOldValue);
            }
        }
        private void mainDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            decimal newvalue = 0;
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                RequestDS.tableRequestRow row = (e.Row.Item as DataRowView).Row as RequestDS.tableRequestRow;
                switch (e.Column.Header.ToString())
                {
                    case "Кол-во мест":
                        if (!row.IscellNumberNull()) newvalue = row.cellNumber; else newvalue = 0;
                        TotalcellNumberTextBox.Text = (decimal.Parse(TotalcellNumberTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Объем, м3":
                        if (!row.IsvolumeNull()) newvalue = row.volume; else newvalue = 0;
                        TotalVolumeTextBox.Text = (decimal.Parse(TotalVolumeTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Вес по док, кг":
                        if (!row.IsofficialWeightNull()) newvalue = row.officialWeight; else newvalue = 0;
                        TotalOfficialWeightTextBox.Text = (decimal.Parse(TotalOfficialWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Вес факт, кг":
                        if (!row.IsactualWeightNull()) newvalue = row.actualWeight; else newvalue = 0;
                        TotalActualWeightTextBox.Text = (decimal.Parse(TotalActualWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Стоимость товара, Е":
                        if (!row.IsgoodValueNull()) newvalue = row.goodValue; else newvalue = 0;
                        TotalGoodValueTextBox.Text = (decimal.Parse(TotalGoodValueTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                }
            }
            else
            {
                switch (e.Column.Header.ToString())
                {
                    case "Кол-во мест":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            TotalcellNumberTextBox.Text = (decimal.Parse(TotalcellNumberTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Объем, м3":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            TotalVolumeTextBox.Text = (decimal.Parse(TotalVolumeTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Вес по док, кг":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            TotalOfficialWeightTextBox.Text = (decimal.Parse(TotalOfficialWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Вес факт, кг":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            TotalActualWeightTextBox.Text = (decimal.Parse(TotalActualWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Стоимость товара, Е":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            TotalGoodValueTextBox.Text = (decimal.Parse(TotalGoodValueTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                }
            }
        }
        private void mainDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                totalDataRefresh();
            }
        }
        private void mainDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if ((e.Row.Item is DataRowView) && (e.Row.Item as DataRowView).Row.RowState == DataRowState.Detached)
            {
                TotalCountTextBox.Text = (int.Parse(TotalCountTextBox.Text) + 1).ToString();
            }
        }
        private void mainDataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if ((e.Row.Item is DataRowView) && ((e.Row.Item as DataRowView).Row.RowState == DataRowState.Detached | (e.Row.Item as DataRowView).Row.RowState == DataRowState.Deleted))
            {
                totalDataRefresh();
            }
        }
        private void mainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == mainDataGrid) totalDataRefresh();
        }
        #endregion

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges()|| MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?","Обновление данных",MessageBoxButton.YesNo,MessageBoxImage.Question)!=MessageBoxResult.Yes) DataLoad();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.mainDataGrid.ItemsSource) as BindingListCollectionView;
            this.mainDataGrid.CurrentItem = view.AddNew();

            RequestItem_Click(this, new RoutedEventArgs());
        }
        private void RequestItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainDataGrid.CurrentItem != null & mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
            {

                if (!this.mainDataGrid.CurrentCell.IsValid) //для обновления Grid
                {
                    if (!this.mainDataGrid.IsFocused) this.mainDataGrid.Focus();
                    this.mainDataGrid.CurrentCell = new DataGridCellInfo(this.mainDataGrid.CurrentItem, this.mainDataGrid.Columns[4]);
                    //this.mainDataGrid.SelectedCells.Add(cellInfo);
                    //this.mainDataGrid.ScrollIntoView(this.mainDataGrid.CurrentItem);
                }
                RequestItemWin newWin = null;
                foreach (Window item in this.OwnedWindows)
                {
                    if (item.Name == "winRequestItem")
                    {
                        if ((item as RequestItemWin).mainGrid.DataContext.Equals(this.mainDataGrid.CurrentItem))
                            newWin = item as RequestItemWin;
                    }
                }
                if (newWin == null)
                {
                    newWin = new RequestItemWin();
                    newWin.Owner = this;
                    ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
                    newWin.statusComboBox.ItemsSource = new System.Data.DataView(refDS.tableRequestStatus, "rowId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
                    newWin.statusComboBox.IsDropDownOpen = false;
                    newWin.goodsComboBox.ItemsSource = new System.Data.DataView(refDS.tableGoodsType, "Iditem>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
                    newWin.parceltypeComboBox.ItemsSource = new System.Data.DataView(refDS.tableParcelType);
                    if (!((this.mainDataGrid.CurrentItem as DataRowView).Row as RequestDS.tableRequestRow).IsfullNumberNull()) { newWin.parceltypeComboBox.IsEnabled = false;}
                    //newWin.forwarderComboBox.ItemsSource = new System.Data.DataView(refDS.tableForwarder, "itemId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
                    //newWin.storeComboBox.ItemsSource = new System.Data.DataView(refDS.tableStore, "storeId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);

                    RequestDS requestDS = ((RequestDS)(this.FindResource("requestDS")));
                    newWin.customerComboBox.ItemsSource = new System.Data.DataView(requestDS.tableCustomerName, string.Empty, "customerName", System.Data.DataViewRowState.CurrentRows);
                    newWin.agentComboBox.ItemsSource = new System.Data.DataView(requestDS.tableAgentName, string.Empty, "agentName", System.Data.DataViewRowState.CurrentRows);
                    newWin.mainGrid.DataContext = this.mainDataGrid.CurrentItem;
                    newWin.RequestItemViewCommand = new Classes.Domain.RequestItemViewCommand(((this.mainDataGrid.CurrentItem as DataRowView).Row as CustomBrokerWpf.RequestDS.tableRequestRow).requestId);
                    newWin.thisStoragePointValidationRule.RequestId = ((this.mainDataGrid.CurrentItem as DataRowView).Row as CustomBrokerWpf.RequestDS.tableRequestRow).requestId;
                    newWin.Show();
                }
                else
                {
                    newWin.Activate();
                    if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
                }
            }
        }

        private void SortAZButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainDataGrid.CurrentColumn != null)
            {
                try
                {
                    BindingListCollectionView view = CollectionViewSource.GetDefaultView(mainDataGrid.ItemsSource) as BindingListCollectionView;
                    System.ComponentModel.SortDescription newsort = new System.ComponentModel.SortDescription(mainDataGrid.CurrentColumn.SortMemberPath, System.ComponentModel.ListSortDirection.Ascending);
                    view.SortDescriptions.Insert(0, newsort);
                    mainDataGrid.CurrentColumn.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Невозможно изменить сортировку во время редактирования данных. \n Завершите редактирование строки.", "Сортировка", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void SortZAButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainDataGrid.CurrentColumn != null)
                {
                    BindingListCollectionView view = CollectionViewSource.GetDefaultView(mainDataGrid.ItemsSource) as BindingListCollectionView;
                    System.ComponentModel.SortDescription newsort = new System.ComponentModel.SortDescription(mainDataGrid.CurrentColumn.SortMemberPath, System.ComponentModel.ListSortDirection.Descending);
                    view.SortDescriptions.Insert(0, newsort);
                    mainDataGrid.CurrentColumn.SortDirection = System.ComponentModel.ListSortDirection.Descending;
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Невозможно изменить сортировку во время редактирования данных. \n Завершите редактирование строки.", "Сортировка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void SoprtClean_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(mainDataGrid.ItemsSource) as BindingListCollectionView;
                view.SortDescriptions.Clear();
                foreach (DataGridColumn item in mainDataGrid.Columns)
                {
                    item.SortDirection = null;
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Невозможно изменить сортировку во время редактирования данных. \n Завершите редактирование строки.", "Сортировка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FastFilterRun();
            }
        }
        private void FastFilterRun()
        {
            this.Filter.RemoveCurrentWhere();
            if (ClientFilter.HasValue) this.Filter.SetNumber(this.Filter.FilterWhereId, "customerId",0, ClientFilter.Value.ToString());
            if (!string.IsNullOrEmpty(this.StoragePointFilter)) this.Filter.ConditionValueAdd(this.Filter.ConditionAdd(this.Filter.FilterWhereId, "storagePoint", "="), this.StoragePointFilter, 0);
            if (!ClientFilter.HasValue & string.IsNullOrEmpty(this.StoragePointFilter)) this.Filter.GetDefaultFilter(SQLFilterPart.Where);
            this.runFilter();
        }
        private int? myclientfilter;
        public int? ClientFilter
        {
            set
            {
                myclientfilter = value;
                PropertyChangedNotification("ClientFilter");
            }
            get { return myclientfilter; }
        }
        private string mystoragepointfilter;
        public string StoragePointFilter
        {
            set
            {
                mystoragepointfilter = value;
                PropertyChangedNotification("StoragePointFilter");
            }
            get { return mystoragepointfilter; }
        }
        private void FastFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FastFilterRun();
        }

        //INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        private void ColmarkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainDataGrid.SelectedItems.Count > 0 & e.AddedItems.Count > 0)
            {
                RequestDS.tableRequestRow row;
                foreach (DataRowView viewrow in mainDataGrid.SelectedItems)
                {
                    if (viewrow != mainDataGrid.CurrentItem)
                    {
                        row = viewrow.Row as RequestDS.tableRequestRow;
                        row.colmark = (e.AddedItems[0] as System.Windows.Shapes.Rectangle).Fill.ToString();
                        row.EndEdit();
                    }
                }
            }
        }

    }
}
