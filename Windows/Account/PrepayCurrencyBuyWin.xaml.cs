﻿using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class PrepayCurrencyBuyWin : Window
    {
        public PrepayCurrencyBuyWin()
        {
            InitializeComponent();
            mydischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { MainDataGrid });
        }

        private DataModelClassLibrary.BindingDischarger mydischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mydischanger; } }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PrepayCurrencyBuyViewCommand cmd = e.NewValue as PrepayCurrencyBuyViewCommand;
            if (cmd != null)
            {
                cmd.EndEdit = mydischanger.EndEdit;
                cmd.CancelEdit = mydischanger.CancelEdit;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PrepayCurrencyBuyViewCommand cmd = this.DataContext as PrepayCurrencyBuyViewCommand;
            bool isdirty = !mydischanger.EndEdit();
            if (!isdirty)
                foreach (PrepayCurrencyBuyVM item in cmd.Items)
                    if (item.IsDirty)
                    { isdirty = true; break; }
            if (!isdirty)
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
                //if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                //App.Current.MainWindow.Activate();
                this.Owner.Activate();
            }
        }
    }
}
