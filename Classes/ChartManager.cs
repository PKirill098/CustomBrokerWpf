using System;
using System.Collections.Generic;
using System.Windows.Data;
using chart = System.Windows.Controls.DataVisualization.Charting;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Chart
{
    internal class ChartManager : IDisposable
    {
        private System.Windows.Window myownerwindow;
        private ListNotifyChanged<Series> myserieslist;

        internal string ChartTitle { set; get; }
        internal string LegendTitle { set; get; }
        internal string AxisXTitle { set; get; }
        internal string AxisYTitle { set; get; }
        internal SeriesType ChartType { set; get; }

        internal ChartManager(System.Windows.Window ownerwindow)
            :this(ownerwindow,null) {}
        internal ChartManager(System.Windows.Window ownerwindow, ListNotifyChanged<Series> datasource)
            : base()
        {
            myownerwindow = ownerwindow;
            myserieslist = (datasource!=null?datasource:new ListNotifyChanged<Series>());
        }
        internal ListNotifyChanged<Series> ChartSeries
        {
            get { return myserieslist; }
            set { myserieslist = value; }
        }
        internal void CreateChart()
        {
            chart.Chart currentchart;
            chart.DataPointSeries chartseries;
            ParcelReportChartWin chartwin = new ParcelReportChartWin();
            chartwin.Owner = myownerwindow;
            //chart.LinearAxis yaxis = new chart.LinearAxis();
            //yaxis.Orientation = chart.AxisOrientation.X;
            //yaxis.SeriesHost = chartwin.mainChart.Series as chart.ISeriesHost;
            //yaxis.Title=this.ChartTitle;
            //chartwin.mainChart.Axes.Add(yaxis);
            currentchart=chartwin.mainChart;
            foreach (Chart.Series item in this.myserieslist)
            {
                switch (ChartType)
                {
                    case SeriesType.Column:
                        chartseries = new chart.ColumnSeries();
                        break;
                    case SeriesType.Pie:
                        chartseries = new chart.PieSeries();
                        if (chartwin.mainChart.Series.Count > 0)
                        {
                            currentchart = new chart.Chart();
                            currentchart.LegendTitle = this.AxisXTitle;
                            //chartwin.mainGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
                            //System.Windows.Controls.RowDefinition rowdef = new System.Windows.Controls.RowDefinition();
                            //rowdef.Height = System.Windows.GridLength.Auto;
                            //chartwin.mainGrid.RowDefinitions.Add(rowdef);
                            chartwin.mainGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                            //System.Windows.Controls.Grid.SetColumn(currentchart, chartwin.mainGrid.ColumnDefinitions.Count - 1);
                            System.Windows.Controls.Grid.SetRow(currentchart, chartwin.mainGrid.RowDefinitions.Count - 1);
                            chartwin.mainGrid.Children.Add(currentchart);
                        }
                        System.Windows.Controls.Primitives.ToggleButton expendbutton = new System.Windows.Controls.Primitives.ToggleButton();
                        expendbutton.ToolTip = "Растянуть на все окно";
                        expendbutton.Click += chartwin.ExpandButtom_Click;
                        expendbutton.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        expendbutton.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        expendbutton.Margin = new System.Windows.Thickness(10D);
                        expendbutton.Opacity = 0.3D;
                        expendbutton.Width = 30D;
                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                        image.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(@"/CustomBrokerWpf;component/Images/window.png", System.UriKind.Relative));
                        expendbutton.Content = image;
                        System.Windows.Controls.Grid.SetRow(expendbutton, chartwin.mainGrid.RowDefinitions.Count - 1);
                        chartwin.mainGrid.Children.Add(expendbutton);
                        currentchart.Title = item.Name;
                        break;
                    case SeriesType.Area:
                        chartseries = new chart.AreaSeries();
                        break;
                    case SeriesType.Line:
                        chartseries = new chart.LineSeries();
                        break;
                    default:
                        chartseries = new chart.ScatterSeries();
                        break;
                }
                chartseries.IndependentValueBinding = new Binding("Key");
                chartseries.DependentValueBinding = new Binding("Value");
                chartseries.Title = item.Name;
                chartseries.ItemsSource = item.DataPoints;
                currentchart.Series.Add(chartseries);
            }
            chartwin.Show();
            if (chartwin.mainChart.ActualAxes.Count > 0)
            {
                if (!string.IsNullOrEmpty(this.LegendTitle)) chartwin.mainChart.LegendTitle = this.LegendTitle;
                (chartwin.mainChart.ActualAxes[0] as chart.DisplayAxis).Title = this.AxisXTitle;
                if (!string.IsNullOrEmpty(this.AxisYTitle)) (chartwin.mainChart.ActualAxes[1] as chart.DisplayAxis).Title = this.AxisYTitle;
            }
            else
                chartwin.mainChart.LegendTitle = this.AxisXTitle;
        }

        public void Dispose()
        {
            myownerwindow = null;
        }
    }
    internal class Series
    {
        internal string Name { set; get; }
        internal ListNotifyChanged<KeyValuePair<object, float>> DataPoints { get; set; }
    }
    internal enum SeriesType:byte
    { Column, Pie, Area, Line }
}
