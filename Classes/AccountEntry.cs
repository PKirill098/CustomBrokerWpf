using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    internal class AccountEntry
    {
        internal int ID { set; get; }
        internal int DebitAccountID { set; get; }
        internal int CreditAccountID { set; get; }
        internal decimal DebitAmount { set; get; }
        internal decimal CreditAmount { set; get; }
        internal string Type { set; get; }
        internal string Description { set; get; }
        internal bool isFix { set; get; }
        internal string UpdateWho { set; get; }
        internal DateTime UpdateWhen { set; get; }
    }
}
