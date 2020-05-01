using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows;
using System.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class ContactPointValueConverter : IMultiValueConverter
    {
        string pointname;
        string pointtemp= string.Empty;
        internal string PointType
        {
            set { pointtemp = value; }
            get { return pointtemp; }
        }
        ReferenceDS ds;
        public ReferenceDS DS
        {
            set { ds = value; }
            get { return ds; }
        }
        
            public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            pointname = values[0].ToString();
            return values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            string pointValue = value.ToString();
            if (pointValue.Length > 0)
            {
                try
                {
                    ReferenceDS.ContactPointTypeTbDataTable pointtype = ds.ContactPointTypeTb;
                    ReferenceDS.ContactPointTypeTbRow typerow = pointtype.FindBypointName(pointname);
                    if (typerow != null) pointtemp = typerow.pointtemplate;
                    else pointtemp = string.Empty;

                    if (pointtemp == "telnumber")
                    {

                        char s;
                        byte p = 0;
                        bool isClose = false;
                        bool isOpen = false;
                        StringBuilder ss = new StringBuilder();
                        char[] charValue = charReverse(pointValue.ToCharArray());
                        for (int i = 0; i < charValue.Length; i++)
                        {
                            s = charValue[i];
                            //if ((s >= '0' & s <= '9')
                            //    | (s == '(' & !isClose) | (s == ')' & !isOpen)
                            //    | (s == '-' & (p == 5 | p == 2) & !isOpen))
                            if (((s != '-') & (s != '(') & (s != ' ')) || (s == '-' & (p == 5 | p == 2) & !isOpen) || ((s == '(') & (p == 15)))
                            {
                                p++;
                                if (s == ')')
                                {
                                    isOpen = true;
                                    ss.Append(' ');
                                    p++;
                                }
                                if (s == '(')
                                {
                                    isClose = true;
                                    ss.Append('(');
                                    s = ' ';
                                    p++;
                                }
                                if ((p == 15) & (s != '(') & !isClose)
                                {
                                    ss.Append("( ");
                                    isClose = true;
                                    p = 17;
                                }
                                if (p == 10 & s != ')' & !isOpen)
                                {
                                    ss.Append(" )");
                                    isOpen = true;
                                    p = 12;
                                }
                                if (p == 6 & s != '-' & !isOpen)
                                {
                                    ss.Append("-");
                                    p = 7;
                                }
                                if (p == 3 & s != '-' & !isOpen)
                                {
                                    ss.Append("-");
                                    p = 4;
                                }
                                ss.Append(s);
                            }
                        }
                        if (ss.Length == 9) ss.Append(" )594( 7+");
                        else if (ss.Length == 14) ss.Append(isClose ? " 7+" : "( 7+");
                        else if (ss.Length == 16) ss.Append("7+");
                        else if ((ss.Length == 17) & ss.ToString().EndsWith("8")) ss.Replace("8", "7+", 16, 1);
                        else if ((ss.Length > 16) & !ss.ToString().EndsWith("+")) ss.Append("+");
                        charValue = new char[ss.Length];
                        ss.CopyTo(0, charValue, 0, ss.Length);
                        pointValue = string.Concat(charReverse(charValue));
                    }
                }
                catch{}
            }
            pointtemp = string.Empty;
            object[] values = { Binding.DoNothing, pointValue };
            return values;
        }
        private char[] charReverse(char[] chars)
        {
            int l = chars.Length;
            char[] rchar = new char[l];
            for (int i = 0; i < l; i++) rchar[i] = chars[l - i - 1];
            return rchar;
        }
    }

    class ContactPointValidationRule : ValidationRule
    {
        ReferenceDS ds;
        public ReferenceDS dsTemplate
        {
            set { ds = value; }
            get { return ds; }
        }

        string poinname=string.Empty; 
        public object PointType
        {
            set
            {
                if (value is DataRowView)
                {
                    poinname = ((value as DataRowView).Row as CustomerDS.tableCustomerContactPointRow).PointName;
                    ReferenceDS.ContactPointTypeTbDataTable pointtype = ds.ContactPointTypeTb;
                    ReferenceDS.ContactPointTypeTbRow typerow = pointtype.FindBypointName(poinname);
                    if (typerow!=null) poinname = pointtype.FindBypointName(poinname).pointtemplate;
                    else poinname = string.Empty;
                }
                else poinname = string.Empty;
            }
            get { return Binding.DoNothing; }
        }
        //static ContactPointValidationRule()
        //{
        //    PointTypeProperty = DependencyProperty.Register("PointType", typeof(string), typeof(ContactPointValidationRule));
        //}

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            DataRowView vw = (value as BindingGroup).Items[0] as DataRowView;
            if (poinname == "telnumber")
            {
                string pointValue = value.ToString();
                if (pointValue.Length > 0)
                {
                    if (pointValue.Length < 7)
                    {
                        return new ValidationResult(false, "Некорректное значение телефонного номера!");
                    }
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
