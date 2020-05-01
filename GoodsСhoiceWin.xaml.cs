using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для GoodsСhoiceWin.xaml
    /// </summary>
    public partial class GoodsСhoiceWin : Window
    {
        public GoodsСhoiceWin()
        {
            InitializeComponent();
        }

        private void СhoiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainDataGrid.SelectedItem!=null)
                this.Close();
            else
                MessageBox.Show("Необходимо сделать выбор", "Выбор ДС");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal Classes.Domain.Goods GoodsСhoiced
        { get { return mainDataGrid.SelectedItem as Classes.Domain.Goods; } }
    }
}
