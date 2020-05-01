using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    internal class ConcurrencyManager
    {
        private Dictionary<string, PropertyConcurrencyManager> mycheckproperties;
        internal Dictionary<string, PropertyConcurrencyManager> CheckProperties { get { return mycheckproperties; } }

        internal bool isConflict
        {
            get
            {
                bool isconflict = false;
                foreach (PropertyConcurrencyManager item in mycheckproperties.Values) isconflict = isconflict | item.isConflict;
                return isconflict;
            }
        }
        internal Dictionary<string, PropertyConcurrencyManager> ConflictProperties
        {
            get
            {
                Dictionary<string, PropertyConcurrencyManager> conflicts = new Dictionary<string, PropertyConcurrencyManager>();
                foreach (PropertyConcurrencyManager item in mycheckproperties.Values)
                    if (item.isConflict)
                        conflicts.Add(item.PropertyName,item);
                return conflicts;
            }
        }
        internal bool isNeedUpdate
        {
            get
            {
                bool isneedupdate = false;
                foreach (PropertyConcurrencyManager item in mycheckproperties.Values) isneedupdate = isneedupdate | item.isNeedUpdate;
                return isneedupdate;
            }
        }
        internal Dictionary<string, PropertyConcurrencyManager> NeedUpdateProperties
        {
            get
            {
                Dictionary<string, PropertyConcurrencyManager> conflicts = new Dictionary<string, PropertyConcurrencyManager>();
                foreach (PropertyConcurrencyManager item in mycheckproperties.Values)
                    if (item.isNeedUpdate)
                        conflicts.Add(item.PropertyName, item);
                return conflicts;
            }
        }

        internal ConcurrencyManager()
        {
            mycheckproperties = new Dictionary<string, PropertyConcurrencyManager>();
        }

        internal void AddCheckProperty(PropertyConcurrencyManager property)
        {
            mycheckproperties.Add(property.PropertyName, property);
        }
        internal void AddCheckPropertyArray(PropertyConcurrencyManager[] properties)
        {
            foreach (PropertyConcurrencyManager property in properties) this.AddCheckProperty(property);
        }
        internal void Clear()
        {
            mycheckproperties.Clear();
        }

        internal void UserConflictResolution()
        {
            if (isConflict)
            {
                UserConflictResolutionWin reswin = new UserConflictResolutionWin();

                reswin.ShowInTaskbar = false;
                reswin.ShowDialog();
            }
        }
    }

    internal class PropertyConcurrencyManager<T> : PropertyConcurrencyManager
    {
        private T myserver;
        internal T Server { get { return myserver; } }
        private T myunchanged;
        internal T Unchanged { get { return myunchanged; } }
        private T mycurrent;
        internal T Current { get { return mycurrent; } }

        override internal object ServerBinding { get { return myserver; } }
        override internal object CurrentBinding { get { return mycurrent; } set { mycurrent = (T)value; myisneedupdate = true; } }

        private Func<T, T, bool> mycheck;
        internal Func<T, T, bool> GetCheck { set { mycheck = value; } }

        private PropertyConcurrencyManager() : base() { }
        internal PropertyConcurrencyManager(string propertyName, string title, T server, T unchanged, T current, Func<T, T, bool> servercurrentcheck, string valuepath, string displaypath, object list)
            : this()
        {
            myname = propertyName;
            myserver = server;
            myunchanged = unchanged;
            mycurrent = current;

            myisconflict = false;
            myisneedupdate = false;
            mycheck = servercurrentcheck;
            Check();

            myvaluepath = valuepath;
            mydisplaypath = displaypath;
            mylist = list;
        }
        internal PropertyConcurrencyManager(string propertyName, string title, T server, T unchanged, T current, Func<T, T, bool> servercurrentcheck)
            : this(propertyName, title, server, unchanged, current, servercurrentcheck, null, null, null) {}
        internal PropertyConcurrencyManager(string propertyName, string title, T server, T unchanged, T current)
            : this(propertyName, title, server, unchanged, current, CheckEquals) { }

        internal static bool CheckEquals(T t1, T t2)
        { return t1 != null ? t1.Equals(t2) : t2 == null; }

        internal void Check()
        {
            if (!mycheck(myserver, mycurrent))
            {
                if (CheckEquals(myunchanged, mycurrent))
                {
                    mycurrent = myserver;
                    myisneedupdate = true;
                }
                else if (!CheckEquals(myunchanged, myserver)) myisconflict = true;
            }
        }

    }

    internal abstract class PropertyConcurrencyManager
    {
        protected string mytitle;
        internal string Title { set { mytitle = value; } get { return mytitle; } }
        protected string myname;
        internal string PropertyName { set { myname = value; } get { return myname; } }
        protected bool myisconflict;
        internal bool isConflict { get { return myisconflict; } }
        protected bool myisneedupdate;
        internal bool isNeedUpdate { get { return myisneedupdate; } }

        abstract internal object ServerBinding { get; }
        abstract internal object CurrentBinding { get; set; }

        protected string myvaluepath;
        internal string ValuePath { set { myvaluepath = value; } get { return myvaluepath; } }
        protected string mydisplaypath;
        internal string DisplayPath { set { mydisplaypath = value; } get { return mydisplaypath; } }
        protected object mylist;
        internal object ItemSource { set { mylist = value; } get { return mylist; } }
    }
}
