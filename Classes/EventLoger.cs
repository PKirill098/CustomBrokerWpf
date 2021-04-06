using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    public class EventLoger:lib.Common.EventLogDBM
    {
        internal EventLoger():base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
        }
    }
}
