using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для RequestHistoryWin.xaml
    /// </summary>
    public partial class RequestHistoryWin : Window
    {
        public RequestHistoryWin()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //int rowid = 0;
        //internal int RowId
        //{
        //    set { rowid = value; }
        //    get { return rowid; }
        //} 
        //private void winRequestHistory_Loaded(object sender, RoutedEventArgs e)
        //{
        //    RequestHistoryDS ds = this.FindResource("keyRequestHistoryDS") as RequestHistoryDS;
        //    RequestHistoryDSTableAdapters.adapterRequestHistory adapter = new RequestHistoryDSTableAdapters.adapterRequestHistory();
        //    adapter.Fill(ds.tableRequestHistory, rowid);
        //    mainDataGrid.ItemsSource = ds.tableRequestHistory.DefaultView;
        //}
    }
}
