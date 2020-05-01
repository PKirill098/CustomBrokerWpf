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
    /// Interaction logic for AllPriceItemWin.xaml
    /// </summary>
    public partial class AllPriceItemWin : Window
    {
        public AllPriceItemWin()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Classes.Domain.AllPriceCommand cmd = this.DataContext as Classes.Domain.AllPriceCommand;
            if (vmEndEdit() && cmd.Item.Validate(true))
            {
                if (cmd.Item.IsDirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!cmd.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                cmd.Reject.Execute(null);
                        }
                    }
                    else
                        cmd.Reject.Execute(null);
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool vmEndEdit()
        {
            bool isEnd = true;
            //isEnd = isEnd & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            //isEnd = isEnd & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
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
            //this.mainDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            //this.mainDataGrid.CancelEdit(DataGridEditingUnit.Row);
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

    }
}
