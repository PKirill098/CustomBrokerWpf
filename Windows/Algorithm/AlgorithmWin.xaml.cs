using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Algorithm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;


namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class AlgorithmWin : Window
    {
        private AlgorithmFormulaCommand mycmd;
        private lib.BindingDischarger mydischarger;

        public AlgorithmWin()
        {
            InitializeComponent();
            mydischarger = new lib.BindingDischarger(this, new DataGrid[] { this.DataGrid1, this.DataGrid2 });
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue!=null)
            {
                mycmd = e.NewValue as AlgorithmFormulaCommand;
                mycmd.CancelEdit = mydischarger.CancelEdit;
                mycmd.EndEdit = mydischarger.EndEdit;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mydischarger.EndEdit())
            {
                bool isdirty = false;
                foreach (Algorithm item in mycmd.Algorithms) { isdirty = isdirty | item.IsDirty; if (isdirty) break; }
                foreach (AlgorithmFormula item in mycmd.AlgorithmFormulas) { isdirty = isdirty | item.IsDirty; if (isdirty) break; }
                if (isdirty & !mycmd.IsReadOnly)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!mycmd.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
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
                if (!e.Cancel) (App.Current.MainWindow as DataModelClassLibrary.Interfaces.IMainWindow).ListChildWindow.Remove(this);
                if (this.Owner != null) this.Owner.Focus();
            }
        }

        private void CloseButton_Clic(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ColumnsRefresh()
        {
            for (int i = 0; i < mycmd.Algorithms.Count; i++)
            {
                if (i > this.DataGrid1.Columns.Count - 4)
                    this.AddAlgorithm(i);
                this.DataGrid1.Columns[i + 2].Header = mycmd.Algorithms[i].Name;
            }
        }
        private void AddAlgorithm(int index)
        {
            //DataGridTemplateColumn column = new DataGridTemplateColumn();
            //column.CanUserSort = false;
            //column.Width = 100;
            //column.DisplayIndex = index+4;

            //Style style = new Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));
            //style.Setters.Add(new Setter(DataGridColumnHeader.MarginProperty, 10));
            //style.Setters.Add(new Setter(DataGridColumnHeader.BorderBrushProperty, Brushes.Black));
            //style.Setters.Add(new Setter(DataGridColumnHeader.BorderThicknessProperty, 2));
            //Binding bnd = new Binding("Algorithms[" + index.ToString() + "].Name");
            //FrameworkElementFactory box = new FrameworkElementFactory(typeof(TextBox));
            //box.SetValue(TextBox.FontWeightProperty,FontWeights.Bold);
            //box.SetBinding(TextBox.TextProperty, bnd);
            //DataTemplate tmpl = new DataTemplate();
            //tmpl.DataType = typeof(Algorithm);
            //tmpl.VisualTree = box;
            //style.Setters.Add(new Setter(DataGridColumnHeader.ContentTemplateProperty, tmpl));
            //column.HeaderStyle = style;
            //column.Header = mycmd.Algorithms[index];

            //grd = new Grid();
            //grdcoldef = new ColumnDefinition();
            //grdcoldef.Width = GridLength.Auto;
            //grd.ColumnDefinitions.Add(grdcoldef);
            //grdcoldef = new ColumnDefinition();
            //grdcoldef.Width = GridLength.Auto;
            //grd.ColumnDefinitions.Add(grdcoldef);

            //bnd = new Binding("Algorithms[" + index.ToString() + "].Value1");
            //block = new TextBlock();
            //block.TextAlignment = TextAlignment.Left;
            //block.SetBinding(TextBlock.TextProperty, bnd);
            //Grid.SetRow(block, 0);
            //grd.Children.Add(block);

            //bnd = new Binding("Algorithms[" + index.ToString() + "].Value2");
            //block = new TextBlock();
            //block.TextAlignment = TextAlignment.Left;
            //block.SetBinding(TextBlock.TextProperty, bnd);
            //Grid.SetRow(block, 1);
            //grd.Children.Add(block);

            //tmpl = new DataTemplate(grd);
            //column.CellTemplate = tmpl;
            //FrameworkElementFactory grd = new FrameworkElementFactory(typeof(Grid));
            

            //ColumnDefinitionCollection clodef = grd.;
            //ColumnDefinition grdcoldef = new ColumnDefinition();
            //grdcoldef.Width = GridLength.Auto;

            //grd.SetValue(Grid.ColumnDefinitions.Add(grdcoldef);
            //grdcoldef = new ColumnDefinition();
            //grdcoldef.Width = GridLength.Auto;
            //grd.ColumnDefinitions.Add(grdcoldef);

            //bnd = new Binding("Algorithms[" + index.ToString() + "]. Value1");
            //bnd.StringFormat = "N2";
            //box = new TextBox();
            //box.TextAlignment = TextAlignment.Left;
            //box.SetBinding(TextBox.TextProperty, bnd);
            //Grid.SetRow(box, 0);
            //grd.Children.Add(box);

            //bnd = new Binding("Algorithms[" + index.ToString() + "]. Value2");
            //bnd.StringFormat = "N2";
            //box = new TextBox();
            //box.TextAlignment = TextAlignment.Left;
            //box.SetBinding(TextBox.TextProperty, bnd);
            //Grid.SetRow(box, 1);
            //grd.Children.Add(box);

            //tmpl = new DataTemplate();
            //tmpl.DataType = typeof(AlgorithmValues);
            //column.CellEditingTemplate = tmpl;

            //style = new Style(typeof(DataGridCell));
            //if (column.CellStyle != null)
            //    style.BasedOn = column.CellStyle;
            //Binding binding = new Binding("Branches[" + index.ToString() + "].Goods.ColorMark");
            //binding.Mode = BindingMode.OneWay;
            //style.Setters.Add(new Setter(DataGridCell.BackgroundProperty, binding));
            //style.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black)));
            //binding = new Binding("Content.Text");
            //binding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
            //style.Setters.Add(new Setter(DataGridCell.ToolTipProperty, binding));
            //column.CellStyle = style;
            //this.DataGrid1.Columns.Add(column);
        }
        private void DataGrid1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged) //this.ParcelRequestDataGrid.IsLoaded && 
            {
                foreach (DataGridColumn item in this.DataGrid1.Columns)
                    if ((this.DataGrid1.IsLoaded && item.ActualWidth != this.DataGrid2.Columns[item.DisplayIndex].ActualWidth) || item.ActualWidth > this.DataGrid2.Columns[item.DisplayIndex].ActualWidth)
                        this.DataGrid2.Columns[item.DisplayIndex].Width = item.ActualWidth;
            }
        }
        private void DataGrid2_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                for (int i = 0; i < this.DataGrid2.Columns.Count; i++)
                    if ((this.DataGrid2.IsLoaded && this.DataGrid1.Columns[i].ActualWidth != this.DataGrid2.Columns[i].ActualWidth) || this.DataGrid1.Columns[i].ActualWidth < this.DataGrid2.Columns[i].ActualWidth)
                        this.DataGrid1.Columns[i].Width = this.DataGrid2.Columns[i].ActualWidth;
            }
        }

        private void AddAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            this.AddAlgorithm(mycmd.Algorithms.Count);
        }

        private void AlgorithmHeader_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(!mycmd.AlgorithmIsReadOnly & e.ClickCount>1)
            {
                AlgorithmHeaderChanging(e.OriginalSource);
            }
        }
        private void AlgorithmHeaderChanging(object source)
        {
            if(source is TextBlock)
            {
                (source as TextBlock).Visibility = Visibility.Collapsed;
                (((source as TextBlock).Parent as StackPanel).Children[1] as TextBox).Visibility = Visibility.Visible;
            }
            else if(source is Border)
            {
                (((source as Border).Child as Grid).Children[0] as TextBlock).Visibility = Visibility.Collapsed;
                (((source as Border).Child as Grid).Children[1] as TextBox).Visibility = Visibility.Visible;
            }
        }
        private void AlgorithmHeader_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox cntr = e.Source as TextBox;
            ((cntr.Parent as StackPanel).Children[0] as TextBlock).Visibility = Visibility.Visible;
            cntr.Visibility = Visibility.Collapsed;
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (e.Source as DataGrid).SelectedItems.Count>0;
        }
        private void Delete_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            mycmd.Delete.Execute((e.Source as DataGrid).SelectedItem);
        }
        private void DeleteAlgorithm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (e.Source as DataGrid).CurrentColumn!=null;
        }
        private void DeleteAlgorithm_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            int n = (e.Source as DataGrid).CurrentColumn.DisplayIndex - 4;
            if(n>=0) mycmd.DeleteAlgorithm(n);
        }

		private void MoveDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
            e.CanExecute = mycmd.MoveDown==null ? false : mycmd.MoveDown.CanExecute((e.Source as DataGrid).SelectedItem);
		}
		private void MoveDown_Execute(object sender, ExecutedRoutedEventArgs e)
		{
            DataGrid dg = e.Source as DataGrid;
            dg.CommitEdit();
            mycmd.MoveDown.Execute(dg.SelectedItem);
            dg.Focus();
		}
		private void MoveUp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
            e.CanExecute = mycmd.MoveUp==null ? false : mycmd.MoveUp.CanExecute((e.Source as DataGrid).SelectedItem);
		}
		private void MoveUp_Execute(object sender, ExecutedRoutedEventArgs e)
		{
            DataGrid dg = e.Source as DataGrid;
            dg.CommitEdit();
            mycmd.MoveUp.Execute(dg.SelectedItem);
            dg.Focus();
		}
	}
}
