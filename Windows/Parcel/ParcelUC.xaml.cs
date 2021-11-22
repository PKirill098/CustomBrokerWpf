using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Windows.Parcel
{
    /// <summary>
    /// Логика взаимодействия для ParcelUC.xaml
    /// </summary>
    public partial class ParcelUC : UserControl
    {

        public ParcelUC()
        {
            InitializeComponent();
        }

        private Window mywindow;
        private lib.BindingDischarger myparcelbinddisp;
        public lib.ViewModelBaseCommand ParentDataContext
        {
            get { return GetValue(ParentDataContextProperty) as lib.ViewModelBaseCommand; }
            set
            { 
                SetValue(ParentDataContextProperty, value);
            }
        }
        public static readonly DependencyProperty ParentDataContextProperty
            = DependencyProperty.Register("ParentDataContext",typeof(lib.ViewModelBaseCommand),typeof(ParcelUC),new PropertyMetadata(null));
        private Classes.Domain.ParcelVM myparcel;

        private void FindParents()
        {
            mywindow = null;
            FrameworkElement element = this;
            while (mywindow == null & element != null)
                if (element.Parent is Window) mywindow = element.Parent as Window;
                else 
                {
                    element = element.Parent as FrameworkElement;
                    
                    //if (win.Parent is TabItem) myhostelement = win.Parent as TabItem; 
                }
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FindParents();
            myparcelbinddisp = new lib.BindingDischarger(mywindow, new DataGrid[] { this.ParcelRequestDataGrid, NoParcelRequestDataGrid, SpecificationDataGrid });
            if(this.ParentDataContext!=null)
            {
                lib.ViewModelBaseCommand cmd = this.ParentDataContext as lib.ViewModelBaseCommand;
                cmd.EndEdit = myparcelbinddisp.EndEdit;
                cmd.CancelEdit = myparcelbinddisp.CancelEdit;
            }

            //Синхронизация ширины столбцов
            for (int i = 0; i < this.ParcelRequestDataGrid.Columns.Count; i++)
                if (this.ParcelRequestDataGrid.Columns[i].ActualWidth > this.NoParcelRequestDataGrid.Columns[i].ActualWidth)
                    this.NoParcelRequestDataGrid.Columns[i].Width = this.ParcelRequestDataGrid.Columns[i].ActualWidth;
                else if (this.ParcelRequestDataGrid.Columns[i].ActualWidth < this.NoParcelRequestDataGrid.Columns[i].ActualWidth)
                    this.ParcelRequestDataGrid.Columns[i].Width = this.NoParcelRequestDataGrid.Columns[i].ActualWidth;
            DependencyPropertyDescriptor textDescr = DependencyPropertyDescriptor.FromProperty(DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));
            if (textDescr != null)
            {
                foreach (DataGridColumn column in this.ParcelRequestDataGrid.Columns)
                {
                    textDescr.AddValueChanged(column, delegate
                    {
                        if (column.DisplayIndex >= 0) ParcelRequestDataGrid_SizeChanged(column);
                    });
                }
                foreach (DataGridColumn column in this.NoParcelRequestDataGrid.Columns)
                {
                    textDescr.AddValueChanged(column, delegate
                    {
                        if (column.DisplayIndex >= 0) NoParcelRequestDataGrid_SizeChanged(column);
                    });
                }
            }
        }
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                myparcel = e.NewValue as Classes.Domain.ParcelVM;
            else
                myparcel = null;
        }

        private void ParceltoExcelButton_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).CommandParameter = MessageBox.Show("Перенести в Excel только новые заявки?", "в Excel", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
        private void MailSMS_Click(object sender, RoutedEventArgs e)
        {
            MailSMSWin win = new MailSMSWin();
            int parcelid = this.DataContext != null ? (this.DataContext as Classes.Domain.ParcelVM).DomainObject.Id : 0;
            Classes.MailSMSCommand cmd = new Classes.MailSMSCommand(parcelid);
            win.DataContext = cmd;
            win.Owner = mywindow;
            win.Show();
        }
        private void ParcelFilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mywindow.OwnedWindows)
            {
                if (item.Name == "winParcelFilter") ObjectWin = item;
            }
            if (this.ParcelFilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new ParcelFilterWin() { FilterOwner = (lib.Interfaces.IFilterWindowOwner)this.ParentDataContext };
                    ObjectWin.Owner = mywindow;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
            else
            {
                if (ObjectWin != null)
                {
                    ObjectWin.Close();
                }
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            mywindow.Close();
        }

        private bool mycolumnchanging;//Проверить наличие SortMemberPath у столбцов DataGridTemplateColumn, если сортировка не нужна добавить произвольное значение и установить CanUserSort="False"
        private void ParcelRequestDataGrid_ColumnDisplayIndexChanged(object sender, DataGridColumnEventArgs e)
        {
            if (!mycolumnchanging && this.ParcelRequestDataGrid.IsLoaded)
            {
                DataGridColumn column = null;
                foreach (DataGridColumn item in this.NoParcelRequestDataGrid.Columns)
                {
                    if (string.Equals(item.SortMemberPath, e.Column.SortMemberPath))
                    { column = item; break; }
                }
                if (column != null && column.DisplayIndex != e.Column.DisplayIndex)
                {
                    mycolumnchanging = true;
                    column.DisplayIndex = e.Column.DisplayIndex;
                    mycolumnchanging = false;
                }
            }
        }
        private void NoParcelRequestDataGrid_ColumnDisplayIndexChanged(object sender, DataGridColumnEventArgs e)
        {
            if (!mycolumnchanging && this.NoParcelRequestDataGrid.IsLoaded)
            {
                DataGridColumn column = null;
                foreach (DataGridColumn item in this.ParcelRequestDataGrid.Columns)
                {
                    if (string.Equals(item.SortMemberPath, e.Column.SortMemberPath))
                    { column = item; break; }
                }
                if (column != null && column.DisplayIndex != e.Column.DisplayIndex)
                {
                    mycolumnchanging = true;
                    column.DisplayIndex = e.Column.DisplayIndex;
                    mycolumnchanging = false;
                }
            }
        }
        private void ParcelRequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridCellInfo cellinf;
            if (!(e.OriginalSource is DataGrid) || myparcel == null) return;
            foreach (Classes.Domain.RequestVM rowview in e.RemovedItems)
            {
                if (!(rowview is Classes.Domain.RequestVM)) continue;
                rowview.Selected = false;
                if (rowview.ParcelGroup.HasValue)
                {
                    foreach (Classes.Domain.RequestVM itemrow in ParcelRequestDataGrid.SelectedItems)
                    {
                        if (itemrow.ParcelGroup.HasValue && rowview.ParcelGroup == itemrow.ParcelGroup)
                        {
                            ParcelRequestDataGrid.SelectedItems.Remove(itemrow);
                            foreach (DataGridColumn colm in this.ParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(itemrow, colm);
                                if (ParcelRequestDataGrid.SelectedCells.Contains(cellinf)) ParcelRequestDataGrid.SelectedCells.Remove(cellinf);
                            }
                            break;
                        }
                    }
                }
            }
            foreach (Classes.Domain.RequestVM rowview in e.AddedItems)
            {
                if (!(rowview is Classes.Domain.RequestVM)) continue;
                rowview.Selected = true;
                if (rowview.ParcelGroup.HasValue)
                {
                    foreach (Classes.Domain.RequestVM viewrow in myparcel.ParcelRequests)
                    {
                        if (viewrow.ParcelGroup.HasValue && rowview.ParcelGroup == viewrow.ParcelGroup && !ParcelRequestDataGrid.SelectedItems.Contains(viewrow))
                        {
                            ParcelRequestDataGrid.SelectedItems.Add(viewrow);
                            foreach (DataGridColumn colm in this.ParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(viewrow, colm);
                                if (!ParcelRequestDataGrid.SelectedCells.Contains(cellinf)) ParcelRequestDataGrid.SelectedCells.Add(cellinf);
                            }
                            break;
                        }
                    }
                }
            }
        }
        private void NoParcelRequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridCellInfo cellinf;
            if (!(e.OriginalSource is DataGrid) || myparcel == null) return;
            Classes.Domain.RequestVM[] noreadyrowview = new Classes.Domain.RequestVM[NoParcelRequestDataGrid.Items.Count];
            foreach (Classes.Domain.RequestVM rowview in e.AddedItems)
            {
                if (!(rowview is Classes.Domain.RequestVM)) continue;
                rowview.Selected = true;
                if (rowview.ParcelGroup.HasValue)
                {
                    foreach (Classes.Domain.RequestVM viewrow in myparcel.Requests)
                    {
                        if (viewrow.ParcelGroup.HasValue && rowview.ParcelGroup == viewrow.ParcelGroup && !NoParcelRequestDataGrid.SelectedItems.Contains(viewrow))
                        {
                            NoParcelRequestDataGrid.SelectedItems.Add(viewrow);
                            foreach (DataGridColumn colm in this.NoParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(viewrow, colm);
                                if (!NoParcelRequestDataGrid.SelectedCells.Contains(cellinf)) NoParcelRequestDataGrid.SelectedCells.Add(cellinf);
                            }
                            break;
                        }
                    }
                }
            }
            foreach (Classes.Domain.RequestVM rowview in e.RemovedItems)
            {
                if (!(rowview is Classes.Domain.RequestVM)) continue;
                rowview.Selected = false;
                if (rowview.ParcelGroup.HasValue)
                {
                    foreach (Classes.Domain.RequestVM viewrow in NoParcelRequestDataGrid.SelectedItems)
                    {
                        if (viewrow.ParcelGroup.HasValue && rowview.ParcelGroup == viewrow.ParcelGroup)
                        {
                            NoParcelRequestDataGrid.SelectedItems.Remove(viewrow);
                            foreach (DataGridColumn colm in this.NoParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(viewrow, colm);
                                if (NoParcelRequestDataGrid.SelectedCells.Contains(cellinf)) NoParcelRequestDataGrid.SelectedCells.Remove(cellinf);
                            }
                            break;
                        }
                    }
                }
            }

            myparcel.ResetFree();
            foreach (Classes.Domain.RequestVM rowview in NoParcelRequestDataGrid.SelectedItems)
            {
                if (rowview.Volume.HasValue)
                {
                    myparcel.VolumeFree = -rowview.Volume.Value;
                }
                if (rowview.ActualWeight.HasValue)
                {
                    myparcel.ActualWeightFree = -rowview.ActualWeight.Value;
                }
                if (rowview.OfficialWeight.HasValue)
                {
                    myparcel.OfficialWeightFree = -rowview.OfficialWeight.Value;
                }
                if (rowview.Invoice.HasValue)
                {
                    myparcel.InvoiceFree = -rowview.Invoice.Value;
                }
                if (rowview.InvoiceDiscount.HasValue)
                {
                    myparcel.InvoiceDiscountFree = -rowview.InvoiceDiscount.Value;
                }
                if (rowview.CellNumber.HasValue)
                {
                    myparcel.CellNumberFree = -rowview.CellNumber.Value;
                }
            }
        }

        private void RequestsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGrid)?.CurrentItem is Classes.Domain.RequestVM)
            {
                if (e.OriginalSource is TextBlock && ((sender as DataGrid).CurrentCell.Column.SortMemberPath == "StorePointDate" || (sender as DataGrid).CurrentCell.Column?.SortMemberPath == "Id"))
                {
                    RequestNewWin newWin = null;
                    DataGrid dg = sender as DataGrid;
                    foreach (Window item in mywindow.OwnedWindows)
                    {
                        if (item.Name == "winRequestNew")
                        {
                            if ((item.DataContext as Classes.Domain.RequestVMCommand).VModel.Id == (dg.CurrentItem as Classes.Domain.RequestVM).Id)
                                newWin = item as RequestNewWin;
                        }
                    }
                    if (newWin == null)
                    {
                        newWin = new RequestNewWin();
                        newWin.Owner = mywindow;

                        newWin.thisStoragePointValidationRule.RequestId = (dg.CurrentItem as Classes.Domain.RequestVM).Id;
                        Classes.Domain.RequestVMCommand cmd = new Classes.Domain.RequestVMCommand((dg.CurrentItem as Classes.Domain.RequestVM), myparcel.ParcelRequests);
                        newWin.DataContext = cmd;
                        newWin.Show();
                    }
                    else
                    {
                        newWin.Activate();
                        if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
                    }
                }
                e.Handled = true;
            }
        }
        private void RequestsDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = e.Row.Item != null && !(e.Row.Item as Classes.Domain.RequestVM).DomainObject.Blocking();
        }
        private void RequestsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if ((e.Row.Item as RequestVM).DomainState == lib.DomainObjectState.Unchanged) (e.Row.Item as RequestVM).DomainObject.UnBlocking();
        }

        private void RequestFolderOpen_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button && (sender as Button).Tag is Classes.Domain.RequestVM)
            {
                try
                {
                    Classes.Domain.RequestVM item = (sender as Button).Tag as Classes.Domain.RequestVM;
                    myparcelbinddisp.EndEdit();
                    item.DomainObject.DocFolderOpen();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Папка документов");
                }
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
            }
            newHistory.Owner = mywindow;
            newHistory.Show();
        }

        private void RequestAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (myparcel == null) return;
            if (myparcel.DomainState == lib.DomainObjectState.Added)
                (this.ParentDataContext as lib.ViewModelBaseCommand).Save.Execute(null);
            if ((NoParcelRequestDataGrid.SelectedIndex > -1) | (NoParcelRequestDataGrid.Items.Count == 1))
            {
                if (NoParcelRequestDataGrid.Items.Count == 1) this.NoParcelRequestDataGrid.SelectedItems.Add(this.NoParcelRequestDataGrid.Items[0]);

                Classes.Domain.RequestVM[] rows = new Classes.Domain.RequestVM[NoParcelRequestDataGrid.SelectedItems.Count];
                for (int i = 0; i < NoParcelRequestDataGrid.SelectedItems.Count; i++)
                {
                    Classes.Domain.RequestVM row = this.NoParcelRequestDataGrid.SelectedItems[i] as Classes.Domain.RequestVM;
                    if (!row.InvoiceDiscountFill.Value)
                    {
                        MessageBox.Show("В заявке " + row.StorePointDate + " инвойс со скидкой не разнесен по юр лицам!", "Постановка в загрузку", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    if (!row.Validate(true))
                    {
                        MessageBox.Show(row.Errors, "Постановка в загрузку", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    rows[i] = this.NoParcelRequestDataGrid.SelectedItems[i] as Classes.Domain.RequestVM;
                }
                this.NoParcelRequestDataGrid.SelectedItems.Clear();
                foreach (Classes.Domain.RequestVM row in rows)
                {
                    try
                    {
                        if (row.DomainObject.Blocking())
                        {
                            myparcel.ParcelRequests.EditItem(row);
                            myparcel.Requests.EditItem(row);
                            row.DomainObject.Parcel = myparcel.DomainObject;
                            row.DomainObject.Status = myparcel.Status;
                            myparcel.Requests.CommitEdit();
                            myparcel.ParcelRequests.CommitEdit();
                        }
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message, "Поставка заявки в загрузку"); }
                }
            }
            else
            {
                MessageBox.Show("Выделите строки в нижнем списке", "Постановка в загрузку", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void RequestOutButton_Click(object sender, RoutedEventArgs e)
        {
            if ((ParcelRequestDataGrid.SelectedIndex > -1) | (ParcelRequestDataGrid.Items.Count == 1))
            {
                if (ParcelRequestDataGrid.Items.Count == 1) this.ParcelRequestDataGrid.SelectedItems.Add(this.ParcelRequestDataGrid.Items[0]);
                Classes.Domain.RequestVM[] rows = new Classes.Domain.RequestVM[ParcelRequestDataGrid.SelectedItems.Count];
                for (int i = 0; i < ParcelRequestDataGrid.SelectedItems.Count; i++)
                {
                    rows[i] = this.ParcelRequestDataGrid.SelectedItems[i] as Classes.Domain.RequestVM;
                }
                ParcelRequestDataGrid.SelectionChanged -= ParcelRequestDataGrid_SelectionChanged;
                foreach (Classes.Domain.RequestVM row in rows)
                {
                    try
                    {
                        if (row.DomainObject.Blocking())
                        {
                            myparcel.ParcelRequests.EditItem(row);
                            myparcel.Requests.EditItem(row);
                            row.DomainObject.ParcelId = null; // не устанавливать через Parcel не обновляется после Refresh
                            row.StoreInform = null;
                            row.DomainObject.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 40);
                            myparcel.Requests.CommitEdit();
                            myparcel.ParcelRequests.CommitEdit();
                        }
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message, "Снятие заявки с загрузки"); }
                }
                ParcelRequestDataGrid.SelectionChanged += ParcelRequestDataGrid_SelectionChanged;
            }
            else
            {
                MessageBox.Show("Выделите строку в верхнем списке", "Снятие с загрузки", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void SpecificationDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGrid)?.CurrentItem is Classes.Specification.SpecificationVM)
            {
                if ((sender as DataGrid).CurrentCell.Column.SortMemberPath == "CFPR" || (sender as DataGrid).CurrentCell.Column.SortMemberPath == "Importer.Name")
                {
                    SpecificationWin newWin = null;
                    DataGrid dg = sender as DataGrid;
                    foreach (Window item in mywindow.OwnedWindows)
                    {
                        if (item.Name == "winSpecification")
                        {
                            if ((item.DataContext as Classes.Specification.SpecificationVMCommand).VModel.Id == (dg.CurrentItem as Classes.Specification.SpecificationVM).Id)
                                newWin = item as SpecificationWin;
                        }
                    }
                    if (newWin == null)
                    {
                        newWin = new SpecificationWin();
                        newWin.Owner = mywindow;

                        Classes.Specification.SpecificationVMCommand cmd = new Classes.Specification.SpecificationVMCommand((dg.CurrentItem as Classes.Specification.SpecificationVM), myparcel.Specifications);
                        newWin.DataContext = cmd;
                        newWin.Show();
                    }
                    else
                    {
                        newWin.Activate();
                        if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
                    }
                }
                e.Handled = true;
            }
        }

        private void ParcelRequestDataGrid_SizeChanged(DataGridColumn column)
        {
            int position = this.ParcelRequestDataGrid.Columns.IndexOf(column);
            if ((this.ParcelRequestDataGrid.IsLoaded && column.ActualWidth != this.NoParcelRequestDataGrid.Columns[position].ActualWidth) || column.ActualWidth > this.NoParcelRequestDataGrid.Columns[position].ActualWidth)
                this.NoParcelRequestDataGrid.Columns[position].Width = column.ActualWidth;
        }
        private void NoParcelRequestDataGrid_SizeChanged(DataGridColumn column)
        {
            int position = this.NoParcelRequestDataGrid.Columns.IndexOf(column);
            if ((this.NoParcelRequestDataGrid.IsLoaded && column.ActualWidth != this.ParcelRequestDataGrid.Columns[position].ActualWidth) || column.ActualWidth > this.ParcelRequestDataGrid.Columns[position].ActualWidth)
                this.ParcelRequestDataGrid.Columns[position].Width = column.ActualWidth;
        }
    }
}
