using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class MailTemplate : lib.DomainBaseStamp
    {
        public MailTemplate(int id,long stamp,lib.DomainObjectState mstate
            ,string name, int state,string subject,string body,int? delay
            ) :base(id,stamp,null,null, mstate)
        {
            myname = name;
            mystate = state;
            mysubject = subject;
            mybody = body;
            mydelay = delay;
        }
        public MailTemplate():this(lib.NewObjectId.NewId,0,lib.DomainObjectState.Added
            ,null,23, null, null,null){ }

        private string myname;
        public string Name
        {
            set { SetProperty<string>(ref myname, value); }
            get { return myname; }
        }
        private int mystate;
        public int State
        {
            set { SetProperty<int>(ref mystate, value); }
            get { return mystate; }
        }
        private string mysubject;
        public string Subject
        {
            set { SetProperty<string>(ref mysubject, value); }
            get { return mysubject; }
        }
        private string mybody;
        public string Body
        {
            set { SetProperty<string>(ref mybody, value); }
            get { return mybody; }
        }
        private int? mydelay;
        public int? Delay
        {
            set { SetProperty<int?>(ref mydelay, value); }
            get { return mydelay; }
        }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            MailTemplate newitem = (MailTemplate)sample;
            this.Name = newitem.Name;
            this.State = newitem.State;
            this.Subject = newitem.Subject;
            this.Body = newitem.Body;
            this.Delay = newitem.Delay;
        }
    }

    internal class MailTemplateStore : lib.DomainStorageLoad<MailTemplate, MailTemplateDBM>
    {
        public MailTemplateStore(MailTemplateDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(MailTemplate olditem, MailTemplate newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class MailTemplateDBM : lib.DBManagerStamp<MailTemplate>
    {
        public MailTemplateDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.MailTemplate_sp";
            InsertCommandText = "dbo.MailTemplateAdd_sp";
            UpdateCommandText = "dbo.MailTemplateUpd_sp";
            DeleteCommandText = "dbo.MailTemplateDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@param1", System.Data.SqlDbType.Int), new SqlParameter("@param2", System.Data.SqlDbType.Int)};
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@nametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@statetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@subjecttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@bodytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@parameter1true", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0]
                ,new SqlParameter("@name", System.Data.SqlDbType.NVarChar,50)
                ,new SqlParameter("@state", System.Data.SqlDbType.Int)
                ,new SqlParameter("@subject", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@body", System.Data.SqlDbType.NVarChar,1000)
                ,new SqlParameter("@parameter1", System.Data.SqlDbType.Int)
            };
        }

        internal int? Id
        { set { SelectParams[0].Value = value; } }
        internal int? State
        {
            set { SelectParams[1].Value = value; SelectParams[0].Value = null; }
        }

        protected override MailTemplate CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new MailTemplate(reader.GetInt32(0),reader.GetInt64(1),lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"))
                , reader.IsDBNull(reader.GetOrdinal("state")) ? 0 : reader.GetInt32(reader.GetOrdinal("state"))
                ,reader.IsDBNull(reader.GetOrdinal("subject"))?null: reader.GetString(reader.GetOrdinal("subject"))
                , reader.IsDBNull(reader.GetOrdinal("body")) ? null : reader.GetString(reader.GetOrdinal("body"))
                , reader.IsDBNull(reader.GetOrdinal("parameter1")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("parameter1")));
        }
        protected override void GetOutputSpecificParametersValue(MailTemplate item)
        {
        }
        protected override bool SaveChildObjects(MailTemplate item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(MailTemplate item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override bool SetSpecificParametersValue(MailTemplate item)
        {
            myupdateparams[1].Value = item.HasPropertyOutdatedValue("Name");
            myupdateparams[2].Value = item.HasPropertyOutdatedValue("State");
            myupdateparams[3].Value = item.HasPropertyOutdatedValue("Subject");
            myupdateparams[4].Value = item.HasPropertyOutdatedValue("Body");
            myupdateparams[5].Value = item.HasPropertyOutdatedValue("Delay");
            myinsertupdateparams[1].Value = item.Name;
            myinsertupdateparams[2].Value = item.State;
            myinsertupdateparams[3].Value = item.Subject;
            myinsertupdateparams[4].Value = item.Body;
            myinsertupdateparams[5].Value = item.Delay;
            return true;
        }
        protected override void CancelLoad()
        { }
    }

    public class MailTemplateVM : lib.ViewModelErrorNotifyItem<MailTemplate>
    {
        public MailTemplateVM(MailTemplate model):base(model)
        {
            //ValidetingProperties.AddRange(new string[] { "Subject" });
            DeleteRefreshProperties.AddRange(new string[] { "Subject", "Body" });
            InitProperties();
        }
        public MailTemplateVM():this(new MailTemplate()) { }

        public int? State
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.State, value.Value)))
                {
                    string name = "State";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.State);
                    ChangingDomainProperty = name; this.DomainObject.State = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.State : (int?)null; }
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
        public string Body
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Body, value)))
                {
                    string name = "Body";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Body);
                    ChangingDomainProperty = name; this.DomainObject.Body = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Body : null; }
        }
        public string Subject
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Subject, value)))
                {
                    string name = "Subject";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Subject);
                    ChangingDomainProperty = name; this.DomainObject.Subject = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Subject : null; }
        }
        public int? Delay
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Delay.HasValue!=value.HasValue || (value.HasValue && this.DomainObject.Delay.Value != value.Value)))
                {
                    string name = "Delay";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Delay);
                    ChangingDomainProperty = name; this.DomainObject.Delay = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Delay : null; }
        }
        private FlowDocument mydocument;
        public FlowDocument Document
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(mydocument, value)))
                {
                    string name = "Document";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Body);
                    mydocument = value;
                    DocumentUpdateModel();
                }
            }
            get { return this.IsEnabled ? mydocument : null; }
        }
        internal void DocumentUpdateModel()
        {
            ChangingDomainProperty = "Body";
            this.DomainObject.Body = XamlWriter.Save(mydocument);
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            //if(property== "Body" & ChangingDomainProperty!= "Body")
            //{
            //    if (!string.IsNullOrEmpty(this.DomainObject.Body))
            //        mydocument = (FlowDocument)XamlReader.Parse(this.DomainObject.Body);
            //    else
            //        mydocument = new FlowDocument();
            //}
        }
        protected override void InitProperties()
        {
            //if (!string.IsNullOrEmpty(this.DomainObject.Body))
            //    mydocument = (FlowDocument)XamlReader.Parse(this.DomainObject.Body);
            //else
            //    mydocument = new FlowDocument();
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Body":
                    this.DomainObject.Body = (string)value;
                    break;
                case "Name":
                    this.DomainObject.Name = (string)value;
                    break;
                case "State":
                    this.DomainObject.State = (int)value;
                    break;
                case "Subject":
                    this.DomainObject.Subject = (string)value;
                    break;
                case "Delay":
                    this.DomainObject.Delay = (int?)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            //string errmsg = null;
            //switch (propertyname)
            //{
            //    //case "Body":
            //    //    if (string.IsNullOrEmpty(this.Body))
            //    //    {
            //    //        errmsg = "Не указан техт письма!";
            //    //        isvalid = false;
            //    //    }
            //    //    break;
            //    case "Subject":
            //        if (string.IsNullOrEmpty(this.Subject))
            //        {
            //            errmsg = "Не указана тема письма!";
            //            isvalid = false;
            //        }
            //        break;
            //}
            //if (inform)
            //{
            //    if (isvalid)
            //        ClearErrorMessageForProperty(propertyname);
            //    else
            //        AddErrorMessageForProperty(propertyname, errmsg);
            //}
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return false;
        }
    }

    internal class MailTemplateSynchronizer : lib.ModelViewCollectionsSynchronizer<MailTemplate, MailTemplateVM>
    {
        protected override MailTemplate UnWrap(MailTemplateVM wrap)
        {
            return wrap.DomainObject as MailTemplate;
        }
        protected override MailTemplateVM Wrap(MailTemplate fill)
        {
            return new MailTemplateVM(fill);
        }
    }

    public class MailTemplateCurrentCMD : lib.ViewModelCurrentItemCommand<MailTemplateVM>
    {
        internal MailTemplateCurrentCMD()
        {
            mydbm = new MailTemplateDBM();
            //mysync = new MailTemplateSynchronizer();
            mydbm.Fill();
            if (mydbm.Errors.Count > 0)
                this.OpenPopup("Загрузка данных\n" + mydbm.ErrorMessage, true);
            else
            {
                //mysync.DomainCollection = (mydbm as MailTemplateDBM).Collection;
                //base.Collection = mysync.ViewModelCollection;
                base.Collection = (mydbm as MailTemplateDBM).Collection;
            }
            base.DeleteQuestionHeader = "Удалить шаблон?";
            
            mymail = new Mail();
            mysendmail = new lib.RelayCommand(SendMailExecute, SendMailCanExecute);
        }

        private Mail mymail;
        private string myaddress;
        public string Address
        { set { myaddress = value; } get { return myaddress; } }
        private Request myrequest;
        private string myrequestid;
        public string RequestId
        { set { myrequestid = value; } get { return myrequestid; } }

        private lib.RelayCommand mysendmail;
        public lib.RelayCommand SendMail
        { set { mysendmail = value; } get { return mysendmail; } }
        private void SendMailExecute(object parametr)
        {
            try {
                string body=null;
                if(string.IsNullOrEmpty(myrequestid))
                    body = this.CurrentItem.Body;
                else 
                {
                    int requestid;
                    List<lib.DBMError> err = new List<lib.DBMError>();
                    int.TryParse(myrequestid, out requestid);
                    if (myrequest?.Id != requestid)
                        myrequest = CustomBrokerWpf.References.RequestStore.GetItemLoad(requestid, out err);
                    if(myrequest != null)
                    {
                        RequestMailState bodycreator = new RequestMailState(myrequest, this.CurrentItem.State.Value);
                        body = bodycreator.CreateBody(this.CurrentItem.DomainObject, new MailStateCustomer(0,0,lib.DomainObjectState.Sealed,0, myrequest.CustomerLegals.FirstOrDefault((RequestCustomerLegal legal)=> { return legal.Selected; })?.CustomerLegal.Id??0,0,null));
                    }
                    else
                        this.OpenPopup(err.Count>0 ? err[0].Message : "Не найдена заявка № " + myrequestid??string.Empty, true);
                }
                if(body!=null) mymail.Send(string.Empty, myaddress, this.CurrentItem.Subject, body, BodySubtype.html);
            }
            catch(Exception ex) { this.OpenPopup(ex.Message, true); }
        }
        private bool SendMailCanExecute(object parametr)
        {
            return !(string.IsNullOrEmpty(myaddress) || myaddress.IndexOf('@') < 1);
        }

        protected override bool CanDeleteData(object parametr)
        {
            return this.CurrentItem != null;
        }
        protected override bool CanRejectChanges()
        {
            return this.CurrentItem != null;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override MailTemplateVM CreateCurrentViewItem(lib.DomainBaseNotifyChanged domainobject)
        {
            //this.CurrentItem?.DocumentUpdateModel();
            return new MailTemplateVM(domainobject as MailTemplate);
        }
        protected override void OnCurrentItemChanged()
        {
            
        }
        protected override void OtherViewRefresh()
        {
        }
        protected override void RefreshData(object parametr)
        {
            //lib.ReferenceSimpleItem current = this.CurrentItem?.State;
            MailTemplate current = this.CurrentItem?.DomainObject;
            (mydbm as MailTemplateDBM).Fill();
            if (mydbm.Errors.Count > 0)
                this.OpenPopup("Обновление данных\n" + mydbm.ErrorMessage, true);
            this.Items.MoveCurrentTo(current);
            //if (current != null)
            //{
            //    this.CurrentState = current;
            //}
        }
        protected override void RejectChanges(object parametr)
        {
           this.CurrentItem.Reject.Execute(null);
        }
        public override bool SaveDataChanges()
        {
            //this.CurrentItem.DocumentUpdateModel();
            return base.SaveDataChanges();
        }
        protected override void SettingView()
        {
            base.SettingView();
            myview.SortDescriptions.Add(new SortDescription("State", ListSortDirection.Ascending));
        }
    }
}
