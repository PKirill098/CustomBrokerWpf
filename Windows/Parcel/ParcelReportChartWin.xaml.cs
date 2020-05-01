using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ParcelReportChartWin.xaml
    /// </summary>
    public partial class ParcelReportChartWin : Window
    {
        public ParcelReportChartWin()
        {
            InitializeComponent();
        }

        internal void ExpandButtom_Click(object sender, RoutedEventArgs e)
        {
if(e.OriginalSource is System.Windows.Controls.Primitives.ToggleButton)
{
    System.Windows.Controls.Primitives.ToggleButton button = e.OriginalSource as System.Windows.Controls.Primitives.ToggleButton;
    string uribitmap, tooltext;
    int rowcur = System.Windows.Controls.Grid.GetRow(button);
    if (button.IsChecked.HasValue && button.IsChecked.Value)
    {
        uribitmap = @"/CustomBrokerWpf;component/Images/window_split_ver.png";
        tooltext = "Показать все диаграммы";
        for (int i = 0; i < mainGrid.RowDefinitions.Count; i++)
        {
            if (i != rowcur) mainGrid.RowDefinitions[i].Height = new GridLength(0D);
            //else mainGrid.RowDefinitions[i].Height = new GridLength(1D,GridUnitType.Star);
        }
    }
    else
    {
        uribitmap = @"/CustomBrokerWpf;component/Images/window.png";
        tooltext = "Растянуть на все окно";
        for (int i = 0; i < mainGrid.RowDefinitions.Count; i++)
        {
            if (i != rowcur) mainGrid.RowDefinitions[i].Height = new GridLength(1D, GridUnitType.Star);
        }
    }
    (button.Content as System.Windows.Controls.Image).Source =new System.Windows.Media.Imaging.BitmapImage(new System.Uri(uribitmap, System.UriKind.Relative));
    button.ToolTip = tooltext;
}
        }
    }
}
