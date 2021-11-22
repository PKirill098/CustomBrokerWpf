using System;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ParcelTransactionWin.xaml
    /// </summary>
    public partial class ParcelTransactionWin : Window, ISQLFiltredWindow
    {
        public ParcelTransactionWin()
        {
            InitializeComponent();
            this.ParcelTransactionUC.CloseButton.Click += thisCloseButton_Click;
            this.ParcelTransactionUC.CloseButton.Visibility = Visibility.Visible;
            this.ParcelTransactionUC.CloseButtonSeparator.Visibility = Visibility.Visible;
            this.ParcelTransactionUC.MainMenuSeparator.Visibility = Visibility.Visible;
            this.ParcelTransactionUC.MainMenu.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.ParcelTransactionUC.SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                this.ParcelTransactionUC.Filter.RemoveCurrentWhere();
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void RunFilter()
        {
            this.ParcelTransactionUC.RunFilter();
        }
        public bool IsShowFilter
        {
            get
            {
                return this.ParcelTransactionUC.IsShowFilter;
            }
            set
            {
                this.ParcelTransactionUC.IsShowFilter=value;
            }
        }
        public SQLFilter Filter
        {
            get
            {
                return this.ParcelTransactionUC.Filter;
            }
            set
            {
                this.ParcelTransactionUC.Filter=value;
            }
        }
    }
}
