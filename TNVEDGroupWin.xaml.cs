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
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for TNVEDGroupWin.xaml
    /// </summary>
    public partial class TNVEDGroupWin : Window
    {
        private lib.BindingDischarger mybindingdischarger;
        public TNVEDGroupWin()
        {
            InitializeComponent();
            mybindingdischarger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { GoodsDataGrid });
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GoodsDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            (this.DataContext as Classes.Specification.TNVEDGroupCommand).VModel.DomainObject.GoodsChanged();
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue!=null)
            {
                (e.NewValue as Classes.Specification.TNVEDGroupCommand).CancelEdit = mybindingdischarger.CancelEdit;
                (e.NewValue as Classes.Specification.TNVEDGroupCommand).EndEdit = mybindingdischarger.EndEdit;
            }
        }

        private void DeleteGoods(object sender, ExecutedRoutedEventArgs e)
        {
            (this.DataContext as Classes.Specification.TNVEDGroupCommand).GoodsDeleteExec((sender as DataGrid).SelectedItems);
            e.Handled = true;
        }
        private void CanDeleteGoods(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as DataGrid).SelectedItems.Count > 0 && (this.DataContext as Classes.Specification.TNVEDGroupCommand).GoodsDeleteCanExec(e.Parameter);
            e.Handled = true;
        }
    }
}
