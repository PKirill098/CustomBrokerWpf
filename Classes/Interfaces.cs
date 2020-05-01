namespace KirillPolyanskiy.CustomBrokerWpf
{
    internal interface IFiltredWindow
    {
        bool IsShowFilter { set; get; }
        ItemFilter[] Filter { set; get; }
    }
    internal interface IViewModelWindous
    {
        bool vmEndEdit();
        void vmCancelEdit();
    }
    public class ItemFilter
    {
        private string filterproperty;
        private string filteroperation;
        private string filtervalue;
        internal ItemFilter(string PropertyName, string Operation, string Value)
        {
            filterproperty = PropertyName;
            filteroperation = Operation;
            filtervalue = Value;
        }
        internal ItemFilter() : this(string.Empty, string.Empty, string.Empty) { }
        internal string PropertyName
        {
            get { return filterproperty; }
            set { filterproperty = value; }
        }
        internal string Operation
        {
            get { return filteroperation; }
            set { filteroperation = value; }
        }
        internal string Value
        {
            get { return filtervalue; }
            set { filtervalue = value; }
        }
    }
    internal interface IReference<T>
    {
        void Refresh();
        T FindFirstItem(string propertyName, object value);
    }
}
