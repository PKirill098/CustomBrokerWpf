using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для RequestNewWin.xaml
    /// </summary>
    public partial class RequestNewWin : Window
    {
        public RequestNewWin()
        {
            InitializeComponent();
            mydischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { LegalInvoiceDataGrid }); //, PaymentsInvoiceDataGrid, PrepaymentsDataGrid, FinalPaymentsDataGrid, CurrencyDataGrid, SellingDataGrid
        }
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            mycmd = e.NewValue as Classes.Domain.RequestVMCommand;
            if (mycmd != null)
            {
                mycmd.EndEdit = mydischanger.EndEdit;
                mycmd.CancelEdit = mydischanger.CancelEdit;
                if(mycmd.VModel.DomainState==DataModelClassLibrary.DomainObjectState.Unchanged)
                    mycmd.SaveRefresh.Execute(null);
            }
        }

        private DataModelClassLibrary.BindingDischarger mydischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mydischanger; } }
        private Classes.Domain.RequestVMCommand mycmd;

        private void HistoryOpen_Click(object sender, RoutedEventArgs e)
        {
            RequestHistoryWin newHistory = new RequestHistoryWin();
            if ((sender as Button).Tag is RequestVM)
            {
                Request request = ((sender as Button).Tag as RequestVM).DomainObject;
                RequestHistoryViewCommand cmd = new RequestHistoryViewCommand(request);
                newHistory.DataContext = cmd;
                newHistory.Owner = this;
                newHistory.Show();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClientHyperlink_Click(object sender, RoutedEventArgs e)
        {
            ClientWin win = new ClientWin();
            win.CustomerNameList.Text = ((sender as Hyperlink).Inlines.FirstInline as Run).Text;
            win.ShowDialog();
            ((sender as Hyperlink).Inlines.FirstInline as Run).Text = win.CustomerNameList.Text;
        }
        private void CustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Customer costomer = (Customer)(sender as Button).Tag;
            if (costomer!=null)
            {
                Window ObjectWin = null;
                foreach (Window item in this.OwnedWindows)
                {
                    if (item.Name == "winClientItem" && (item.DataContext as Classes.Domain.CustomerCommand).VModel.DomainObject == costomer) ObjectWin = item;
                }
                if (ObjectWin == null)
                {
                    Classes.Domain.CustomerCommand cmd = new Classes.Domain.CustomerCommand(new CustomerVM(costomer), null);
                    ObjectWin = new ClientItemWin();
                    ObjectWin.Owner = this;
                    ObjectWin.DataContext = cmd;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
        }
        private void AgentButton_Click(object sender, RoutedEventArgs e)
        {
            Agent agent = (Agent)(sender as Button).Tag;
            if (agent!=null)
            {
                Window ObjectWin = null;
                foreach (Window item in this.OwnedWindows)
                {
                    if (item.Name == "winAgentItem" && (item.DataContext as Classes.Domain.AgentCommand).VModel.DomainObject == agent) ObjectWin = item;
                }
                if (ObjectWin == null)
                {
                    Classes.Domain.AgentCommand cmd = new Classes.Domain.AgentCommand(new AgentVM(agent));
                    ObjectWin = new AgentItemWin();
                    ObjectWin.Owner = this;
                    ObjectWin.DataContext = cmd;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
        }
        private void LegalHyperlink_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AgentHyperlink_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CustomerLegalOpen_Click(object sender, RoutedEventArgs e)
        {
            Classes.Domain.CustomerLegalVM legal = (sender as Button).Tag as Classes.Domain.CustomerLegalVM;
            Classes.Domain.CustomerLegalVMCommand cmd = new Classes.Domain.CustomerLegalVMCommand(legal, null);
            ClientLegalWin win = new ClientLegalWin();
            win.DataContext = cmd;
            win.Show();
        }

        private void CommandBindingDelete_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mycmd.PrepayDel.CanExecute(null);
            e.Handled = true;
        }
        private void CommandBindingDelete_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if((sender as DataGrid)?.CurrentItem is Classes.Domain.Account.PrepayCustomerRequestVM)
                mycmd.PrepayDel.Execute((sender as DataGrid).CurrentItem);
        }
        private void CommandBindingAdd_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mycmd.PrepayAddCanExec();
            e.Handled = true;
        }
        private void CommandBindingAdd_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if ((sender as DataGrid)?.CurrentItem is RequestCustomerLegalVM)
                mycmd.PrepayAddExec((sender as DataGrid).CurrentItem);
            else if ((sender as DataGrid)?.CurrentItem is Classes.Domain.Account.PrepayCustomerRequestVM)
                mycmd.PrepayAddExec(((sender as DataGrid).CurrentItem as Classes.Domain.Account.PrepayCustomerRequestVM).Customer);
        }

        private void AlgorithmButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winAlgorithm" && !(item.DataContext is Classes.Domain.Algorithm.AlgorithmConsolidateCommand)) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new AlgorithmWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = (this.DataContext as Classes.Domain.RequestVMCommand).AlgorithmCommand;
                ObjectWin.WindowState = WindowState.Normal;
                ObjectWin.SizeToContent = SizeToContent.WidthAndHeight;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void AlgorithmConButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Domain.RequestVMCommand cmd = this.DataContext as Classes.Domain.RequestVMCommand;
            if (cmd.VModel.Parcel != null)
            {
                Window ObjectWin = null;
                foreach (Window item in this.OwnedWindows)
                {
                    if (item.Name == "winAlgorithm" & item.DataContext is Classes.Domain.Algorithm.AlgorithmConsolidateCommand) ObjectWin = item;
                }
                if (ObjectWin == null)
                {
                    ObjectWin = new AlgorithmWin();
                    ObjectWin.Owner = this;
                    ObjectWin.DataContext = cmd.AlgorithmConCommand;
                    ObjectWin.WindowState = WindowState.Normal;
                    ObjectWin.SizeToContent = SizeToContent.WidthAndHeight;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
            else
                cmd.OpenPopup("Необходимо включить заявку в перевозку!", false);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Classes.Domain.RequestVMCommand cmd = this.DataContext as Classes.Domain.RequestVMCommand;
            if (mydischanger.EndEdit() && !cmd.VModel.IsDirty)
            {
                bool isdirty = cmd.VModel.DomainObject.IsDirty;
                if (!isdirty & cmd.IsEditable)
                {
                    foreach (Classes.Domain.Algorithm.AlgorithmFormula item in cmd.AlgorithmCommand.AlgorithmFormulas)
                        if (!isdirty)
                            foreach (Classes.Domain.Algorithm.AlgorithmValuesVM value in item.AlgorithmValues)
                                isdirty = isdirty | value.DomainObject.IsDirty;
                        else break;
                    foreach (Classes.Domain.Algorithm.AlgorithmFormula item in cmd.AlgorithmConCommand.AlgorithmFormulas)
                        if (!isdirty)
                            foreach (Classes.Domain.Algorithm.AlgorithmValuesVM value in item.AlgorithmValues)
                                isdirty = isdirty | value.DomainObject.IsDirty;
                        else break;
                }
                if (isdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!cmd.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения в ДС не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                cmd.Reject.Execute(null);
                        }
                    }
                    else
                    {
                        cmd.Reject.Execute(null);
                    }
                }
                cmd.IsEditable = false;
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                {
                    cmd.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                if (!e.Cancel) (App.Current.MainWindow as DataModelClassLibrary.Interfaces.IMainWindow).ListChildWindow.Remove(this);
                App.Current.MainWindow.Activate();
            }
        }

        private void BindingUpdate(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BindingExpression be;
                be = (sender as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                if (be != null)
                {
                    if (be.IsDirty) be.UpdateSource();
                }
            }
        }

        private void PrepayButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winRequestPrepay" ) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new RequestPrepayWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = new Classes.Domain.Account.PrepayCustomerRequestCustomerCommander(((sender as Button).Tag as RequestCustomerLegalVM).DomainObject);
                ObjectWin.WindowState = WindowState.Normal;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winRichText") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new RichTextWin();
                ObjectWin.Owner = this;
                //ObjectWin.DataContext = new Classes.Domain.Account.PrepayCustomerRequestCustomerCommander(((sender as Button).Tag as RequestCustomerLegalVM).DomainObject);
                ObjectWin.WindowState = WindowState.Normal;
                ObjectWin.Owner = this;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }

        private void MainScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(e.HeightChanged && e.NewSize.Height - 19 > CargoBorder.ActualHeight)
            {
                AlgorithmScrollViewer.Height = e.NewSize.Height - 20;
            }
        }
    }
}
