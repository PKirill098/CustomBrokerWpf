using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для StoreMergeFilterWin.xaml
    /// </summary>
    public partial class StoreMergeFilterWin : Window
    {
        private bool isChanchedRequest;
        private bool isChanchedStorageDate;
        private bool isChanchedStoragePoint;
        private bool isChanchedcellNumber;
        private bool isChanchedvolume;
        private bool isChanchedBrutto;
        private bool isChanchedNetto;
        private bool isChanchedgoodValue;
        private bool isChanchedStorageNote;
        private bool isChanchedCustomer;
        private bool isChanchedAgent;
        private bool isChanchedStore;
        private lib.SQLFilter.SQLFilter filter;

        public StoreMergeFilterWin()
        {
            InitializeComponent();
        }

        private void winStoreMergeFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS ds = this.FindResource("keyReferenceDS") as ReferenceDS;
            ListCollectionView storeview = new ListCollectionView(CustomBrokerWpf.References.Stores);
            storeListBox.ItemsSource = storeview;
            filter = (this.Owner as StoreMergeWin).Filter;
            Fill();
        }
        private void winStoreMergeFilter_Closed(object sender, EventArgs e)
        {
            (this.Owner as StoreMergeWin).IsShowFilter = false;
        }

        #region ToolBar
        private void RunFilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IInputElement focelm = FocusManager.GetFocusedElement(this);
                FocusManager.SetFocusedElement(this, RunFilterButton);
                Actualization();
                ((this.Owner as StoreMergeWin).DataContext as StorageDataManager).Refresh.Execute(null);
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
            filter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
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
                    filter.SetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
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
        #endregion
        #region Set Changed
        private void requestComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedRequest = true;
            if (requestComboBox.SelectedIndex > 0) requestTextBox.Clear();
        }
        private void requestTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedRequest = true;
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
            isChanchedBrutto = true;
        }
        private void officialWeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedBrutto = true;
        }
        private void actualWeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedNetto = true;
        }
        private void actualWeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedNetto = true;
        }
        private void goodValueComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedgoodValue = true;
        }
        private void goodValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedgoodValue = true;
        }
        private void customerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedCustomer = true;
        }
        private void agentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedAgent = true;
        }
        private void storageNoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedStorageNote = true;
        }
        private void storeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedStore = true;
        }
        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }
       #endregion

        private void Fill()
        {
            isChanchedRequest = false;
            isChanchedStorageDate = false;
            isChanchedStoragePoint = false;
            isChanchedcellNumber = false;
            isChanchedvolume = false;
            isChanchedBrutto = false;
            isChanchedNetto = false;
            isChanchedgoodValue = false;
            isChanchedStorageNote = false;
            isChanchedCustomer = false;
            isChanchedAgent = false;
            isChanchedStore = false;

            List<lib.SQLFilter.SQLFilterCondition> listCond;
            List<lib.SQLFilter.SQLFilterValue> listValue;

            string date1, date2;
            filter.PullDate(filter.FilterWhereId, "StorageDate", "storageDate", out date1, out date2);
            startStorageDatePicker.Text = date1;
            stopStorageDatePicker.Text = date2;

            listCond = filter.ConditionGet(filter.FilterWhereId, "requestId");
            if (listCond.Count > 0)
            {
                switch (listCond[0].propertyOperator.ToLower())
                {
                    case "=":
                        this.requestComboBox.SelectedIndex = 0;
                        break;
                    case "is null":
                        this.requestComboBox.SelectedIndex = 1;
                        break;
                }
                listValue = filter.ValueGet(listCond[0].propertyid);
                if (listValue.Count > 0) this.requestTextBox.Text = listValue[0].value;
            }
            listCond = filter.ConditionGet(filter.FilterWhereId, "storagePoint");
            if (listCond.Count > 0)
            {
                switch (listCond[0].propertyOperator.ToLower())
                {
                    case "=":
                        this.storagePointComboBox.SelectedIndex = 0;
                        listValue = filter.ValueGet(listCond[0].propertyid);
                        if (listValue.Count > 0) this.storagePointTextBox.Text = listValue[0].value;
                        break;
                    case "is null":
                        this.storagePointComboBox.SelectedIndex = 1;
                        break;
                }
            }

            string text; byte selindex;
            filter.PullNumber(filter.FilterWhereId, "cellnumber", out text, out selindex);
            this.cellNumberComboBox.SelectedIndex = selindex;
            this.cellNumberTextBox.Text = text;
            filter.PullNumber(filter.FilterWhereId, "volume", out text, out selindex);
            this.volumeComboBox.SelectedIndex = selindex;
            this.volumeTextBox.Text = text;
            filter.PullNumber(filter.FilterWhereId, "grossweight", out text, out selindex);
            this.officialWeightComboBox.SelectedIndex = selindex;
            this.officialWeightTextBox.Text = text;
            filter.PullNumber(filter.FilterWhereId, "netweight", out text, out selindex);
            this.actualWeightComboBox.SelectedIndex = selindex;
            this.actualWeightTextBox.Text = text;
            filter.PullNumber(filter.FilterWhereId, "goodvalue", out text, out selindex);
            this.goodValueComboBox.SelectedIndex = selindex;
            this.goodValueTextBox.Text = text;

            filter.PullString(filter.FilterWhereId, "customer", out text);
            this.customerTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "agent", out text);
            this.agentTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "storageNote", out text);
            this.storageNoteTextBox.Text = text;

            filter.PullListBox(filter.FilterWhereId, "storeId", "Id", this.storeListBox, true);
        }
        private void Actualization()
        {
            if (isChanchedRequest)
            {
                string oper = "=";
                switch (requestComboBox.SelectedIndex)
                {
                    case 0:
                        oper = "=";
                        break;
                    case 1:
                        oper = "is null";
                        break;
                }
                List<lib.SQLFilter.SQLFilterCondition> cond = filter.ConditionGet(filter.FilterWhereId, "requestId");
                if (cond.Count > 0)
                {
                    if (requestComboBox.SelectedIndex == 0 & requestTextBox.Text.Length == 0)
                    {
                        filter.ConditionDel(cond[0].propertyid);
                    }
                    else
                    {
                        if (cond[0].propertyOperator != oper) filter.ConditionUpd(cond[0].propertyid, oper);
                        List<lib.SQLFilter.SQLFilterValue> val = filter.ValueGet(cond[0].propertyid);
                        if (val.Count > 0)
                        {
                            filter.ConditionValueUpd(val[0].valueId, requestTextBox.Text, 0);
                        }
                        else
                        {
                            filter.ConditionValueAdd(cond[0].propertyid, requestTextBox.Text, 0);
                        }
                    }
                }
                else if (!(requestComboBox.SelectedIndex == 0 & requestTextBox.Text.Length == 0))
                {
                    filter.ConditionValueAdd(filter.ConditionAdd(filter.FilterWhereId, "requestId", oper), requestTextBox.Text, 0);
                }
                isChanchedRequest = false;
            }
            if (isChanchedStorageDate)
            {
                filter.SetDate(filter.FilterWhereId, "StorageDate", "storageDate", startStorageDatePicker.SelectedDate, stopStorageDatePicker.SelectedDate);
                isChanchedStorageDate = false;
            }
            if (isChanchedStoragePoint)
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
                List<lib.SQLFilter.SQLFilterCondition> cond = filter.ConditionGet(filter.FilterWhereId, "storagePoint");
                if (cond.Count > 0)
                {
                    if (storagePointComboBox.SelectedIndex == 0 & storagePointTextBox.Text.Length == 0)
                    {
                        filter.ConditionDel(cond[0].propertyid);
                    }
                    else
                    {
                        if (cond[0].propertyOperator != oper) filter.ConditionUpd(cond[0].propertyid, oper);
                        List<lib.SQLFilter.SQLFilterValue> val = filter.ValueGet(cond[0].propertyid);
                        if (val.Count > 0)
                        {
                            filter.ConditionValueUpd(val[0].valueId, storagePointTextBox.Text, 0);
                        }
                        else
                        {
                            filter.ConditionValueAdd(cond[0].propertyid, storagePointTextBox.Text, 0);
                        }
                    }
                }
                else if (!(storagePointComboBox.SelectedIndex == 0 & storagePointTextBox.Text.Length == 0))
                {
                    filter.ConditionValueAdd(filter.ConditionAdd(filter.FilterWhereId, "storagePoint", oper), storagePointTextBox.Text, 0);
                }
                isChanchedStoragePoint = false;
            }
            if (isChanchedcellNumber)
            {
                filter.SetNumber(filter.FilterWhereId, "cellnumber", (lib.SQLFilter.Operators)volumeComboBox.SelectedIndex, cellNumberTextBox.Text);
                isChanchedcellNumber = false;
            }
            if (isChanchedvolume)
            {
                filter.SetNumber(filter.FilterWhereId, "volume", (lib.SQLFilter.Operators)volumeComboBox.SelectedIndex, volumeTextBox.Text);
                isChanchedvolume = false;
            }
            if (isChanchedBrutto)
            {
                filter.SetNumber(filter.FilterWhereId, "grossweight", (lib.SQLFilter.Operators)volumeComboBox.SelectedIndex, officialWeightTextBox.Text);
                isChanchedBrutto = false;
            }
            if (isChanchedNetto)
            {
                filter.SetNumber(filter.FilterWhereId, "netweight", (lib.SQLFilter.Operators)volumeComboBox.SelectedIndex, actualWeightTextBox.Text);
                isChanchedNetto = false;
            }
            if (isChanchedgoodValue)
            {
                filter.SetNumber(filter.FilterWhereId, "goodvalue", (lib.SQLFilter.Operators)volumeComboBox.SelectedIndex, goodValueTextBox.Text);
                isChanchedgoodValue = false;
            }
            if (isChanchedCustomer)
            {
                filter.SetString(filter.FilterWhereId, "customer", customerTextBox.Text);
                isChanchedCustomer = false;
            }
            if (isChanchedAgent)
            {
                filter.SetString(filter.FilterWhereId, "agent", agentTextBox.Text);
                isChanchedAgent = false;
            }
            if (isChanchedStorageNote)
            {
                filter.SetString(filter.FilterWhereId, "storagenote", storageNoteTextBox.Text);
                isChanchedStorageNote = false;
            }

            if (isChanchedStore)
            {
                int i = 0;
                string[] values = new string[this.storeListBox.SelectedItems.Count];
                foreach (lib.ReferenceSimpleItem rowview in this.storeListBox.SelectedItems)
                {
                    values[i] = rowview.Id.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "storeId", values);
                isChanchedStore = false;
            }
            (this.Owner as StoreMergeWin).setFilterButtonImage();
        }
        private void ClearFilter()
        {
            this.requestComboBox.SelectedIndex = 0;
            this.requestTextBox.Clear();
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
            this.customerTextBox.Clear();
            this.agentTextBox.Clear();
            this.storageNoteTextBox.Clear();
            this.storeListBox.SelectedItems.Clear();
        }
    }
}
