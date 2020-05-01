namespace KirillPolyanskiy.CustomBrokerWpf
{


    public partial class RecipientDS
    {
        partial class tableContactPointDataTable
        {
            private bool isUpdatedPointName = false;
            public override void EndInit()
            {
                base.EndInit();
                this.ColumnChanging += new System.Data.DataColumnChangeEventHandler(PointColumnChangeEvent);

            }
            private void PointColumnChangeEvent(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                if (e.Column.ColumnName == "PointName" | e.Column.ColumnName == "PointValue")
                {
                    tableContactPointRow row = e.Row as tableContactPointRow;
                    if (e.Column.ColumnName == "PointName" & row.PointValue.Length > 0)
                    {
                        isUpdatedPointName = true;
                        row.PointValue = ConvertPointValue(e.ProposedValue.ToString(), row.PointValue);
                        isUpdatedPointName = false;
                    }
                    else if (!isUpdatedPointName & e.Column.ColumnName == "PointValue" & !row.IsPointNameNull()) e.ProposedValue = ConvertPointValue(row.PointName, e.ProposedValue.ToString());
                }
            }
            private string ConvertPointValue(string pointName, string pointValue)
            {
                if (pointValue.Length > 0)
                {
                    try
                    {
                        string pointtemp = string.Empty;
                        ReferenceDS ds = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
                        ReferenceDS.ContactPointTypeTbDataTable pointtype = ds.ContactPointTypeTb;
                        ReferenceDS.ContactPointTypeTbRow typerow = pointtype.FindBypointName(pointName);
                        if (typerow != null) pointtemp = typerow.pointtemplate;
                        else pointtemp = string.Empty;

                        if (pointtemp == "telnumber")
                        {
                            char s;
                            byte p = 0;
                            bool isClose = false;
                            bool isOpen = false;
                            System.Text.StringBuilder ss = new System.Text.StringBuilder();
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
                    catch { }
                }
                return pointValue;
            }
            private char[] charReverse(char[] chars)
            {
                int l = chars.Length;
                char[] rchar = new char[l];
                for (int i = 0; i < l; i++) rchar[i] = chars[l - i - 1];
                return rchar;
            }
        }
    }
}
