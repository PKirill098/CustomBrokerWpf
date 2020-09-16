using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Manager : lib.DomainBaseNotifyChanged
    {
        public Manager(int id, lib.DomainObjectState state
            , lib.ReferenceSimpleItem group,string name, byte unfile
            ) : base(id, state)
        {
            mygroup = group;
            myname = name;
            myunfile = unfile;
            myparticipant = CustomBrokerWpf.References.Participants.FindFirstItem("Id",this.Id);
        }
        public Manager() : this(lib.NewObjectId.NewId,lib.DomainObjectState.Added , null, null, 0) { }

        private lib.ReferenceSimpleItem mygroup;
        public lib.ReferenceSimpleItem Group
        { set { SetProperty<lib.ReferenceSimpleItem>(ref mygroup, value); } get { return mygroup; } }
        private string myname;
        public string Name
        { set { SetProperty<string>(ref myname, value); } get { return myname; } }
        public string NameComb
        { get { return myname ?? myparticipant.Name; } }
        private lib.ReferenceSimpleItem myparticipant;
        public lib.ReferenceSimpleItem Participant
        { get { return myparticipant; } }
        private byte myunfile;
        public byte Unfile
        { private set { myunfile = value; } get { return myunfile; } } 

        internal void UpdateProperties(Manager sample)
        {
            this.Group = sample.Group;
            this.Name = sample.Name;
            this.Unfile = sample.Unfile;
            this.AcceptChanches();
        }
    }

    public class ManagerDBM : lib.DBManager<Manager>
    {
        public ManagerDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.Manager_sp";
            InsertCommandText = "dbo.ManagerAdd_sp";
            UpdateCommandText = "dbo.ManagerUpd_sp";
            DeleteCommandText = "dbo.ManagerDel_sp";

            myupdateparams = new SqlParameter[]
            {
                new SqlParameter("@groupidupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@nameupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@id", System.Data.SqlDbType.Int)
               ,new SqlParameter("@groupid",System.Data.SqlDbType.Int)
               ,new SqlParameter("@name", System.Data.SqlDbType.NVarChar,25)
             };
        }

        protected override Manager CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new Manager(reader.GetInt32(0),lib.DomainObjectState.Unchanged
                ,CustomBrokerWpf.References.ManagerGroups.FindFirstItem("Id", reader.GetInt32(1))
                ,reader.IsDBNull(2)?null:reader.GetString(2)
                , reader.GetByte(3));
        }
        protected override void GetOutputParametersValue(Manager item)
        {
        }
        protected override void ItemAcceptChanches(Manager item)
        {
            item.AcceptChanches();
        }
        protected override void CancelLoad()
        {
        }
        protected override bool SaveChildObjects(Manager item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Manager item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(Manager item)
        {
            myupdateparams[0].Value = true;
            myupdateparams[1].Value = true;
            myinsertupdateparams[0].Value = item.Id;
            myinsertupdateparams[1].Value = item.Group?.Id;
            myinsertupdateparams[2].Value = item.Name;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
    }

    internal class ManagerCollection : lib.ReferenceCollection<Manager>
    {
        public ManagerCollection() : base(new ManagerDBM())
        {
        }

        protected override int Compare(Manager item1, Manager item2)
        {
            return (item1.Name??item1.Participant.Name).CompareTo(item2.Name ?? item2.Participant.Name);
        }
        protected override bool IsFirst(Manager item, string propertyName, object value)
        {
            bool isfirst=false;
            switch (propertyName)
            {
                case "Id":
                    isfirst = item.Id == (int)value;
                    break;
            }
            return isfirst;
        }
        protected override void UpdateItem(Manager olditem, Manager newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class ManagerVM : lib.ViewModelErrorNotifyItem<Manager>
    {
        public ManagerVM(Manager model):base(model)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.Id),nameof(this.Group) });
            InitProperties();
        }
        public ManagerVM():this(new Manager()) { }

        public new int Id
        { set { this.DomainObject.Id = value; } get { return this.DomainObject.Id; } }
        public lib.ReferenceSimpleItem Group
        {
            set
            {
                if (!this.IsReadOnly && !object.Equals(this.DomainObject.Group, value))
                {
                    string name = nameof(this.Group);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Group);
                    ChangingDomainProperty = name; this.DomainObject.Group = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Group : null; }
        }
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

        protected override bool DirtyCheckProperty()
        {
            return this.Group==null;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
        }
        protected override void InitProperties()
        {
        }
        protected override void RejectProperty(string property, object value)
        {
            switch(property)
            {
                case "Group":
                    this.Group=(lib.ReferenceSimpleItem)value;
                    break;
                case "Name":
                    this.Name = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.Id):
                    if(this.Id<0)
                    {
                        isvalid = false;
                        errmsg = "Необходимо указать имя входа!";
                    }
                    break;
                case nameof(this.Group):
                    if (this.Group==null)
                    {
                        isvalid = false;
                        errmsg = "Необходимо указать группу менеджеров!";
                    }
                    break;
            }
            if (isvalid)
                this.ClearErrorMessageForProperty(propertyname);
            else if (inform)
                AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    public class ManagerSynchronizer : lib.ModelViewCollectionsSynchronizer<Manager, ManagerVM>
    {
        protected override Manager UnWrap(ManagerVM wrap)
        {
            return wrap.DomainObject as Manager;
        }
        protected override ManagerVM Wrap(Manager fill)
        {
            return new ManagerVM(fill);
        }
    }

    public class ManagerViewCommand : lib.ViewModelViewCommand
    {
        internal ManagerViewCommand():base()
        {
            mymaindbm = new ManagerDBM();
            mydbm = mymaindbm;
            mymaindbm.Fill();
            mysync = new ManagerSynchronizer();
            mysync.DomainCollection = mymaindbm.Collection;
            base.Collection = mysync.ViewModelCollection;

            myparticipants = new ListCollectionView(CustomBrokerWpf.References.Participants);
            myparticipants.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name",System.ComponentModel.ListSortDirection.Ascending));
            mygroups = new ListCollectionView(CustomBrokerWpf.References.ManagerGroups);

            if (mymaindbm.Errors.Count > 0)
                this.OpenPopup(mymaindbm.ErrorMessage, true);
        }

        private ManagerDBM mymaindbm;
        private ManagerSynchronizer mysync;

        private ListCollectionView myparticipants;
        public ListCollectionView Participants
        { get { return myparticipants; } }
        private ListCollectionView mygroups;
        public ListCollectionView Groups
        { get { return mygroups; } }

        protected override bool CanAddData(object parametr)
        {
            return true;
        }
        protected override bool CanDeleteData(object parametr)
        {
           return true;
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override bool CanRejectChanges()
        {
            return false;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void OtherViewRefresh()
        {
        }
        protected override void RefreshData(object parametr)
        {
            mymaindbm.Fill();
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name",System.ComponentModel.ListSortDirection.Ascending));
        }
    }
}
