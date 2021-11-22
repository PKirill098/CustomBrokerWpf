using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for WayBillClientWin.xaml
    /// </summary>
    public partial class WayBillClientWin : Window
	{
		public WayBillClientWin()
		{
			InitializeComponent();
		}
        internal bool DataEndEdit()
        {
            bool isEnd = true;
            isEnd = isEnd & this.mainDataGrid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.mainDataGrid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Row, true);
            return isEnd;
        }
        internal void DataCancelEdit()
        {
            this.mainDataGrid.CancelEdit(System.Windows.Controls.DataGridEditingUnit.Cell);
            this.mainDataGrid.CancelEdit(System.Windows.Controls.DataGridEditingUnit.Row);
        }
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
