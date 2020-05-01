﻿using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
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
            Classes.Domain.RequestVMCommand cmd = e.NewValue as Classes.Domain.RequestVMCommand;
            if (cmd != null)
            {
                cmd.EndEdit = mydischanger.EndEdit;
                cmd.CancelEdit = mydischanger.CancelEdit;
                if(cmd.VModel.DomainState==DataModelClassLibrary.DomainObjectState.Unchanged)
                    cmd.SaveRefresh.Execute(null);
            }
        }

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
            string name = (string)(sender as Button).Tag;
            if (!string.IsNullOrEmpty(name))
            {
                ClientWin win = new ClientWin();
                win.Show();
                win.CustomerNameList.Text = name;
            }
        }
        private void AgentButton_Click(object sender, RoutedEventArgs e)
        {
            string name = (string)(sender as Button).Tag;
            if (!string.IsNullOrEmpty(name))
            {
                AgentWin agentWin = new AgentWin();
                agentWin.Show();
                agentWin.AgentNameList.Text = name;
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

        private DataModelClassLibrary.BindingDischarger mydischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mydischanger; } }

        private void CommandBinding_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            //DataGrid grid = sender as DataGrid;
            //System.Windows.Data.ListCollectionView view = grid.ItemsSource as System.Windows.Data.ListCollectionView;
            //Classes.Domain.RequestPaymentVM item = grid.SelectedItem as Classes.Domain.RequestPaymentVM;
            //if (view.IsAddingNew)
            //    view.CancelNew();
            //else
            //{
            //    view.EditItem(item);
            //    item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            //    view.CommitEdit();
            //}
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
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winAlgorithm" & item.DataContext is Classes.Domain.Algorithm.AlgorithmConsolidateCommand) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new AlgorithmWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = (this.DataContext as Classes.Domain.RequestVMCommand).AlgorithmConCommand;
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
                            foreach (Classes.Domain.Algorithm.AlgorithmValuesVM value in item.Algorithms)
                                isdirty = isdirty | value.DomainObject.IsDirty;
                        else break;
                    foreach (Classes.Domain.Algorithm.AlgorithmFormula item in cmd.AlgorithmConCommand.AlgorithmFormulas)
                        if (!isdirty)
                            foreach (Classes.Domain.Algorithm.AlgorithmValuesVM value in item.Algorithms)
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
    }
}
