using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class EventLogFilterWin : Window
    {
        private SQLFilter filter;

        private bool isChangedHide;
        private bool isChangedWhen;
        private bool isChangedWho;
        private bool isChangedWhat;

        public EventLogFilterWin()
        {
            InitializeComponent();
        }

        private void winFilter_Loaded(object sender, RoutedEventArgs e)
        {
            this.WhoListBox.ItemsSource = References.Users;
            this.WhatListBox.ItemsSource = References.EventLogTypes;
            filter = (this.Owner as ISQLFiltredWindow).Filter;
            Fill();
        }
        private void winFilter_Closed(object sender, EventArgs e)
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            isChangedHide = true;
        }
        private void PeriodDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChangedWhen = true;
        }
        private void WhoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChangedWho = true;
        }
        private void WhatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChangedWhat = true;
        }

        private void Fill()
        {
            List<SQLFilterCondition> cond1 = filter.ConditionGet(filter.FilterWhereId, "hide");
            if (cond1.Count > 0)
            {
                List<SQLFilterValue> values = filter.ValueGet(cond1[0].propertyid);
                this.HideCheckBox.IsChecked = values[0].value != "0";
            }
            else
            {
                this.HideCheckBox.IsChecked = false;
            }
            string date1, date2;
            filter.PullDate(filter.FilterWhereId, "when", "whenstart", out date1, out date2);
            startPeriodDatePicker.Text = date1;
            filter.PullDate(filter.FilterWhereId, "when", "whenstop", out date1, out date2);
            stopPeriodDatePicker.Text = date2;
            filter.PullListBox(filter.FilterWhereId, "who", "Name", this.WhoListBox, true);
            filter.PullListBox(filter.FilterWhereId, "what", "Name", this.WhatListBox, true);
        }
        private void Actualization()
        {
            if (isChangedHide | isChangedWhen | isChangedWho | isChangedWhat)
            {
                List<SQLFilterCondition> cond1 = filter.ConditionGet(filter.FilterWhereId, "hide");
                if (cond1.Count > 0)
                {
                    List<SQLFilterValue> values = filter.ValueGet(cond1[0].propertyid);
                    if (values.Count > 0)
                    {
                        if (this.HideCheckBox.IsChecked.Value)
                            filter.ConditionValueUpd(values[0].valueId, "inform", 1);
                        else
                            filter.ConditionValueUpd(values[0].valueId, "0", 0);
                    }
                    else
                    {
                        if (this.HideCheckBox.IsChecked.Value)
                            filter.ConditionValueAdd(cond1[0].propertyid, "inform", 1);
                        else
                            filter.ConditionValueAdd(cond1[0].propertyid, "0", 0);
                    }
                }
                else
                    if (this.HideCheckBox.IsChecked.Value)
                    filter.ConditionValueAdd(filter.ConditionAdd(filter.FilterWhereId, "hide", "="), "inform", 1);
                else
                    filter.ConditionValueAdd(filter.ConditionAdd(filter.FilterWhereId, "hide", "="), "0", 0);
                isChangedHide = false;
            }
            if (isChangedWhen)
            {
                filter.SetDatePeriod(filter.FilterWhereId, "when", "whenstart", "whenstop", startPeriodDatePicker.SelectedDate, stopPeriodDatePicker.SelectedDate);
                isChangedWhen = false;
            }
            if (isChangedWho)
            {
                int i = 0;
                string[] values = new string[this.WhoListBox.SelectedItems.Count];
                foreach (Classes.Principal item in this.WhoListBox.SelectedItems)
                {
                    values[i] = item.Name;
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "who", values);
                isChangedWho = false;
            }
            if (isChangedWhat)
            {
                int i = 0;
                string[] values = new string[this.WhatListBox.SelectedItems.Count];
                foreach (Classes.EventLogType item in this.WhatListBox.SelectedItems)
                {
                    values[i] = item.Name;
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "what", values);
                isChangedWhat = false;
            }
        }
        private void ClearFilter()
        {
            HideCheckBox.IsChecked = false;
            startPeriodDatePicker.Text = string.Empty;
            stopPeriodDatePicker.Text = string.Empty;
            WhoListBox.SelectedItems.Clear();
            WhatListBox.SelectedItems.Clear();
        }

    }
}
