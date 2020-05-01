using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public static class TelephoneNumberFormat
    {
        static TelephoneNumberFormat() { }
        public static string ConvertValue(string pointValue)
        {
            if (pointValue.Length > 0)
            {
                try
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
                catch { }
            }
            return pointValue;
        }
        private static char[] charReverse(char[] chars)
        {
            int l = chars.Length;
            char[] rchar = new char[l];
            for (int i = 0; i < l; i++) rchar[i] = chars[l - i - 1];
            return rchar;
        }
    }
}
