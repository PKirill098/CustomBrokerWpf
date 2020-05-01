using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class PaymentListFilterWin : Window
    {
        private bool isChanchedPPNumber;
        private bool isChanchedEnterDate;
        private bool isChanchedPPDate;
        private bool isChanchedTransDate;
        private bool isChanchedPPSum;
        private bool isChanchedTran;
        private bool isChanchedJoin;
        private bool isChanchedPurpose;
        private bool isChanchedNote;
        private bool isChanchedPayer;
        private bool isChanchedRecipient;
        private bool isChanchedParcel;

        private SQLFilter filter;
        PaymentlistUC myparent;

        public PaymentListFilterWin()
        {
            InitializeComponent();
        }

        private void winPaymentListFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refDS.tableFullNumber.Count==0) refDS.FullNumberRefresh();
            this.parcelListBox.ItemsSource = refDS.tableFullNumber.DefaultView;
            if(Owner is PaymentListWin)
                myparent=(Owner as PaymentListWin).PaymentlistUC;
            //else
            //    myparent = (Owner as MainWindow).PaymentlistUC;
            System.Data.DataView payerview = new System.Data.DataView(myparent.thisDS.tableCustomerName, string.Empty, "[customerName]", System.Data.DataViewRowState.CurrentRows);
            this.payerListBox.ItemsSource = payerview;
            System.Data.DataView recipientview = new System.Data.DataView(myparent.thisDS.tableLegalEntity, string.Empty, "[namelegal]", System.Data.DataViewRowState.CurrentRows);
            recipientListBox.ItemsSource = recipientview;
            filter = myparent.Filter;
            Fill();
        }
        private void winPaymentListFilter_Closed(object sender, EventArgs e)
        {
            myparent.IsShowFilter = false;
        }

        private void RunFilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IInputElement focelm = FocusManager.GetFocusedElement(this);
                FocusManager.SetFocusedElement(this, RunFilterButton);
                Actualization();
                myparent.RunFilter();
                FocusManager.SetFocusedElement(this, focelm);
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
        private void DefaultFilterButton_Click(object sender, RoutedEventArgs e)
        {
            filter.RemoveCurrentWhere();
            filter.GetDefaultFilter(SQLFilterPart.Where);
            Fill();
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
                    popInf.PlacementTarget = sender as UIElement;
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
        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFilter();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region ChangedEvent
        private void PPNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedPPNumber = true;
        }
        private void EnterDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedEnterDate = true;
        }
        private void PPDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedPPDate = true;
        }
        private void TransDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedTransDate = true;
        }

        private void PPSumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedPPSum = true;
        }
        private void PPSumTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedPPSum = true;
        }
        private void TranComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedTran = true;
        }
        private void JoinComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedJoin = true;
        }

        private void purposeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedPurpose = true;
        }
        private void noteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedNote = true;
        }

        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }
        private void payerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedPayer = true;
        }
        private void recipientListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedRecipient = true;
        }
        private void parcelListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedParcel = true;
        }
