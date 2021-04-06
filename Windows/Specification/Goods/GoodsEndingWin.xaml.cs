using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for GoodsEndingWin.xaml
    /// </summary>
    public partial class GoodsEndingWin : Window
    {
        private DataModelClassLibrary.BindingDischarger mybd;

        public GoodsEndingWin()
        {
            InitializeComponent();
            mybd = new DataModelClassLibrary.BindingDischarger(this, new[] { mainDataGrid });
        }
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                Classes.Domain.GoodsViewCommand cmd = (e.NewValue as Classes.Domain.GoodsViewCommand);
                cmd.EndEdit = mybd.EndEdit;
                cmd.CancelEdit = mybd.CancelEdit;
                cmd.Items.SortDescriptions.Clear();
                cmd.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("DaysEnd", System.ComponentModel.ListSortDirection.Ascending));
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Classes.Domain.GoodsViewCommand vm = this.DataContext as Classes.Domain.GoodsViewCommand;
            if (mybd.EndEdit())
            {
                bool isdirty = false;
                foreach (Classes.Domain.GoodsVM item in vm.Items.SourceCollection) isdirty = isdirty | item.IsDirty;
                if (isdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!vm.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                                e.Cancel = true;
                            else
                                vm.Reject.Execute(null);
                        }
                    }
                    else
                    {
                        vm.Reject.Execute(null);
                    }
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                {
                    vm.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                this.DataContext = null;
                if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            }
        }

        private void InfoBatchButton_Click(object sender, RoutedEventArgs e)
        {
            mybd.EndEdit();
            GoodsItemWin win = new GoodsItemWin();
            Classes.Domain.GoodsCommand cmd = new Classes.Domain.GoodsCommand((sender as Button).Tag as Classes.Domain.GoodsVM, (this.DataContext as Classes.Domain.GoodsViewCommand).Items);
            cmd.EndEdit = win.vmEndEdit;
            cmd.CancelEdit = win.vmCancelEdit;
            win.DataContext = cmd;
            win.Owner = this;
            win.Show();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
