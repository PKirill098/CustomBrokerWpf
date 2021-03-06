﻿using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
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
    /// Логика взаимодействия для PrepayRubPayWin.xaml
    /// </summary>
    public partial class PrepayRubPayWin : Window
    {
        public PrepayRubPayWin()
        {
            InitializeComponent();
            mydischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { MainDataGrid });
        }
        private DataModelClassLibrary.BindingDischarger mydischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mydischanger; } }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PrepayRubPayViewCommand cmd = this.DataContext as PrepayRubPayViewCommand;
            bool isdirty = !mydischanger.EndEdit();
            if (!isdirty)
                foreach (PrepayRubPayVM item in cmd.Items)
                    if (item.IsDirty)
                    { isdirty = true; break; }
            if (!isdirty)
            {
                if (!cmd.SaveDataChanges())
                {
                    this.Activate();
                    if (MessageBox.Show("\nИзменения не сохранены. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                //if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                //App.Current.MainWindow.Activate();
                this.Owner.Activate();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PrepayRubPayViewCommand cmd = e.NewValue as PrepayRubPayViewCommand;
            if (cmd != null)
            {
                cmd.EndEdit = mydischanger.EndEdit;
                cmd.CancelEdit = mydischanger.CancelEdit;
            }
        }
    }
}
