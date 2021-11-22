using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для CostFilterWin.xaml
    /// </summary>
    public partial class ExpenditureFilterWin : Window
    {
        private SQLFilter filter;

        private bool isChanchedDatePeriod;
        private bool isChanchedDate;
        private bool isChanchedSumEx;
        private bool isChanchedSumPayCurr;
        private bool isChanchedSumPayRub;
        private bool isChanchedDetail;
        private bool isChanchedDescr;
        private bool isChanchedSubject;
        private bool isChanchedNumberIIn;
        private bool isChanchedDateIIn;
        private bool isChanchedType;
        private bool isChanchedItem;
        private bool isChanchedCurrency;
        private bool isChanchedRecipient;
        private bool isChanchedLegal;
        private bool isChanchedParcel;

        public ExpenditureFilterWin()
        {
            InitializeComponent();
        }

        private void winCostFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            System.Data.DataView typeview = new System.Data.DataView(refDS.tableExpenditureType, string.Empty, "NameET", DataViewRowState.CurrentRows);
            this.typeListBox.ItemsSource = typeview;
            System.Data.DataView itemview = new System.Data.DataView(refDS.tableExpenditureItem, string.Empty, "nameEI", DataViewRowState.CurrentRows);
            this.itemListBox.ItemsSource = itemview;
            System.Data.DataView currencyview = new System.Data.DataView(refDS.tableAccountCurrency, string.Empty, "currency", DataViewRowState.CurrentRows);
            this.currencyListBox.ItemsSource = currencyview;
            ExpenditureDSTableAdapters.RecipientListAdapter recipientAdapter = new ExpenditureDSTableAdapters.RecipientListAdapter();
            recipientAdapter.Fill((this.Owner as ExpenditureListWin).thisDS.tableRecipientList);
            System.Data.DataView recipientview = new System.Data.DataView((this.Owner as ExpenditureListWin).thisDS.tableRecipientList, string.Empty, "recipient", DataViewRowState.CurrentRows);
            this.recipientListBox.ItemsSource = recipientview;
            if (refDS.tableLegalEntity.Count == 0) refDS.LegalEntityRefresh();
            System.Data.DataView legalview = new System.Data.DataView(refDS.tableLegalEntity, string.Empty, "namelegal", DataViewRowState.CurrentRows);
            this.legalListBox.ItemsSource = legalview;
            System.Data.DataView parcelview = new System.Data.DataView(refDS.tableFullNumber, string.Empty, "sort Desc", DataViewRowState.CurrentRows);
            this.parcelListBox.ItemsSource = parcelview;

            filter = (this.Owner as ISQLFiltredWindow).Filter;
            Fill();
        }
        private void winCostFilter_Closed(object sender, EventArgs e)
        {
            (this.Owner as ISQLFiltredWindow).IsShowFilter = false;
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
                    filter.SetDefaultFilter(SQLFilterPart.Where);
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

        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }

        #region ChangedEvent
        private void PeriodDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedDatePeriod = true;
        }
        private void SumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedSumEx = true;
        }
        private void SumTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedSumEx = true;
        }
        private void SumPayCurrComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedSumPayCurr = true;
        }
        private void SumPayCurrTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedSumPayCurr = true;
        }
        private void SumPayRubComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedSumPayRub = true;
        }
        private void SumPayRubTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedSumPayRub = true;
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedDate = true;
        }
        private void DetailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedDetail = true;
        }
        private void NumberIInTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedNumberIIn = true;
        }
        private void SubjectTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedSubject = true;
        }
        private void DateIInTextBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedDateIIn = true;
        }
        private void DescrTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedDescr = true;
        }
        private void TypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedType = true;
        }
        private void ItemlListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedItem = true;
        }
        private void RecipientListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedRecipient = true;
        }
        private void LegalListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedLegal = true;
        }
        private void ParcelListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedParcel = true;
        }
        private void CurrencyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedCurrency = true;
        }
        #endregion

        private void Actualization()
        {
            if (isChanchedDatePeriod)
            {
                filter.SetDatePeriod(filter.FilterWhereId, "DatePeriod", "periodstart", "periodstop", startPeriodDatePicker.SelectedDate, stopPeriodDatePicker.SelectedDate);
                isChanchedDatePeriod = false;
            }
            if (isChanchedNumberIIn)
            {
                filter.SetRange(filter.FilterWhereId, "numberIIn", startNumberIInTextBox.Text, stopNumberIInTextBox.Text);
                isChanchedNumberIIn = false;
            }
            if (isChanchedSumEx)
            {
                filter.SetNumber(filter.FilterWhereId, "sumEx", sumComboBox.SelectedIndex, sumTextBox.Text);
                isChanchedSumEx = false;
            }
            if (isChanchedSumPayCurr)
            {
                filter.SetNumber(filter.FilterWhereId, "sumPayCurr", sumPayCurrComboBox.SelectedIndex, sumPayCurrTextBox.Text);
                isChanchedSumPayCurr = false;
            }
            if (isChanchedSumPayRub)
            {
                filter.SetNumber(filter.FilterWhereId, "sumPayRub", sumPayRubComboBox.SelectedIndex, sumPayRubTextBox.Text);
                isChanchedSumPayRub = false;
            }
            if (isChanchedDate)
            {
                filter.SetDate(filter.FilterWhereId, "DateEx", "date", startDatePicker.SelectedDate, stopDatePicker.SelectedDate);
                isChanchedDate = false;
            }
            if (isChanchedDateIIn)
            {
                filter.SetDate(filter.FilterWhereId, "DateIIn", "dateIIn", startDateIInDatePicker.SelectedDate, stopDateIInDatePicker.SelectedDate);
                isChanchedDateIIn = false;
            }
            if (isChanchedDetail)
            {
                filter.SetString(filter.FilterWhereId, "detail", detailTextBox.Text);
                isChanchedDetail = false;
            }
            if (isChanchedSubject)
            {
                filter.SetString(filter.FilterWhereId, "subject", subjectTextBox.Text);
                isChanchedSubject = false;
            }
            if (isChanchedDescr)
            {
                filter.SetString(filter.FilterWhereId, "descr", descrTextBox.Text);
                isChanchedDescr = false;
            }
            if (isChanchedType)
            {
                int i = 0;
                string[] values = new string[this.typeListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.typeListBox.SelectedItems)
                {
                    ReferenceDS.tableExpenditureTypeRow row = rowview.Row as ReferenceDS.tableExpenditureTypeRow;
                    values[i] = row.TypeID.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "type", values);
                isChanchedType = false;
            }
            if (isChanchedItem)
            {
                int i = 0;
                string[] values = new string[this.itemListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.itemListBox.SelectedItems)
                {
                    ReferenceDS.tableExpenditureItemRow row = rowview.Row as ReferenceDS.tableExpenditureItemRow;
                    values[i] = row.expenditureItemID.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "item", values);
                isChanchedItem = false;
            }
            if (isChanchedRecipient)
            {
                int i = 0;
                string[] values = new string[this.recipientListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.recipientListBox.SelectedItems)
                {
                    ExpenditureDS.tableRecipientListRow row = rowview.Row as ExpenditureDS.tableRecipientListRow;
                    values[i] = row.recipient;
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "recipient", values);
                isChanchedRecipient = false;
            }
            if (isChanchedLegal)
            {
                int i = 0;
                string[] values = new string[this.legalListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.legalListBox.SelectedItems)
                {
                    ReferenceDS.tableLegalEntityRow row = rowview.Row as ReferenceDS.tableLegalEntityRow;
                    values[i] = row.accountid.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "legal", values);
                isChanchedLegal = false;
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
            if (isChanchedCurrency)
            {
                int i = 0;
                string[] values = new string[this.currencyListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.currencyListBox.SelectedItems)
                {
                    ReferenceDS.tableAccountCurrencyRow row = rowview.Row as ReferenceDS.tableAccountCurrencyRow;
                    values[i] = row.currency;
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "currency", values);
                isChanchedCurrency = false;
            }
        }
        private void Fill()
        {
            string str1 = null, str2 = null;
            List<SQLFilterCondition> cond1 = filter.ConditionGet(filter.FilterWhereId, "numberIIn");
            if (cond1.Count > 0)
            {
                List<SQLFilterValue> values = filter.ValueGet(cond1[0].propertyid);
                switch (cond1[0].propertyOperator.ToLower())
                {
                    case "between":
                        int v1 = int.Parse(values[0].value);
                        int v2 = int.Parse(values[1].value);
                        if (v1 < v2)
                        {
                            str1 = v1.ToString();
                            str2 = v2.ToString();
                        }
                        else
                        {
                            str1 = v2.ToString();
                            str2 = v1.ToString();
                        }
                        break;
                    case ">=":
                        str1 = values[0].value;
                        break;
                    case "<=":
                        str2 = values[0].value;
                        break;
                }
                startNumberIInTextBox.Text = str1;
                stopNumberIInTextBox.Text = str2;
            }
            else
            {
                startNumberIInTextBox.Clear();
                stopNumberIInTextBox.Clear();
            }
            string date1, date2;
            filter.PullDate(filter.FilterWhereId, "DatePeriod", "periodstart", out date1, out date2);
            startPeriodDatePicker.Text = date1;
            filter.PullDate(filter.FilterWhereId, "DatePeriod", "periodstop", out date1, out date2);
            stopPeriodDatePicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "DateEx", "date", out date1, out date2);
            startDatePicker.Text = date1;
            stopDatePicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "DateIIn", "dateIIn", out date1, out date2);
            startDateIInDatePicker.Text = date1;
            stopDateIInDatePicker.Text = date2;
            string text; byte selindex;
            filter.PullString(filter.FilterWhereId, "detail", out text);
            this.detailTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "subject", out text);
            this.subjectTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "descr", out text);
            this.descrTextBox.Text = text;
            filter.PullNumber(filter.FilterWhereId, "sumEx", out text, out selindex);
            this.sumComboBox.SelectedIndex = selindex;
            this.sumTextBox.Text = text;
            filter.PullNumber(filter.FilterWhereId, "sumPayCurr", out text, out selindex);
            this.sumPayCurrComboBox.SelectedIndex = selindex;
            this.sumPayCurrTextBox.Text = text;
            filter.PullNumber(filter.FilterWhereId, "sumPayRub", out text, out selindex);
            this.sumPayRubComboBox.SelectedIndex = selindex;
            this.sumPayRubTextBox.Text = text;
            filter.PullListBox(filter.FilterWhereId, "type", "TypeID", this.typeListBox, true);
            filter.PullListBox(filter.FilterWhereId, "item", "expenditureItemID", this.itemListBox, true);
            filter.PullListBox(filter.FilterWhereId, "currency", "currency", this.currencyListBox, true);
            filter.PullListBox(filter.FilterWhereId, "recipient", "recipientEx", this.recipientListBox, true);
            filter.PullListBox(filter.FilterWhereId, "legal", "accountid", this.legalListBox, true);
            filter.PullListBox(filter.FilterWhereId, "parcel", "parcelId", this.parcelListBox, true);
            isChanchedDate = false;
            isChanchedSumEx = false;
            isChanchedSumPayCurr=false;
            isChanchedSumPayRub=false;
            isChanchedDescr = false;
            isChanchedDetail = false;
            isChanchedSubject=false;
            isChanchedItem=false;
            isChanchedNumberIIn=false;
            isChanchedDateIIn=false;
            isChanchedType = false;
            isChanchedCurrency=false;
            isChanchedRecipient=false;
            isChanchedLegal=false;
            isChanchedParcel = false;
        }
        private void ClearFilter()
        {
            startPeriodDatePicker.Text = string.Empty;
            stopPeriodDatePicker.Text = string.Empty;
            startDatePicker.Text = string.Empty;
            stopDatePicker.Text = string.Empty;
            sumComboBox.SelectedIndex = 0;
            this.sumTextBox.Clear();
            sumPayCurrComboBox.SelectedIndex = 0;
            this.sumPayCurrTextBox.Clear();
            sumPayRubComboBox.SelectedIndex = 0;
            this.sumPayRubTextBox.Clear();
            startNumberIInTextBox.Clear();
            stopNumberIInTextBox.Clear();
            detailTextBox.Clear();
            subjectTextBox.Clear();
            startDateIInDatePicker.Text = string.Empty;
            stopDateIInDatePicker.Text = string.Empty;
            descrTextBox.Clear();
            typeListBox.SelectedItems.Clear();
            itemListBox.SelectedItems.Clear();
            currencyListBox.SelectedItems.Clear();
            recipientListBox.SelectedItems.Clear();
            legalListBox.SelectedItems.Clear();
            parcelListBox.SelectedItems.Clear();
        }
    }
}
