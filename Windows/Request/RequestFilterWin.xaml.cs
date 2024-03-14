using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для RequestFilterWin.xaml
    /// </summary>
    public partial class RequestFilterWin : Window
    {
        private lib.Interfaces.IFilterWindowOwner myfilterowner;
        internal lib.Interfaces.IFilterWindowOwner FilterOwner
        {
            set { myfilterowner = value; }
            get { return myfilterowner; }
        }

        private bool isChanchedRequestId;
        private bool isChanchedRequestDate;
        private bool isChanchedSpecDate;
        private bool isChanchedStorageDate;
        private bool isChanchedStoragePoint;
        private bool isChanchedcellNumber;
        private bool isChanchedvolume;
        private bool isChanchedofficialWeight;
        private bool isChanchedactualWeight;
        private bool isChanchedgoodValue;
        private bool isChanchedCustomerNote;
        private bool isChanchedStorageNote;
        private bool isChanchedManagerNote;
        private bool isChanchedStatus;
        private bool isChanchedManagerGroup;
        private bool isChanchedCustomer;
        private bool isChanchedLoadDescription;
        private bool isChanchedAgent;
        private bool isChanchedStore;
        private bool isChanchedForwarder;
        private bool isChanchedFreight;
        private bool isChanchedParcelType;
        private int idRequestId = 0;

        public RequestFilterWin()
        {
            InitializeComponent();
        }

        private void winRequestFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS ds = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (ds.tableRequestStatus.Count == 0) ds.RequestStatusRefresh();
            System.Data.DataView statusview = new System.Data.DataView(ds.tableRequestStatus);
            this.statusListBox.ItemsSource = statusview;
            if (ds.tableManagerGroup.Count == 0) ds.ManagerGroupRefresh();
            System.Data.DataView managerview = new System.Data.DataView(ds.tableManagerGroup, string.Empty, "[managergroupName]", System.Data.DataViewRowState.CurrentRows);
            managerGroupListBox.ItemsSource = managerview;
            goodsListBox.ItemsSource = CustomBrokerWpf.References.GoodsTypesParcel;
            storeListBox.ItemsSource = CustomBrokerWpf.References.Stores;
            if (ds.tableForwarder.Count == 0)
            {
                ReferenceDSTableAdapters.ForwarderAdapter adapter = new ReferenceDSTableAdapters.ForwarderAdapter();
                adapter.Fill(ds.tableForwarder);
            }
            System.Data.DataView forwarderview = new System.Data.DataView(ds.tableForwarder, string.Empty, "[itemName]", System.Data.DataViewRowState.CurrentRows);
            forwarderListBox.ItemsSource = forwarderview;
            this.parceltypeComboBox.Items.Add("EURO"); this.parceltypeComboBox.Items.Add("USD");
            if (ds.tableCustomerName.Count == 0) ds.CustomerNameRefresh();
            System.Data.DataView customerview = new System.Data.DataView(ds.tableCustomerName, string.Empty, "[customerName]", System.Data.DataViewRowState.CurrentRows);
            this.customerListBox.ItemsSource = customerview;
            System.Windows.Data.ListCollectionView agentview = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.AgentNames);
            CustomBrokerWpf.References.AgentNames.RefreshViewAdd(agentview);
            agentview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            agentListBox.DisplayMemberPath = "Name";
            agentListBox.SetValue(TextSearch.TextPathProperty, "Name");
            agentListBox.ItemsSource = agentview;
            Fill();
        }
        private void Fill()
        {
            List<lib.SQLFilter.SQLFilterCondition> listCond;
            List<lib.SQLFilter.SQLFilterValue> listValue;
            List<lib.SQLFilter.SQLFilterCondition> RequestIdCond = myfilterowner.Filter.ConditionGet(myfilterowner.Filter.FilterWhereId, "RequestId");
            if (RequestIdCond.Count > 0)
            {
                idRequestId = RequestIdCond[0].propertyid;
                List<lib.SQLFilter.SQLFilterValue> values = myfilterowner.Filter.ValueGet(idRequestId);
                switch (RequestIdCond[0].propertyOperator.ToLower())
                {
                    case "between":
                        int v1 = int.Parse(values[0].value);
                        int v2 = int.Parse(values[1].value);
                        if (v1 < v2)
                        {
                            startRequestIdTextBox.Text = v1.ToString();
                            stopRequestIdTextBox.Text = v2.ToString();
                        }
                        else
                        {
                            startRequestIdTextBox.Text = v2.ToString();
                            stopRequestIdTextBox.Text = v1.ToString();
                        }
                        break;
                    case ">=":
                        startRequestIdTextBox.Text = values[0].value;
                        break;
                    case "<=":
                        stopRequestIdTextBox.Text = values[0].value;
                        break;
                }
            }
            else
            {
                startRequestIdTextBox.Clear();
                stopRequestIdTextBox.Clear();
            }

            string date1, date2;
            myfilterowner.Filter.PullDate(myfilterowner.Filter.FilterWhereId, "RequestDate", "requestDate", out date1, out date2);
            startRequestDatePicker.Text = date1;
            stopRequestDatePicker.Text = date2;
            myfilterowner.Filter.PullDate(myfilterowner.Filter.FilterWhereId, "SpecDate", "specification", out date1, out date2);
            startSpecDatePicker.Text = date1;
            stopSpecDatePicker.Text = date2;
            myfilterowner.Filter.PullDate(myfilterowner.Filter.FilterWhereId, "StorageDate", "storageDate", out date1, out date2);
            startStorageDatePicker.Text = date1;
            stopStorageDatePicker.Text = date2;

            listCond = myfilterowner.Filter.ConditionGet(myfilterowner.Filter.FilterWhereId, "storagePoint");
            if (listCond.Count > 0)
            {
                switch (listCond[0].propertyOperator.ToLower())
                {
                    case "=":
                        this.storagePointComboBox.SelectedIndex = 0;
                        break;
                    case "is null":
                        this.storagePointComboBox.SelectedIndex = 1;
                        break;
                }
                listValue = myfilterowner.Filter.ValueGet(listCond[0].propertyid);
                if (listValue.Count > 0) this.storagePointTextBox.Text = listValue[0].value;
            }

            string text; byte selindex;
            myfilterowner.Filter.PullNumber(myfilterowner.Filter.FilterWhereId, "cellNumber", out text, out selindex);
            this.cellNumberComboBox.SelectedIndex = selindex;
            this.cellNumberTextBox.Text = text;
            myfilterowner.Filter.PullNumber(myfilterowner.Filter.FilterWhereId, "volume", out text, out selindex);
            this.volumeComboBox.SelectedIndex = selindex;
            this.volumeTextBox.Text = text;
            myfilterowner.Filter.PullNumber(myfilterowner.Filter.FilterWhereId, "officialWeight", out text, out selindex);
            this.officialWeightComboBox.SelectedIndex = selindex;
            this.officialWeightTextBox.Text = text;
            myfilterowner.Filter.PullNumber(myfilterowner.Filter.FilterWhereId, "actualWeight", out text, out selindex);
            this.actualWeightComboBox.SelectedIndex = selindex;
            this.actualWeightTextBox.Text = text;
            myfilterowner.Filter.PullNumber(myfilterowner.Filter.FilterWhereId, "goodValue", out text, out selindex);
            this.goodValueComboBox.SelectedIndex = selindex;
            this.goodValueTextBox.Text = text;
            myfilterowner.Filter.PullNumber(myfilterowner.Filter.FilterWhereId, "currency", out text, out selindex);
            if(text!= string.Empty) this.parceltypeComboBox.SelectedValue = byte.Parse(text);

            myfilterowner.Filter.PullString(myfilterowner.Filter.FilterWhereId, "CustomerNote", out text);
            this.customerNoteTextBox.Text = text;
            myfilterowner.Filter.PullString(myfilterowner.Filter.FilterWhereId, "storageNote", out text);
            this.storageNoteTextBox.Text = text;
            myfilterowner.Filter.PullString(myfilterowner.Filter.FilterWhereId, "managerNote", out text);
            this.managerNoteTextBox.Text = text;

            myfilterowner.Filter.PullListBox(myfilterowner.Filter.FilterWhereId, "status", "rowId", this.statusListBox, true);
            myfilterowner.Filter.PullListBox(myfilterowner.Filter.FilterWhereId, "managerGroupId", "managergroupID", this.managerGroupListBox, true);
            myfilterowner.Filter.PullListBox(myfilterowner.Filter.FilterWhereId, "customerId", "customerID", this.customerListBox, true);
            myfilterowner.Filter.PullListBox(myfilterowner.Filter.FilterWhereId, "loadDescription", "Name", this.goodsListBox, true);
            myfilterowner.Filter.PullListBox(myfilterowner.Filter.FilterWhereId, "agentId", "Id", this.agentListBox, true);
            myfilterowner.Filter.PullListBox(myfilterowner.Filter.FilterWhereId, "storeid", "Id", this.storeListBox, true);
            myfilterowner.Filter.PullListBox(myfilterowner.Filter.FilterWhereId, "forwarder", "itemId", this.forwarderListBox, true);
 
            isChanchedRequestId = false;
            isChanchedRequestDate = false;
            isChanchedSpecDate = false;
            isChanchedStorageDate = false;
            isChanchedStoragePoint = false;
            isChanchedcellNumber = false;
            isChanchedvolume = false;
            isChanchedofficialWeight = false;
            isChanchedactualWeight = false;
            isChanchedgoodValue = false;
            isChanchedCustomerNote = false;
            isChanchedStorageNote = false;
            isChanchedManagerNote = false;
            isChanchedStatus = false;
            isChanchedManagerGroup = false;
            isChanchedCustomer = false;
            isChanchedLoadDescription = false;
            isChanchedAgent = false;
            isChanchedStore = false;
            isChanchedForwarder = false;
            isChanchedFreight = false;
            isChanchedParcelType = false;
        }
        private void winRequestFilter_Closed(object sender, EventArgs e)
        {
            myfilterowner.IsShowFilterWindow = false;
        }

        private void RunFilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IInputElement focelm = FocusManager.GetFocusedElement(this);
                FocusManager.SetFocusedElement(this, RunFilterButton);
                Actualization();
                {
                    (myfilterowner as Classes.Domain.RequestViewCommand).RequestClientFilter = null;
                    (myfilterowner as Classes.Domain.RequestViewCommand).RequestStoragePointFilter = string.Empty;
                    myfilterowner.RunFilter(null);
                }
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
        private void Actualization()
        {
            if (isChanchedRequestId)
            {
                string text1 = startRequestIdTextBox.Text; string text2 = stopRequestIdTextBox.Text;
                string oper = text1.Length > 0 & text2.Length > 0 ? "between" : text1.Length > 0 ? ">=" : text2.Length > 0 ? "<=" : "";
                if (idRequestId != 0)
                {
                    if (oper.Length > 0)
                    {
                        myfilterowner.Filter.ConditionUpd(idRequestId, oper);
                        myfilterowner.Filter.ConditionValuesDel(idRequestId);
                        if (text1.Length > 0) myfilterowner.Filter.ConditionValueAdd(idRequestId, text1, 0);
                        if (text2.Length > 0) myfilterowner.Filter.ConditionValueAdd(idRequestId, text2, 0);
                    }
                    else
                    {
                        myfilterowner.Filter.ConditionDel(idRequestId);
                        idRequestId = 0;
                    }
                }
                else if (oper.Length > 0)
                {
                    idRequestId = myfilterowner.Filter.ConditionAdd(myfilterowner.Filter.FilterWhereId, "RequestId", oper);
                    if (text1.Length > 0) myfilterowner.Filter.ConditionValueAdd(idRequestId, text1, 0);
                    if (text2.Length > 0) myfilterowner.Filter.ConditionValueAdd(idRequestId, text2, 0);
                }
                isChanchedRequestId = false;
            }
            if (isChanchedRequestDate)
            {
                myfilterowner.Filter.SetDate(myfilterowner.Filter.FilterWhereId, "RequestDate", "requestDate", startRequestDatePicker.SelectedDate, stopRequestDatePicker.SelectedDate);
                isChanchedRequestDate = false;
            }
            if (isChanchedSpecDate)
            {
                myfilterowner.Filter.SetDate(myfilterowner.Filter.FilterWhereId, "SpecDate", "specification", startSpecDatePicker.SelectedDate, stopSpecDatePicker.SelectedDate);
                isChanchedSpecDate = false;
            }
            if (isChanchedStorageDate)
            {
                myfilterowner.Filter.SetDate(myfilterowner.Filter.FilterWhereId, "StorageDate", "storageDate", startStorageDatePicker.SelectedDate, stopStorageDatePicker.SelectedDate);
                isChanchedStorageDate = false;
            }
            if (true)
            {
                string oper = "=";
                switch (storagePointComboBox.SelectedIndex)
                {
                    case 0:
                        oper = "=";
                        break;
                    case 1:
                        oper = "is null";
                        break;
                }
                List<lib.SQLFilter.SQLFilterCondition> cond = myfilterowner.Filter.ConditionGet(myfilterowner.Filter.FilterWhereId, "storagePoint");
                if (cond.Count > 0)
                {
                    if (storagePointComboBox.SelectedIndex == 0 & storagePointTextBox.Text.Length == 0)
                    {
                        myfilterowner.Filter.ConditionDel(cond[0].propertyid);
                    }
                    else
                    {
                        if (cond[0].propertyOperator != oper) myfilterowner.Filter.ConditionUpd(cond[0].propertyid, oper);
                        List<lib.SQLFilter.SQLFilterValue> val = myfilterowner.Filter.ValueGet(cond[0].propertyid);
                        if (val.Count > 0)
                        {
                            myfilterowner.Filter.ConditionValueUpd(val[0].valueId, storagePointTextBox.Text, 0);
                        }
                        else
                        {
                            myfilterowner.Filter.ConditionValueAdd(cond[0].propertyid, storagePointTextBox.Text, 0);
                        }
                    }
                }
                else if (!(storagePointComboBox.SelectedIndex == 0 & storagePointTextBox.Text.Length == 0))
                {
                    myfilterowner.Filter.ConditionValueAdd(myfilterowner.Filter.ConditionAdd(myfilterowner.Filter.FilterWhereId, "storagePoint", oper), storagePointTextBox.Text, 0);
                }
                isChanchedStoragePoint = false;
            }
            if (isChanchedcellNumber)
            {
                myfilterowner.Filter.SetNumber(myfilterowner.Filter.FilterWhereId, "cellNumber", (string)cellNumberComboBox.SelectedItem, cellNumberTextBox.Text);
                isChanchedcellNumber = false;
            }
            if (isChanchedvolume)
            {
                myfilterowner.Filter.SetNumber(myfilterowner.Filter.FilterWhereId, "volume", (string)volumeComboBox.SelectedItem, volumeTextBox.Text);
                isChanchedvolume = false;
            }
            if (isChanchedofficialWeight)
            {
                myfilterowner.Filter.SetNumber(myfilterowner.Filter.FilterWhereId, "officialWeight", (string)officialWeightComboBox.SelectedItem, officialWeightTextBox.Text);
                isChanchedofficialWeight = false;
            }
            if (isChanchedactualWeight)
            {
                myfilterowner.Filter.SetNumber(myfilterowner.Filter.FilterWhereId, "actualWeight", (string)actualWeightComboBox.SelectedItem, actualWeightTextBox.Text);
                isChanchedactualWeight = false;
            }
            if (isChanchedgoodValue)
            {
                myfilterowner.Filter.SetNumber(myfilterowner.Filter.FilterWhereId, "goodValue", (string)goodValueComboBox.SelectedItem, goodValueTextBox.Text);
                isChanchedgoodValue = false;
            }
            if (isChanchedCustomerNote)
            {
                myfilterowner.Filter.SetString(myfilterowner.Filter.FilterWhereId, "CustomerNote", customerNoteTextBox.Text);
                isChanchedCustomerNote = false;
            }
            if (isChanchedStorageNote)
            {
                myfilterowner.Filter.SetString(myfilterowner.Filter.FilterWhereId, "storageNote", storageNoteTextBox.Text);
                isChanchedStorageNote = false;
            }
            if (isChanchedManagerNote)
            {
                myfilterowner.Filter.SetString(myfilterowner.Filter.FilterWhereId, "managerNote", managerNoteTextBox.Text);
                isChanchedManagerNote = false;
            }

            if (isChanchedStatus)
            {
                int i = 0;
                string[] values = new string[this.statusListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.statusListBox.SelectedItems)
                {
                    ReferenceDS.tableRequestStatusRow status = rowview.Row as ReferenceDS.tableRequestStatusRow;
                    values[i] = status.rowId.ToString();
                    i++;
                }
                myfilterowner.Filter.SetList(myfilterowner.Filter.FilterWhereId, "status", values);
                isChanchedStatus = false;
            }
            if (isChanchedManagerGroup)
            {
                int i = 0;
                string[] values = new string[this.managerGroupListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.managerGroupListBox.SelectedItems)
                {
                    ReferenceDS.tableManagerGroupRow row = rowview.Row as ReferenceDS.tableManagerGroupRow;
                    values[i] = row.managergroupID.ToString();
                    i++;
                }
                myfilterowner.Filter.SetList(myfilterowner.Filter.FilterWhereId, "managerGroupId", values);
                isChanchedManagerGroup = false;
            }
            if (true)
            {
                int i = 0;
                string[] values = new string[this.customerListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.customerListBox.SelectedItems)
                {
                    ReferenceDS.tableCustomerNameRow row = rowview.Row as ReferenceDS.tableCustomerNameRow;
                    values[i] = row.customerID.ToString();
                    i++;
                }
                myfilterowner.Filter.SetList(myfilterowner.Filter.FilterWhereId, "customerId", values);
                isChanchedCustomer = false;
            }
            if (isChanchedLoadDescription)
            {
                int i = 0;
                string[] values = new string[this.goodsListBox.SelectedItems.Count];
                foreach (DataModelClassLibrary.ReferenceSimpleItem rowview in this.goodsListBox.SelectedItems)
                {
                    values[i] = rowview.Name;
                    i++;
                }
                myfilterowner.Filter.SetList(myfilterowner.Filter.FilterWhereId, "loadDescription", values);
                isChanchedLoadDescription = false;
            }
            if (isChanchedAgent)
            {
                int i = 0;
                string[] values = new string[this.agentListBox.SelectedItems.Count];
                foreach (DataModelClassLibrary.ReferenceSimpleItem item in this.agentListBox.SelectedItems)
                {
                    values[i] = item.Id.ToString();
                    i++;
                }
                myfilterowner.Filter.SetList(myfilterowner.Filter.FilterWhereId, "agentId", values);
                isChanchedAgent = false;
            }
            if (isChanchedStore)
            {
                int i = 0;
                string[] values = new string[this.storeListBox.SelectedItems.Count];
                foreach (DataModelClassLibrary.ReferenceSimpleItem item in this.storeListBox.SelectedItems)
                {
                    values[i] = item.Id.ToString();
                    i++;
                }
                myfilterowner.Filter.SetList(myfilterowner.Filter.FilterWhereId, "storeid", values);
                isChanchedStore = false;
            }
            if (isChanchedForwarder)
            {
                int i = 0;
                string[] values = new string[this.forwarderListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.forwarderListBox.SelectedItems)
                {
                    ReferenceDS.tableForwarderRow row = rowview.Row as ReferenceDS.tableForwarderRow;
                    values[i] = row.itemId.ToString();
                    i++;
                }
                myfilterowner.Filter.SetList(myfilterowner.Filter.FilterWhereId, "forwarder", values);
                isChanchedForwarder = false;
            }
            if (isChanchedFreight)
            {
                string f = this.frieghtComboBox.SelectedIndex > 1 ? "IS NULL" : "NOT NULL";
                List<lib.SQLFilter.SQLFilterCondition> cond = myfilterowner.Filter.ConditionGet(myfilterowner.Filter.FilterWhereId, "freight");
                if (this.frieghtComboBox.SelectedIndex > 0)
                {
                    if (cond.Count > 0)
                    {
                        myfilterowner.Filter.ConditionUpd(cond[0].propertyid, f);
                    }
                    else
                    {
                        myfilterowner.Filter.ConditionAdd(myfilterowner.Filter.FilterWhereId, "freight", f);
                    }
                }
                else if (cond.Count > 0)
                {
                    myfilterowner.Filter.ConditionDel(cond[0].propertyid);
                }
                isChanchedFreight = false;
            }
            if(isChanchedParcelType)
            {
                if (parceltypeComboBox.SelectedIndex != -1)
                    myfilterowner.Filter.SetNumber(myfilterowner.Filter.FilterWhereId, "currency", "=",parceltypeComboBox.SelectedIndex.ToString());
                else
                    myfilterowner.Filter.SetNumber(myfilterowner.Filter.FilterWhereId, "currency", "=", string.Empty);
                isChanchedParcelType = false;
            }
        }

        private void RemoveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            myfilterowner.Filter.RemoveCurrentWhere();
        }

        private void SaveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Фильтр по умолчанию будет заменён текущим фильтром.\nПродолжить?", "Сохранение фильтра", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
            {
                try
                {
                    Actualization();
                    myfilterowner.Filter.SetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
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

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFilter();
        }
        private void ClearFilter()
        {
            this.startRequestIdTextBox.Clear();
            this.stopRequestIdTextBox.Clear();
            this.startRequestDatePicker.Text = string.Empty;
            this.stopRequestDatePicker.Text = string.Empty;
            this.startSpecDatePicker.Text = string.Empty;
            this.stopSpecDatePicker.Text = string.Empty;
            this.startStorageDatePicker.Text = string.Empty;
            this.stopStorageDatePicker.Text = string.Empty;
            this.storagePointComboBox.SelectedIndex = 0;
            this.storagePointTextBox.Clear();
            this.cellNumberComboBox.SelectedIndex = 0;
            this.cellNumberTextBox.Clear();
            this.volumeComboBox.SelectedIndex = 0;
            this.volumeTextBox.Clear();
            this.officialWeightComboBox.SelectedIndex = 0;
            this.officialWeightTextBox.Clear();
            this.actualWeightComboBox.SelectedIndex = 0;
            this.actualWeightTextBox.Clear();
            this.goodValueComboBox.SelectedIndex = 0;
            this.goodValueTextBox.Clear();
            this.customerNoteTextBox.Clear();
            this.storageNoteTextBox.Clear();
            this.managerNoteTextBox.Clear();
            this.statusListBox.SelectedItems.Clear();
            this.managerGroupListBox.SelectedItems.Clear();
            this.customerListBox.SelectedItems.Clear();
            this.goodsListBox.SelectedItems.Clear();
            this.agentListBox.SelectedItems.Clear();
            this.storeListBox.SelectedItems.Clear();
            this.forwarderListBox.SelectedItems.Clear();
            this.frieghtComboBox.SelectedIndex = 0;
            this.parceltypeComboBox.SelectedIndex = -1;
        }

        private void RequestIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TextBox textbox = (sender as TextBox);
            //string text1 = textbox.Text;
            //int n ;
            //if (int.TryParse(text1, out n)) textbox.BorderBrush = null;
            //else textbox.BorderBrush = System.Windows.Media.Brushes.Red;
            isChanchedRequestId = true;
        }
        private void RequestDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedRequestDate = true;
        }
        private void SpecDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedSpecDate = true;
        }
        private void StorageDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedStorageDate = true;
        }
        private void storagePointComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedStoragePoint = true;
            if (storagePointComboBox.SelectedIndex > 0) storagePointTextBox.Clear();
        }
        private void storagePointTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedStoragePoint = true;
        }
        private void cellNumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedcellNumber = true;
        }
        private void cellNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedcellNumber = true;
        }
        private void volumeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedvolume = true;
        }
        private void volumeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedvolume = true;
        }
        private void officialWeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedofficialWeight = true;
        }
        private void officialWeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedofficialWeight = true;
        }
        private void actualWeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedactualWeight = true;
        }
        private void actualWeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedactualWeight = true;
        }
        private void goodValueComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedgoodValue = true;
        }
        private void goodValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedgoodValue = true;
        }
        private void customerNoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedCustomerNote = true;
        }
        private void storageNoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedStorageNote = true;
        }
        private void managerNoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedManagerNote = true;
        }
        private void statusListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedStatus = true;
        }
        private void managerGroupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedManagerGroup = true;
        }
        private void customerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedCustomer = true;
        }
        private void goodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedLoadDescription = true;
        }
        private void agentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedAgent = true;
        }
        private void storeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedStore = true;
        }
        private void forwarderListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedForwarder = true;
        }
        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }
        private void frieghtComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedFreight = true;
        }
        private void parceltypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedParcelType = true;
        }

        private void DefaultFilterButton_Click(object sender, RoutedEventArgs e)
        {
            myfilterowner.Filter.RemoveCurrentWhere();
            myfilterowner.Filter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            Fill();
        }
    }
}
