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
    /// Interaction logic for MappingWin.xaml
    /// </summary>
    public partial class MappingWin : Window
    {
        public MappingWin()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SynonymDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            (this.DataContext as Classes.Specification.MappingCommand).VModel.SynonymsChanged();
        }
        private void GenderDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            (this.DataContext as Classes.Specification.MappingCommand).VModel.GendersChanged();
        }

        public bool vmEndEdit()
        {
            bool isEnd = true;
            isEnd = isEnd & this.SynonymDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.SynonymDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            isEnd = isEnd & this.GenderDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.GenderDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
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
            this.SynonymDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.SynonymDataGrid.CancelEdit(DataGridEditingUnit.Row);
            this.GenderDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.GenderDataGrid.CancelEdit(DataGridEditingUnit.Row);
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

        private void DeleteSynonym(object sender, ExecutedRoutedEventArgs e)
        {
            (this.DataContext as Classes.Specification.MappingCommand).SynonymsDeleteExec((sender as DataGrid).SelectedItems);
            e.Handled = true;
        }
        private void CanDeleteSynonym(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as DataGrid).SelectedItems.Count>0 && (this.DataContext as Classes.Specification.MappingCommand).SynonymsDeleteCanExec(e.Parameter);
            e.Handled = true;
        }
        private void DeleteGender(object sender, ExecutedRoutedEventArgs e)
        {
            (this.DataContext as Classes.Specification.MappingCommand).GendersDeleteExec((sender as DataGrid).SelectedItems);
            e.Handled = true;
        }
        private void CanDeleteGender(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as DataGrid).SelectedItems.Count > 0 && (this.DataContext as Classes.Specification.MappingCommand).GendersDeleteCanExec(e.Parameter);
            e.Handled = true;
        }
    }
}
