using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для CustomerBalanceFilterWin.xaml
    /// </summary>
    public partial class CustomerBalanceFilterWin : Window
    {
        private bool isChanchedDelay;
        private bool isChanchedDebtor;
        private bool isChanchedSum;
        private bool isChanchedCustomer;
        private bool isChanchedParcel;

        private SQLFilter custfilter;
        private SQLFilter tranfilter;

        public CustomerBalanceFilterWin()
        {
            InitializeComponent();
        }

        private void winCustomerBalanceFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS ds = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (ds.tableCustomerName.Count == 0)
            {
                ReferenceDSTableAdapters.CustomerNameAdapter custadapter = new ReferenceDSTableAdapters.CustomerNameAdapter();
                custadapter.Fill(ds.tableCustomerName);
            }
            System.Data.DataView customerview = new System.Data.DataView(ds.tableCustomerName, string.Empty, "[customerName]", System.Data.DataViewRowState.CurrentRows);
            this.customerListBox.ItemsSource = customerview;
            if (ds.tableFullNumber.Count == 0)
            {
                ReferenceDSTableAdapters.FullNumberAdapter parceladapter = new ReferenceDSTableAdapters.FullNumberAdapter();
                parceladapter.Fill(ds.tableFullNumber);
            }
            System.Data.DataView parcelview = new System.Data.DataView(ds.tableFullNumber, string.Empty, "[sort] Desc", System.Data.DataViewRowState.CurrentRows);
            this.parcelListBox.ItemsSource = parcelview;
            this.parcelListBox.SelectAll();
            custfilter = (this.Owner as CustomerBalanceWin).CustFilter;
            tranfilter = (this.Owner as CustomerBalanceWin).TranFilter;
            Fill();
        }
        private void winCustomerBalanceFilter_Closed(object sender, EventArgs e)
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
            custfilter.RemoveCurrentWhere();
            custfilter.GetDefaultFilter(SQLFilterPart.Where);
            tranfilter.RemoveCurrentWhere();
            tranfilter.GetDefaultFilter(SQLFilterPart.Where);
            Fill();
        }
        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFilter();
        }
        private void RemoveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            custfilter.RemoveCurrentWhere();
            tranfilter.RemoveCurrentWhere();
        }
        private void SaveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Фильтр по умолчанию будет заменён текущим фильтром.\nПродолжить?", "Сохранение фильтра", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
            {
                try
                {
                    Actualization();
                    custfilter.SetDefaultFilterWhere();
                    tranfilter.SetDefaultFilterWhere();
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

        private void debtorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            isChanchedDebtor = true;
            if (debtorCheckBox.IsChecked.Value)
            {
                this.sumComboBox.IsEnabled = false;
                this.sumTextBox.IsEnabled = false;
            }
            else
            {
                this.sumComboBox.IsEnabled = true;
                this.sumTextBox.IsEnabled = true;
            }
        }
        private void delayPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedDelay = true;
        }
        private void SumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedSum = true;
        }
        private void SumTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedSum = true;
        }
        private void customerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedCustomer = true;
        }
        private void parcelListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedParcel = true;
        }
        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }

        private void Fill()
        {
            this.delayPicker.Text =(this.Owner as CustomerBalanceWin).LastDateInvoice.AddDays(-1D).ToShortDateString();
            string text; byte selindex;
            custfilter.PullNumber(custfilter.FilterWhereId, "balance", out text, out selindex);
            if (text.Length>0 && (text.Equals("0.005") & selindex == 1))
            {
                this.sumComboBox.IsEnabled = false;
                this.sumTextBox.IsEnabled = false;
                this.debtorCheckBox.IsChecked = true;
            }
            else
            {
                this.sumComboBox.IsEnabled = true;
                this.sumTextBox.IsEnabled = true;
                this.debtorCheckBox.IsChecked = false;
                this.sumComboBox.SelectedIndex = selindex;
                this.sumTextBox.Text = text;
            }
            custfilter.PullListBox(custfilter.FilterWhereId, "customerid", "customerID", this.customerListBox,true);
            custfilter.PullListBox(tranfilter.FilterWhereId, "customerid", "customerID", this.parcelListBox,false);
            isChanchedDelay=false;
            isChanchedDebtor = false;
            isChanchedSum = false;
            isChanchedCustomer = false;
            isChanchedParcel = false;
        }
        private void Actualization()
        {
            if (isChanchedDelay)
            {
                (this.Owner as CustomerBalanceWin).LastDateInvoice = this.delayPicker.SelectedDate.GetValueOrDefault(DateTime.Today).AddDays(1D);
                isChanchedDelay = false;
            }
            if (this.debtorCheckBox.IsChecked.Value)
            {
                if (isChanchedDebtor)
                {
                    custfilter.SetNumber(custfilter.FilterWhereId, "balance", 1, "0.005");
                    isChanchedDebtor = false;
                }
            }
            else
            {
                if (isChanchedSum | isChanchedDebtor)
                {
                    custfilter.SetNumber(custfilter.FilterWhereId, "balance", sumComboBox.SelectedIndex, sumTextBox.Text);
                    isChanchedDebtor = false;
                    isChanchedSum = false;
                }
            }
            if (isChanchedCustomer)
            {
                int i = 0;
                string[] values = new string[this.customerListBox.SelectedItems.Count];
                foreach (System.Data.DataRowView rowview in this.customerListBox.SelectedItems)
                {
                    ReferenceDS.tableCustomerNameRow row = rowview.Row as ReferenceDS.tableCustomerNameRow;
                    values[i] = row.customerID.ToString();
                    i++;
                }
                custfilter.SetList(custfilter.FilterWhereId, "customerid", values);
                isChanchedCustomer = false;
            }
            if (isChanchedParcel)
            {
                int i = 0;
                string[] values = new string[this.parcelListBox.Items.Count - this.parcelListBox.SelectedItems.Count];
                if (this.parcelListBox.Items.Count - this.parcelListBox.SelectedItems.Count > 0)
                {
                    foreach (System.Data.DataRowView rowview in this.parcelListBox.Items)
                    {
                        if (!parcelListBox.SelectedItems.Contains(rowview))
                        {
                            ReferenceDS.tableFullNumberRow row = rowview.Row as ReferenceDS.tableFullNumberRow;
                            values[i] = row.parcelId.ToString();
                            i++;
                        }
                    }
                }
                tranfilter.SetList(tranfilter.FilterWhereId, "noparcel", values);
                isChanchedCustomer = false;
            }

        }
        private void ClearFilter()
        {
            this.delayPicker.Text = string.Empty;
            this.debtorCheckBox.IsChecked = false;
            this.sumComboBox.SelectedIndex = 0;
            this.sumTextBox.Clear();
            customerListBox.SelectedItems.Clear();
            this.parcelListBox.SelectAll();
        }

    }
}
