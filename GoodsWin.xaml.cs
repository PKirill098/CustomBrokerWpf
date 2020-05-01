using KirillPolyanskiy.CustomBrokerWpf;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Specification;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for GoodsWin.xaml
    /// </summary>
    public partial class GoodsWin : Window
    {
        private lib.Metadata.MetadataDataGrid mygoodsmetadatadatagrid;
        private lib.BindingDischarger mybranchbindingdischarger;
        private lib.BindingDischarger mytnvedbindingdischarger;

        public GoodsWin()
        {
            InitializeComponent();
            mybranchbindingdischarger = new lib.BindingDischarger(this, new DataGrid[] { this.BranchDataGrid });
            mytnvedbindingdischarger = new lib.BindingDischarger(this, new DataGrid[] { this.TNVEDGroupDataGrid });
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Classes.Specification.MaterialViewCommand matcmd = new Classes.Specification.MaterialViewCommand();
            matcmd.EndEdit = this.materialEndEdit;
            matcmd.CancelEdit = this.materialCancelEdit;
            this.MaterialTab.DataContext = matcmd;
            this.IngredientTab.DataContext = matcmd;
            Classes.Domain.GoodsViewCommand gcmd = new Classes.Domain.GoodsViewCommand();
            gcmd.EndEdit = this.goodsEndEdit;
            gcmd.CancelEdit = this.goodsCancelEdit;
            this.GoodsTab.DataContext = gcmd;
            mygoodsmetadatadatagrid = new lib.Metadata.MetadataDataGrid("mainDataGrid", null, mainDataGrid);
            mygoodsmetadatadatagrid.ConnectionString = References.ConnectionString;
            mygoodsmetadatadatagrid.ExcludeColumnsAdd(new int[] { 0, 3, 9, 13 });
            mygoodsmetadatadatagrid.Set();
            Classes.Domain.BranchCountryCommand bcmd = new Classes.Domain.BranchCountryCommand();
            bcmd.CancelEdit = mybranchbindingdischarger.CancelEdit;
            bcmd.EndEdit = mybranchbindingdischarger.EndEdit;
            this.BranchTab.DataContext = bcmd;
            BranchColumnsRefresh();
            Classes.Specification.MappingViewCommand mapcmd = new Classes.Specification.MappingViewCommand();
            mapcmd.EndEdit = this.mappingEndEdit;
            mapcmd.CancelEdit = this.mappingCancelEdit;
            //CollectionViewSource mappingmaterial = this.MappingTab.FindResource("keyMappingMaterials") as CollectionViewSource;
            //mappingmaterial.Source = mapcmd.Materials;
            //mappingmaterial.View.Filter = delegate (object item) { Material mitem = item as Material; return Classes.Specification.MappingViewCommand.ViewFilterDefault(item) & (mitem.Id == 12 | mitem.Id == 13 | mitem.Upper?.Id == 15 | mitem.Upper?.Id == 16 | mitem.Upper?.Id==22 | mitem.Upper?.Id == 23); };
            this.MappingTab.DataContext = mapcmd;
            Classes.Domain.GenderViewCommand grcmd = new Classes.Domain.GenderViewCommand();
            grcmd.EndEdit = this.genderEndEdit;
            grcmd.CancelEdit = this.genderCancelEdit;
            this.GenderTab.DataContext = grcmd;
            Classes.Specification.TNVEDGroupViewCommand tgcmd = new Classes.Specification.TNVEDGroupViewCommand();
            tgcmd.CancelEdit = mytnvedbindingdischarger.CancelEdit;
            tgcmd.EndEdit = mytnvedbindingdischarger.EndEdit;
            this.TNVEDGroupTab.DataContext = tgcmd;

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Classes.Specification.MaterialViewCommand matcmd = this.MaterialTab.DataContext as Classes.Specification.MaterialViewCommand;
            Classes.Specification.MaterialViewCommand ingcmd = this.IngredientTab.DataContext as Classes.Specification.MaterialViewCommand;
            Classes.Domain.GoodsViewCommand vm = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            Classes.Domain.BranchCountryCommand brhcmd = this.BranchTab.DataContext as Classes.Domain.BranchCountryCommand;
            Classes.Specification.MappingViewCommand mapcmd = this.MappingTab.DataContext as Classes.Specification.MappingViewCommand;
            Classes.Domain.GenderViewCommand gnrcmd = new Classes.Domain.GenderViewCommand();
            if (vmEndEdit() & mappingEndEdit() & materialEndEdit() & ingredientEndEdit() & genderEndEdit() & mybranchbindingdischarger.EndEdit())
            {
                bool isdirty = false, matdirty = false, mapdirty = false, gnrdirty = false, brhdirty = false;
                foreach (Classes.Specification.MaterialVM item in ingcmd.Items.SourceCollection) matdirty = matdirty | item.IsDirty;
                foreach (Classes.Domain.GoodsVM item in vm.Items.SourceCollection) isdirty = isdirty | item.IsDirty;
                foreach (Classes.Domain.BranchCountry item in brhcmd.Items.SourceCollection) brhdirty = brhdirty | item.IsDirty;
                foreach (Classes.Specification.MappingVM item in mapcmd.Items.SourceCollection) mapdirty = mapdirty | item.IsDirty;
                gnrdirty = gnrcmd.IsDirtyTree;
                if (isdirty | matdirty | mapdirty | gnrdirty | brhdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!vm.SaveDataChanges())
                        {
                            this.Activate();
                            GoodsTab.IsSelected = true;
                            if (MessageBox.Show("\nИзменения в ДС не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                vm.Reject.Execute(null);
                        }
                        if (!brhcmd.SaveDataChanges())
                        {
                            this.Activate();
                            BranchTab.IsSelected = true;
                            if (MessageBox.Show("\nИзменения в филиалах не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                brhcmd.Reject.Execute(null);
                        }
                        if (!ingcmd.SaveDataChanges())
                        {
                            this.Activate();
                            IngredientTab.IsSelected = true;
                            if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            //else
                            //    matcmd.Reject.Execute(null);
                        }
                        if (!mapcmd.SaveDataChanges())
                        {
                            this.Activate();
                            MappingTab.IsSelected = true;
                            if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                mapcmd.Reject.Execute(null);
                        }
                        if (!gnrcmd.SaveDataChanges())
                        {
                            this.Activate();
                            GenderTab.IsSelected = true;
                            if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            //else
                            //    gnrcmd.Reject.Execute(null);
                        }
                    }
                    else
                    {
                        vm.Reject.Execute(null);
                        brhcmd.Reject.Execute(null);
                        //matcmd.Reject.Execute(null);
                        //gnrcmd.Reject.Execute(null);
                        mapcmd.Reject.Execute(null);
                    }
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
                    vm.Reject.Execute(null);
                    brhcmd.Reject.Execute(null);
                    //matcmd.Reject.Execute(null);
                    mapcmd.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                mygoodsmetadatadatagrid.Save();
                References.BranchStore.Clear();
                References.GoodsStore.Clear();
                this.GoodsTab.DataContext = null;
                this.BranchTab.DataContext = null;
                this.MaterialTab.DataContext = null;
                this.IngredientTab.DataContext = null;
                this.MappingTab.DataContext = null;
                this.GenderTab.DataContext = null;
                if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainDataGrid.ScrollIntoView(CollectionView.NewItemPlaceholder);
            OpenItem((this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand).Items.AddNewItem(new Classes.Domain.GoodsVM()) as Classes.Domain.GoodsVM);
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InfoBatchButton_Click(object sender, RoutedEventArgs e)
        {
            OpenItem((sender as Button).Tag as Classes.Domain.GoodsVM);
        }
        private void OpenItem(Classes.Domain.GoodsVM item)
        {
            vmEndEdit();
            GoodsItemWin win = new GoodsItemWin();
            Classes.Domain.GoodsCommand cmd = new Classes.Domain.GoodsCommand(item, (this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand).Items);
            cmd.EndEdit = win.vmEndEdit;
            cmd.CancelEdit = win.vmCancelEdit;
            win.DataContext = cmd;
            win.Owner = this;
            win.Show();
        }

        private void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            (this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand)?.FilterRun.Execute(null);
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand).FilterRun.Execute(null);
        }

        private void MaterialAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.materialDataGrid.ScrollIntoView(CollectionView.NewItemPlaceholder);
        }
        private void MaterialTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (this.MaterialTab.DataContext as Classes.Specification.MaterialViewCommand).FilterRun.Execute(null);
        }

        private void ExpandItems(bool isExpanded)
        {
            for (int i = 0; i < this.GenderTreeView.Items.Count; i++)
            {
                TreeViewItem itemTreeView = (TreeViewItem)GenderTreeView.ItemContainerGenerator.ContainerFromIndex(i);
                ExpandItem(itemTreeView, isExpanded);
            }
        }
        private void ExpandItem(TreeViewItem item, bool isExpanded)
        {
            item.IsExpanded = isExpanded;
            for (int i = 0; i < item.Items.Count; i++)
            {
                TreeViewItem itemTreeView = (TreeViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
                if (itemTreeView != null) ExpandItem(itemTreeView, isExpanded);
            }
        }
        private void ButtonExpended_Click(object sender, RoutedEventArgs e)
        {
            ExpandItems(true);
        }
        private void ButtonCollapsed_Click(object sender, RoutedEventArgs e)
        {
            ExpandItems(false);
        }

        public bool goodsEndEdit()
        {
            bool isEnd = true;
            isEnd = isEnd & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            isEnd = isEnd & vmEndEdit();
            return isEnd;
        }
        public void goodsCancelEdit()
        {
            this.mainDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.mainDataGrid.CancelEdit(DataGridEditingUnit.Row);
            vmCancelEdit();
        }
        public bool mappingEndEdit()
        {
            bool isEnd = true;
            isEnd = isEnd & this.mappingDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.mappingDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            isEnd = isEnd & vmEndEdit();
            return isEnd;
        }
        public void mappingCancelEdit()
        {
            this.mappingDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.mappingDataGrid.CancelEdit(DataGridEditingUnit.Row);
            vmCancelEdit();
        }
        public bool materialEndEdit()
        {
            bool isEnd = true;
            isEnd = isEnd & this.materialDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.materialDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            isEnd = isEnd & vmEndEdit();
            return isEnd;
        }
        public void materialCancelEdit()
        {
            this.materialDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.materialDataGrid.CancelEdit(DataGridEditingUnit.Row);
            vmCancelEdit();
        }
        public bool ingredientEndEdit()
        {
            bool isEnd = true;
            isEnd = isEnd & this.IngredientNatDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.IngredientNatDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            isEnd = isEnd & this.IngredientChmDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.IngredientChmDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            isEnd = isEnd & this.IngredientOthDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.IngredientOthDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            isEnd = isEnd & this.IngredientNoWDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.IngredientNoWDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            isEnd = isEnd & this.IngredientHlfDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.IngredientHlfDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            isEnd = isEnd & vmEndEdit();
            return isEnd;
        }
        public void ingredientCancelEdit()
        {
            this.IngredientNatDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.IngredientNatDataGrid.CancelEdit(DataGridEditingUnit.Row);
            this.IngredientChmDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.IngredientChmDataGrid.CancelEdit(DataGridEditingUnit.Row);
            this.IngredientOthDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.IngredientOthDataGrid.CancelEdit(DataGridEditingUnit.Row);
            this.IngredientNoWDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.IngredientNoWDataGrid.CancelEdit(DataGridEditingUnit.Row);
            this.IngredientHlfDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.IngredientHlfDataGrid.CancelEdit(DataGridEditingUnit.Row);
            vmCancelEdit();
        }
        public bool genderEndEdit()
        {
            bool isEnd = true;
            isEnd = isEnd & vmEndEdit();
            return isEnd;
        }
        public void genderCancelEdit()
        {
            vmCancelEdit();
        }
        public bool vmEndEdit()
        {
            bool isEnd = true;
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

        private void IngredientDataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w;
            w = IngredientChmDataGrid.Columns[0].ActualWidth;
            if (w < IngredientHlfDataGrid.Columns[0].ActualWidth)
                w = IngredientHlfDataGrid.Columns[0].ActualWidth;
            if (w < IngredientNatDataGrid.Columns[0].ActualWidth)
                w = IngredientNatDataGrid.Columns[0].ActualWidth;
            if (w < IngredientNoWDataGrid.Columns[0].ActualWidth)
                w = IngredientNoWDataGrid.Columns[0].ActualWidth;
            if (w < IngredientOthDataGrid.Columns[0].ActualWidth)
                w = IngredientOthDataGrid.Columns[0].ActualWidth;
            if (IngredientChmDataGrid.IsInitialized && w != IngredientChmDataGrid.Columns[0].ActualWidth)
                IngredientChmDataGrid.Columns[0].MinWidth = w;
            if (IngredientHlfDataGrid.IsInitialized && w != IngredientHlfDataGrid.Columns[0].ActualWidth)
                IngredientHlfDataGrid.Columns[0].MinWidth = w;
            if (IngredientNatDataGrid.IsInitialized && w != IngredientNatDataGrid.Columns[0].ActualWidth)
                IngredientNatDataGrid.Columns[0].MinWidth = w;
            if (IngredientNoWDataGrid.IsInitialized && w != IngredientNoWDataGrid.Columns[0].ActualWidth)
                IngredientNoWDataGrid.Columns[0].MinWidth = w;
            if (IngredientOthDataGrid.IsInitialized && w != IngredientOthDataGrid.Columns[0].ActualWidth)
                IngredientOthDataGrid.Columns[0].MinWidth = w;
        }

        private void IngredientDelete(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить материал?", "Удаление материала", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                (this.MaterialTab.DataContext as Classes.Specification.MaterialViewCommand).IngredientDelete(sender);
            e.Handled = true;
        }
        private void IngredientCanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as DataGrid).SelectedItems.Count > 0 && (this.MaterialTab.DataContext as Classes.Specification.MaterialViewCommand).IngredientCanDelete(sender);
            e.Handled = true;
        }
        private void IngredientOpen(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            MaterialWin mwin = null;
            foreach (Window win in this.OwnedWindows)
            {
                if (win.Name == "winMaterial")
                    if (object.Equals((win.DataContext as Classes.Specification.MaterialCommand).VModel, dg.SelectedItem)) mwin = win as MaterialWin;
            }
            if (mwin == null)
            {
                ingredientEndEdit();
                mwin = new MaterialWin();
                Classes.Specification.MaterialCommand cmd = new Classes.Specification.MaterialCommand(dg.SelectedItem as Classes.Specification.MaterialVM, dg.ItemsSource as ListCollectionView);
                cmd.EndEdit = mwin.vmEndEdit;
                cmd.CancelEdit = mwin.vmCancelEdit;
                mwin.DataContext = cmd;
                mwin.Owner = this;
                mwin.Show();
            }
            else
            {
                mwin.Activate();
                if (mwin.WindowState == WindowState.Minimized) mwin.WindowState = WindowState.Normal;
            }

            e.Handled = true;
        }
        private void IngredientCanOpen(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as DataGrid).SelectedItem != null;
            e.Handled = true;
        }
        private void IngredientNew(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            System.Windows.Data.ListCollectionView view = dg.ItemsSource as System.Windows.Data.ListCollectionView;
            if (ingredientEndEdit() && !(view.IsAddingNew | view.IsEditingItem))
            {
                int upperid = 0;
                switch (dg.Name)
                {
                    case "IngredientNatDataGrid":
                        upperid = 15;
                        break;
                    case "IngredientChmDataGrid":
                        upperid = 16;
                        break;
                    case "IngredientOthDataGrid":
                        upperid = 23;
                        break;
                    case "IngredientNoWDataGrid":
                        upperid = 22;
                        break;
                    case "IngredientHlfDataGrid":
                        upperid = 17;
                        break;
                }

                Classes.Specification.MaterialVM newitem = view.AddNew() as Classes.Specification.MaterialVM;
                newitem.DomainObject.Upper = References.Materials.FindFirstItem("Id", upperid);
                view.CommitNew();
                Classes.Specification.MaterialCommand cmd = new Classes.Specification.MaterialCommand(newitem, view);
                MaterialWin mwin = new MaterialWin();
                cmd.EndEdit = mwin.vmEndEdit;
                cmd.CancelEdit = mwin.vmCancelEdit;
                mwin.DataContext = cmd;
                mwin.Owner = this;
                mwin.Show();
                e.Handled = true;
            }
        }
        private void IngredientCanNew(object sender, CanExecuteRoutedEventArgs e)
        {
            System.Windows.Data.ListCollectionView view = (sender as DataGrid).ItemsSource as System.Windows.Data.ListCollectionView;
            e.CanExecute = !(view.IsAddingNew | view.IsEditingItem);
            e.Handled = true;
        }
        private void IngredientTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (this.MaterialTab.DataContext as Classes.Specification.MaterialViewCommand).FilterRun.Execute(null);
        }
        private void IngredientSelect_Click(object sender, RoutedEventArgs e)
        {

        }
        private void IngredientSelect(string find)
        {
            System.Collections.Generic.List<Material> items = new System.Collections.Generic.List<Material>();
            foreach (Material item in References.Materials)
            {
                if (item.Upper == null) continue;
                if (string.Equals(item.GoodsName, find))
                { items.Add(item); continue; }
                bool where = true;
                string[] str;
                str = find.Trim().ToLower().Split(' ');
                foreach (string stritem in str)
                {
                    where &= item.Name.ToLower().IndexOf(stritem) > -1;
                }
                if (where) items.Add(item);
            }
            //for(int i=0;i<items.Count;i++)
        }

        private void CertTypeFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            //if(cmd.CertTypeFilterCommand?.Items==null) cmd.CertTypeFilterCommand?.FillAsync();
            if (cmd.CertTypeFilterCommand != null && !cmd.CertTypeFilterCommand.FilterOn) cmd.CertTypeFilterCommand?.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("CertTypeFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void GoodsNameFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            //if (cmd.GoodsNameFilterCommand!=null && cmd.GoodsNameFilterCommand.Items == null)
            if (cmd.GoodsNameFilterCommand != null && !cmd.GoodsNameFilterCommand.FilterOn) cmd.GoodsNameFilterCommand?.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("GoodsNameFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void GenderFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.mainDataGrid.FindResource("GenderFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void MaterialFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.MaterialFilterCommand != null && !cmd.MaterialFilterCommand.FilterOn) cmd.MaterialFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("MaterialFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ContextureFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.ContextureFilterCommand != null && !cmd.ContextureFilterCommand.FilterOn) cmd.ContextureFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("ContextureFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void TNVEDFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.TNVEDFilterCommand != null && !cmd.TNVEDFilterCommand.FilterOn) cmd.TNVEDFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("TNVEDFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void BrandFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.BrandFilterCommand != null && !cmd.BrandFilterCommand.FilterOn) cmd.BrandFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("BrandFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ProducerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.ProducerFilterCommand != null && !cmd.ProducerFilterCommand.FilterOn) cmd.ProducerFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("ProducerFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void TitleCountryFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.TitleCountryFilterCommand != null && !cmd.TitleCountryFilterCommand.FilterOn) cmd.TitleCountryFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("TitleCountryFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void Cat1FilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.Cat1FilterCommand?.Items == null && !cmd.Cat1FilterCommand.FilterOn) cmd.Cat1FilterCommand?.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("Cat1FilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CertificateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.CertificateFilterCommand.Items == null && !cmd.CertificateFilterCommand.FilterOn) cmd.CertificateFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("CertificateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ContractNmbrFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.ContractNmbrFilterCommand.Items == null && !cmd.ContractNmbrFilterCommand.FilterOn) cmd.ContractNmbrFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("ContractNmbrFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CertStopFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.mainDataGrid.FindResource("CertStopFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void VendorCodeFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.VendorCodeFilterCommand.Items == null && !cmd.VendorCodeFilterCommand.FilterOn) cmd.VendorCodeFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("VendorCodeFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void DeclarantFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.GoodsViewCommand cmd = this.GoodsTab.DataContext as Classes.Domain.GoodsViewCommand;
            if (cmd.DeclarantFilterCommand.Items == null && !cmd.DeclarantFilterCommand.FilterOn) cmd.DeclarantFilterCommand.FillAsync();
            Popup ppp = this.mainDataGrid.FindResource("DeclarantFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }


        private void OnThumbLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            Popup ppp = this.BranchDataGrid.FindResource("PopupProducerFilter") as Popup;
            if (double.IsNaN(ppp.Width))
                ppp.Width = (((sender as Thumb).Parent as Grid).Parent as Border).ActualWidth;
            double xadjust = ppp.Width - e.HorizontalChange;
            if (xadjust >= 50)
            {
                ppp.HorizontalOffset = ppp.HorizontalOffset + e.HorizontalChange;
                ppp.Width = xadjust;
            }
        }
        private void OnThumbTopDragDelta(object sender, DragDeltaEventArgs e)
        {
            Popup ppp = this.BranchDataGrid.FindResource("PopupProducerFilter") as Popup;
            if (double.IsNaN(ppp.Height))
                ppp.Height = (((sender as Thumb).Parent as Grid).Parent as Border).ActualHeight;
            double yadjust = ppp.Height - e.VerticalChange;
            if (yadjust >= 100)
            {
                ppp.VerticalOffset = ppp.VerticalOffset + e.VerticalChange;
                ppp.Height = yadjust;
            }
        }
        private void OnThumbRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            Popup ppp = this.BranchDataGrid.FindResource("PopupProducerFilter") as Popup;
            if (double.IsNaN(ppp.Width))
                ppp.Width = (((sender as Thumb).Parent as Grid).Parent as Border).ActualWidth;
            double xadjust = ppp.Width + e.HorizontalChange;
            if (xadjust >= 50)
            {
                ppp.Width = xadjust;
                ppp.UpdateLayout();
            }
        }
        private void OnThumbBottomDragDelta(object sender, DragDeltaEventArgs e)
        {
            Popup ppp = this.BranchDataGrid.FindResource("PopupProducerFilter") as Popup;
            if (double.IsNaN(ppp.Height))
                ppp.Height = (((sender as Thumb).Parent as Grid).Parent as Border).ActualHeight;
            double yadjust = ppp.Height + e.VerticalChange;
            if (yadjust >= 100)
            {
                ppp.Height = yadjust;
            }
        }
        #region Branch
        bool BranchTabInit;
        private void BranchTab_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!BranchTabInit)
                (this.BranchTab.DataContext as Classes.Domain.BranchCountryCommand)?.Refresh.Execute(null);
            BranchTabInit = true;
        }

        private void BranchDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Column.Width = DataGridLength.SizeToCells;
            Classes.Domain.BranchCountry item = e.Row.DataContext as Classes.Domain.BranchCountry;
            if (item.Branches[e.Column.DisplayIndex - 2] == null)
            {
                Classes.Domain.BranchCountryCommand brhcmd = this.BranchTab.DataContext as Classes.Domain.BranchCountryCommand;
                Classes.Domain.BranchVM branch = new Classes.Domain.BranchVM();
                item.Branches[e.Column.DisplayIndex - 2] = branch;
                branch.DomainObject.Goods = item.Goods.DomainObject;
                branch.DomainObject.Country = brhcmd.Countries[e.Column.DisplayIndex - 2];
                item.PropertyChangedNotification("Branches");
            }
        }
        private void BranchDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            e.Column.Width = DataGridLength.SizeToHeader;
            Classes.Domain.BranchCountry item = e.Row.DataContext as Classes.Domain.BranchCountry;
            if (string.IsNullOrWhiteSpace((e.EditingElement as TextBox).Text) & item.Branches[e.Column.DisplayIndex - 2]?.DomainState == lib.DomainObjectState.Added)
                item.Branches[e.Column.DisplayIndex - 2] = null;
        }

        private void BranchAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.BranchDataGrid.ScrollIntoView(CollectionView.NewItemPlaceholder);
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            BranchColumnsRefresh();
        }
        private void BranchColumnsRefresh()
        {
            Classes.Domain.BranchCountryCommand brhcmd = this.BranchTab.DataContext as Classes.Domain.BranchCountryCommand;
            brhcmd.CountriesRefresh();
            for (int i = 0; i < brhcmd.Countries.Length; i++)
            {
                if (i > BranchDataGrid.Columns.Count - 3)
                {
                    DataGridTextColumn column = new DataGridTextColumn();
                    column.CanUserResize = false;
                    column.CanUserSort = false;
                    column.Width = DataGridLength.SizeToHeader;
                    column.Binding = new Binding("Branches[" + i.ToString() + "].Name");
                    Style style = new Style(typeof(DataGridCell));
                    if (column.CellStyle != null)
                        style.BasedOn = column.CellStyle;
                    Binding binding = new Binding("Branches[" + i.ToString() + "].Goods.ColorMark");
                    binding.Mode = BindingMode.OneWay;
                    style.Setters.Add(new Setter(DataGridCell.BackgroundProperty, binding));
                    style.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black)));
                    binding = new Binding("Content.Text");
                    binding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
                    style.Setters.Add(new Setter(DataGridCell.ToolTipProperty, binding));
                    column.CellStyle = style;
                    BranchDataGrid.Columns.Add(column);
                }
                BranchDataGrid.Columns[i + 2].Header = brhcmd.Countries[i];
            }
        }

        private void BranchCertificate2_OpenFilter(object sender, MouseButtonEventArgs e)
        {
            (this.BranchTab.DataContext as Classes.Domain.BranchCountryCommand).CertificateFilterCommand.Refresh();
            Popup ppp = this.BranchDataGrid.FindResource("Certificate2FilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void BranchProducer_OpenFilter(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.BranchCountryCommand brhcmd = this.BranchTab.DataContext as Classes.Domain.BranchCountryCommand;
            brhcmd.ProducerFilterCommand?.Refresh();
            //if (myBranchProducerWindowFilter == null)
            //{
            //    myBranchProducerWindowFilter = new libui.CheckListBoxWindow();
            //    myBranchProducerWindowFilter.DataContext = brhcmd.ProducerFilterCommand;
            //    Point loc = this.BranchDataGrid.PointToScreen(new Point(0, 0));
            //    myBranchProducerWindowFilter.Left = loc.X + this.BranchDataGrid.Columns[0].ActualWidth+ this.BranchDataGrid.Columns[1].ActualWidth;
            //    myBranchProducerWindowFilter.Top = loc.Y;
            //    myBranchProducerWindowFilter.MaxHeight = References.WorkAreaHight;
            //    myBranchProducerWindowFilter.SizeToContent = SizeToContent.Manual;
            //    myBranchProducerWindowFilter.Height = References.WorkAreaHight - myBranchCertificate2WindowFilter.Top;
            //    myBranchProducerWindowFilter.Width = 300;
            //}
            //myBranchProducerWindowFilter.Show();
            //System.Windows.Controls.Primitives.Popup ppp = new System.Windows.Controls.Primitives.Popup();
            //ppp.Placement = PlacementMode.Right;
            //ppp.PopupAnimation = PopupAnimation.Fade;
            //ppp.AllowsTransparency = true;
            //ppp.StaysOpen = false;
            //Border br = new Border();
            //br.BorderBrush = System.Windows.Media.Brushes.Black;
            //br.BorderThickness = new Thickness(1D);
            //br.Background = System.Windows.Media.Brushes.WhiteSmoke;
            //br.CornerRadius = new CornerRadius(7D);
            //libui.CheckListBoxUC uc = new libui.CheckListBoxUC();
            //br.Child = uc;
            //ppp.Child = br;
            Popup ppp = this.BranchDataGrid.FindResource("PopupProducerFilter") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            //ppp.DataContext = brhcmd.ProducerFilterCommand;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        #endregion
        #region Mapping
        private void MappingAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.mappingDataGrid.ScrollIntoView(CollectionView.NewItemPlaceholder);
            MappingOpenItem((this.MappingTab.DataContext as Classes.Specification.MappingViewCommand).Items.AddNewItem(new Classes.Specification.MappingVM()) as Classes.Specification.MappingVM);
        }
        private void MappingTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (this.MappingTab.DataContext as Classes.Specification.MappingViewCommand).FilterRun.Execute(null);
        }
        private void MappingInfoBatchButton_Click(object sender, RoutedEventArgs e)
        {
            MappingOpenItem((sender as Button).Tag as Classes.Specification.MappingVM);
        }
        private void MappingOpenItem(Classes.Specification.MappingVM item)
        {
            vmEndEdit();
            MappingWin win = new MappingWin();
            Classes.Specification.MappingCommand cmd = new Classes.Specification.MappingCommand(item, (this.MappingTab.DataContext as Classes.Specification.MappingViewCommand).Items);
            cmd.EndEdit = win.vmEndEdit;
            cmd.CancelEdit = win.vmCancelEdit;
            win.DataContext = cmd;

            win.Owner = this;
            win.Show();
        }

        private void MappingGoodsNameFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            MappingViewCommand cmd = this.MappingTab.DataContext as MappingViewCommand;
            if (cmd.GoodsNameFilterCommand != null && !cmd.GoodsNameFilterCommand.FilterOn) cmd.GoodsNameFilterCommand?.FillAsync();
            Popup ppp = this.mappingDataGrid.FindResource("GoodsNameFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void MappingTNVEDFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            MappingViewCommand cmd = this.MappingTab.DataContext as MappingViewCommand;
            if (cmd.TNVEDFilterCommand != null && !cmd.TNVEDFilterCommand.FilterOn) cmd.TNVEDFilterCommand?.FillAsync();
            Popup ppp = this.mappingDataGrid.FindResource("TNVEDFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void MappingMaterialFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            MappingViewCommand cmd = this.MappingTab.DataContext as MappingViewCommand;
            if (cmd.MaterialFilterCommand != null && !cmd.MaterialFilterCommand.FilterOn) cmd.MaterialFilterCommand?.FillAsync();
            Popup ppp = this.mappingDataGrid.FindResource("MaterialFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void MappingGenderFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.mappingDataGrid.FindResource("GenderFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        #endregion
        #region TNVED Group
        private void TNVEDGroupAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.TNVEDGroupDataGrid.ScrollIntoView(CollectionView.NewItemPlaceholder);
            TNVEDGroupOpenItem((this.TNVEDGroupTab.DataContext as Classes.Specification.TNVEDGroupViewCommand).Items.AddNewItem(new Classes.Specification.TNVEDGroupVM()) as Classes.Specification.TNVEDGroupVM);
        }
        private void TNVEDGroupInfoBatchButton_Click(object sender, RoutedEventArgs e)
        {
            TNVEDGroupOpenItem((sender as Button).Tag as Classes.Specification.TNVEDGroupVM);
        }

        private void TNVEDGroupOpenItem(Classes.Specification.TNVEDGroupVM item)
        {
            vmEndEdit();
            TNVEDGroupWin win = new TNVEDGroupWin();
            Classes.Specification.TNVEDGroupCommand cmd = new Classes.Specification.TNVEDGroupCommand(item, (this.TNVEDGroupTab.DataContext as Classes.Specification.TNVEDGroupViewCommand).Items);
            win.DataContext = cmd;
            win.Owner = this;
            win.Show();
        }

        private void TNVEDGroupTNVEDFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            TNVEDGroupViewCommand cmd = this.TNVEDGroupTab.DataContext as TNVEDGroupViewCommand;
            if (cmd.TNVEDFilterCommand != null && !cmd.TNVEDFilterCommand.FilterOn) cmd.TNVEDFilterCommand?.FillAsync();
            Popup ppp = this.TNVEDGroupDataGrid.FindResource("TNVEDFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void TNVEDGroupMaterialFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            TNVEDGroupViewCommand cmd = this.TNVEDGroupTab.DataContext as TNVEDGroupViewCommand;
            if (cmd.MaterialFilterCommand != null && !cmd.MaterialFilterCommand.FilterOn) cmd.MaterialFilterCommand?.FillAsync();
            Popup ppp = this.TNVEDGroupDataGrid.FindResource("MaterialFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void TNVEDGroupGoodsNameFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            TNVEDGroupViewCommand cmd = this.TNVEDGroupTab.DataContext as TNVEDGroupViewCommand;
            if (cmd.GoodsNameFilterCommand != null && !cmd.GoodsNameFilterCommand.FilterOn) cmd.GoodsNameFilterCommand?.FillAsync();
            Popup ppp = this.TNVEDGroupDataGrid.FindResource("GoodsNameFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        #endregion

    }
}
