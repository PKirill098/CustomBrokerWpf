using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public class DinamicPropertyItem
    {
        internal DinamicPropertyItem() { }
        internal DinamicPropertyItem(Type propertytype,object value):this()
        { this.PropertyType = propertytype; this.Value = value; }

        public Type PropertyType { set; get; }
        public object Value { set; get; }
    }

    public class DinamicPropertyConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string param=parameter as string;
            Dictionary<string, DinamicPropertyItem> dictionary=value as Dictionary<string, DinamicPropertyItem>;
            if (dictionary.ContainsKey(param))
            {
                DinamicPropertyItem property = dictionary[parameter as string];
                return property.Value;
            }
            else
                return Binding.DoNothing;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class DinamicPropertySort : System.Collections.IComparer
    {
        List<SortDescriptor> property_;
        public List<SortDescriptor> Propertys { get { return property_; } }
        public DinamicPropertySort():base()
        { property_ = new List<SortDescriptor>(); }
        public int Compare(object x, object y)
        {
            int result=0;
            foreach (SortDescriptor property in Propertys)
            {
                if (!(x as Dictionary<string, DinamicPropertyItem>).ContainsKey(property.Property)) continue;
                DinamicPropertyItem dx = (x as Dictionary<string, DinamicPropertyItem>)[property.Property];
                DinamicPropertyItem dy = (y as Dictionary<string, DinamicPropertyItem>)[property.Property];
                if (dx.Value == null | dy.Value == null)
                {
                    if (dx.Value == null & dy.Value == null) result = 0;
                    else if (dx.Value == null) result = -1;
                    else result = 1;
                    if (property.Direction != System.ComponentModel.ListSortDirection.Ascending) result = -result;
                }
                else if (dx.Value == System.DBNull.Value | dy.Value == System.DBNull.Value)
                {
                    if (dx.Value == System.DBNull.Value & dy.Value == System.DBNull.Value) result = 0;
                    else if (dx.Value == System.DBNull.Value) result = -1;
                    else result= 1;
                    if (property.Direction != System.ComponentModel.ListSortDirection.Ascending) result = -result;
                }
                else if (property.Direction == System.ComponentModel.ListSortDirection.Ascending)
                    result = ((IComparable)dx.Value).CompareTo(dy.Value);
                else
                    result = ((IComparable)dy.Value).CompareTo(dx.Value);
                
                if (result != 0) break;
            }
            return result;
        }
    }
    
    public class SortDescriptor
    {
        public string Property { set; get; }
        public System.ComponentModel.ListSortDirection Direction { set; get; }
    }
    public class DinamicPropertySource
    {
        public DinamicPropertySource(System.Collections.Generic.Dictionary<string, DinamicPropertyItem> PropertyCollection)
        { this.DinamicProperties = PropertyCollection; }
        public System.Collections.Generic.Dictionary<string, DinamicPropertyItem> DinamicProperties { set; get; }
    }
}
