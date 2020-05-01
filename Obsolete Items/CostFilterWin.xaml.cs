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

        private bool isChanchedNumber;
        private bool isChanchedDate;
        private bool isChanchedSum;
        private bool isChanchedDescr;
        private bool isChanchedLegal;

        public ExpenditureFilterWin()
        {
            InitializeComponent();
        }

        private void winCostFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            System.Data.DataView legalview = new System.Data.DataView(refDS.tableLegalEntity, string.Empty, "namelegal", DataViewRowState.CurrentRows);
            this.legalListBox.ItemsSource = legalview;
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
                (this.Owner as ISQLFiltredWindow).runFilter();
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
            filter.RemoveFilter();
            filter.GetDefaultFilter();
            Fill();
        }
        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFilter();
        }
        private void RemoveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            filter.RemoveFilter();
        }
        private void SaveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Фильтр по умолчанию будет заменён текущим фильтром.\nПродолжить?", "Сохранение фильтра", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
            {
                try
                {
                    Actualization();
                    filter.SetDefaultFilter();
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

        #region ChangedEvent
        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedDate = true;
        }
        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedNumber = true;
        }
        private void SumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedSum = true;
        }
        private void SumTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedSum = true;
        }
        private void descrTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedDescr = true;
        }
        private void LegalListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedLegal = true;
        }
        #endregion

        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }

        private void Actualization()
        {
            if (isChanchedNumber)
            {
                int n;
                //string text11=null;
                string text1 = startNumberTextBox.Text;
                n = text1.IndexOf('-');
                if (n > 0)
                {
                    //l = text1.Length;
                    //if (l - 1 > n) text11 = text1.Substring(n + 1, l - n - 1).Trim();
                    text1 = text1.Substring(0, n).Trim();
                }
                //string text21 = null;
                string text2 = stopNumberTextBox.Text;
                n = text2.IndexOf('-');
                if (n > 0)
                {
                    //l = text2.Length;
                    //if (l - 1 > n) text21 = text2.Substring(n + 1, l - n - 1).Trim();
                    text2 = text2.Substring(0, n).Trim();
                }
                string oper1 = text1.Length > 0 & text2.Length > 0 ? "between" : text1.Length > 0 ? ">=" : text2.Length > 0 ? "<=" : "";
                //string oper2 = text11.Length > 0 & text21.Length > 0 ? "between" : text11.Length > 0 ? ">=" : text21.Length > 0 ? "<=" : "";
                List<SQLFilterCondition> cond1 = filter.ConditionGet(filter.FilterSQLID, "parcelnumber");
                if (cond1.Count > 0)
                {
                    if (oper1.Length > 0)
                    {
                        filter.ConditionUpd(cond1[0].propertyid, oper1);
                        filter.ConditionValuesDel(cond1[0].propertyid);
                        if (text1.Length > 0) filter.ConditionValueAdd(cond1[0].propertyid, text1, 0);
                        if (text2.Length > 0) filter.ConditionValueAdd(cond1[0].propertyid, text2, 0);
                    }
                    else
                    {
                        filter.ConditionDel(cond1[0].propertyid);
                    }
                }
                else if (oper1.Length > 0)
                {
                    int idCondition = filter.ConditionAdd(filter.FilterSQLID, "parcelnumber", oper1);
                    if (text1.Length > 0) filter.ConditionValueAdd(idCondition, text1, 0);
                    if (text2.Length > 0) filter.ConditionValueAdd(idCondition, text2, 0);
                }
                //List<SQLFilterCondition> cond2 = filter.ConditionGet(filter.FilterSQLID, "lorry");
                //if (cond2.Count > 0)
                //{
                //    if (oper2.Length > 0)
                //    {
                //        filter.ConditionUpd(cond2[0].propertyid, oper2);
                //        filter.ConditionValuesDel(cond2[0].propertyid);
                //        if (text11.Length > 0) filter.ConditionValueAdd(cond2[0].propertyid, text11, 0);
                //        if (text21.Length > 0) filter.ConditionValueAdd(cond2[0].propertyid, text21, 0);
                //    }
                //    else
                //    {
                //        filter.ConditionDel(cond2[0].propertyid);
                //    }
                //}
                //else if (oper2.Length > 0)
                //{
                //    int idCondition = filter.ConditionAdd(filter.FilterSQLID, "lorry", oper2);
                //    if (text11.Length > 0) filter.ConditionValueAdd(idCondition, text11, 0);
                //    if (text21.Length > 0) filter.ConditionValueAdd(idCondition, text21, 0);
                //}
                isChanchedNumber = false;
            }
            if (isChanchedDate)
            {
                filter.SetDate(filter.FilterSQLID, "DateTran", "operdate", startDatePicker.SelectedDate, stopDatePicker.SelectedDate);
                isChanchedDate = false;
            }
            if (isChanchedDescr)
            {
                filter.SetString(filter.FilterSQLID, "descr", descrTextBox.Text);
                isChanchedDescr = false;
            }
            if (isChanchedSum)
            {
                filter.SetNumber(filter.FilterSQLID, "opersum", sumComboBox.SelectedIndex, sumTextBox.Text);
                isChanchedSum = false;
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
                filter.SetList(filter.FilterSQLID, "legal", values);
                isChanchedLegal = false;
            }
        }
        private void Fill()
        {
            string str1=null, str2=null;
            List<SQLFilterCondition> cond1 = filter.ConditionGet(filter.FilterSQLID, "parcelnumber");
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
                startNumberTextBox.Text = str1;
                stopNumberTextBox.Text = str2;
            }
            else
            {
                startNumberTextBox.Clear();
                stopNumberTextBox.Clear();
            }
            //List<SQLFilterCondition> cond2 = filter.ConditionGet(filter.FilterSQLID, "lorry");
            //if (cond2.Count > 0)
            //{
            //    List<SQLFilterValue> values = filter.ValueGet(cond2[0].propertyid);
            //    switch (cond2[0].propertyOperator.ToLower())
            //    {
            //        case "between":
            //            int v1 = int.Parse(values[0].value);
            //            int v2 = int.Parse(values[1].value);
            //            if (v1 < v2)
            //            {
            //                str1 = values[0].value;
            //                str2 = v2.ToString();
            //            }
            //            else
            //            {
            //                str1 = v2.ToString();
            //                str2 = values[0].value;
            //            }
            //            break;
            //        case ">=":
            //            str1 = values[0].value;
            //            break;
            //        case "<=":
            //            str2 = values[0].value;
            //            break;
            //    }
            //}
            //if (str1.Length + str2.Length > 0)
            //{

            //}
            //else
            //{
            //    startNumberTextBox.Clear();
            //    stopNumberTextBox.Clear();
            //}
            string date1, date2;
            filter.PullDate(filter.FilterSQLID, "DateTran", "operdate", out date1, out date2);
            startDatePicker.Text = date1;
            stopDatePicker.Text = date2;
            string text; byte selindex;
            filter.PullString(filter.FilterSQLID, "descr", out text);
            this.descrTextBox.Text = text;
            filter.PullNumber(filter.FilterSQLID, "opersum", out text, out selindex);
            this.sumComboBox.SelectedIndex = selindex;
            this.sumTextBox.Text = text;
            filter.PullListBox(filter.FilterSQLID, "legal", "accountid", this.legalListBox,true);
            isChanchedNumber = false;
            isChanchedDate = false;
            isChanchedSum = false;
            isChanchedDescr = false;
            isChanchedLegal = false;
        }
        private void ClearFilter()
        {
            startNumberTextBox.Clear();
            stopNumberTextBox.Clear();
            startDatePicker.Text = string.Empty;
            stopDatePicker.Text = string.Empty;
            sumComboBox.SelectedIndex = 0;
            this.sumTextBox.Clear();
            descrTextBox.Clear();
            legalListBox.SelectedItems.Clear();
        }
    }
}
