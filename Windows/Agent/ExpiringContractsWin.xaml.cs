using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ExpiringContractsWin.xaml
    /// </summary>
    public partial class ExpiringContractsWin : Window
    {
        private DataModelClassLibrary.BindingDischarger mybd;
        private Classes.Domain.ContractCMD mycmd;

        public ExpiringContractsWin()
        {
            InitializeComponent();
            mybd = new DataModelClassLibrary.BindingDischarger(this, new[] { mainDataGrid });
        }
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                mycmd = (e.NewValue as Classes.Domain.ContractCMD);
                mycmd.EndEdit = mybd.EndEdit;
                mycmd.CancelEdit = mybd.CancelEdit;
                //mycmd.Items.SortDescriptions.Clear();
                //mycmd.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("DaysEnd", System.ComponentModel.ListSortDirection.Ascending));
            }
        }

        private void MainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGrid)?.CurrentItem is Classes.Domain.ContractVM)
            {
                if (e.OriginalSource is TextBlock && ((sender as DataGrid).CurrentCell.Column.SortMemberPath == "Agent.Name"))
                {
                    Classes.Domain.Agent agent = ((sender as DataGrid)?.CurrentItem as Classes.Domain.ContractVM).Agent;

                    AgentItemWin win = null;
                    foreach (Window item in this.OwnedWindows)
                    {
                        if (item.Name == "winAgentItem" && (item.DataContext as Classes.Domain.AgentCommand).VModel.DomainObject == agent)
                        {
                            win = item as AgentItemWin;
                            break;
                        }
                    }
                    if (win == null)
                    {
                        Classes.Domain.AgentCommand cmd = new Classes.Domain.AgentCommand(new Classes.Domain.AgentVM(agent));
                        win = new AgentItemWin();
                        win.Owner = this;
                        win.DataContext = cmd;
                        win.Show();
                    }
                    else
                    {
                        win.Activate();
                        if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
                    }
                }
                e.Handled = true;
            }
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //Classes.Domain.ContractVM item = this.ContractDataGrid.SelectedItem as Classes.Domain.ContractVM;
            //e.CanExecute = item.DomainState == DataModelClassLibrary.DomainObjectState.Added;
            e.Handled = true;
        }
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Classes.Domain.ContractVM item = this.ContractDataGrid.SelectedItem as Classes.Domain.ContractVM;
            //mycmd.VModel.Contracts.EditItem(item);
            //item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            //mycmd.VModel.Contracts.CommitEdit();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mybd.EndEdit())
            {
                bool isdirty = false;
                foreach (Classes.Domain.ContractVM item in mycmd.Items.SourceCollection) isdirty = isdirty | item.IsDirty;
                if (isdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!mycmd.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                                e.Cancel = true;
                            else
                                mycmd.Reject.Execute(null);
                        }
                    }
                    else
                    {
                        mycmd.Reject.Execute(null);
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
                    mycmd.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                this.DataContext = null;
                if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            }
        }
    }
}
