using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для AllPriceWin.xaml
    /// </summary>
    public partial class AllPriceWin : Window
    {
        public AllPriceWin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Classes.Domain.AllPriceViewCommand vm = new Classes.Domain.AllPriceViewCommand();
            vm.EndEdit = this.vmEndEdit;
            vm.CancelEdit = this.vmCancelEdit;
            this.DataContext = vm;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Classes.Domain.AllPriceViewCommand vm = this.DataContext as Classes.Domain.AllPriceViewCommand;
            if (vmEndEdit())
            {
                bool isdirty = false;
                foreach (Classes.Domain.AllPriceVM item in vm.Items.SourceCollection) isdirty = isdirty | item.IsDirty;
                if (isdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!vm.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                vm.Reject.Execute(null);
                        }
                    }
                    else
                        vm.Reject.Execute(null);
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                    vm.Reject.Execute(null);
            }
            if (!e.Cancel)
            {
                this.DataContext = null;
                if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            }
        }

        public bool vmEndEdit()
        {
            bool isEnd = true;
            isEnd = isEnd & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            BindingExpression be = GetBinding();
            if (be != null)
            {
                if (be.IsDirty) be.UpdateSource();
                isEnd = !be.HasError;
            }
            return isEnd;
        }
        public void vmCancelEdit()
        {
            this.mainDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.mainDataGrid.CancelEdit(DataGridEditingUnit.Row);
            BindingExpression be = GetBinding();
            if (be != null)
            {
                if (be.IsDirty) be.UpdateTarget();
            }
        }
        private BindingExpression GetBinding()
        {
            BindingExpression be = null;
            IInputElement fcontrol = FocusManager.GetFocusedElement(this);
            while (fcontrol != null & be == null)
            {
                if (fcontrol is TextBox)
                    be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                else if (fcontrol is ComboBox)
                    be = (fcontrol as FrameworkElement).GetBindingExpression(ComboBox.TextProperty);
                fcontrol = System.Windows.Media.VisualTreeHelper.GetParent(fcontrol as FrameworkElement) as FrameworkElement;
            }
            return be;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void InfoBatchButton_Click(object sender, RoutedEventArgs e)
        {
            OpenItem((sender as Button).Tag as Classes.Domain.AllPriceVM);
        }

        private void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            (this.DataContext as Classes.Domain.AllPriceViewCommand).FilterRun.Execute(null);
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (this.DataContext as Classes.Domain.AllPriceViewCommand).FilterRun.Execute(null);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainDataGrid.ScrollIntoView(CollectionView.NewItemPlaceholder);

            OpenItem((this.DataContext as Classes.Domain.AllPriceViewCommand).Items.AddNewItem(new Classes.Domain.AllPriceVM()) as Classes.Domain.AllPriceVM);
        }
        private void OpenItem(Classes.Domain.AllPriceVM item)
        {
            vmEndEdit();
            AllPriceItemWin win = new AllPriceItemWin();
            win.DataContext = new Classes.Domain.AllPriceCommand(item, (this.DataContext as Classes.Domain.AllPriceViewCommand).Items);
            win.Owner = this;
            win.Show();
        }
    }
}
