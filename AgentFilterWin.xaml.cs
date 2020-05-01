using System;
using System.Text;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для AgentFilterWin.xaml
    /// </summary>
    public partial class AgentFilterWin : Window
    {
        public AgentFilterWin()
        {
            InitializeComponent();
        }

        private void winAgentFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS ds = this.FindResource("keyReferenceDS") as ReferenceDS;
            System.Data.DataView brandview= new System.Data.DataView(ds.tableBrand, string.Empty, "brandName", System.Data.DataViewRowState.CurrentRows);
            this.brandListBox.ItemsSource = brandview;
            ItemFilter[] filter = (this.Owner as IFiltredWindow).Filter;
            foreach (ItemFilter filteritem in filter)
            {
                if (!(filteritem is ItemFilter)) continue;
                switch (filteritem.PropertyName)
                {
                    case "brandID":
                        string[] brandIDs=filteritem.Value.Split(',');
                        brandview.Sort = "brandID";
                        foreach (string brandID in brandIDs)
                        {
                            System.Data.DataRowView[] brand = brandview.FindRows(brandID);
                            if (brand.Length>0) this.brandListBox.SelectedItems.Add(brand[0]);
                        }
                        brandview.Sort = "brandName";
                    break;
                }
            }
        }

        private void RunFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ItemFilter[] newfilter = new ItemFilter[1];
            if (this.brandListBox.SelectedItems.Count > 0)
            {
                ItemFilter brandFilter = new ItemFilter("brandID", "In", "");
                StringBuilder brandsId = new StringBuilder();
                foreach (System.Data.DataRowView rowview in this.brandListBox.SelectedItems)
                {
                    brandsId.Append("," + (rowview.Row as ReferenceDS.tableBrandRow).brandID.ToString());
                }
                brandsId.Remove(0, 1);
               
                brandFilter.Value = brandsId.ToString();
                newfilter[0] = brandFilter;
            }

            (this.Owner as IFiltredWindow).Filter = newfilter;

        }

        private void RemoveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            this.brandListBox.SelectedItems.Clear();
            (this.Owner as IFiltredWindow).Filter=new ItemFilter[0];
        }

        private void winAgentFilter_Closed(object sender, EventArgs e)
        {
            (this.Owner as IFiltredWindow).IsShowFilter = false;
        }

    }
}
