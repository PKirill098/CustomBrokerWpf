using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ClientFilterWin.xaml
    /// </summary>
    public partial class ClientFilterWin : Window
    {
        public ClientFilterWin()
        {
            InitializeComponent();
        }
        
        private void winClientFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS ds = this.FindResource("keyReferenceDS") as ReferenceDS;
            ListCollectionView managerview = new ListCollectionView(CustomBrokerWpf.References.ManagerGroups);
            managerview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            managerGroupListBox.ItemsSource = managerview;
            System.Data.DataView paytypeview = new System.Data.DataView(ds.tablePaymentType, string.Empty, "[paytypeName]", System.Data.DataViewRowState.CurrentRows);
            this.paytypeListBox.ItemsSource = paytypeview;
            System.Data.DataView deliveryview = new System.Data.DataView(ds.DeliveryType, string.Empty, "[deliverytypeName]", System.Data.DataViewRowState.CurrentRows);
            this.deliveryListBox.ItemsSource = deliveryview;
            System.Data.DataView townview = new System.Data.DataView(ds.tableTown, string.Empty, "[townName]", System.Data.DataViewRowState.CurrentRows);
            this.TownComboBox.ItemsSource = townview;
            string[] ids;
            ItemFilter[] filter = (this.Owner as IFiltredWindow).Filter;
            foreach (ItemFilter filteritem in filter)
            {
                if (!(filteritem is ItemFilter)) continue;
                switch (filteritem.PropertyName)
                {
                    case "AliasCustomer":
                        this.AliasCustomerTextBox.Text = filteritem.Value;
                        break;
                    case "CustomerRecipient":
                        this.recipientNameTextBox.Text = filteritem.Value;
                        break;
                    case "managergroupID":
                        ids = filteritem.Value.Split(',');
                        foreach (string id in ids)
                        {
                            ReferenceSimpleItem item = CustomBrokerWpf.References.ManagerGroups.FindFirstItem("Id",int.Parse(id));
                            if (item != null) this.managerGroupListBox.SelectedItems.Add(item);
                        }
                        break;
                    case "paytypeID":
                        ids = filteritem.Value.Split(',');
                        paytypeview.Sort = "paytypeID";
                        foreach (string id in ids)
                        {
                            System.Data.DataRowView[] rowview = paytypeview.FindRows(id);
                            if (rowview.Length > 0) this.paytypeListBox.SelectedItems.Add(rowview[0]);
                        }
                        paytypeview.Sort = "paytypeName";
                        break;
                    case "deliverytypeID":
                        ids = filteritem.Value.Split(',');
                        deliveryview.Sort = "deliverytypeID";
                        foreach (string id in ids)
                        {
                            System.Data.DataRowView[] rowview = deliveryview.FindRows(id);
                            if (rowview.Length > 0) this.deliveryListBox.SelectedItems.Add(rowview[0]);
                        }
                        deliveryview.Sort = "deliverytypeName";
                        break;
                    case "customerID":
                        this.IdCustomerTextBox.Text = filteritem.Value;
                        break;
                    case "customerDayEntry":
                        if (filteritem.Operation == "Between")
                        {
                            this.startDayEntryPicker.SelectedDate = DateTime.Parse(filteritem.Value.Substring(19, filteritem.Value.IndexOf("' AND") - 19));
                            this.stopDayEntryPicker.SelectedDate = DateTime.Parse(filteritem.Value.Substring(filteritem.Value.IndexOf("<'") + 2, filteritem.Value.Length - filteritem.Value.IndexOf("<'") - 3)).AddDays(-1);
                        }
                        else if (filteritem.Operation == ">")
                            this.startDayEntryPicker.SelectedDate = DateTime.Parse(filteritem.Value.Substring(19, filteritem.Value.Length - 20));
                        else if (filteritem.Operation == "<")
                            this.stopDayEntryPicker.SelectedDate = DateTime.Parse(filteritem.Value.Substring(18, filteritem.Value.Length - 19)).AddDays(-1);
                        break;
                    case "customerRecommend":
                        this.CustomerRecommendTextBox.Text = filteritem.Value;
                        break;
                    case "customerNoteSpecial":
                        this.NoteSpecialTextBox.Text = filteritem.Value;
                        break;
                    case "Town":
                        this.TownComboBox.Text = filteritem.Value;
                        break;
                    case "Locality":
                        this.LocalityTextBox.Text = filteritem.Value;
                        break;
                    case "FIO":
                        this.FIOTextBox.Text = filteritem.Value;
                        break;
                    case "PointValue":
                        this.PointValueTextBox.Text = filteritem.Value;
                        break;
                }
            }
        }

        private void RunFilterButton_Click(object sender, RoutedEventArgs e)
        {
            IInputElement felement = FocusManager.GetFocusedElement(this);
            FocusManager.SetFocusedElement(this, RunFilterButton);
            StringBuilder strbild = new StringBuilder();
            ItemFilter[] newfilter = new ItemFilter[13];
            if (this.AliasCustomerTextBox.Text.Trim().Length > 0)
            {
                newfilter[0] = new ItemFilter("AliasCustomer", "Like", this.AliasCustomerTextBox.Text.Trim());
            }
            if (this.recipientNameTextBox.Text.Trim().Length > 0)
            {
                newfilter[1] = new ItemFilter("CustomerRecipient", "Like", this.recipientNameTextBox.Text.Trim());
            }
            if (managerGroupListBox.SelectedItems.Count > 0)
            {
                strbild.Clear();
                foreach (ReferenceSimpleItem rowview in this.managerGroupListBox.SelectedItems)
                {
                    strbild.Append("," + rowview.Id.ToString());
                }
                strbild.Remove(0, 1);
                newfilter[2] = new ItemFilter("managergroupID", "In", strbild.ToString());
            }
            if (paytypeListBox.SelectedItems.Count > 0)
            {
                strbild.Clear();
                foreach (System.Data.DataRowView rowview in this.paytypeListBox.SelectedItems)
                {
                    strbild.Append("," + (rowview.Row as ReferenceDS.tablePaymentTypeRow).paytypeID.ToString());
                }
                strbild.Remove(0, 1);
                newfilter[3] = new ItemFilter("paytypeID", "In", strbild.ToString());
            }
            if (deliveryListBox.SelectedItems.Count > 0)
            {
                strbild.Clear();
                foreach (System.Data.DataRowView rowview in this.deliveryListBox.SelectedItems)
                {
                    strbild.Append("," + (rowview.Row as ReferenceDS.DeliveryTypeRow).deliverytypeID.ToString());
                }
                strbild.Remove(0, 1);
                newfilter[4] = new ItemFilter("deliverytypeID", "In", strbild.ToString());
            }
            if (this.IdCustomerTextBox.Text.Trim().Length > 0)
            {
                newfilter[5] = new ItemFilter("customerID", "=", this.IdCustomerTextBox.Text.Trim());
            }
            if ((this.startDayEntryPicker.SelectedDate.HasValue) & (this.stopDayEntryPicker.SelectedDate.HasValue))
            {
                newfilter[6] = new ItemFilter("customerDayEntry", "Between", this.startDayEntryPicker.SelectedDate.Value.ToShortDateString() + " " + this.stopDayEntryPicker.SelectedDate.Value.AddDays(1D).ToShortDateString());
            }
            else if (this.startDayEntryPicker.SelectedDate.HasValue)
            {
                newfilter[6] = new ItemFilter("customerDayEntry", ">", this.startDayEntryPicker.SelectedDate.ToString());
            }
            else if (this.stopDayEntryPicker.SelectedDate.HasValue)
            {
                newfilter[6] = new ItemFilter("customerDayEntry", "<", this.stopDayEntryPicker.SelectedDate.Value.AddDays(1D).ToString());
            }
            if (this.CustomerRecommendTextBox.Text.Trim().Length > 0)
            {
                newfilter[7] = new ItemFilter("customerRecommend", "Like", this.CustomerRecommendTextBox.Text.Trim());
            }
            if (this.NoteSpecialTextBox.Text.Trim().Length > 0)
            {
                newfilter[8] = new ItemFilter("customerNoteSpecial", "Like", this.NoteSpecialTextBox.Text.Trim());
            }
            if (this.TownComboBox.Text.Trim().Length > 0)
            {
                newfilter[9] = new ItemFilter("Town", "=", this.TownComboBox.Text.Trim());
            }
            if (this.LocalityTextBox.Text.Trim().Length > 0)
            {
                newfilter[10] = new ItemFilter("Locality", "Like", this.LocalityTextBox.Text.Trim());
            }
            if (this.FIOTextBox.Text.Trim().Trim().Length > 0)
            {
                newfilter[11] = new ItemFilter("FIO", "Like", this.FIOTextBox.Text.Trim());
            }
            if (this.PointValueTextBox.Text.Trim().Length > 0)
            {
                newfilter[12] = new ItemFilter("PointValue", "Like", this.PointValueTextBox.Text.Trim());
            }

            (this.Owner as IFiltredWindow).Filter = newfilter;
            FocusManager.SetFocusedElement(this, felement);
        }

        private void RemoveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            AliasCustomerTextBox.Text = string.Empty;
            recipientNameTextBox.Clear();
            managerGroupListBox.SelectedItems.Clear();
            paytypeListBox.SelectedItems.Clear();
            deliveryListBox.SelectedItems.Clear();
            this.IdCustomerTextBox.Clear();
            startDayEntryPicker.Text = string.Empty;
            this.stopDayEntryPicker.Text = string.Empty;
            this.CustomerRecommendTextBox.Clear();
            this.NoteSpecialTextBox.Clear();
            this.TownComboBox.Text = string.Empty;
            this.LocalityTextBox.Clear();
            this.FIOTextBox.Clear();
            this.PointValueTextBox.Clear();

            (this.Owner as IFiltredWindow).Filter = new ItemFilter[0];
        }

        private void winClientFilter_Closed(object sender, EventArgs e)
        {
            (this.Owner as IFiltredWindow).IsShowFilter = false;
        }

    }
}
