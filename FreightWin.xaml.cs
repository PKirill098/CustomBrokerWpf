using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для FreightWin.xaml
    /// </summary>
    public partial class FreightWin : Window
    {
        FreightDS meDS;
        private int freightId;
        internal int FreightId
        { set { freightId = value; } get { return freightId; } }
        RequestDS.tableRequestRow requestRow;
        internal RequestDS.tableRequestRow RequestRow { set { requestRow = value; } get { return requestRow; } }

        public FreightWin()
        {
            InitializeComponent();
            meDS = new FreightDS();
        }

        private void winFreight_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refDS.tableForwarder.Count == 0)
            {
                ReferenceDSTableAdapters.ForwarderAdapter adapterStore = new ReferenceDSTableAdapters.ForwarderAdapter();
                adapterStore.Fill(refDS.tableForwarder);
            }
            forwarderComboBox.ItemsSource = new System.Data.DataView(refDS.tableForwarder, string.Empty, "itemName", System.Data.DataViewRowState.CurrentRows);
            shippingComboBox.ItemsSource = new System.Data.DataView(meDS.tableAgentAddress);
            contactComboBox.ItemsSource = new System.Data.DataView(meDS.tableAgentContact);
            agenDataLoad(false);
            //Binding goodsBinding = new Binding();
            ////goodsBinding.Source=meDS.tableFreight.DefaultView;
            //goodsBinding.Converter = new ChildRelationConverter();
            //goodsBinding.ConverterParameter = "tableFreight_FreightGoods_sp";
            //goodsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, goodsBinding);
            DataLoad();
            Binding sumBinding = new Binding();
            sumBinding.Source = requestRow;
            sumBinding.Path = new PropertyPath("goodValue");
            sumBinding.StringFormat = "N";
            sumBinding.NotifyOnValidationError = true;
            sumBinding.ValidatesOnExceptions = true;
            sumBinding.TargetNullValue = string.Empty;
            goodValueTextBox.SetBinding(TextBox.TextProperty, sumBinding);
            mainGrid.DataContext = meDS.tableFreight.DefaultView;
        }
        private void winFreight_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить фрахт?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(meDS.tableFreight.DefaultView) as BindingListCollectionView;
                if (view.CurrentItem != null) (view.CurrentItem as System.Data.DataRowView).Delete();
                this.Close();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }
        private void agentComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AgentOpen();
        }
        private void agentButton_Click(object sender, RoutedEventArgs e)
        {
            AgentOpen();
        }
        private void agentrefreshButton_Click(object sender, RoutedEventArgs e)
        {
            agenDataLoad(true);
        }

        private void DataLoad()
        {
            try
            {
                goodsDataGrid.ItemsSource = null;
                //Binding goodsBinding = (goodsDataGrid as FrameworkElement).GetBindingExpression(DataGrid.ItemsSourceProperty).ParentBinding;
                //BindingOperations.ClearBinding(goodsDataGrid, DataGrid.ItemsSourceProperty);
                FreightDSTableAdapters.FreightAdapter freightAdapter = new FreightDSTableAdapters.FreightAdapter();
                freightAdapter.Fill(meDS.tableFreight, freightId);
                FreightDSTableAdapters.FreightGoodsAdapter goodsAdapter = new FreightDSTableAdapters.FreightGoodsAdapter();
                goodsAdapter.Fill(meDS.tableFreightGoods, freightId);
                if (meDS.tableFreight.Count == 0) (CollectionViewSource.GetDefaultView(meDS.tableFreight.DefaultView) as BindingListCollectionView).AddNew();
                BindingListCollectionView view=CollectionViewSource.GetDefaultView(meDS.tableFreight.DefaultView) as BindingListCollectionView;
                view.MoveCurrentToFirst();
                //goodsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, goodsBinding);
                goodsDataGrid.ItemsSource = meDS.tableFreightGoods.DefaultView;
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
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {

                if (this.requestRow.RowState != System.Data.DataRowState.Deleted)
                {
                    BindingListCollectionView view;
                    view = CollectionViewSource.GetDefaultView(meDS.tableFreight.DefaultView) as BindingListCollectionView;

                    IInputElement fcontrol = FocusManager.GetFocusedElement(this);
                    if (fcontrol is TextBox && this.requestRow.RowState != DataRowState.Detached)
                    {
                        BindingExpression be;
                        be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                        if (be != null)
                        {
                            //DataRow row = (view.CurrentItem as DataRowView).Row;
                            //DateTime dt;
                            //bool isDirty = false;
                            //switch (be.ParentBinding.Path.Path)
                            //{
                            //    case "freightDate":
                            //    case "sendingdate":
                            //    case "arrivaldate":
                            //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !DateTime.TryParse((fcontrol as TextBox).Text, out dt) || row.Field<DateTime>(be.ParentBinding.Path.Path) != dt;
                            //        break;
                            //    case "freightNote":
                            //    case "goodValueTextBox":
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
                    this.goodsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                    view = CollectionViewSource.GetDefaultView(goodsDataGrid.ItemsSource) as BindingListCollectionView;
                    if (view.IsAddingNew) view.CommitNew();
                    if (view.IsEditingItem) view.CommitEdit();
                    int countUpdate;
                    FreightDSTableAdapters.FreightAdapter freightAdapter = new FreightDSTableAdapters.FreightAdapter();
                    freightAdapter.Update(meDS.tableFreight);
                    FreightDSTableAdapters.FreightGoodsAdapter goodsAdapter = new FreightDSTableAdapters.FreightGoodsAdapter();
                    countUpdate=goodsAdapter.Update(meDS.tableFreightGoods);

                    if (meDS.tableFreight.Count == 0)
                    {
                        if (!this.requestRow.IsfreightNull()) this.requestRow.SetfreightNull();
                        this.freightId = 0;
                    }
                    else
                    {
                        if (this.requestRow.IsfreightNull() || (this.requestRow.freight != meDS.tableFreight[0].freightId)) this.requestRow.freight = meDS.tableFreight[0].freightId;
                        if ((this.agentComboBox.SelectedValue!=null) && (this.requestRow.IsagentIdNull() || (this.requestRow.agentId != (int)this.agentComboBox.SelectedValue))) this.requestRow.agentId = (int)this.agentComboBox.SelectedValue;
                        if (this.requestRow.IsforwarderNull() || (!this.requestRow.forwarder.Equals(this.forwarderComboBox.Text))) this.requestRow.forwarder = this.forwarderComboBox.Text;
                        if (countUpdate > 0)
                        {
                            decimal w = 0, v = 0; byte c = 0;
                            foreach (FreightDS.tableFreightGoodsRow row in meDS.tableFreightGoods)
                            {
                                if (!row.IscellnumberNull()) c = (byte)(c + row.cellnumber);
                                if (!row.IsgrossweightNull()) w = w + row.grossweight;
                                if (!row.IsvolumeNull()) v = v + row.volume;
                            }
                            if (c != 0) this.requestRow.cellNumber = c;
                            if (w != 0) this.requestRow.officialWeight = w;
                            if (v != 0) this.requestRow.volume = v;
                        }
                        this.freightId = meDS.tableFreight[0].freightId;
                    }
                    isSuccess = true;
                }
           }
            catch (Exception ex)
            {
                if (ex is System.Data.NoNullAllowedException)
                {
                    if (ex.Message.IndexOf("forwarderId") > -1) MessageBox.Show("Необходимо указать Экспедитора!", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (ex.Message.IndexOf("sendingdate") > -1) MessageBox.Show("Необходимо указать Желаемую дату отправки!", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    else MessageBox.Show(ex.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (ex is System.Data.SqlClient.SqlException) 
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

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 & e.ChangedButton == MouseButton.Left) AgentOpen();
        }
        private void AgentOpen()
        {
            if (this.agentComboBox.Text.Length > 0)
            {
                AgentWin agentWin = new AgentWin();
                agentWin.Show();
                agentWin.AgentNameList.Text = this.agentComboBox.Text;
            }
        }
        private void agentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            agenDataLoad(false);
        }
        private void agenDataLoad(bool isrefresh)
        {
            if (agentComboBox.SelectedValue!=null)
            {
                Binding shipBinding = (shippingComboBox as FrameworkElement).GetBindingExpression(ComboBox.SelectedValueProperty).ParentBinding;
                Binding contactBinding = (contactComboBox as FrameworkElement).GetBindingExpression(ComboBox.SelectedValueProperty).ParentBinding;
                if (isrefresh)
                {
                    BindingOperations.ClearBinding(shippingComboBox, ComboBox.SelectedValueProperty);
                    BindingOperations.ClearBinding(contactComboBox, ComboBox.SelectedValueProperty);
                }
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(shippingComboBox.ItemsSource) as BindingListCollectionView;
                using (view.DeferRefresh())
                {
                    try
                    {
                        int agentid = (int)agentComboBox.SelectedValue;
                        FreightDSTableAdapters.AgentAddressAdapter addressAdapter = new FreightDSTableAdapters.AgentAddressAdapter();
                        addressAdapter.Fill(meDS.tableAgentAddress, agentid);
                        FreightDSTableAdapters.AgentContactAdapter contactAdapter = new FreightDSTableAdapters.AgentContactAdapter();
                        contactAdapter.Fill(meDS.tableAgentContact, agentid);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (isrefresh)
                {
                    shippingComboBox.SetBinding(ComboBox.SelectedValueProperty, shipBinding);
                    contactComboBox.SetBinding(ComboBox.SelectedValueProperty, contactBinding);
                }
            }
            else
            {
                meDS.tableAgentAddress.Clear();
            }
        }

        private void contactComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contactPointDataLoad();
        }
        private void contactPointDataLoad()
        {
            if (contactComboBox.SelectedItem != null)
            {
                try
                {
                    int contactid = ((contactComboBox.SelectedItem as System.Data.DataRowView).Row as FreightDS.tableAgentContactRow).ContactID;
                    ContactPointDataGrid.ItemsSource = null;
                    FreightDSTableAdapters.ContactPointAdapter pointAdapter = new FreightDSTableAdapters.ContactPointAdapter();
                    pointAdapter.Fill(meDS.tableContactPoint, contactid);
                    ContactPointDataGrid.ItemsSource = new System.Data.DataView(meDS.tableContactPoint);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void toExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(Environment.CurrentDirectory + @"\Templates\Freight.xltx")) return;
            SaveChanges();

            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            try
            {
                int r; int ir = 0;
                FreightDS.tableFreightRow freightRow = meDS.tableFreight[0] as FreightDS.tableFreightRow;
                Excel.Workbook exWb = exApp.Workbooks.Add(Environment.CurrentDirectory + @"\Templates\Freight.xltx");
                Excel.Worksheet exWh = exWb.Sheets[1];
                exWh.Cells[7, 9] = freightRow.freightId;
                exWh.Cells[7, 14] = freightRow.freightDate;
                if (forwarderComboBox.SelectedItem != null)
                {
                    ReferenceDS.tableForwarderRow forwarderRow = (forwarderComboBox.SelectedItem as DataRowView).Row as ReferenceDS.tableForwarderRow;
                    exWh.Cells[9, 3] = forwarderRow.itemName;
                    if (!forwarderRow.IspersonNull()) exWh.Cells[9, 15] = forwarderRow.person;
                }
                if (!requestRow.IsloadDescriptionNull())
                {
                    exWh.Cells[19, 8] = requestRow.loadDescription;
                }
                exWh.Cells[21, 8] = DateTime.Today.ToShortDateString();
                r = 24;
                int m = 0; decimal w = 0;
                foreach (DataRow row in meDS.tableFreightGoods.Rows)
                {
                    FreightDS.tableFreightGoodsRow goodsRow=row as FreightDS.tableFreightGoodsRow;
                    if (r > 36)
                    {
                        Excel.Range range= exWh.Rows[r-2, r-1];
                        range.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                        exWh.Range[exWh.Cells[r, 3],exWh.Cells[r+1, 7]].Merge(false);
                        exWh.Range[exWh.Cells[r, 8], exWh.Cells[r + 1, 16]].Merge(false);
                        exWh.Range[exWh.Cells[r, 17], exWh.Cells[r + 1, 22]].Merge(false);
                        exWh.Range[exWh.Cells[r, 23], exWh.Cells[r + 1, 28]].Merge(false);
                        ir=ir+2;
                    }
                    if (!goodsRow.IscellnumberNull())
                    {
                        exWh.Cells[r, 3] = goodsRow.cellnumber;
                        m = m+goodsRow.cellnumber;
                    }
                    if (!goodsRow.IsvolumeNull()) exWh.Cells[r, 8] = goodsRow.volume;
                    if (!goodsRow.IsgrossweightNull())
                    {
                        exWh.Cells[r, 17] = goodsRow.grossweight;
                        w = w+goodsRow.grossweight;
                    }
                    if (!goodsRow.IspackagetypeNull()) exWh.Cells[r, 23] = goodsRow.packagetype;
                    r=r+2;
                }
                exWh.Cells[39+ir, 3] = m;
                exWh.Cells[39 + ir, 17] = w;
                if (!requestRow.IsgoodValueNull()) exWh.Cells[39 + ir, 28] = requestRow.goodValue;
                StringBuilder strBild=new StringBuilder();
                if (!requestRow.IscustomerNameNull()) strBild.Append(requestRow.customerName);
                if (!freightRow.IsfreightNoteNull())
                {
                    strBild.Append(" ");
                    strBild.Append(freightRow.freightNote);
                }
                exWh.Cells[41 + ir, 15] = strBild.ToString();
                exWh.Cells[89 + ir, 1] = freightRow.insurance;
                exWh.Cells[59 + ir, 9] = this.agentComboBox.Text;
                exWh.Cells[60 + ir, 9] = this.shippingComboBox.Text;
                strBild.Clear();
                ReferenceDS refDS= this.FindResource("keyReferenceDS") as ReferenceDS;
                if (refDS.ContactPointTypeTb.Count == 0)
                {
                    ReferenceDSTableAdapters.ContactPointTypeAdapter pointtypeAdapter = new ReferenceDSTableAdapters.ContactPointTypeAdapter();
                    pointtypeAdapter.Fill(refDS.ContactPointTypeTb);
                }
                foreach (DataRow row in meDS.tableContactPoint.Rows)
                {
                    FreightDS.tableContactPointRow pointRow = row as FreightDS.tableContactPointRow;
                    ReferenceDS.ContactPointTypeTbRow pointtypeRow= refDS.ContactPointTypeTb.FindBypointName(pointRow.PointName);
                    if (pointtypeRow != null && (!pointtypeRow.IspointtemplateNull() && pointtypeRow.pointtemplate=="telnumber"))
                    {
                        strBild.Append(", ");
                        strBild.Append(pointRow.PointValue);
                    }
                }
                if(strBild.Length>0) strBild.Remove(0,2);
                exWh.Cells[61 + ir, 9] = strBild.ToString();
                exWh.Cells[62 + ir, 11] = this.contactComboBox.Text;
                if (!freightRow.IssendingdateNull()) exWh.Cells[65 + ir, 11] = freightRow.sendingdate.ToShortDateString();
                if (!freightRow.IsarrivaldateNull()) exWh.Cells[65 + ir, 24] = freightRow.arrivaldate.ToShortDateString();
                exWh.Cells[82 + ir, 20] = this.forwarderComboBox.Text;

                exApp.DisplayAlerts = true;
                exApp.ScreenUpdating = true;
                exApp.Visible = true;
            }
            catch (Exception ex)
            {
                if (exApp != null)
                {
                    foreach (Excel.Workbook itemBook in exApp.Workbooks)
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
    }
}
