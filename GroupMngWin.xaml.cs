﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для GroupMngWin.xaml
    /// </summary>
    public partial class GroupMngWin : Window
    {
        public GroupMngWin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }
        private void DataLoad()
        {
            try
            {
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDS groupMngDS = ((KirillPolyanskiy.CustomBrokerWpf.ReferenceDS)(this.FindResource("keyReferenceDS")));
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ManagerGroupAdapter adapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ManagerGroupAdapter();
                adapter.ClearBeforeFill = false;
                adapter.Fill(groupMngDS.tableManagerGroup);
            System.Windows.Data.CollectionViewSource tableManagerGroupViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("MngGroupVS")));
            tableManagerGroupViewSource.View.MoveCurrentToFirst();
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                    {
                        errs.Append(sqlerr.Message + "\n");
                    }
                    MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    DataLoad();
                }
            }
        }
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                if (mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true) && mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
                {
                    KirillPolyanskiy.CustomBrokerWpf.ReferenceDS itemGroupMngDS = ((KirillPolyanskiy.CustomBrokerWpf.ReferenceDS)(this.FindResource("keyReferenceDS")));
                    KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ManagerGroupAdapter GroupMngAdapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ManagerGroupAdapter();
                    GroupMngAdapter.Update(itemGroupMngDS.tableManagerGroup);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                        {
                            errs.Append(sqlerr.Message + "\n");
                        }
                        MessageBox.Show(errs.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    isSuccess = SaveChanges();
                }
            }
            return isSuccess;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
        }
    }
}
