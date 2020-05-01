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
    /// Interaction logic for MaterialWin.xaml
    /// </summary>
    public partial class MaterialWin : Window
    {
        public MaterialWin()
        {
            InitializeComponent();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Classes.Specification.MaterialCommand cmd = this.DataContext as Classes.Specification.MaterialCommand;
            if (vmEndEdit())
            {
                bool isdirty = cmd.VModel.IsDirty;
                if(!isdirty)
                    foreach (Classes.Specification.MaterialVM item in cmd.VModel.SubProducts.SourceCollection) isdirty |= object.Equals(item.DomainObject.Upper, cmd.VModel.DomainObject) && (item as Classes.Specification.MaterialVM).IsDirty;
                if (isdirty)
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
                        }
                    }
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
            if (!e.Cancel)
            {
                this.DataContext = null;
                if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            }
        }

        private void DeleteSubMaterial(object sender, ExecutedRoutedEventArgs e)
        {
            (this.DataContext as Classes.Specification.MaterialCommand).DeleteSubMaterialExec((sender as DataGrid).SelectedItems);
            e.Handled = true;
        }
        private void CanDeleteSubMaterial(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as DataGrid).SelectedItems.Count > 0 && (this.DataContext as Classes.Specification.MaterialCommand).DeleteSubMaterialCanExec(e.Parameter);
            e.Handled = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public bool vmEndEdit()
        {
            bool isEnd = true;
            isEnd = isEnd & this.materialDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.materialDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            BindingExpression be = GetBinding();
            if (be != null)
            {
                if (be.IsDirty) be.UpdateSource();
                isEnd &= !be.HasError;
            }
            return isEnd;
        }
        public void vmCancelEdit()
        {
            this.materialDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.materialDataGrid.CancelEdit(DataGridEditingUnit.Row);
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

        private void materialDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            (e.NewItem as Classes.Specification.MaterialVM).DomainObject.Upper = (this.DataContext as Classes.Specification.MaterialCommand).VModel.DomainObject;
            (e.NewItem as Classes.Specification.MaterialVM).GoodsName = (this.DataContext as Classes.Specification.MaterialCommand).VModel.GoodsName;
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if((sender as DataGrid).CurrentCell.Column?.DisplayIndex==0)
            (this.DataContext as Classes.Specification.MaterialCommand).VModel.SubProductsChanged();
        }
    }
}
