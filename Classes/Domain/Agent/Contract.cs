﻿using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Contract : lib.DomainBaseStamp
    {
        public Contract(int id,long stamp,DateTime? updated,string updater,lib.DomainObjectState state
            ,Agent agent,decimal amount,DateTime? date,bool expired,DateTime expirydate,string number
            ) :base(id, stamp, updated, updater,state)
        {
            myagent = agent;
            myamount = amount;
            mydate = date;
            myexpired = expired;
            myexpirydate = expirydate;
            mynumber = number;
        }
        public Contract():this(lib.NewObjectId.NewId,0,null,null,lib.DomainObjectState.Added
            , null, 0, null, false, DateTime.Now.AddDays(240), null)
        { }

        private Agent myagent;
        public Agent Agent
        {
            set { SetProperty<Agent>(ref myagent, value); }
            get { return myagent; }
        }
        private decimal myamount;
        public decimal Amount
        {
            set { SetProperty<decimal>(ref myamount, value); }
            get { return myamount; }
        }
        private DateTime? mydate;
        public DateTime? Date
        {
            set 
            {
                DateTime? olddate = mydate;
                SetProperty<DateTime?>(ref mydate, value);
            }
            get { return mydate; }
        }
        private bool myexpired;
        public bool Expired
        {
            set { SetProperty<bool>(ref myexpired, value); }
            get { return myexpired; }
        }
        private DateTime myexpirydate;
        public DateTime ExpiryDate
        {
            set { SetProperty<DateTime>(ref myexpirydate, value); }
            get { return myexpirydate; }
        }
        private string mynumber;
        public string Number
        {
            set { SetProperty<string>(ref mynumber, value); }
            get { return mynumber; }
        }

        protected override void PropertiesUpdate(DomainBaseReject sample)
        {
            Contract templ = sample as Contract;
            //this.Agent = templ.Agent;
            this.Amount = templ.Amount;
            this.Date = templ.Date;
            this.Expired = templ.Expired;
            this.ExpiryDate = templ.ExpiryDate;
            this.Number = templ.Number;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Agent):
                    this.Agent = (Agent)value;
                    break;
                case nameof(this.Amount):
                    this.Amount = (decimal)value;
                    break;
                case nameof(this.Date):
                    this.Date = (DateTime?)value;
                    break;
                case nameof(this.Expired):
                    this.Expired = (bool)value;
                    break;
                case nameof(this.ExpiryDate):
                    this.ExpiryDate = (DateTime)value;
                    break;
                case nameof(this.Number):
                    this.Number = (string)value;
                    break;
            }
        }
    }

    public class ContractDBM : lib.DBManagerWhoWhen<Contract>
    {
        internal ContractDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "account.Contract_sp";
            InsertCommandText = "account.ContractAdd_sp";
            UpdateCommandText = "account.ContractUpd_sp";
            DeleteCommandText = "account.ContractDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int), new SqlParameter("@agent", System.Data.SqlDbType.Int) };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0]
                , new SqlParameter("@agent", System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@amountupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@dateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@expiredupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@expirydateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@numberupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0]
               ,myinsertupdateparams[1]
               ,myinsertupdateparams[2]
               ,new SqlParameter("@amount",System.Data.SqlDbType.Money)
               ,new SqlParameter("@date",System.Data.SqlDbType.DateTime2)
               ,new SqlParameter("@expired", System.Data.SqlDbType.Bit)
               ,new SqlParameter("@expirydate",System.Data.SqlDbType.DateTime2)
               ,new SqlParameter("@number", System.Data.SqlDbType.NVarChar,10)
             };
        }

        private Agent myagent;
        public Agent Agent
        {
            set { myagent = value; this.ItemId = null; }
            get { return myagent; }
        }

        protected override void CancelLoad()
        {
        }
        protected override Contract CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            List<lib.DBMError> errors;
            Agent agent = CustomBrokerWpf.References.AgentStore.GetItemLoad(reader.GetInt32(this.Fields["agent"]), addcon, out errors);
            this.Errors.AddRange(errors);
            return new Contract(reader.GetInt32(this.Fields["id"]), reader.GetInt64(this.Fields["stamp"])
                ,reader.GetDateTime(this.Fields["updated"]),reader.GetString(this.Fields["updater"])
                ,lib.DomainObjectState.Unchanged
                , agent
                ,reader.GetDecimal(this.Fields["amount"])
                ,reader.IsDBNull(this.Fields["date"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["date"])
                ,reader.GetBoolean(this.Fields["expired"])
                ,reader.GetDateTime(this.Fields["expirydate"])
                ,reader.IsDBNull(this.Fields["number"]) ? null : reader.GetString(this.Fields["number"]));
        }
        protected override void GetOutputSpecificParametersValue(Contract item)
        {
            //if(item.DomainState==lib.DomainObjectState.Added)
            //    if (myinsertparams[1].Value != DBNull.Value) item.Stamp = (Int64)myinsertparams[1].Value;
        }
        protected override bool SaveChildObjects(Contract item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Contract item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            SelectParams[0].Value = this.ItemId;
            SelectParams[1].Value=myagent?.Id;
        }
        protected override bool SetSpecificParametersValue(Contract item)
        {
            foreach (SqlParameter par in this.InsertParams)
                switch(par.ParameterName)
            {
                    case "@agent":
                        par.Value = item.Agent?.Id;
                        break;
            }
            foreach (SqlParameter par in this.UpdateParams)
                switch (par.ParameterName)
                {
                    case "@amountupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Contract.Amount));
                        break;
                    case "@dateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Contract.Date));
                        break;
                    case "@expiredupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Contract.Expired));
                        break;
                    case "@expirydateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Contract.ExpiryDate));
                        break;
                    case "@numberupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Contract.Number));
                        break;
                }
            foreach (SqlParameter par in this.InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@amount":
                        par.Value = item.Amount;
                        break;
                    case "@date":
                        par.Value = item.Date;
                        break;
                    case "@expired":
                        par.Value = item.Expired;
                        break;
                    case "@expirydate":
                        par.Value = item.ExpiryDate;
                        break;
                    case "@number":
                        par.Value = item.Number;
                        break;
                }
            return item.Agent?.Id > 0;
        }
    }

    public class ContractVM : lib.ViewModelErrorNotifyItem<Contract>
    {
        public ContractVM(Contract model):base(model)
        {
            DeleteRefreshProperties.AddRange(new string[] { nameof(ContractVM.Amount), nameof(this.Date), nameof(this.Expired), nameof(this.ExpiryDate), nameof(this.Number) });
            InitProperties();
        }
        public ContractVM():this(new Contract()) { }

        public decimal? Amount
        {
            set
            {
                if (!this.IsReadOnly && value.HasValue && !decimal.Equals(this.DomainObject.Amount, value.Value))
                {
                    string name = nameof(this.Amount);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Amount);
                    ChangingDomainProperty = name; this.DomainObject.Amount = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Amount : (decimal?)null; }
        }
        public DateTime? Date
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Date.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.Date.Value, value.Value))))
                {
                    string name = nameof(this.Date);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Date);
                    ChangingDomainProperty = name; this.DomainObject.Date = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Date : (DateTime?)null; }
        }
        public bool Expired
        {
            set
            {
                if (!(this.IsReadOnly || bool.Equals(this.DomainObject.Expired, value)))
                {
                    string name = nameof(this.Expired);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Expired);
                    ChangingDomainProperty = name; this.DomainObject.Expired = value;
                }
            }
            get { return this.IsEnabled ? !this.DomainObject.Expired : false; }
        }
        public DateTime? ExpiryDate
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(this.DomainObject.ExpiryDate, value.Value)))
                {
                    string name = nameof(this.ExpiryDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ExpiryDate);
                    ChangingDomainProperty = name; this.DomainObject.ExpiryDate = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ExpiryDate : (DateTime?)null; }
        }
        public string Number
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Number, value)))
                {
                    string name = nameof(this.Number);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Number);
                    ChangingDomainProperty = name; this.DomainObject.Number = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Number : null; }
        }

        protected override bool DirtyCheckProperty()
        {
            return false;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
        }
        protected override void InitProperties()
        {
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Amount):
                    this.DomainObject.Amount = (decimal)value;
                    break;
                case nameof(this.Date):
                    this.DomainObject.Date = (DateTime?)value;
                    break;
                case nameof(this.Expired):
                    this.DomainObject.Expired = (bool)value;
                    break;
                case nameof(this.ExpiryDate):
                    this.DomainObject.ExpiryDate = (DateTime)value;
                    break;
                case nameof(this.Number):
                    this.DomainObject.Number = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            return true;
        }
    }

    internal class ContractSynchronizer : lib.ModelViewCollectionsSynchronizer<Contract, ContractVM>
    {
        protected override Contract UnWrap(ContractVM wrap)
        {
            return wrap.DomainObject;
        }
        protected override ContractVM Wrap(Contract fill)
        {
            return new ContractVM(fill);
        }
    }
}