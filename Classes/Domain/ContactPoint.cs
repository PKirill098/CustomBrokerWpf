using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;


namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class ContactPoint : lib.DomainBaseReject
    {
        public ContactPoint(int id, lib.DomainObjectState state
            ,string name,string value
            ) : base(id, state)
        {
            myname = name;
            myvalue = value;
        }
        public ContactPoint() : this(lib.NewObjectId.NewId,lib.DomainObjectState.Added,null,null) { }

        private string myname;
        public string Name
        {
            set
            {
                SetProperty<string>(ref myname, value,()=> { myvalue = ConvertPointValue(myname, myvalue); PropertyChangedNotification("Value"); });

            }
            get { return myname; }
        }
        private string myvalue;
        public string Value
        {
            set
            {
                SetProperty<string>(ref myvalue, value, () => { myvalue=ConvertPointValue(myname, myvalue); });
            }
            get { return myvalue; }
        }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }

        private string ConvertPointValue(string pointName, string pointValue)
        {
            if (pointName?.Length > 0 & pointValue?.Length > 0 )
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
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            throw new NotImplementedException();
        }
    }

    public class ContactPointDBM : lib.DBManagerId<ContactPoint>
    {
        public ContactPointDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.ContactPoints_sp";
            InsertCommandText = "dbo.ContactPointAdd_sp";
            UpdateCommandText = "dbo.ContactPointUpd_sp";
            DeleteCommandText = "dbo.ContactPointDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@contactid", System.Data.SqlDbType.Int),
            };
            SqlParameter paridout = new SqlParameter("@ContactPointId", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@ContactPointId", System.Data.SqlDbType.Int);
            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@ContactId", System.Data.SqlDbType.Int) };
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {new SqlParameter("@PointName", System.Data.SqlDbType.NVarChar,100),new SqlParameter("@PointValue", System.Data.SqlDbType.NVarChar,40) };
            mydeleteparams = new SqlParameter[] { parid };
        }

        public override int? ItemId
        {
            set
            {
                SelectParams[0].Value = value;
            }
            get
            {
                return (int?)SelectParams[0].Value;
            }
        }
        protected override ContactPoint CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new ContactPoint(reader.GetInt32(0), lib.DomainObjectState.Unchanged, reader.IsDBNull(1) ? null : reader.GetString(1), reader.GetString(2));
        }
        protected override void GetOutputParametersValue(ContactPoint item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(ContactPoint item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(ContactPoint item) { return true; }
        protected override bool SaveIncludedObject(ContactPoint item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(ContactPoint item)
        {
            myinsertparams[1].Value = this.ItemId;
            myupdateparams[0].Value = item.Id;
            myinsertupdateparams[0].Value = item.Name;
            myinsertupdateparams[1].Value = item.Value;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override void CancelLoad()
        { }
    }

    public class ContactPointVM : lib.ViewModelErrorNotifyItem<ContactPoint>
    {
        public ContactPointVM(ContactPoint item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { "Value" });
            DeleteRefreshProperties.AddRange(new string[] { "Name","Value" });
            InitProperties();
        }
        public ContactPointVM() : this(new ContactPoint()) { }

        public string Name
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Name, value)))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Name);
                    ChangingDomainProperty = name; this.DomainObject.Name = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Name : null; }
        }
        private string myvalue;
        public string Value
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(myvalue, value)))
                {
                    string name = "Value";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Value);
                    myvalue = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Value = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? myvalue : null; }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Value":
                    myvalue = this.DomainObject.Value;
                    break;
            }
        }
        protected override void InitProperties()
        {
            myvalue = this.DomainObject.Value;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Name":
                    this.DomainObject.Name = (string)value;
                    break;
                case "Value":
                    if (myvalue != this.DomainObject.Value)
                        myvalue = this.DomainObject.Value;
                    else
                        this.Value = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Value":
                    if (string.IsNullOrEmpty(this.Value))
                    {
                        errmsg = "Значение контакта не может быть пустым!";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myvalue!= this.DomainObject.Value;
        }
    }

    internal class ContactPointSynchronizer : lib.ModelViewCollectionsSynchronizer<ContactPoint, ContactPointVM>
    {
        protected override ContactPoint UnWrap(ContactPointVM wrap)
        {
            return wrap.DomainObject as ContactPoint;
        }
        protected override ContactPointVM Wrap(ContactPoint fill)
        {
            return new ContactPointVM(fill);
        }
    }
}
