
namespace KirillPolyanskiy.CustomBrokerWpf
{
    static internal class References
    {
        static private Classes.EventLogTypeList myeventlogtype;
        static internal Classes.EventLogTypeList EventLogTypes
        {
            get
            {
                if (myeventlogtype == null) myeventlogtype = new Classes.EventLogTypeList();
                return myeventlogtype;
            }
        }

        static private WithdrawalRecipientList mywithdrawalRecipient;
        static internal WithdrawalRecipientList WithdrawalRecipient
        {
            get
            {
                if (mywithdrawalRecipient == null) mywithdrawalRecipient = new WithdrawalRecipientList();
                return mywithdrawalRecipient;
            }
        }

        static private Classes.PrincipalList myusers;
        static internal Classes.PrincipalList Users
        {
            get
            {
                if (myusers == null) myusers = new Classes.PrincipalList(false);
                return myusers;
            }
        }
        static private Classes.PrincipalList myroles;
        static internal Classes.PrincipalList Roles
        {
            get
            {
                if (myroles == null) myroles = new Classes.PrincipalList(true);
                return myroles;
            }
        }
    }
}
