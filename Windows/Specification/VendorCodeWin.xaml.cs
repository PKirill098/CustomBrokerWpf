using KirillPolyanskiy.CustomBrokerWpf.Classes.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для VendorCodeWin.xaml
    /// </summary>
    public partial class VendorCodeWin : Window
    {
        public VendorCodeWin()
        {
            InitializeComponent();
            mybindingdischarger = new lib.BindingDischarger(this, new DataGrid[] { this.MainDataGrid });
            mymetadatadatagrid=new lib.Metadata.MetadataDataGrid(nameof(this.MainDataGrid),null,this.MainDataGrid);
            mymetadatadatagrid.ConnectionString = References.ConnectionString;
            //mymetadatadatagrid.ExcludeColumnsAdd(new int[] { 0, 3, 9, 13 });
            mymetadatadatagrid.Set();
            mycmd = new VendorCodeViewCommand();
            mycmd.CancelEdit = mybindingdischarger.CancelEdit;
            mycmd.EndEdit = mybindingdischarger.EndEdit;
            this.DataContext = mycmd;
        }

        private lib.Metadata.MetadataDataGrid mymetadatadatagrid;
        private lib.BindingDischarger mybindingdischarger;
        private VendorCodeViewCommand mycmd;

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mybindingdischarger.EndEdit())
            {
                bool isdirty = false;
                foreach (VendorCodeVM item in mycmd.Items.SourceCollection) isdirty = isdirty | item.DomainObject.IsDirty;
                if (isdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!mycmd.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения в ДС не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                mycmd.Reject.Execute(null);
                        }
                    }
                    else
                        mycmd.Reject.Execute(null);
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
                    mycmd.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                (App.Current.MainWindow as MainWindow).Activate();
                mymetadatadatagrid.Save();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainDataGrid.ScrollIntoView(CollectionView.NewItemPlaceholder);
        }

        private void BrandFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.BrandFilter != null && !mycmd.BrandFilter.FilterOn) mycmd.BrandFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("BrandFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ContextureFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.ContextureFilter != null && !mycmd.ContextureFilter.FilterOn) mycmd.ContextureFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("ContextureFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CountryRuFilterFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.CountryRuFilter != null && !mycmd.CountryRuFilter.FilterOn) mycmd.CountryRuFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("CountryRuFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void DescriptionFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.DescriptionFilter != null && !mycmd.DescriptionFilter.FilterOn) mycmd.DescriptionFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("DescriptionFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void GenderFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("GenderFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void GoodsFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.GoodsFilter != null && !mycmd.GoodsFilter.FilterOn) mycmd.GoodsFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("GoodsFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void NoteFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.NoteFilter != null && !mycmd.NoteFilter.FilterOn) mycmd.NoteFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("NoteFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void TNVEDFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.TNVEDFilter != null && !mycmd.TNVEDFilter.FilterOn) mycmd.TNVEDFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("TNVEDFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void TranslationFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.TranslationFilter != null && !mycmd.TranslationFilter.FilterOn) mycmd.TranslationFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("TranslationFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void VendorCodeFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.VendorCodeFilter != null && !mycmd.VendorCodeFilter.FilterOn) mycmd.VendorCodeFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("VendorCodeFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Copy_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainDataGrid.SelectedCells.Count == 1)
            {
                DataGridCellInfo cell = MainDataGrid.SelectedCells[0];
                if (cell.IsValid & cell.Item is VendorCodeVM)
                {
                    DataObject data;
                    switch (MainDataGrid.SelectedCells[0].Column.SortMemberPath)
                    {
                        case nameof(VendorCodeVM.Updated):
                            data = new DataObject(typeof(DateTime),(cell.Item as VendorCodeVM).Updated);
                            break;
                        default:
                            string str;
                            char[] trim = new char[] {(char)10, (char)13};
                            switch (MainDataGrid.SelectedCells[0].Column.SortMemberPath)
                            {
                                case nameof(VendorCodeVM.Code):
                                    str = (cell.Item as VendorCodeVM).Code;
                                    break;
                                case nameof(VendorCodeVM.Brand):
                                    str = (cell.Item as VendorCodeVM).Brand;
                                    break;
                                case nameof(VendorCodeVM.Goods):
                                    str = (cell.Item as VendorCodeVM).Goods;
                                    break;
                                case nameof(VendorCodeVM.Description):
                                    str = (cell.Item as VendorCodeVM).Description;
                                    break;
                                case nameof(VendorCodeVM.Contexture):
                                    str = (cell.Item as VendorCodeVM).Contexture;
                                    break;
                                case nameof(VendorCodeVM.Gender):
                                    str = (cell.Item as VendorCodeVM).Gender;
                                    break;
                                case nameof(VendorCodeVM.TNVED):
                                    str = (cell.Item as VendorCodeVM).TNVED;
                                    break;
                                case nameof(VendorCodeVM.Translation):
                                    str = (cell.Item as VendorCodeVM).Translation;
                                    break;
                                case nameof(VendorCodeVM.CountryRU):
                                    str = (cell.Item as VendorCodeVM).CountryRU;
                                    break;
                                case nameof(VendorCodeVM.Note):
                                    str = (cell.Item as VendorCodeVM).Note;
                                    break;
                                default:
                                    str = string.Empty;
                                    break;
                            }
                            str = str.TrimEnd(trim);
                            data = new DataObject(typeof(string), str);
                            break;
                    }
                    Clipboard.SetDataObject(data, false);
                }
            }
        }
        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!e.CanExecute)
                e.CanExecute = Clipboard.ContainsText();
        }
        private void Paste_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (Clipboard.ContainsText() && MainDataGrid.SelectedCells.Count > 0)
            {
                char[] trim = new char[] { (char)10, (char)13 };
                string text = Clipboard.GetText().TrimEnd(trim);
                foreach (DataGridCellInfo cell in MainDataGrid.SelectedCells)
                { 
                    VendorCodeVM item = cell.Item as VendorCodeVM;
                    switch (cell.Column.SortMemberPath)
                    {
                        case nameof(VendorCodeVM.Code):
                            item.Code= text;
                            break;
                        case nameof(VendorCodeVM.Brand):
                            item.Brand = text;
                            break;
                        case nameof(VendorCodeVM.Goods):
                            item.Goods = text;
                            break;
                        case nameof(VendorCodeVM.Description):
                            item.Description = text;
                            break;
                        case nameof(VendorCodeVM.Contexture):
                            item.Contexture = text;
                            break;
                        case nameof(VendorCodeVM.Gender):
                            item.Gender = text;
                            break;
                        case nameof(VendorCodeVM.TNVED):
                            item.TNVED = text;
                            break;
                        case nameof(VendorCodeVM.Translation):
                            item.Translation = text;
                            break;
                        case nameof(VendorCodeVM.CountryRU):
                            item.CountryRU = text;
                            break;
                        case nameof(VendorCodeVM.Note):
                            item.Note = text;
                            break;
                    }
                }
            }
        }
    }
}
