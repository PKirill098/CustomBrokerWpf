using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ParcelFilterWin.xaml
    /// </summary>
    public partial class ParcelFilterWin : Window
    {
        private lib.SQLFilter.SQLFilter filter;
        private lib.Interfaces.IFilterWindowOwner myfilterowner;
        internal lib.Interfaces.IFilterWindowOwner FilterOwner
        {
            set { myfilterowner = value; }
            get { return myfilterowner; }
        }
        private bool isChanchedNumber;
        private bool isChanchedShipnumber;
        private bool isChanchedShipPlanDate;
        private bool isChanchedShipDate;
        private bool isChanchedPreparation;
        private bool isChanchedBorderDate;
        private bool isChanchedTerminalIn;
        private bool isChanchedTerminalOut;
        private bool isChanchedUnloaded;
        private bool isChanchedCarrier;
        private bool isChanchedCarrierPerson;
        private bool isChanchedCarrierTel;
        private bool isChanchedTrucker;
        private bool isChanchedTruckerTel;
        private bool isChanchedLorry;
        private bool isChanchedLorryVolume;
        private bool isChanchedLorryWeight;
        private bool isChanchedLorryVin;
        private bool isChanchedTrailerVin;
        private bool isChanchedParcelType;
        private bool isChanchedStatus;
        private bool isChanchedGoodsType;

        public ParcelFilterWin()
        {
            InitializeComponent();
        }

        private void RunFilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IInputElement focelm = FocusManager.GetFocusedElement(this);
                FocusManager.SetFocusedElement(this, RunFilterButton);
                Actualization();
                if(myfilterowner!=null)
                    myfilterowner.RunFilter(null);
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

        private void Actualization()
        {
            if (isChanchedNumber)
            {
                int n;
                string text1 = startNumberTextBox.Text;
                n = startNumberTextBox.Text.IndexOf('-');
                if (n > 0) text1 = startNumberTextBox.Text.Substring(0, n);
                string text2 = stopNumberTextBox.Text;
                n = stopNumberTextBox.Text.IndexOf('-');
                if (n > 0) text2 = stopNumberTextBox.Text.Substring(0, n);
                string oper = text1.Length > 0 & text2.Length > 0 ? "between" : text1.Length > 0 ? ">=" : text2.Length > 0 ? "<=" : "";
                List<lib.SQLFilter.SQLFilterCondition> cond = filter.ConditionGet(filter.FilterWhereId, "parcelnumber");
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
                    int idCondition = filter.ConditionAdd(filter.FilterWhereId, "parcelnumber", oper);
                    if (text1.Length > 0) filter.ConditionValueAdd(idCondition, text1, 0);
                    if (text2.Length > 0) filter.ConditionValueAdd(idCondition, text2, 0);
                }
                isChanchedNumber = false;
            }
            if (isChanchedShipnumber)
            {
                string oper = "=";
                switch (shipnumberComboBox.SelectedIndex)
                {
                    case 0:
                        oper = "=";
                        break;
                    case 1:
                        oper = "is null";
                        break;
                }
                List<lib.SQLFilter.SQLFilterCondition> cond = filter.ConditionGet(filter.FilterWhereId, "shipmentnumber");
                if (cond.Count > 0)
                {
                    if (shipnumberComboBox.SelectedIndex == 0 & shipnumberTextBox.Text.Length == 0)
                    {
                        filter.ConditionDel(cond[0].propertyid);
                    }
                    else
                    {
                        if (cond[0].propertyOperator != oper) filter.ConditionUpd(cond[0].propertyid, oper);
                        List<lib.SQLFilter.SQLFilterValue> val = filter.ValueGet(cond[0].propertyid);
                        if (val.Count > 0)
                        {
                            filter.ConditionValueUpd(val[0].valueId, shipnumberTextBox.Text, 0);
                        }
                        else
                        {
                            filter.ConditionValueAdd(cond[0].propertyid, shipnumberTextBox.Text, 0);
                        }
                    }
                }
                else if (!(shipnumberComboBox.SelectedIndex == 0 & shipnumberTextBox.Text.Length == 0))
                {
                    filter.ConditionValueAdd(filter.ConditionAdd(filter.FilterWhereId, "shipmentnumber", oper), shipnumberTextBox.Text, 0);
                }
                isChanchedShipnumber = false;
            }
            if (isChanchedShipPlanDate)
            {
                filter.SetDate(filter.FilterWhereId, "ShipPlanDate", "shipplandate", startShipplandatePicker.SelectedDate, stopShipplandatePicker.SelectedDate);
                isChanchedShipPlanDate = false;
            }
            if (isChanchedShipDate)
            {
                filter.SetDate(filter.FilterWhereId, "ShipDate", "shipdate", startShipdatePicker.SelectedDate, stopShipdatePicker.SelectedDate);
                isChanchedShipDate = false;
            }
            if (isChanchedPreparation)
            {
                filter.SetDate(filter.FilterWhereId, "PreparationDate", "preparation", startPreparationPicker.SelectedDate, stopPreparationPicker.SelectedDate);
                isChanchedPreparation = false;
            }
            if (isChanchedBorderDate)
            {
                filter.SetDate(filter.FilterWhereId, "BorderDate", "borderdate", startBorderdatePicker.SelectedDate, stopBorderdatePicker.SelectedDate);
                isChanchedBorderDate = false;
            }
            if (isChanchedTerminalIn)
            {
                filter.SetDate(filter.FilterWhereId, "TerminalInDate", "terminalin", startTerminalinPicker.SelectedDate, stopTerminalinPicker.SelectedDate);
                isChanchedTerminalIn = false;
            }
            if (isChanchedTerminalOut)
            {
                filter.SetDate(filter.FilterWhereId, "TerminalOutDate", "terminalout", startTerminaloutPicker.SelectedDate, stopTerminaloutPicker.SelectedDate);
                isChanchedTerminalOut = false;
            }
            if (isChanchedUnloaded)
            {
                filter.SetDate(filter.FilterWhereId, "UnloadedDate", "unloaded", startUnloadedPicker.SelectedDate, stopUnloadedPicker.SelectedDate);
                isChanchedUnloaded = false;
            }
            if (isChanchedCarrier)
            {
                filter.SetString(filter.FilterWhereId, "carrier", carrierTextBox.Text);
                isChanchedCarrier = false;
            }
            if (isChanchedCarrierPerson)
            {
                filter.SetString(filter.FilterWhereId, "carrierperson", carrierpersonTextBox.Text);
                isChanchedCarrierPerson = false;
            }
            if (isChanchedCarrierTel)
            {
                filter.SetString(filter.FilterWhereId, "carriertel", carriertelTextBox.Text);
                isChanchedCarrierTel = false;
            }
            if (isChanchedTrucker)
            {
                filter.SetString(filter.FilterWhereId, "trucker", truckerTextBox.Text);
                isChanchedTrucker = false;
            }
            if (isChanchedTruckerTel)
            {
                filter.SetString(filter.FilterWhereId, "truckertel", truckertelTextBox.Text);
                isChanchedTruckerTel = false;
            }
            if (isChanchedLorry)
            {
                filter.SetString(filter.FilterWhereId, "lorry", lorryTextBox.Text);
                isChanchedLorry = false;
            }
            if (isChanchedLorryVolume)
            {
                lib.SQLFilter.Operators oper = lib.SQLFilter.Operators.Equal;
                switch (volumeComboBox.SelectedIndex)
                {
                    case 1:
                        oper = lib.SQLFilter.Operators.Greater;
                        break;
                    case 2:
                        oper = lib.SQLFilter.Operators.Less;
                        break;
                }
                filter.SetNumber(filter.FilterWhereId, "lorryvolume", oper, volumeTextBox.Text);
                isChanchedLorryVolume = false;
            }
            if (isChanchedLorryWeight)
            {
                lib.SQLFilter.Operators oper = lib.SQLFilter.Operators.Equal;
                switch (weightComboBox.SelectedIndex)
                {
                    case 1:
                        oper = lib.SQLFilter.Operators.Greater;
                        break;
                    case 2:
                        oper = lib.SQLFilter.Operators.Less;
                        break;
                }
                filter.SetNumber(filter.FilterWhereId, "lorrytonnage", oper, weightTextBox.Text);
                isChanchedLorryWeight = false;
            }
            if (isChanchedLorryVin)
            {
                filter.SetString(filter.FilterWhereId, "lorryvin", lorryvinTextBox.Text);
                isChanchedLorryVin = false;
            }
            if (isChanchedTrailerVin)
            {
                filter.SetString(filter.FilterWhereId, "trailervin", trailervinTextBox.Text);
                isChanchedTrailerVin = false;
            }
            //if (isChanchedParcelType)
            //{
            //    int i = 0;
            //    string[] values = new string[this.parcelTypeListBox.SelectedItems.Count];
            //    foreach (System.Data.DataRowView rowview in this.parcelTypeListBox.SelectedItems)
            //    {
            //        ReferenceDS.tableParcelTypeRow row = rowview.Row as ReferenceDS.tableParcelTypeRow;
            //        values[i] = row.parceltypeid.ToString();
            //        i++;
            //    }
            //    filter.SetList(filter.FilterWhereId, "parceltype", values);
            //    isChanchedParcelType = false;
            //}
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
                    values[i] = rowview.Id.ToString();
                    i++;
                }
                filter.SetList(filter.FilterWhereId, "goodstype", values);
                isChanchedGoodsType = false;
            }
        }
        private void ClearFilter()
        {
            startNumberTextBox.Clear();
            stopNumberTextBox.Clear();
            this.shipnumberComboBox.SelectedIndex = 0;
            this.shipnumberTextBox.Clear();
            startShipplandatePicker.Text = string.Empty;
            stopShipplandatePicker.Text = string.Empty;
            startShipdatePicker.Text = string.Empty;
            stopShipdatePicker.Text = string.Empty;
            startPreparationPicker.Text = string.Empty;
            stopPreparationPicker.Text = string.Empty;
            startBorderdatePicker.Text = string.Empty;
            stopBorderdatePicker.Text = string.Empty;
            startTerminalinPicker.Text = string.Empty;
            stopTerminalinPicker.Text = string.Empty;
            startTerminaloutPicker.Text = string.Empty;
            stopTerminaloutPicker.Text = string.Empty;
            startUnloadedPicker.Text = string.Empty;
            stopUnloadedPicker.Text = string.Empty;
            carrierTextBox.Clear();
            carrierpersonTextBox.Clear();
            carriertelTextBox.Clear();
            truckerTextBox.Clear();
            truckertelTextBox.Clear();
            lorryTextBox.Clear();
            volumeComboBox.SelectedIndex = 0;
            volumeTextBox.Clear();
            weightComboBox.SelectedIndex = 0;
            weightTextBox.Clear();
            lorryvinTextBox.Clear();
            trailervinTextBox.Clear();
            parcelTypeListBox.SelectedItems.Clear();
            statusListBox.SelectedItems.Clear();
            goodstypeListBox.SelectedItems.Clear();
        }
        private void Fill()
        {
            List<lib.SQLFilter.SQLFilterCondition> cond = filter.ConditionGet(filter.FilterWhereId, "parcelnumber");
            if (cond.Count > 0)
            {
                List<lib.SQLFilter.SQLFilterValue> values = filter.ValueGet(cond[0].propertyid);
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
            List<lib.SQLFilter.SQLFilterCondition> listCond;
            List<lib.SQLFilter.SQLFilterValue> listValue;
            listCond = filter.ConditionGet(filter.FilterWhereId, "shipmentnumber");
            if (listCond.Count > 0)
            {
                switch (listCond[0].propertyOperator.ToLower())
                {
                    case "=":
                        this.shipnumberComboBox.SelectedIndex = 0;
                        listValue = filter.ValueGet(listCond[0].propertyid);
                        if (listValue.Count > 0) this.shipnumberTextBox.Text = listValue[0].value;
                        break;
                    case "is null":
                        this.shipnumberComboBox.SelectedIndex = 1;
                        break;
                }
            }
            string date1, date2;
            filter.PullDate(filter.FilterWhereId, "ShipPlanDate", "shipplandate", out date1, out date2);
            startShipplandatePicker.Text = date1;
            stopShipplandatePicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "ShipDate", "shipdate", out date1, out date2);
            startShipdatePicker.Text = date1;
            stopShipdatePicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "PreparationDate", "preparation", out date1, out date2);
            startPreparationPicker.Text = date1;
            stopPreparationPicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "BorderDate", "borderdate", out date1, out date2);
            startBorderdatePicker.Text = date1;
            stopBorderdatePicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "TerminalInDate", "terminalin", out date1, out date2);
            startTerminalinPicker.Text = date1;
            stopTerminalinPicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "TerminalOutDate", "terminalout", out date1, out date2);
            startTerminaloutPicker.Text = date1;
            stopTerminaloutPicker.Text = date2;
            filter.PullDate(filter.FilterWhereId, "UnloadedDate", "unloaded", out date1, out date2);
            startUnloadedPicker.Text = date1;
            stopUnloadedPicker.Text = date2;
            string text; byte selindex;
            filter.PullString(filter.FilterWhereId, "carrier", out text);
            this.carrierTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "carrierperson", out text);
            this.carrierpersonTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "carriertel", out text);
            this.carriertelTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "trucker", out text);
            this.truckerTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "truckertel", out text);
            this.truckertelTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "lorry", out text);
            this.lorryTextBox.Text = text;
            filter.PullNumber(filter.FilterWhereId, "lorryvolume", out text, out selindex);
            this.volumeComboBox.SelectedIndex = selindex;
            this.volumeTextBox.Text = text;
            filter.PullNumber(filter.FilterWhereId, "lorrytonnage", out text, out selindex);
            this.weightComboBox.SelectedIndex = selindex;
            this.weightTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "lorryvin", out text);
            this.lorryvinTextBox.Text = text;
            filter.PullString(filter.FilterWhereId, "trailervin", out text);
            this.trailervinTextBox.Text = text;

            filter.PullListBox(filter.FilterWhereId, "parceltype", "parceltypeid", this.parcelTypeListBox,true);
            filter.PullListBox(filter.FilterWhereId, "parcelstatus", "rowId", this.statusListBox, true);
            filter.PullListBox(filter.FilterWhereId, "goodstype", "Iditem", this.goodstypeListBox, true);
            isChanchedNumber=false;
            isChanchedShipnumber = false;
            isChanchedShipPlanDate = false;
            isChanchedShipDate = false;
            isChanchedPreparation = false;
            isChanchedBorderDate = false;
            isChanchedTerminalIn = false;
            isChanchedTerminalOut = false;
            isChanchedUnloaded = false;
            isChanchedCarrier = false;
            isChanchedCarrierPerson = false;
            isChanchedCarrierTel = false;
            isChanchedTrucker = false;
            isChanchedTruckerTel = false;
            isChanchedLorry = false;
            isChanchedLorryVolume = false;
            isChanchedLorryWeight = false;
            isChanchedLorryVin = false;
            isChanchedTrailerVin = false;
            isChanchedParcelType = false;
            isChanchedStatus = false;
            isChanchedGoodsType = false;
        }

        #region ChangedEvent
        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedNumber = true;
        }
        private void shipnumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedShipnumber = true;
        }
        private void shipnumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedShipnumber = true;
        }
       private void UnloadedPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedUnloaded = true;
        }
        private void TerminaloutPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedTerminalOut = true;
        }
        private void TerminalinPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedTerminalIn = true;
        }
        private void BorderdatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedBorderDate = true;
        }
        private void PreparationPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedPreparation = true;
        }
        private void ShipdatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedShipDate = true;
        }
        private void ShipplandatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedShipPlanDate = true;
        }
        private void CarrierTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedCarrier = true;
        }
        private void CarrierpersonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedCarrierPerson = true;
        }
        private void CarriertelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedCarrierTel = true;
        }
        private void TruckerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedTrucker = true;
        }
        private void TruckertelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedTruckerTel = true;
        }
        private void LorryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedLorry = true;
        }
        private void VolumeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedLorryVolume = true;
        }
        private void VolumeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedLorryVolume = true;
        }
        private void WeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChanchedLorryWeight = true;
        }
        private void WeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedLorryWeight = true;
        }
        private void LorryvinTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedLorryVin = true;
        }
        private void TrailervinTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanchedTrailerVin = true;
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
        #endregion

        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }

        private void winParcelFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refDS.tableRequestStatus.Count == 0)
            {
                ReferenceDSTableAdapters.RequestStatusAdapter parceltypeAdapter = new ReferenceDSTableAdapters.RequestStatusAdapter();
                parceltypeAdapter.Fill(refDS.tableRequestStatus);
            }
            System.Data.DataView statusview = new System.Data.DataView(refDS.tableRequestStatus, "rowId>49", "rowId", DataViewRowState.CurrentRows);
            this.statusListBox.ItemsSource = statusview;
            //if (refDS.tableParcelType.Count == 0)
            //{
            //    ReferenceDSTableAdapters.ParcelTypeAdapter parceltypeAdapter = new ReferenceDSTableAdapters.ParcelTypeAdapter();
            //    parceltypeAdapter.Fill(refDS.tableParcelType);
            //}
            //System.Data.DataView typeview = new System.Data.DataView(refDS.tableParcelType);
            //this.parcelTypeListBox.ItemsSource = typeview;
            this.goodstypeListBox.ItemsSource = CustomBrokerWpf.References.GoodsTypesParcel;
            if (myfilterowner != null)
                filter = myfilterowner.Filter;
            Fill();
        }
        private void winParcelFilter_Closed(object sender, EventArgs e)
        {
            if (myfilterowner != null)
                myfilterowner.IsShowFilterWindow = false;
        }
    }
}
