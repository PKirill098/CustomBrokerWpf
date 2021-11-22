namespace KirillPolyanskiy.CustomBrokerWpf
{


    public partial class ReferenceDS
    {
        internal void AccountCurrencyRefresh()
        {
            ReferenceDSTableAdapters.AccountCurrencyAdapter adapter = new ReferenceDSTableAdapters.AccountCurrencyAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableAccountCurrency);
            this.tableAccountCurrency.DefaultView.Sort = "currency";
        }
        //internal void AccountTransactionTypeRefresh()
        //{
        //	ReferenceDSTableAdapters.AccountTransactionTypeAdapter adapter = new ReferenceDSTableAdapters.AccountTransactionTypeAdapter();
        //	adapter.ClearBeforeFill = false;
        //	adapter.Fill(this.tableAccountTransactionType);
        //	this.tableAccountTransactionType.DefaultView.Sort = "typedescr";
        //}
        internal void AddressTypeRefresh()
        {
            ReferenceDSTableAdapters.AddressTypeAdapter adapter = new ReferenceDSTableAdapters.AddressTypeAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableAddressType);
            this.tableAddressType.DefaultView.Sort = "addresstypeName";
        }
        internal void CustomerNameRefresh()
        {
            ReferenceDSTableAdapters.CustomerNameAdapter adapter = new ReferenceDSTableAdapters.CustomerNameAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableCustomerName);
            this.tableCustomerName.DefaultView.Sort = "customerName";
        }
        //internal void ExpenditureItemRefresh()
        //{
        //	AccountTransactionTypeRefresh();
        //	ReferenceDSTableAdapters.ExpenditureItemAdapter adapter = new ReferenceDSTableAdapters.ExpenditureItemAdapter();
        //	adapter.ClearBeforeFill = false;
        //	adapter.Fill(this.tableExpenditureItem);
        //	this.tableExpenditureItem.DefaultView.Sort = "nameEI";
        //}
        internal void ExpenditureTypeRefresh()
        {
            ReferenceDSTableAdapters.ExpenditureTypeAdapter adapter = new ReferenceDSTableAdapters.ExpenditureTypeAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableExpenditureType);
            this.tableExpenditureType.DefaultView.Sort = "NameET";
        }
        internal void FullNumberRefresh()
        {
            ReferenceDSTableAdapters.FullNumberAdapter adapter = new ReferenceDSTableAdapters.FullNumberAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableFullNumber);
            this.tableFullNumber.DefaultView.Sort = "sort DESC";
        }
        internal void LegalEntityRefresh()
        {
            ReferenceDSTableAdapters.LegalEntityAdapter adapter = new ReferenceDSTableAdapters.LegalEntityAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableLegalEntity);
            if (!this.tableLegalEntity.Rows.Contains(0)) this.tableLegalEntity.AddtableLegalEntityRow(string.Empty, 0, true, false).AcceptChanges();
            this.tableLegalEntity.DefaultView.Sort = "namelegal";
        }
        internal void ManagerGroupRefresh()
        {
            ReferenceDSTableAdapters.ManagerGroupAdapter adapter = new ReferenceDSTableAdapters.ManagerGroupAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableManagerGroup);
            this.tableManagerGroup.DefaultView.Sort = "managergroupName";
        }
        internal void RequestStatusRefresh()
        {
            ReferenceDSTableAdapters.RequestStatusAdapter adapter = new ReferenceDSTableAdapters.RequestStatusAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableRequestStatus);
            this.tableRequestStatus.DefaultView.Sort = "rowId";
        }
        internal void GoodsTypeRefresh()
        {
            ReferenceDSTableAdapters.GoodsTypeAdapter adapter = new ReferenceDSTableAdapters.GoodsTypeAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableGoodsType);
            this.tableGoodsType.DefaultView.Sort = "Nameitem";
        }
        internal void ParcelTypeRefresh()
        {
            ReferenceDSTableAdapters.ParcelTypeAdapter adapter = new ReferenceDSTableAdapters.ParcelTypeAdapter();
            adapter.ClearBeforeFill = false;
            adapter.Fill(this.tableParcelType);
        }
        //internal void StoreRefresh()
        //{
        //	ReferenceDSTableAdapters.StoreAdapter adapter = new ReferenceDSTableAdapters.StoreAdapter();
        //	adapter.ClearBeforeFill = false;
        //	adapter.Fill(this.tableStore);
        //	this.tableStore.DefaultView.Sort = "storeName";
        //}
    }
}
