using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class ParcelReportWin : Window, ISQLFiltredWindow
    {
        List<Dictionary<string, DinamicPropertyItem>> ParcelReportList;
        ManagerDataGridColumnsVM managerdatagrid;

        public ParcelReportWin()
        {
            InitializeComponent();
            ParcelReportList = new List<Dictionary<string, DinamicPropertyItem>>();//ParcelReport
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            managerdatagrid = new ManagerDataGridColumnsVM("ParcelReport", this.mainDataGrid, this.totalGrid, this);
            this.DataContext = managerdatagrid;
            managerdatagrid.DataLoad = DataLoad;
            mainDataGrid.ItemsSource = ParcelReportList;
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(ParcelReportList);
            view.CustomSort = this.managerdatagrid.Sorter;//применение начальной сортировки
            SelectChartComboBox.IsDropDownOpen = true; SelectChartComboBox.IsDropDownOpen = false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            managerdatagrid.Filter.RemoveFilter();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }
        private void toExcelButton_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            try
            {
                string wpfconstr, datasource;
                wpfconstr = KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString;
                datasource = wpfconstr.Substring(wpfconstr.IndexOf("Data Source"), wpfconstr.IndexOf(";", wpfconstr.IndexOf("Initial Catalog")) - wpfconstr.IndexOf("Data Source"));
                Excel.Workbook exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                Excel.PivotCaches caches = exWb.PivotCaches();
                Excel.PivotCache cache = caches.Create(Excel.XlPivotTableSourceType.xlExternal);
                cache.Connection = @"OLEDB;Provider=SQLOLEDB;" + datasource + ";Integrated Security=SSPI";
                cache.CommandType = Excel.XlCmdType.xlCmdSql;
                cache.CommandText = "EXEC dbo.ParcelReport_sp " + this.Filter.FilterWhereId.ToString();
                Excel.PivotTable pivot = cache.CreatePivotTable(exWh.Cells[1, 1], "Parcels", Type.Missing, Type.Missing);
                //pivot.PivotFields("parcelid").Orientation = Excel.XlPivotFieldOrientation.xlHidden;
                //pivot.PivotFields("parcelgroup").DisplayInReport = false;
                //pivot.PivotFields("customerID").DisplayInReport = false;
                pivot.PivotFields("fullnumber").Caption = "Перевозка";
                pivot.PivotFields("lorry").Caption = "Машина";
                pivot.PivotFields("period").Caption = "Период";
                pivot.PivotFields("deliveryprice").Caption = "Стоимость доставки, $/m3";
                pivot.PivotFields("insuranceprice").Caption = "Стоимость страховки, $";
                pivot.PivotFields("usdrate").Caption = "Курс доллара";
                pivot.PivotFields("ratedate").Caption = "Дата курса";
                pivot.PivotFields("shipplandate").Caption = "Дата отгрузки план";
                pivot.PivotFields("terminalout").Caption = "Растаможено";
                pivot.PivotFields("requestId").Caption = "Заявка";
                pivot.PivotFields("customerName").Caption = "Клиент";
                pivot.PivotFields("managergroupName").Caption = "Группа менеджеров";
                pivot.PivotFields("loadDescription").Caption = "Груз";
                pivot.PivotFields("agentName").Caption = "Поставщик";
                pivot.PivotFields("cellNumber").Caption = "Кол-во мест";
                pivot.PivotFields("officialWeight").Caption = "Вес по док, кг";
                pivot.PivotFields("actualWeight").Caption = "Вес факт, кг";
                pivot.PivotFields("calcweight").Caption = "Вес расчетный, кг";
                pivot.PivotFields("volume").Caption = "Объем, м3";
                pivot.PivotFields("goodValue").Caption = "ЕU";
                pivot.PivotFields("customerNote").Caption = "Примечание клиенту";
                pivot.PivotFields("managerNote").Caption = "Примечание менеджера";
                pivot.PivotFields("customs000").Caption = "Таможенный";
                pivot.PivotFields("delivery00").Caption = "Доставка";
                pivot.PivotFields("discount00").Caption = "Наценка";
                pivot.PivotFields("prggermany").Caption = "Скидка";
                pivot.PivotFields("storegermn").Caption = "Доп.услуги";
                pivot.PivotFields("freightgmn").Caption = "Фрахт";
                pivot.PivotFields("preparatgm").Caption = "Оформление";
                pivot.PivotFields("deliveryms").Caption = "Довоз";
                pivot.PivotFields("escortmscw").Caption = "Корректировка";
                pivot.PivotFields("sertificat").Caption = "Серт-ты";
                pivot.PivotFields("claim00000").Caption = "Претензии";
                pivot.PivotFields("return0000").Caption = "Возврат";
                pivot.PivotFields("others0000").Caption = "Прочие, руб";
                pivot.PivotFields("prgmoscow0").Caption = "Накладные";
                pivot.PivotFields("insurance0").Caption = "Страховка";
                pivot.PivotFields("currencysum").Caption = "Сумма, $";
                pivot.PivotFields("costkg").Caption = "Ст-ть кг, $";
                pivot.PivotFields("invoicesum").Caption = "Счет, руб";
                pivot.PivotFields("invoicedate").Caption = "Счет, дата";
                pivot.PivotFields("paysum").Caption = "Оплата, руб";
                pivot.PivotFields("namelegal").Caption = "Получатель";
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
                MessageBox.Show(ex.Message, "Отчет");
            }
            finally
            {
                exApp.DisplayAlerts = true;
                exApp.ScreenUpdating = true;
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void transactionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == mainDataGrid) totalDataRefresh();
        }

        private void DataLoad()
        {
            //mainDataGrid.ItemsSource = null;
            if (managerdatagrid.Filter.isEmpty)
            {
                PopupText.Text = "Установите фильтр, для отображения только необходимых данных!";
                popInf.IsOpen = true;
                return;
            }
            ParcelReportList.Clear();
            using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                try
                {
                    con.Open();
                    SqlParameter where = new SqlParameter("@filterId", managerdatagrid.Filter.FilterWhereId);
                    SqlParameter group = new SqlParameter("@groupid", managerdatagrid.Filter.FilterGroupId);
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.ParcelReport_sp";
                    cmd.Parameters.Add(where); cmd.Parameters.Add(group);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //ParcelReport item = new ParcelReport();.DinamicProperties
                        Dictionary<string, DinamicPropertyItem> item = new Dictionary<string, DinamicPropertyItem>();
                        for (int c = 0; c < reader.FieldCount; c++)
                        {
                            item.Add(reader.GetName(c), new DinamicPropertyItem(reader.GetFieldType(c), (reader.IsDBNull(c) ? null : reader.GetValue(c))));
                        }
                        ParcelReportList.Add(item);
                    }
                    (System.Windows.Data.CollectionViewSource.GetDefaultView(ParcelReportList) as System.Windows.Data.ListCollectionView).Refresh();
                }
                catch (Exception ex)
                {
                    if (ex is System.Data.SqlClient.SqlException)
                    {
                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                        if (err.Number > 49999) MessageBox.Show(err.Message, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                finally { con.Close(); }
            }
            //mainDataGrid.ItemsSource = ParcelReportList;
            setFilterButtonImage();
            //totalGridTun();
            totalDataRefresh();
        }
        private void totalDataRefresh()
        {
            decimal?[,] totalvalue = new decimal?[2,managerdatagrid.Columns.Count];
            string[] totalformat = new string[managerdatagrid.Columns.Count];
            for (int i = 0; i < managerdatagrid.Columns.Count; i++)
                if (managerdatagrid.Columns[i].AggregateFunction == "CNT") totalformat[i] = "00ncr";
                else totalformat[i] = managerdatagrid.Columns[i].MetaData.Datatype;
            System.Collections.IList coll;
            if (this.mainDataGrid.SelectedItems.Count > 1)
                coll = this.mainDataGrid.SelectedItems;
            else
                coll = this.ParcelReportList;
            for (int i = 0; i < coll.Count; i++)
            {
                if (coll[i] is Dictionary<string, DinamicPropertyItem>)
                {
                    decimal value;
                    Dictionary<string, DinamicPropertyItem> row = coll[i] as Dictionary<string, DinamicPropertyItem>;
                    for (int c = 0; c < managerdatagrid.Columns.Count; c++)
                    {
                        if (managerdatagrid.Columns[c].GroupProperty?.SelectName == null) continue;
                        switch (totalformat[c])
                        {
                            case "02nrt":
                            case "00nrt":
                            case "00fcr":
                            case "00ncr":
                                if (row[managerdatagrid.Columns[c].GroupProperty.SelectName].Value != null)
                                {
                                    value =  decimal.Parse(row[managerdatagrid.Columns[c].GroupProperty.SelectName].Value.ToString());
                                    totalvalue[0, c] = (totalvalue[0, c] ?? 0M) + value;
                                    totalvalue[1, c]= (totalvalue[1, c] ?? 0M) + 1;
                                }
                                //switch (managerdatagrid.Columns[c].AggregateFunction)
                                //{
                                //    case "MAX":
                                //        if (!totalvalue[c].HasValue || totalvalue[c] < value) totalvalue[c] = value;
                                //        break;
                                //    case "MIN":
                                //        if (!totalvalue[c].HasValue || totalvalue[c] > value) totalvalue[c] = value;
                                //        break;
                                //    default:
                                //        totalvalue[c] = (totalvalue[c] ?? 0M) + value;
                                //        break;
                                //}
                                //break;
                                //value = ((int?)row[managerdatagrid.Columns[c].GroupProperty.SelectName].Value ?? 0M);
                                //switch (managerdatagrid.Columns[c].AggregateFunction)
                                //{
                                //    case "MAX":
                                //        if (!totalvalue[c].HasValue || totalvalue[c] < value) totalvalue[c] = value;
                                //        break;
                                //    case "MIN":
                                //        if (!totalvalue[c].HasValue || totalvalue[c] > value) totalvalue[c] = value;
                                //        break;
                                //    case "AVG":

                                //        break;
                                //    case "STD":

                                //        break;
                                //    default:
                                //        totalvalue[c] = (totalvalue[c] ?? 0M) + ((int?)row[managerdatagrid.Columns[c].GroupProperty.SelectName].Value ?? 0M);
                                //        break;
                                //}
                                break;
                        }
                    }
                }
            }
            string format = string.Empty;
            IFormatProvider culture = this.Language.GetEquivalentCulture();
            for (int i = 0; i < managerdatagrid.Columns.Count; i++)
            {
                //if (totalvalue[i].HasValue && (managerdatagrid.Columns[i].AggregateFunction == "AVG" | managerdatagrid.Columns[i].AggregateFunction == "STD"))
                //    totalvalue[i] = decimal.Divide(totalvalue[i].Value, coll.Count);
                switch (totalformat[i])
                {
                    case "02nrt":
                        format = "N";
                        break;
                    case "00ncr":
                    case "00nrt":
                        format = "N0";
                        break;
                    case "00fcr":
                        format = "F0";
                        break;
                }
                foreach (TextBox item in this.totalGrid.Children)
                {
                    if ((int)item.Tag == managerdatagrid.Columns[i].GetHashCode())
                        item.Text = totalvalue[0,i].HasValue ? (Grid.GetRow(item) == 0 ? totalvalue[0,i].Value: decimal.Divide(totalvalue[0,i].Value, totalvalue[1, i].Value)).ToString(format, culture) : string.Empty;
                }
            }
        }
        internal ManagerDataGridColumnsVM ManagerDataGridColumns { get { return managerdatagrid; } }

        #region Filter
        public bool IsShowFilter
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        public SQLFilter Filter
        {
            get { return managerdatagrid.Filter; }
            set
            {
                managerdatagrid.Filter.RemoveCurrentWhere();
                managerdatagrid.Filter = value;
                if (this.IsLoaded)
                    DataLoad();
            }
        }
        public void RunFilter()
        {
            DataLoad();
        }
        private void setFilterButtonImage()
        {
            string uribitmap;
            if (managerdatagrid.Filter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (FilterButton.Content as Image).Source = bi3;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winParcelReportFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new ParcelReportFilterWin();
                    ObjectWin.Owner = this;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
            else
            {
                if (ObjectWin != null)
                {
                    ObjectWin.Close();
                }
            }
        }
        #endregion

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            this.managerdatagrid.Sorting(e.Column);
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(ParcelReportList);
            view.CustomSort = this.managerdatagrid.Sorter;
            e.Handled = true;
        }
        private void mainDataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0D)
            {
                ScrollViewer scrol = System.Windows.Media.VisualTreeHelper.GetChild(System.Windows.Media.VisualTreeHelper.GetChild(this.mainDataGrid, 0), 0) as ScrollViewer;
                scrol.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }
        #region Chart
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            //this.PopupChart.StaysOpen = false;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.PopupChart.IsOpen = false;
            //ButtonChart.IsChecked = false;
        }

        private void PopupChart_Closed(object sender, EventArgs e)
        {
            //if (!ButtonChart.IsFocused)
            //ButtonChart.IsChecked = false;
            //this.PopupChart.StaysOpen = true;
        }

        private void ButtonChart_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonChart_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void ButtonChart_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        private void ButtonChart_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Primitives.ToggleButton)
            {

                System.Windows.Controls.Primitives.ToggleButton botton = sender as System.Windows.Controls.Primitives.ToggleButton;
                this.PopupChart.IsOpen = !this.PopupChart.IsOpen;//botton.IsChecked.HasValue && !(botton.IsChecked.Value)
                if (this.PopupChart.IsOpen)
                    this.PopupChart.Focus();
                e.Handled = true;
            }
        }
        #endregion

        private void MainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged && this.mainGrid.IsLoaded)
            {
                if (this.mainGrid.RowDefinitions[1].Height == new GridLength(1, GridUnitType.Auto) && !(this.mainGrid.RowDefinitions[0].ActualHeight + this.mainGrid.RowDefinitions[1].ActualHeight + this.mainGrid.RowDefinitions[2].ActualHeight + 2 < this.mainGrid.ActualHeight))
                    this.mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                else if(this.mainGrid.RowDefinitions[1].Height == new GridLength(1, GridUnitType.Star) && this.mainGrid.RowDefinitions[1].ActualHeight - mainDataGrid.ActualHeight > 2)
                    this.mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
            }
        }
        private void MainDataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged && mainDataGrid.IsLoaded)
                if (this.mainGrid.RowDefinitions[1].Height == new GridLength(1, GridUnitType.Auto) && !(this.mainGrid.RowDefinitions[0].ActualHeight + this.mainGrid.RowDefinitions[1].ActualHeight + this.mainGrid.RowDefinitions[2].ActualHeight + 2 < this.mainGrid.ActualHeight))
                    this.mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                else if(this.mainGrid.RowDefinitions[1].Height == new GridLength(1, GridUnitType.Star) && this.mainGrid.RowDefinitions[1].ActualHeight - mainDataGrid.ActualHeight > 2)
                    this.mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);

        }
    }

    //public class ParcelReport
    //{
    //    #region Property
    //    public System.Collections.Generic.Dictionary<string, DinamicPropertyItem> DinamicProperties { set; get; }
    //    public int ParcelId { get; set; }
    //    public string FullNumber { get; set; }
    //    public string Lorry { set; get; }
    //    public DateTime? Period { set; get; }
    //    public decimal? DeliveryPrice { get; set; }
    //    public decimal? InsurancePrice { get; set; }
    //    public decimal? USDRate { get; set; }
    //    public DateTime? RateDate { set; get; }
    //    public DateTime? ShippLanDate { set; get; }
    //    public DateTime? TerminalOut { set; get; }
    //    public int RequestId { get; set; }
    //    public int? ParcelGroup { get; set; }
    //    public int CustomerID { get; set; }
    //    public string CustomerName { get; set; }
    //    public string ManagerGroup { get; set; }
    //    public string LoadDescription { set; get; }
    //    public string AgentName { set; get; }
    //    public int? CellNumber { set; get; }
    //    public decimal? OfficialWeight { set; get; }
    //    public decimal? ActualWeight { set; get; }
    //    public decimal? CalcWeight { set; get; }
    //    public decimal CostKg { set; get; }
    //    public decimal? Volume { set; get; }
    //    public decimal? GoodValue { set; get; }
    //    public string CustomerNote { set; get; }
    //    public string ManagerNote { set; get; }
    //    public string ColorMark { set; get; }
    //    public decimal? Customs000 { set; get; }
    //    public decimal? Delivery00 { set; get; }
    //    public decimal? Discount00 { set; get; }
    //    public decimal? Prggermany { set; get; }
    //    public decimal? Storegermn { set; get; }
    //    public decimal? Freightgmn { set; get; }
    //    public decimal? Preparatgm { set; get; }
    //    public decimal? Deliveryms { set; get; }
    //    public decimal? Escortmscw { set; get; }
    //    public decimal? Sertificat { set; get; }
    //    public decimal? Claim00000 { set; get; }
    //    public decimal? Return0000 { set; get; }
    //    public decimal? Prgmoscow0 { set; get; }
    //    public decimal? Insurance0 { set; get; }
    //    public decimal? Others0000 { set; get; }
    //    public decimal CurrencySum { set; get; }
    //    public decimal? InvoiceSum { set; get; }
    //    public decimal? PaySum { set; get; }
    //    public string Legal { set; get; }
    //    #endregion

    //    internal ParcelReport()
    //    {
    //        this.ActualWeight = null;
    //        this.AgentName = string.Empty;
    //        this.CalcWeight = null;
    //        this.CellNumber = null;
    //        this.Claim00000 = null;
    //        this.ColorMark = null;
    //        this.CustomerID = 0;
    //        this.CustomerName = string.Empty;
    //        this.CustomerNote = string.Empty;
    //        this.Customs000 = null;
    //        this.Deliveryms = null;
    //        this.Discount00 = null;
    //        this.Escortmscw = null;
    //        this.Freightgmn = null;
    //        this.FullNumber = string.Empty;
    //        this.GoodValue = null;
    //        this.Insurance0 = null;
    //        this.InvoiceSum = null;
    //        this.Legal = string.Empty;
    //        this.LoadDescription = string.Empty;
    //        this.ManagerGroup = string.Empty;
    //        this.ManagerNote = string.Empty;
    //        this.OfficialWeight = null;
    //        this.ParcelGroup = null;
    //        this.ParcelId = 0;
    //        this.PaySum = null;
    //        this.Preparatgm = null;
    //        this.Prggermany = null;
    //        this.Prgmoscow0 = null;
    //        this.RequestId = 0;
    //        this.Return0000 = null;
    //        this.Sertificat = null;
    //        this.Storegermn = null;
    //    }
    //    public ParcelReport(
    //        int parcelid,
    //        string fullnumber,
    //        int requestId,
    //        int? parcelgroup,
    //        int customerID,
    //        string customerName,
    //        string managerGroup,
    //        string loadDescription,
    //        string agentName,
    //        int? cellNumber,
    //        decimal? officialWeight,
    //        decimal? actualWeight,
    //        decimal? calcweight,
    //        decimal? volume,
    //        decimal? goodValue,
    //        string customerNote,
    //        string managerNote,
    //        string colorMark,
    //        decimal? customs000,
    //        decimal? delivery00,
    //        decimal? discount00,
    //        decimal? prggermany,
    //        decimal? storegermn,
    //        decimal? freightgmn,
    //        decimal? preparatgm,
    //        decimal? deliveryms,
    //        decimal? escortmscw,
    //        decimal? sertificat,
    //        decimal? claim00000,
    //        decimal? return0000,
    //        decimal? prgmoscow0,
    //        decimal? insurance0,
    //        decimal? invoicesum,
    //        decimal? paysum,
    //        string legal
    //        )
    //    {
    //        this.ActualWeight = actualWeight;
    //        this.AgentName = agentName;
    //        this.CalcWeight = calcweight;
    //        this.CellNumber = cellNumber;
    //        this.Claim00000 = claim00000;
    //        this.ColorMark = colorMark;
    //        this.CustomerID = customerID;
    //        this.CustomerName = customerName;
    //        this.CustomerNote = customerNote;
    //        this.Customs000 = customs000;
    //        this.Deliveryms = deliveryms;
    //        this.Discount00 = discount00;
    //        this.Escortmscw = escortmscw;
    //        this.Freightgmn = freightgmn;
    //        this.FullNumber = fullnumber;
    //        this.GoodValue = goodValue;
    //        this.Insurance0 = insurance0;
    //        this.InvoiceSum = invoicesum;
    //        this.Legal = legal;
    //        this.LoadDescription = loadDescription;
    //        this.ManagerGroup = managerGroup;
    //        this.ManagerNote = managerNote;
    //        this.OfficialWeight = officialWeight;
    //        this.ParcelGroup = parcelgroup;
    //        this.ParcelId = parcelid;
    //        this.PaySum = paysum;
    //        this.Preparatgm = preparatgm;
    //        this.Prggermany = prggermany;
    //        this.Prgmoscow0 = prgmoscow0;
    //        this.RequestId = requestId;
    //        this.Return0000 = return0000;
    //        this.Sertificat = sertificat;
    //        this.Storegermn = storegermn;
    //    }
    //}
}
