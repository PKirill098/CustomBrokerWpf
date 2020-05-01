using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    internal class ContactPointConverter
    {
        
        //internal string PointFormat(string pointName, string pointValue)
        //{
        //}

        private char[] charReverse(char[] chars)
        {
            int l = chars.Length;
            char[] rchar = new char[l];
            for (int i = 0; i < l; i++) rchar[i] = chars[l - i - 1];
            return rchar;
        }
    }
}