#endregion

        private void Fill()
        {
            isChanchedPPNumber=false;
            isChanchedEnterDate = false;
            isChanchedPPDate = false;
            isChanchedTransDate = false;
            isChanchedPPSum = false;
            isChanchedTran = false;
            isChanchedJoin = false;
            isChanchedPurpose = false;
            isChanchedNote = false;
            isChanchedPayer = false;
            isChanchedRecipient = false;
            isChanchedParcel = false;

            List<SQLFilterCondition> cond;
            cond = filter.ConditionGet(filter.FilterWhereId, "ppNumber");
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
                            startPPNumberTextBox.Text = v1.ToString();
                            stopPPNumberTextBox.Text = v2.ToString();
                        }
                        else
                        {
                            startPPNumberTextBox.Text = v2.ToString();
                            stopPPNumberTextBox.Text = v1.ToString();
                        }
                        break;
                    case ">=":
                        startPPNumberTextBox.Text = values[0].value;
                        break;
                    case "<=":
                        stopPPNumberTextBox.Text = values[0].value;
                        break;
                }
            }
            else
            {
                startPPNumberTextBox.Clear();
                stopPPNumberTextBox.Clear();
            }

            string date1, date2;
            filter.PullDate(filter.FilterWhereId, "PPDate", "ppdate", out date1, out date2);
            startPPDatePicker.Text = date1;
            stopPPDatePicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "EnterDate", "enterdate", out date1, out date2);
            startEnterPicker.Text = date1;
            stopEnterPicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "TransDate", "transdate", out date1, out date2);
            startTransDatePicker.Text = date1;
            stopTransDatePicker.Text = date2;

            string text; byte selindex;
            filter.PullNumber(filter.FilterWhereId, "ppsum", out text, out selindex);
            this.PPSumComboBox.SelectedIndex = selindex;
            this.PPSumTextBox.Text = text;

            cond = filter.ConditionGet(filter.FilterWhereId, "istrans");
            if (cond.Count > 0)
            {
                    if(cond[0].propertyOperator.ToUpper().Equals("NOT NULL")) TranComboBox.SelectedIndex = 1;
                    else TranComboBox.SelectedIndex = 2;   
            }
            else
            {
                TranComboBox.SelectedIndex=0;
            }
            cond = filter.ConditionGet(filter.FilterWhereId, "isjoin");
            if (cond.Count > 0)
            {
                if (cond[0].propertyOperator.ToUpper().Equals("NOT NULL")) JoinComboBox.SelectedIndex = 1;
                else JoinComboBox.SelectedIndex = 2;
            }
            else
            {
                JoinComboBox.SelectedIndex = 0;
            }

            filter.PullString(filter.FilterWhereId, "purpose", out text);
            this.purposeTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "note", out text);
            this.noteTextBox.Text = text;

            filter.PullListBox(filter.FilterWhereId, "payer", "customerID", this.payerListBox, true);
            filter.PullListBox(filter.FilterWhereId, "payaccount", "accountid", this.recipientListBox, true);
            filter.PullListBox(filter.FilterWhereId, "parcel", "parcelId", this.parcelListBox, true);
        }
        private void ClearFilter()
        {
            startPPNumberTextBox.Clear();
            stopPPNumberTextBox.Clear();
            startPPDatePicker.Text = string.Empty;
            stopPPDatePicker.Text = string.Empty;
            startEnterPicker.Text = string.Empty;
            stopEnterPicker.Text = string.Empty;
            startTransDatePicker.Text = string.Empty;
            stopTransDatePicker.Text = string.Empty;
            PPSumComboBox.SelectedIndex = 0;
            PPSumTextBox.Clear();
            TranComboBox.SelectedIndex = 0;
            JoinComboBox.SelectedIndex = 0;
            purposeTextBox.Clear();
            noteTextBox.Clear();
            payerListBox.SelectedItems.Clear();
            recipientListBox.SelectedItems.Clear();
            parcelListBox.SelectedItems.Clear();
        }
        private void Actualization()
        {
            if (isChanchedPPNumber)
            {
                string text1 = startPPNumberTextBox.Text; string text2 = stopPPNumberTextBox.Text;
                string oper = text1.Length > 0 & text2.Length > 0 ? "between" : text1.Length > 0 ? ">=" : text2.Length > 0 ? "<=" : "";
                List<SQLFilterCondition> cond = filter.ConditionGet(filter.FilterWhereId, "ppNumber");
                if (cond.Count > 0)
                {
                    if (oper.Length > 0)
                    {
                        filter.ConditionUpd(cond[0].propertyid, oper);
                        filter.ConditionValuesDel(cond[0].propertyid);
                        if (text1.Length > 0) filter.ConditionValueAdd(cond[0].propertyid, text1, 0);
                        if (text2.Length > 0) filter.ConditionValueAdd(cond[0].propertyid, text2, 0);
                    }
                    else
                    {
                        filter.ConditionDel(cond[0].propertyid);
                    }
                }
                else if (oper.Length > 0)
                {
                    int idCondition = filter.ConditionAdd(filter.FilterWhereId, "ppNumber", oper);
                    if (text1.Length > 0) filter.ConditionValueAdd(idCondition, text1, 0);
                    if (text2.Length > 0) filter.ConditionValueAdd(idCondition, text2, 0);
                }
                isChanchedPPNumber = false;
            }
            if (isChanchedPPDate)
            {
                filter.SetDate(filter.FilterWhereId, "PPDate", "ppdate", startPPDatePicker.SelectedDate, stopPPDatePicker.SelectedDate);
                isChanchedPPDate = false;
            }
            if (isChanchedEnterDate)
            {
                filter.SetDate(filter.FilterWhereId, "EnterDate", "enterdate", startEnterPicker.SelectedDate, stopEnterPicker.SelectedDate);
                isChanchedEnterDate = false;
            }
            if (isChanchedTransDate)
            {
                filter.SetDate(filter.FilterWhereId, "TransDate", "transdate", startTransDatePicker.SelectedDate, stopTransDatePicker.SelectedDate);
                isChanchedTransDate = false;
            }
            if (isChanchedPPSum)
            {
                filter.SetNumber(filter.FilterWhereId, "ppsum", PPSumComboBox.SelectedIndex, PPSumTextBox.Text);
                isChanchedPPSum = false;
            }
            if (isChanchedTran)
            {
                string f = this.TranComboBox.SelectedIndex > 1 ? "<>" : "=";
                List<SQLFilterCondition> cond = filter.ConditionGet(filter.FilterWhereId, "istrans");
                if (this.TranComboBox.SelectedIndex > 0)
                {
                    if (cond.Count > 0)
                    {
                        filter.ConditionUpd(cond[0].propertyid, f);
                    }
                    else
                    {
                        filter.ConditionValueAdd(filter.ConditionAdd(filter.FilterWhereId, "istrans", f), "ppsum", 1);
                    }
                }
                else if (cond.Count > 0)
                {
                    filter.ConditionDel(cond[0].propertyid);
                }
                isChanchedTran = false;
            }
            if (isChanchedJoin)
            {
                string f = this.JoinComboBox.SelectedIndex > 1 ? "<>" : "=";
                List<SQLFilterCondition> cond = filter.ConditionGet(filter.FilterWhereId, "isjoin");
                if (this.JoinComboBox.SelectedIndex > 0)
                {
                    if (cond.Count > 0)
                    {
                        filter.ConditionUpd(cond[0].propertyid, f);
                    }
                    else
                    {
                        filter.ConditionValueAdd(filter.ConditionAdd(filter.FilterWhereId, "isjoin", f), "ppsum", 1);
                    }
                }
                else if (cond.Count > 0)
                {
                    filter.ConditionDel(cond[0].propertyid);
                }
                isChanchedJoin = false;
            }
            if (isChanchedPurpose)
            {
                filter.SetString(filter.FilterWhereId, "purpose", purposeTextBox.Text);
                isChanchedPurpose = false;
            }
            if (isChanchedNote)
            {
                filter.SetString(filter.FilterWhereId, "note", noteTextBox.Text);
                isChanchedNote = false;
            }
            if (isChanchedPayer)
            {
                int i = 0;
                string[] values = new string[this.payerListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.payerListBox.SelectedItems)
                {
                    PaymentDS.tableCustomerNameRow row = rowview.Row as PaymentDS.tableCustomerNameRow;
                    values[i] = row.customerID.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "payer", values);
                isChanchedPayer = false;
            }
            if (isChanchedRecipient)
            {
                int i = 0;
                string[] values = new string[this.recipientListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.recipientListBox.SelectedItems)
                {
                    PaymentDS.tableLegalEntityRow row = rowview.Row as PaymentDS.tableLegalEntityRow;
                    values[i] = row.accountid.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "payaccount", values);
                isChanchedRecipient = false;
            }
            if (isChanchedParcel)
            {
                int i = 0;
                string[] values = new string[this.parcelListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.parcelListBox.SelectedItems)
                {
                    ReferenceDS.tableFullNumberRow row = rowview.Row as ReferenceDS.tableFullNumberRow;
                    values[i] = row.parcelId.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "parcel", values);
                isChanchedParcel = false;
            }
        }

    }
}
