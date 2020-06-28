using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using uclib = KirillPolyanskiy.WpfControlLibrary;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Linq;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class ParcelReportFilterWin : Window
    {
        private ManagerDataGridColumnsVM managerColumns;
        private SQLFilter filter;
        private uclib.CheckListBoxVM myclientvm;
        private CLegalCheckListBoxVM myclegalvm;
        private uclib.CheckListBoxVM myagentvm;

        private bool isChanchedNumber;
        private bool isChanchedShipPlanDate;
        private bool isChanchedGoodsType;
        private bool isChanchedStatus;
        private bool isChanchedParcelType;
        private bool isChanchedServiceType;
        private bool isChanchedAgent;
        private bool myclientischanched;
        private bool myclegalischanched;
        private List<SQLFilterValue> myclegalselectvalue;
        private bool ManagerChanged;
        private bool isChanchedManagerGroup;
        private bool isChanchedLorry;
        private bool isChanchedCarrier;
        private bool isChanchedPeriod;
        private bool isChanchedInvoiceDate;
        private bool isChanchedTerminalOut;

        public ParcelReportFilterWin()
        {
            InitializeComponent();
        }
        private void Filter_Loaded(object sender, RoutedEventArgs e)
        {
            managerColumns = (this.Owner as ParcelReportWin).ManagerDataGridColumns;
            ColumnsTabItem.DataContext = managerColumns;

            myclientvm = new uclib.CheckListBoxVM();
            myclientvm.DisplayPath = "Name";
            myclientvm.SearchPath = "Name";
            myclientvm.GetDisplayPropertyValueFunc = (item) => { return (item as Classes.Domain.Customer).Name; };
            myclientvm.ItemsViewFilterDefault = lib.ViewModelViewCommand.ViewFilterDefault;
            myclientvm.SelectedAll = false;
            myclientvm.RefreshIsVisible = false;
            myclientvm.AreaButtonsIsVisible = false;
            CustomerDBM mydbm = new CustomerDBM();
            mydbm.Fill();
            myclientvm.Items = mydbm.Collection;
            this.customerListBox.ListBox.SelectionChanged += this.CustomerListBox_SelectionChanged;
            customerListBox.DataContext = myclientvm;

            myagentvm = new uclib.CheckListBoxVM();
            myagentvm.DisplayPath = "Name";
            myagentvm.SearchPath = "Name";
            myagentvm.GetDisplayPropertyValueFunc = (item) => { return (item as lib.ReferenceSimpleItem).Name; };
            myagentvm.ItemsViewFilterDefault = lib.ViewModelViewCommand.ViewFilterDefault;
            myagentvm.SelectedAll = false;
            myagentvm.RefreshIsVisible = false;
            myagentvm.AreaButtonsIsVisible = false;
            myagentvm.Items = CustomBrokerWpf.References.AgentNames;
            this.agentListBox.ListBox.SelectionChanged += this.agentListBox_SelectionChanged;
            agentListBox.DataContext = myagentvm;

            myclegalvm = new CLegalCheckListBoxVM();
            myclegalvm.DeferredFill = true;
            myclegalvm.ItemsSource = myclientvm.SelectedItems.OfType<Customer>();
            this.CLegalListBox.ListBox.SelectionChanged += this.CLegalListBox_SelectionChanged;
            this.CLegalListBox.DataContext = myclegalvm;

            serviceTypeListBox.ItemsSource = CustomBrokerWpf.References.ServiceTypes;
            ManagerDBM mdbm = new ManagerDBM(); mdbm.Fill();
            ManagerListBox.ItemsSource = mdbm.Collection;
            managerGroupListBox.ItemsSource = CustomBrokerWpf.References.ManagerGroups;

            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            //if (refDS.tableGoodsType.Count == 0) refDS.GoodsTypeRefresh();
            this.goodstypeListBox.ItemsSource = CustomBrokerWpf.References.GoodsTypesParcel;
            if (refDS.tableRequestStatus.Count == 0) refDS.RequestStatusRefresh();
            this.statusListBox.ItemsSource = refDS.tableRequestStatus.DefaultView;
            if (refDS.tableParcelType.Count == 0) refDS.ParcelTypeRefresh();
            this.parcelTypeListBox.ItemsSource = refDS.tableParcelType.DefaultView;

            filter = (this.Owner as ISQLFiltredWindow).Filter;
            Fill();
        }

        private void Filter_Closed(object sender, EventArgs e)
        {
            if (!ColumnsDataGrid.CommitEdit(DataGridEditingUnit.Cell, true)) ColumnsDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            if (!ColumnsDataGrid.CommitEdit(DataGridEditingUnit.Row, true)) ColumnsDataGrid.CancelEdit(DataGridEditingUnit.Row);
            (this.Owner as ISQLFiltredWindow).IsShowFilter = false;
            this.Owner.Activate();
        }

        private void RunFilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IInputElement focelm = FocusManager.GetFocusedElement(this);
                FocusManager.SetFocusedElement(this, RunFilterButton);
                Actualization();
                (this.Owner as ISQLFiltredWindow).RunFilter();
                FocusManager.SetFocusedElement(this, focelm);
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999) MessageBox.Show(err.Message, "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                        {
                            errs.Append(sqlerr.Message + "\n");
                        }
                        MessageBox.Show(errs.ToString(), "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void DefaultFilterButton_Click(object sender, RoutedEventArgs e)
        {
            filter.RemoveCurrentWhere();
            filter.GetDefaultFilter(SQLFilterPart.Where);
            Fill();
        }
        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFilter();
        }
        private void RemoveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            filter.RemoveCurrentWhere();
        }
        private void SaveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Фильтр по умолчанию будет заменён текущим фильтром.\nПродолжить?", "Сохранение фильтра", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
            {
                try
                {
                    Actualization();
                    filter.SetDefaultFilterWhere();
                    PopupText.Text = "Фильтр по умолчанию изменен.";
                    popInf.IsOpen = true;
                }
                catch (Exception ex)
                {
                    if (ex is System.Data.SqlClient.SqlException)
                    {
                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                        if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение фильтра", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Сохранение фильтра", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение фильтра", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
        }

        private void Actualization()
        {
            if (isChanchedPeriod)
            {
                filter.SetDate(filter.FilterWhereId, "Period", "period", startPeriodPicker.SelectedDate, stopPeriodPicker.SelectedDate);
                isChanchedPeriod = false;
            }
            if (isChanchedInvoiceDate)
            {
                filter.SetDate(filter.FilterWhereId, "InvoiceDate", "invoicedate", startInvoicePicker.SelectedDate, stopInvoicePicker.SelectedDate);
                isChanchedInvoiceDate = false;
            }
            if (isChanchedTerminalOut)
            {
                filter.SetDate(filter.FilterWhereId, "TerminalOut", "terminalout", startTerminalOutPicker.SelectedDate, stopTerminalOutPicker.SelectedDate);
                isChanchedTerminalOut = false;
            }
            if (isChanchedShipPlanDate)
            {
                filter.SetDate(filter.FilterWhereId, "ShipPlanDate", "shipplandate", startShipplandatePicker.SelectedDate, stopShipplandatePicker.SelectedDate);
                isChanchedShipPlanDate = false;
            }
            if (isChanchedNumber)
            {
                filter.SetRange(filter.FilterWhereId, "parcelnumber", this.startNumberTextBox.Text, stopNumberTextBox.Text);
                isChanchedNumber = false;
            }
            if (isChanchedCarrier)
            {
                filter.SetString(filter.FilterWhereId, "carrier", carrierTextBox.Text);
                isChanchedCarrier = false;
            }
            if (isChanchedLorry)
            {
                filter.SetString(filter.FilterWhereId, "lorry", lorryTextBox.Text);
                isChanchedLorry = false;
            }
            if (isChanchedParcelType)
            {
                int i = 0;
                string[] values = new string[this.parcelTypeListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.parcelTypeListBox.SelectedItems)
                {
                    ReferenceDS.tableParcelTypeRow row = rowview.Row as ReferenceDS.tableParcelTypeRow;
                    values[i] = row.parceltypeid.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "parceltype", values);
                isChanchedParcelType = false;
            }
            if (isChanchedStatus)
            {
                int i = 0;
                string[] values = new string[this.statusListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.statusListBox.SelectedItems)
                {
                    ReferenceDS.tableRequestStatusRow row = rowview.Row as ReferenceDS.tableRequestStatusRow;
                    values[i] = row.rowId.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "parcelstatus", values);
                isChanchedStatus = false;
            }
            if (isChanchedGoodsType)
            {
                int i = 0;
                string[] values = new string[this.goodstypeListBox.SelectedItems.Count];
                foreach (DataModelClassLibrary.ReferenceSimpleItem rowview in this.goodstypeListBox.SelectedItems)
                {
                    values[i] = rowview.Name;
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "goodstype", values);
                isChanchedGoodsType = false;
            }
            if(ManagerChanged)
            {
                int i = 0;
                string[] values = new string[this.ManagerListBox.SelectedItems.Count];
                foreach (Manager item in this.ManagerListBox.SelectedItems)
                {
                    values[i] = item.Id.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "manager", values);
                ManagerChanged = false;
            }
            if (isChanchedManagerGroup)
            {
                int i = 0;
                string[] values = new string[this.managerGroupListBox.SelectedItems.Count];
                foreach (lib.ReferenceSimpleItem item in this.managerGroupListBox.SelectedItems)
                {
                    values[i] = item.Id.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "managerGroupId", values);
                isChanchedManagerGroup = false;
            }
            if (myclientischanched)
            {
                if (myclientvm.FilterOn)
                {
                    int i = 0;
                    string[] values = new string[myclientvm.SelectedItems.Count];
                    foreach (Customer rowview in myclientvm.SelectedItems)
                    {
                        values[i] = rowview.Id.ToString();
                        i++;
                    }
                    filter.SetList(filter.FilterWhereId, "customerId", values);
                }
                else
                    filter.SetList(filter.FilterWhereId, "customerId", new string[0]);
                myclientischanched = false;
            }
            if (myclegalischanched)
            {
                if (myclegalvm.FilterOn)
                {
                    int i = 0;
                    string[] values = new string[myclegalvm.SelectedItems.Count];
                    foreach (CustomerLegal item in myclegalvm.SelectedItems)
                    {
                        values[i] = item.Id.ToString();
                        i++;
                    }
                    filter.SetList(filter.FilterWhereId, "clegalid", values);
                }
                else
                    filter.SetList(filter.FilterWhereId, "clegalid", new string[0]);
                myclegalischanched = false;
            }
            if (isChanchedAgent)
            {
                if (myagentvm.FilterOn)
                {
                    int i = 0;
                    string[] values = new string[myagentvm.SelectedItems.Count];
                    foreach (lib.ReferenceSimpleItem rowview in myagentvm.SelectedItems)
                    {
                        values[i] = rowview.Id.ToString();
                        i++;
                    }
                    filter.SetList(filter.FilterWhereId, "agentId", values);
                }
                else
                    filter.SetList(filter.FilterWhereId, "agentId", new string[0]);
                //int i = 0;
                //string[] values = new string[this.agentListBox.SelectedItems.Count];
                //foreach (System.Data.DataRowView rowview in this.agentListBox.SelectedItems)
                //{
                //    RequestDS.tableAgentNameRow row = rowview.Row as RequestDS.tableAgentNameRow;
                //    values[i] = row.agentID.ToString();
                //    i++;
                //}
                //filter.SetList(filter.FilterWhereId, "agentId", values);
                isChanchedAgent = false;
            }
            if (isChanchedServiceType)
            {
                int i = 0;
                string[] values = new string[this.serviceTypeListBox.SelectedItems.Count];
                foreach (lib.ReferenceSimpleItem item in this.serviceTypeListBox.SelectedItems)
                {
                    values[i] = item.Name;
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "servicetype", values);
                isChanchedServiceType = false;
            }
        }
        private void Fill()
        {
            List<SQLFilterCondition> cond = filter.ConditionGet(filter.FilterWhereId, "parcelnumber");
            if (cond.Count > 0)
            {
                List<SQLFilterValue> values = filter.ValueGet(cond[0].propertyid);
                switch (cond[0].propertyOperator.ToLower())
                {
                    case "between":
                        int v1 = int.Parse(values[0].value);
                        int v2 = int.Parse(values[1].value);
                        if (v1 < v2)
                        {
                            startNumberTextBox.Text = v1.ToString();
                            stopNumberTextBox.Text = v2.ToString();
                        }
                        else
                        {
                            startNumberTextBox.Text = v2.ToString();
                            stopNumberTextBox.Text = v1.ToString();
                        }
                        break;
                    case ">=":
                        startNumberTextBox.Text = values[0].value;
                        break;
                    case "<=":
                        stopNumberTextBox.Text = values[0].value;
                        break;
                }
            }
            else
            {
                startNumberTextBox.Clear();
                stopNumberTextBox.Clear();
            }
            string date1, date2;
            filter.PullDate(filter.FilterWhereId, "ShipPlanDate", "shipplandate", out date1, out date2);
            startShipplandatePicker.Text = date1;
            stopShipplandatePicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "Period", "period", out date1, out date2);
            startPeriodPicker.Text = date1;
            stopPeriodPicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "InvoiceDate", "invoicedate", out date1, out date2);
            startInvoicePicker.Text = date1;
            stopInvoicePicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "TerminalOut", "terminalout", out date1, out date2);
            startTerminalOutPicker.Text = date1;
            stopTerminalOutPicker.Text = date2;

            string text;
            filter.PullString(filter.FilterWhereId, "carrier", out text);
            this.carrierTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "lorry", out text);
            this.lorryTextBox.Text = text;

            filter.PullListBox(filter.FilterWhereId, "parceltype", "parceltypeid", this.parcelTypeListBox, true);
            filter.PullListBox(filter.FilterWhereId, "parcelstatus", "rowId", this.statusListBox, true);
            filter.PullListBox(filter.FilterWhereId, "goodstype", "Name", this.goodstypeListBox, true);
            filter.PullListBox(filter.FilterWhereId, "manager", "Id", this.ManagerListBox, true);
            filter.PullListBox(filter.FilterWhereId, "managerGroupId", "Id", this.managerGroupListBox, true);
            myclientvm.Clear();
            List<SQLFilterCondition> listCond;
            listCond = filter.ConditionGet(filter.FilterWhereId, "customerId");
            if (listCond.Count > 0)
            {
                List<SQLFilterValue> listValue = filter.ValueGet(listCond[0].propertyid);
                foreach (SQLFilterValue val in listValue)
                {
                    foreach (DataModelClassLibrary.DomainBaseClass item in myclientvm.Items)
                        if (item.Id == int.Parse(val.value))
                        { this.customerListBox.ListBox.SelectedItems.Add(item); break; }
                }
            }
            myclegalvm.Clear();
            listCond = filter.ConditionGet(filter.FilterWhereId, "clegalid");
            if (listCond.Count > 0)
            {
                myclegalselectvalue = filter.ValueGet(listCond[0].propertyid);
                if (myclegalselectvalue.Count > 0 & myclegalvm.Items != null)
                {
                    foreach (SQLFilterValue val in myclegalselectvalue)
                    {
                        foreach (DataModelClassLibrary.DomainBaseClass item in myclegalvm.Items)
                            if (item.Id == int.Parse(val.value))
                            { myclegalvm.SetSelected(item, false); break; }
                    }
                    myclegalselectvalue.Clear();
                }
            }
            //filter.PullListBox(filter.FilterWhereId, "clegalid", "Id", this.CLegalListBox.ListBox, true);
            myagentvm.Clear();
            listCond = filter.ConditionGet(filter.FilterWhereId, "agentId");
            if (listCond.Count > 0)
            {
                List<SQLFilterValue> listValue = filter.ValueGet(listCond[0].propertyid);
                foreach (SQLFilterValue val in listValue)
                {
                    foreach (lib.ReferenceSimpleItem item in myagentvm.Items)
                        if (item.Id == int.Parse(val.value))
                        { this.agentListBox.ListBox.SelectedItems.Add(item); break; }
                }
            }
            //filter.PullListBox(filter.FilterWhereId, "agentId", "agentID", this.agentListBox, true);
            isChanchedNumber = false;
            isChanchedShipPlanDate = false;
            isChanchedGoodsType = false;
            isChanchedStatus = false;
            isChanchedParcelType = false;
            isChanchedServiceType = false;
            isChanchedAgent = false;
            ManagerChanged = false;
            isChanchedManagerGroup = false;
            isChanchedLorry = false;
            isChanchedCarrier = false;
            isChanchedPeriod = false;
            isChanchedInvoiceDate = false;
            isChanchedTerminalOut = false;
            myclientischanched = false;
            myclegalischanched = false;
        }
        private void ClearFilter()
        {
            this.startNumberTextBox.Clear();
            this.stopNumberTextBox.Clear();
            this.startPeriodPicker.Text = string.Empty;
            this.stopPeriodPicker.Text = string.Empty;
            this.startShipplandatePicker.Text = string.Empty;
            this.stopShipplandatePicker.Text = string.Empty;
            this.startInvoicePicker.Text = string.Empty;
            this.stopInvoicePicker.Text = string.Empty;
            this.startTerminalOutPicker.Text = string.Empty;
            this.stopTerminalOutPicker.Text = string.Empty;
            this.carrierTextBox.Clear();
            this.lorryTextBox.Clear();
            this.parcelTypeListBox.SelectedItems.Clear();
            this.statusListBox.SelectedItems.Clear();
            this.goodstypeListBox.SelectedItems.Clear();
            this.ManagerListBox.SelectedItems.Clear();
            this.managerGroupListBox.SelectedItems.Clear();
            myagentvm.Clear();
            myclientvm.Clear();
            myclegalvm.Clear();
            //this.agentListBox.SelectedItems.Clear();
            this.serviceTypeListBox.SelectedItems.Clear();
        }
        private bool SaveData()
        {
            ColumnsDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            ColumnsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            return true;
        }
        #region  ChangedEvent
        private void CustomerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            myclegalvm.FillSync();
            if (myclegalselectvalue != null && myclegalselectvalue.Count > 0)
            {
                foreach (SQLFilterValue val in myclegalselectvalue)
                {
                    foreach (DataModelClassLibrary.DomainBaseClass item in myclegalvm.Items)
                        if (item.Id == int.Parse(val.value))
                        { myclegalvm.SetSelected(item, false); break; }
                }
                myclegalselectvalue.Clear();
            }
            myclientischanched = true;
        }
        private void CLegalListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            myclegalischanched = true;
        }
        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedNumber = true;
        }
        private void ShipplandatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedShipPlanDate = true;
        }
        private void CarrierTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedCarrier = true;
        }
        private void LorryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedLorry = true;
        }
        private void ManagerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ManagerChanged = true;
        }
        private void managerGroupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedManagerGroup = true;
        }
        private void agentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedAgent = true;
        }
        private void serviceTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedServiceType = true;
        }
        private void ParcelTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedParcelType = true;
        }
        private void StatusListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedStatus = true;
        }
        private void GoodstypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedGoodsType = true;
        }
        private void PeriodPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedPeriod = true;
        }
        private void TerminalOutPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedTerminalOut = true;
        }
        private void InvoiceDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedInvoiceDate = true;
        }
        private void WeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void WeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void VolumeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void VolumeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void GoodValueComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void GoodValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void CostkgComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void CostkgTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void CurrencySumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void CurrencySumTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void OthersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void OthersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void ReturnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ReturnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void InvoiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void InvoiceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void PaySumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void PaySumTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void CustomsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void CustomsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void DeliveryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void DeliveryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void PrggermanyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void PrggermanyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void StoregermnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void StoregermnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void FreightgmnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void FreightgmnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void PreparatgmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void PreparatgmTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void PrgmoscowComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void PrgmoscowTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void InsuranceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void InsuranceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void DeliverymsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void DeliverymsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void SertificatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void SertificatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void ClaimComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ClaimTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void EscortmscwComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void EscortmscwTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #endregion

        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }

        private bool VLegalFilter(object clegal)
        {

            return true;
        }

    }

    public class CLegalCheckListBoxVM : uclib.CheckListBoxVMFill<Customer, CustomerLegal>
    {
        internal CLegalCheckListBoxVM() : base()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return (item as CustomerLegal).Name; };
            this.RefreshIsVisible = false;
            this.AreaButtonsIsVisible = false;
        }

        protected override void AddItem(Customer item)
        {
            foreach (CustomerLegal legal in item.Legals)
                Items.Add(legal);
        }
    }
}
