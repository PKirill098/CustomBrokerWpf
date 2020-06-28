using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Algorithm
{
    public class AlgorithmValuesRequestCon : AlgorithmValuesRequest
    {
        public AlgorithmValuesRequestCon(int id, long stamp, lib.DomainObjectState state, Algorithm algorithm, Formula formula, decimal? value1, decimal? value2, decimal? value1user, decimal? value2user, long afstamp, AlgorithmConsolidateCommand cmd)
            : base(id, stamp, state, algorithm, formula, value1, value2, value1user, value2user, afstamp)
        {
            myvalue1user = value1user;
            myvalue2user = value2user;
            mycmd = cmd;
            RequestSync1();
            RequestSync2();
        }
        public AlgorithmValuesRequestCon(Algorithm algorithm, Formula formula, AlgorithmConsolidateCommand cmd) : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, algorithm, formula, null, null, null, null, 0, cmd) { }

        private AlgorithmConsolidateCommand mycmd;

        protected override void RequestSync1()
        {
            if (mycmd != null)
                switch (this.Formula.Code)
                {
                    case "П9":
                        mycmd.InvoiceDiscount = this.Value1;
                        break;
                    case "П10":
                        mycmd.Weight = this.Value1;
                        break;
                    case "П12":
                        mycmd.CustomsPay = this.Value1;
                        break;
                    case "П17":
                        mycmd.FreightCost = this.Value1;
                        break;
                    case "П18":
                        mycmd.SertificatCost = this.Value1;
                        break;
                    case "П19":
                        mycmd.PreparatnCost = this.Value1;
                        break;
                    case "П20":
                        mycmd.AdditionalCost = this.Value1;
                        break;
                    case "П21":
                        if (mycmd.ServiceType == "ТЭО")
                        {
                            mycmd.Cost = this.Value1;
                        }
                        else
                        {
                            mycmd.Cost = null;
                        }
                        break;
                    case "П30":
                        if (mycmd.ServiceType == "ТЭО")
                        {
                            mycmd.Corr = this.Value1;
                        }
                        break;
                    case "П31":
                        if (mycmd.ServiceType == "ТЭО")
                        {
                            mycmd.Pay = this.Value1;
                        }
                        break;
                    case "П39":
                        if (mycmd.ServiceType == "ТД")
                        {
                            mycmd.Corr = this.Value1;
                        }
                        break;
                    case "П40":
                        if (mycmd.ServiceType == "ТД")
                        {
                            mycmd.Pay = this.Value1;
                        }
                        break;
                    case "П47":
                        mycmd.Income = this.Value1;
                        break;
                    case "П49":
                        mycmd.LogisticsCost = this.Value1;
                        break;
                    case "П50":
                        mycmd.LogisticsPay = this.Value1;
                        break;
                    case "X1":
                        if (this.Value1.HasValue)
                        {
                            foreach (Request request in mycmd.Requests)
                            {
                                if (request.AlgorithmCMD != null)
                                {
                                    decimal? p9 = null;
                                    foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                        if (values.Formula.Code == "П9")
                                        {
                                            p9 = values.Value1;
                                            break;
                                        }
                                    if (p9.HasValue)
                                        foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                            if (values.Formula.Code == "П12" & !values.Value1User.HasValue)
                                            {
                                                values.Value1Templ = this.Value1.Value * p9;
                                                break;
                                            }
                                }
                            }
                        }
                        break;
                    case "X2":
                        if (this.Value1.HasValue)
                        {
                            foreach (Request request in mycmd.Requests)
                            {
                                if (request.AlgorithmCMD != null)
                                {
                                    decimal? p9 = null;
                                    foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                        if (values.Formula.Code == "П9")
                                        {
                                            p9 = values.Value1;
                                            break;
                                        }
                                    if (p9.HasValue)
                                        foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                            if (values.Formula.Code == "П17" & !values.Value1User.HasValue)
                                            {
                                                values.Value1Templ = this.Value1.Value * p9;
                                                break;
                                            }
                                }
                            }
                        }
                        break;
                    case "X3":
                        if (this.Value1.HasValue)
                        {
                            foreach (Request request in mycmd.Requests)
                            {
                                if (request.AlgorithmCMD != null)
                                {
                                    decimal? p9 = null;
                                    foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                        if (values.Formula.Code == "П9")
                                        {
                                            p9 = values.Value1;
                                            break;
                                        }
                                    if (p9.HasValue)
                                        foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                            if (values.Formula.Code == "П18" & !values.Value1User.HasValue)
                                            {
                                                values.Value1Templ = this.Value1.Value * p9;
                                                break;
                                            }
                                }
                            }
                        }
                        break;
                    case "X4":
                        if (this.Value1.HasValue)
                        {
                            foreach (Request request in mycmd.Requests)
                            {
                                if (request.AlgorithmCMD != null)
                                {
                                    decimal? p9 = null;
                                    foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                        if (values.Formula.Code == "П9")
                                        {
                                            p9 = values.Value1;
                                            break;
                                        }
                                    if (p9.HasValue)
                                        foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                            if (values.Formula.Code == "П19" & !values.Value1User.HasValue)
                                            {
                                                values.Value1Templ = this.Value1.Value * p9;
                                                break;
                                            }
                                }
                            }
                        }
                        break;
                    case "X5":
                        if (this.Value1.HasValue)
                        {
                            foreach (Request request in mycmd.Requests)
                            {
                                if (request.AlgorithmCMD != null)
                                {
                                    decimal? p9 = null;
                                    foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                        if (values.Formula.Code == "П9")
                                        {
                                            p9 = values.Value1;
                                            break;
                                        }
                                    if (p9.HasValue)
                                        foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                            if (values.Formula.Code == "П20" & !values.Value1User.HasValue)
                                            {
                                                values.Value1Templ = this.Value1.Value * p9;
                                                break;
                                            }
                                }
                            }
                        }
                        break;
                }
        }
        protected override void RequestSync2()
        {
            if (mycmd != null)
                switch (this.Formula.Code)
                {
                    case "П12":
                        if (this.Value2.HasValue)
                            mycmd.CustomsPayPer = decimal.Divide(this.Value2.Value, 100M);
                        else
                            mycmd.CustomsPayPer = this.Value2;
                        break;
                    case "П17":
                        mycmd.FreightPay = this.Value1;
                        break;
                    case "П18":
                        mycmd.SertificatPay = this.Value1;
                        break;
                    case "П19":
                        mycmd.PreparatnPay = this.Value1;
                        break;
                    case "П20":
                        mycmd.AdditionalPay = this.Value1;
                        break;
                    case "П21":
                        if (mycmd.ServiceType == "ТЭО")
                        {
                            if (this.Value2.HasValue)
                                mycmd.CostPer = decimal.Divide(this.Value2.Value, 100M);
                            else
                                mycmd.CostPer = this.Value2;
                        }
                        else
                        {
                            mycmd.CostPer = null;
                        }
                        break;
                    case "П30":
                        if (mycmd.ServiceType == "ТЭО")
                        {
                            if (this.Value2.HasValue)
                                mycmd.CorrPer = decimal.Divide(this.Value2.Value, 100M);
                            else
                                mycmd.CorrPer = this.Value2;
                        }
                        break;
                    case "П31":
                        if (mycmd.ServiceType == "ТЭО")
                        {
                            if (this.Value2.HasValue)
                                mycmd.PayPer = decimal.Divide(this.Value2.Value, 100M);
                            else
                                mycmd.PayPer = this.Value2;
                            if (this.Value2.HasValue)
                            {
                                foreach (Request request in mycmd.Requests)
                                {
                                    decimal? p40 = null;
                                    foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                    {
                                        if (values.Formula.Code == "П31")
                                        {
                                            if (values.Value1.HasValue & values.Value2.HasValue)
                                                p40 = values.Value1.Value * decimal.Divide(this.Value2.Value, values.Value2.Value) - values.Value1.Value;
                                            break;
                                        }
                                    }
                                    if (p40.HasValue)
                                        foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                        {
                                            if (values.Formula.Code == "П30")
                                            {
                                                values.Value1Templ = (values.Value1Templ ?? 0M) + p40;
                                                break;
                                            }
                                        }
                                }
                            }
                        }
                        break;
                    case "П39":
                        if (mycmd.ServiceType == "ТД")
                        {
                            if (this.Value2.HasValue)
                                mycmd.CorrPer = decimal.Divide(this.Value2.Value, 100M);
                            else
                                mycmd.CorrPer = this.Value2;
                        }
                        break;
                    case "П40":
                        if (mycmd.ServiceType == "ТД")
                        {
                            if (this.Value2.HasValue)
                                mycmd.PayPer = decimal.Divide(this.Value2.Value, 100M);
                            else
                                mycmd.PayPer = this.Value2;
                            if (this.Value1.HasValue & this.Value2.HasValue)
                            {
                                foreach (Request request in mycmd.Requests)
                                {
                                    decimal? p40 = null;
                                    foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                    {
                                        if (values.Formula.Code == "П40")
                                        {
                                            if (values.Value1.HasValue & values.Value2.HasValue)
                                                p40 = values.Value1.Value * decimal.Divide(this.Value2.Value, values.Value2.Value) - values.Value1.Value;
                                            break;
                                        }
                                    }
                                    if (p40.HasValue)
                                        foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                        {
                                            if (values.Formula.Code == "П39")
                                            {
                                                values.Value1Templ = (values.Value1Templ ?? 0M) + p40;
                                                break;
                                            }
                                        }
                                }
                            }
                        }
                        break;
                    case "П47":
                        if (this.Value2.HasValue)
                            mycmd.IncomePer = decimal.Divide(this.Value2.Value, 100M);
                        else
                            mycmd.IncomePer = this.Value2;
                        break;
                }
        }
    }

    internal class AlgorithmValuesRequestConDBM : lib.DBManagerStamp<AlgorithmValuesRequestCon>
    {
        public AlgorithmValuesRequestConDBM(ObservableCollection<Formula> formulas, AlgorithmConsolidateCommand cmd) : base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;
            SelectCommandText = "[dbo].[RequestAlgorithmValuesCon_sp]";
            InsertCommandText = "[dbo].[RequestAlgorithmValuesConAdd_sp]";
            UpdateCommandText = "[dbo].[RequestAlgorithmValuesConUpd_sp]";

            SelectParams = new SqlParameter[]
            {
            new SqlParameter("@parcelid", System.Data.SqlDbType.Int),
            new SqlParameter("@group", System.Data.SqlDbType.NVarChar,5),
            new SqlParameter("@algorithmid", System.Data.SqlDbType.Int),
            };
            InsertParams = new SqlParameter[]
            {
            myinsertparams[0],
            new SqlParameter("@parcelid", System.Data.SqlDbType.Int),
            new SqlParameter("@group", System.Data.SqlDbType.NVarChar,5),
            };
            InsertUpdateParams = new SqlParameter[]
            {
            myinsertupdateparams[0],
            new SqlParameter("@formulaid", System.Data.SqlDbType.Int),
            new SqlParameter("@code", System.Data.SqlDbType.NVarChar,3),
            new SqlParameter("@name", System.Data.SqlDbType.NVarChar,50),
            new SqlParameter("@type", System.Data.SqlDbType.TinyInt),
            new SqlParameter("@formula1", System.Data.SqlDbType.NVarChar,50),
            new SqlParameter("@formula2", System.Data.SqlDbType.NVarChar,50),
            new SqlParameter("@value1", System.Data.SqlDbType.Decimal){Precision=18,Scale=8 },
            new SqlParameter("@value2", System.Data.SqlDbType.Decimal){Precision=18,Scale=8 },
            new SqlParameter("@isuser1", System.Data.SqlDbType.Bit),
            new SqlParameter("@isuser2", System.Data.SqlDbType.Bit),
            new SqlParameter("@afstamp", System.Data.SqlDbType.BigInt)
            };
            //myalgorithm = algorithm;
            myformulas = formulas;
            //mystorage = storage;
            mycmd = cmd;
        }
        //public AlgorithmValuesRequestConDBM(Request request) : this(null, null, request) { }

        private ObservableCollection<Formula> myformulas;
        internal ObservableCollection<Formula> Formulas
        { set { myformulas = value; } }
        //private AlgorithmValuesStorage mystorage;
        //internal AlgorithmValuesStorage Storage
        //{ set { mystorage = value; } }
        private AlgorithmConsolidateCommand mycmd;

        protected override AlgorithmValuesRequestCon CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            int frmid = reader.GetInt32(reader.GetOrdinal("formulaid"));
            Formula formula = null;
            if (mycmd.Parcel.Status.Id < 500)
            {
                foreach (Formula frm in myformulas)
                    if (frm.Id == frmid)
                    {
                        formula = frm;
                        break;
                    }
            }
            else
            {
                formula = new Formula(frmid, 0, lib.DomainObjectState.Sealed, reader.GetString(reader.GetOrdinal("code")), reader.GetString(reader.GetOrdinal("name")), reader.GetByte(reader.GetOrdinal("type")), reader.GetString(reader.GetOrdinal("formula1")), reader.GetString(reader.GetOrdinal("formula2")));
                myformulas.Add(formula);
            }
            AlgorithmValuesRequestCon newitem = new AlgorithmValuesRequestCon(reader.IsDBNull(0) ? lib.NewObjectId.NewId : reader.GetInt32(0), reader.IsDBNull(1) ? 0 : reader.GetInt64(1), (mycmd.Parcel.Status.Id < 500 ? (reader.IsDBNull(0) ? lib.DomainObjectState.Added : lib.DomainObjectState.Modified) : lib.DomainObjectState.Sealed)
                , mycmd.Algorithm, formula
                , reader.IsDBNull(reader.GetOrdinal("value1")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("value1"))
                , reader.IsDBNull(reader.GetOrdinal("value2")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("value2"))
                , reader.IsDBNull(reader.GetOrdinal("value1user")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("value1user"))
                , reader.IsDBNull(reader.GetOrdinal("value2user")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("value2user"))
                , reader.IsDBNull(reader.GetOrdinal("afstamp")) ? 0 : reader.GetInt64(reader.GetOrdinal("afstamp"))
                , mycmd);
            return newitem; //mystorage.UpdateItem(newitem) as AlgorithmValuesRequest
        }
        protected override void GetOutputSpecificParametersValue(AlgorithmValuesRequestCon item)
        {
        }
        protected override bool SaveChildObjects(AlgorithmValuesRequestCon item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(AlgorithmValuesRequestCon item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[0].Value = mycmd.Parcel.Id;
            this.SelectParams[1].Value = mycmd.Group;
            if (mycmd.Parcel.Status.Id < 500) this.SelectParams[2].Value = mycmd.Algorithm.Id; else this.SelectParams[2].Value = null;
        }
        protected override bool SetSpecificParametersValue(AlgorithmValuesRequestCon item)
        {
            myinsertparams[1].Value = mycmd.Parcel.Id;
            myinsertparams[2].Value = mycmd.Group;
            myinsertupdateparams[1].Value = item.Formula.Id;
            myinsertupdateparams[2].Value = item.Formula.Code;
            myinsertupdateparams[3].Value = item.Formula.Name;
            myinsertupdateparams[4].Value = item.Formula.FormulaType;
            myinsertupdateparams[5].Value = item.Formula.Formula1;
            myinsertupdateparams[6].Value = item.Formula.Formula2;
            myinsertupdateparams[7].Value = item.Value1;
            myinsertupdateparams[8].Value = item.Value2;
            myinsertupdateparams[9].Value = item.Value1User.HasValue;
            myinsertupdateparams[10].Value = item.Value2User.HasValue;
            myinsertupdateparams[11].Value = item.AFStamp;
            return true;
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class AlgorithmConsolidateTotalDBM : lib.DBMExec
    {
        internal AlgorithmConsolidateTotalDBM(AlgorithmConsolidateCommand cmd) : base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            SelectCommandText = "dbo.RequestAlgorithmConTotal_sp";
            SelectParams = new SqlParameter[] {
            new SqlParameter("@group", System.Data.SqlDbType.NVarChar,5),
            new SqlParameter("@parcel", System.Data.SqlDbType.Int),
            new SqlParameter("@requestid", System.Data.SqlDbType.Int),
            new SqlParameter("@requestsid", System.Data.SqlDbType.Structured),
            new SqlParameter("@weight", System.Data.SqlDbType.Money),
            new SqlParameter("@volume", System.Data.SqlDbType.SmallMoney),
            new SqlParameter("@cellnumber", System.Data.SqlDbType.SmallInt),
            new SqlParameter("@customspay", System.Data.SqlDbType.Money),
            new SqlParameter("@invoice", System.Data.SqlDbType.Money),
            new SqlParameter("@invoicediscount", System.Data.SqlDbType.Money) };
            SelectParams[3].TypeName = "ID_TVP";
            SelectParams[4].Direction = System.Data.ParameterDirection.Output;
            SelectParams[5].Direction = System.Data.ParameterDirection.Output;
            SelectParams[6].Direction = System.Data.ParameterDirection.Output;
            SelectParams[7].Direction = System.Data.ParameterDirection.Output;
            SelectParams[8].Direction = System.Data.ParameterDirection.Output;
            SelectParams[9].Direction = System.Data.ParameterDirection.Output;

            myccmd = cmd;
        }

        private AlgorithmConsolidateCommand myccmd; // confuse with sqlcommand - base cmd
        internal AlgorithmConsolidateCommand AlgorithmCommand
        {
            set { myccmd = value; }
            get { return myccmd; }
        }
        private decimal? myweight;
        public decimal? Weight
        { get { return DBNull.Value == (this.SelectParams[4].Value ?? DBNull.Value) ? myweight : (myweight ?? 0M) + (decimal)this.SelectParams[4].Value; } }
        private decimal? myvolume;
        public decimal? Volume
        { get { return DBNull.Value == (this.SelectParams[5].Value ?? DBNull.Value) ? myvolume : (myvolume ?? 0M) + (decimal)this.SelectParams[5].Value; } }
        private decimal? mycellnumber;
        public decimal? CellNumber
        { get { return DBNull.Value == (this.SelectParams[6].Value ?? DBNull.Value) ? mycellnumber : (mycellnumber ?? 0M) + (Int16)this.SelectParams[6].Value; } }
        private decimal? mycustompay;
        public decimal? CustomsPay
        { get { return DBNull.Value == (this.SelectParams[7].Value ?? DBNull.Value) ? mycustompay : (mycustompay ?? 0M) + (decimal)this.SelectParams[7].Value; } }
        private decimal? myinvoice;
        public decimal? Invoice
        { get { return DBNull.Value == (this.SelectParams[8].Value ?? DBNull.Value) ? myinvoice : (myinvoice ?? 0M) + (decimal)this.SelectParams[8].Value; } }
        private decimal? myinvoicediscount;
        public decimal? InvoiceDiscount
        { get { return DBNull.Value == (this.SelectParams[9].Value ?? DBNull.Value) ? myinvoicediscount : (myinvoicediscount ?? 0M) + (decimal)this.SelectParams[9].Value; } }

        protected override void PrepareFill(SqlConnection addcon)
        {
            this.SelectParams[0].Value = myccmd.Group;
            this.SelectParams[1].Value = myccmd.Parcel.Id;
            //this.SelectParams[2].Value = myrequest?.Id;
            mycellnumber = null; mycustompay = null; myinvoice = null; myinvoicediscount = null; myvolume = null; myweight = null;
            System.Data.DataTable requestids = new System.Data.DataTable();
            requestids.Columns.Add("id", typeof(Int32));
            foreach (Request req in myccmd.Requests)
            {
                requestids.Rows.Add(req.Id);
                if (req.CellNumber.HasValue) mycellnumber = (mycellnumber ?? 0M) + (req.CellNumber ?? 0M);
                if (req.CustomsPay.HasValue) mycustompay = (mycustompay ?? 0M) + (req.CustomsPay ?? 0M);
                if (req.Invoice.HasValue) myinvoice = (myinvoice ?? 0M) + (req.Invoice ?? 0M);
                if (req.InvoiceDiscount.HasValue) myinvoicediscount = (myinvoicediscount ?? 0M) + (req.InvoiceDiscount ?? 0M);
                if (req.Volume.HasValue) myvolume = (myvolume ?? 0M) + (req.Volume ?? 0M);
                if (req.OfficialWeight.HasValue) myweight = (myweight ?? 0M) + (req.OfficialWeight ?? 0M);
            }
            this.SelectParams[3].Value = requestids;
        }
    }

    public class AlgorithmConsolidateAlgorithmDBM : lib.DBManager<Algorithm>
    {
        public AlgorithmConsolidateAlgorithmDBM(int parcelid, string group)
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            myparcelid = parcelid;
            mygroup = group;
            SelectProcedure = true;
            UpdateProcedure = true;
            SelectCommandText = "RequestAlgorithmConAlgorithm_sp";
            UpdateCommandText = "RequestAlgorithmConAlgorithmUpd_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@parcelid", System.Data.SqlDbType.Int), new SqlParameter("@group", System.Data.SqlDbType.NVarChar, 5) };
            UpdateParams = new SqlParameter[] { new SqlParameter("@parcelid", System.Data.SqlDbType.Int), new SqlParameter("@group", System.Data.SqlDbType.NVarChar, 5), new SqlParameter("@name", System.Data.SqlDbType.NVarChar, 20) };
        }

        private int myparcelid;
        internal int ParcelId
        {
            set { myparcelid = value; }
            get { return myparcelid; }
        }
        private string mygroup;
        internal string Group
        {
            set { mygroup = value; }
            get { return mygroup; }
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[0].Value = myparcelid; this.SelectParams[1].Value = mygroup;
        }
        protected override Algorithm CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new Algorithm(0, lib.DomainObjectState.Sealed, reader.GetString(0), 0);
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SaveIncludedObject(Algorithm item)
        {
            return true;
        }
        protected override bool SaveChildObjects(Algorithm item)
        {
            return true;
        }
        protected override bool SetParametersValue(Algorithm item)
        {
            myupdateparams[0].Value = myparcelid;
            myupdateparams[1].Value = mygroup;
            myupdateparams[2].Value = item.Name;
            if (myparcelid <= 0)
                this.Errors.Add(new lib.DBMError(item, "Алгоритм консолидации. Загрузка не сохранена!", "parcelid"));
            return myparcelid > 0;
        }
        protected override void GetOutputParametersValue(Algorithm item)
        {
        }
        protected override void ItemAcceptChanches(Algorithm item)
        {
            item.AcceptChanches();
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class AlgorithmConsolidateCommand : AlgorithmFormulaCommand
    {
        internal AlgorithmConsolidateCommand(Request request) : base(true)
        {
            myrequest = request;
            myparcel = request.Parcel;
            mygroup = request.Consolidate;
            myrequests = new List<Request>();
            myadbm = new AlgorithmDBM();
            myformulasynchronizer = new FormulaSynchronizer();
            myfdbm = new FormulaDBM();
            mywdbm = new AlgorithmWeightDBM();
            mygwdbm = new AlgorithmConsolidateTotalDBM(this);
            if (myrequest.ParcelId.HasValue) myalgdbm = new AlgorithmConsolidateAlgorithmDBM(myrequest.Parcel.Id, myrequest.Consolidate);
            myvdbm = new AlgorithmValuesRequestConDBM(null, this);
            myalgorithms = new ObservableCollection<Algorithm>();
            myalgorithmformulas = new ObservableCollection<AlgorithmFormula>();
            myview1 = new ListCollectionView(myalgorithmformulas);
            myview1.SortDescriptions.Add(new System.ComponentModel.SortDescription("Formula.Order", System.ComponentModel.ListSortDirection.Ascending));
            myview1.Filter = (object item) => { FormulaVM formula = (item as AlgorithmFormula).Formula; return formula.DomainObject.FormulaType < 100; };
            myview2 = new ListCollectionView(myalgorithmformulas);
            myview2.SortDescriptions.Add(new System.ComponentModel.SortDescription("Formula.Order", System.ComponentModel.ListSortDirection.Ascending));
            myview2.Filter = (object item) => { FormulaVM formula = (item as AlgorithmFormula).Formula; return formula.DomainObject.FormulaType > 100; };

            this.ServiceType = request.ServiceType;
            if (request.ParcelId.HasValue & !string.IsNullOrEmpty(request.Consolidate))
            {
                this.LoadData();
            }
            myrequests.Add(request);
            request.PropertyChanged += Request_PropertyChanged;
            //this.RequestAttached(myrequest);
        }
        internal AlgorithmConsolidateCommand(Parcel parcel, string cons) : base(true)
        {
            myparcel = parcel;
            mygroup = cons;

            myrequests = new List<Request>();
            myadbm = new AlgorithmDBM();
            myformulasynchronizer = new FormulaSynchronizer();
            myfdbm = new FormulaDBM();
            mywdbm = new AlgorithmWeightDBM();
            mygwdbm = new AlgorithmConsolidateTotalDBM(this);
            myalgdbm = new AlgorithmConsolidateAlgorithmDBM(myparcel.Id, mygroup);
            myalgorithms = new ObservableCollection<Algorithm>();
            myalgorithmformulas = new ObservableCollection<AlgorithmFormula>();
            myvdbm = new AlgorithmValuesRequestConDBM(null, this);

            myview1 = new ListCollectionView(myalgorithmformulas);
            myview1.SortDescriptions.Add(new System.ComponentModel.SortDescription("Formula.Order", System.ComponentModel.ListSortDirection.Ascending));
            myview1.Filter = (object item) => { FormulaVM formula = (item as AlgorithmFormula).Formula; return formula.DomainObject.FormulaType < 100; };
            myview2 = new ListCollectionView(myalgorithmformulas);
            myview2.SortDescriptions.Add(new System.ComponentModel.SortDescription("Formula.Order", System.ComponentModel.ListSortDirection.Ascending));
            myview2.Filter = (object item) => { FormulaVM formula = (item as AlgorithmFormula).Formula; return formula.DomainObject.FormulaType > 100; };

            if (myparcel != null & !string.IsNullOrEmpty(mygroup))
            {
                this.LoadData();
            }
        }

        internal Request myrequest;

        private string mygroup;
        internal string Group
        {
            set { mygroup = value; }
            get { return mygroup; }
        }
        private Parcel myparcel;
        internal Parcel Parcel
        {
            set { myparcel = value; }
            get { return myparcel; }
        }
        private List<Request> myrequests;
        internal List<Request> Requests { get { return myrequests; } }
        internal string ServiceType { set; get; }
        private AlgorithmConsolidateAlgorithmDBM myalgdbm;
        private AlgorithmConsolidateTotalDBM mygwdbm;
        private AlgorithmWeightDBM mywdbm;
        private AlgorithmValuesRequestConDBM myvdbm;
        private Algorithm myalgorithm;
        public Algorithm Algorithm
        {
            get { return myalgorithm; }
        }
        private bool myisreadonly;
        public override bool IsReadOnly
        {
            set { myisreadonly = value; PropertyChangedNotification("IsReadOnly"); }
            get { return myrequest.Status.Id > 499 | myisreadonly; }
        }
        public override bool FormulaIsReadOnly
        {
            get { return true; }
        }
        public override bool AlgorithmIsReadOnly
        {
            get { return true; }
        }
        public override Visibility WriterMenuVisible
        {
            get { return Visibility.Collapsed; }
        }
        public override Visibility SaveMenuVisible
        {
            get { return Visibility.Visible; }
        }

        #region Request Properties
        private decimal? myadditionalcost;
        public decimal? AdditionalCost
        {
            set
            {
                if (myadditionalcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myadditionalcost.Value, value.Value)))
                {
                    myadditionalcost = value;
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П20" && !(item.Value1 == value))
                        {
                            item.Value1 = value;
                            break;
                        }
                    this.PropertyChangedNotification("AdditionalCost");
                }
            }
            get { return myadditionalcost; }
        }
        private decimal? myadditionalpay;
        public decimal? AdditionalPay
        {
            set
            {
                if (myadditionalpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myadditionalpay.Value, value.Value)))
                {
                    myadditionalpay = value;
                    this.PropertyChangedNotification("AdditionalPay");
                }
            }
            get { return myadditionalpay; }
        }
        private decimal? mycellnumber;
        public decimal? CellNumber
        {
            set
            {
                if (mycellnumber.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycellnumber.Value, value.Value)))
                {
                    mycellnumber = value;
                    this.PropertyChangedNotification("CellNumber");
                }
            }
            get
            {

                return mycellnumber;
            }
        }
        private decimal? mycustomspay;
        public decimal? CustomsPay
        {
            set
            {
                if (mycustomspay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycustomspay.Value, value.Value)))
                {
                    mycustomspay = value;
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П12")
                        {
                            if (!(item.Value1 == value)) // null !
                                item.Value1 = value;
                            break;
                        }
                    this.PropertyChangedNotification("CustomsPay");
                }
            }
            get
            {

                return mycustomspay;
            }
        }
        private decimal? mycustomspayper;
        public decimal? CustomsPayPer
        {
            set
            {
                if (mycustomspayper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycustomspayper.Value, value.Value)))
                {
                    mycustomspayper = value;
                    this.PropertyChangedNotification("CustomsPayPer");
                }
            }
            get
            {

                return mycustomspayper;
            }
        }
        private decimal? mycorr;
        public decimal? Corr
        {
            set
            {
                if (mycorr.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycorr.Value, value.Value)))
                {
                    mycorr = value;
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if ((this.ServiceType == "ТЭО" && item.Formula.Code == "П30") || (this.ServiceType == "ТД" && item.Formula.Code == "П39"))
                        {
                            if (!(item.Value1 == value)) // null !
                                item.Value1 = value;
                            break;
                        }
                    this.PropertyChangedNotification("Corr");
                }
            }
            get
            {

                return mycorr;
            }
        }
        private decimal? mycorrper;
        public decimal? CorrPer
        {
            set
            {
                if (mycorrper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycorrper.Value, value.Value)))
                {
                    mycorrper = value;
                    this.PropertyChangedNotification("CorrPer");
                }
            }
            get
            {

                return mycorrper;
            }
        }
        private decimal? mycost;
        public decimal? Cost
        {
            set
            {
                if (mycost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycost.Value, value.Value)))
                {
                    mycost = value;
                    this.PropertyChangedNotification("Cost");
                }
            }
            get
            {

                return mycost;
            }
        }
        private decimal? mycostper;
        public decimal? CostPer
        {
            set
            {
                if (mycostper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycostper.Value, value.Value)))
                {
                    mycostper = value;
                    this.PropertyChangedNotification("CostPer");
                }
            }
            get
            {

                return mycostper;
            }
        }
        private decimal? myfreightcost;
        public decimal? FreightCost
        {
            set
            {
                if (myfreightcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myfreightcost.Value, value.Value)))
                {
                    myfreightcost = value;
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П17" && !(item.Value1 == value))
                        {
                            item.Value1 = value;
                            break;
                        }
                    this.PropertyChangedNotification("FreightCost");
                }
            }
            get { return myfreightcost; }
        }
        private decimal? myfreightpay;
        public decimal? FreightPay
        {
            set
            {
                if (myfreightpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myfreightpay.Value, value.Value)))
                {
                    myfreightpay = value;
                    this.PropertyChangedNotification("FreightPay");
                }
            }
            get { return myfreightpay; }
        }
        private decimal? myincome;
        public decimal? Income
        {
            set
            {
                if (myincome.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myincome.Value, value.Value)))
                {
                    myincome = value;
                    this.PropertyChangedNotification("Income");
                }
            }
            get
            {

                return myincome;
            }
        }
        private decimal? myincomeper;
        public decimal? IncomePer
        {
            set
            {
                if (myincomeper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myincomeper.Value, value.Value)))
                {
                    myincomeper = value;
                    this.PropertyChangedNotification("IncomePer");
                }
            }
            get
            {

                return myincomeper;
            }
        }
        private decimal? myinvoice;
        public decimal? Invoice
        {
            set
            {
                if (myinvoice.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myinvoice.Value, value.Value)))
                {
                    myinvoice = value;
                    this.PropertyChangedNotification("Invoice");
                }
            }
            get
            {

                return myinvoice;
            }
        }
        private decimal? myinvoicediscount;
        public decimal? InvoiceDiscount
        {
            set
            {
                if ((myinvoicediscount.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myinvoicediscount.Value, value.Value))))
                {
                    myinvoicediscount = value;
                    this.PropertyChangedNotification("InvoiceDiscount");
                }
            }
            get
            {

                return myinvoicediscount;
            }
        }
        private decimal? mylogisticscost;
        public decimal? LogisticsCost
        {
            set
            {
                if (mylogisticscost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mylogisticscost.Value, value.Value)))
                {
                    mylogisticscost = value;
                    this.PropertyChangedNotification("LogisticsCost");
                }
            }
            get
            {

                return mylogisticscost;
            }
        }
        private decimal? mylogisticspay;
        public decimal? LogisticsPay
        {
            set
            {
                if (mylogisticspay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mylogisticspay.Value, value.Value)))
                {
                    mylogisticspay = value;
                    this.PropertyChangedNotification("LogisticsPay");
                }
            }
            get { return mylogisticspay; }
        }
        private decimal? mypay;
        public decimal? Pay
        {
            set
            {
                if (mypay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mypay.Value, value.Value)))
                {
                    mypay = value;
                    this.PropertyChangedNotification("Pay");
                }
            }
            get
            {
                return mypay;
            }
        }
        private decimal? mypayper;
        public decimal? PayPer
        {
            set
            {
                if (mypayper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mypayper.Value, value.Value)))
                {
                    mypayper = value;
                    this.PropertyChangedNotification("PayPer");
                }
            }
            get
            {

                return mypayper;
            }
        }
        private decimal? mypreparatncost;
        public decimal? PreparatnCost
        {
            set
            {
                if (mypreparatncost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mypreparatncost.Value, value.Value)))
                {
                    mypreparatncost = value;
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П19" && !(item.Value1 == value))
                        {
                            item.Value1 = value;
                            break;
                        }
                    this.PropertyChangedNotification("PreparatnCost");
                }
            }
            get { return mypreparatncost; }
        }
        private decimal? mypreparatnpay;
        public decimal? PreparatnPay
        {
            set
            {
                if (mypreparatnpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mypreparatnpay.Value, value.Value)))
                {
                    mypreparatnpay = value;
                    this.PropertyChangedNotification("PreparatnPay");
                }
            }
            get { return mypreparatnpay; }
        }
        private decimal? mysertificatcost;
        public decimal? SertificatCost
        {
            set
            {
                if (mysertificatcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mysertificatcost.Value, value.Value)))
                {
                    mysertificatcost = value;
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П18" && !(item.Value1 == value))
                        {
                            item.Value1 = value;
                            break;
                        }
                    this.PropertyChangedNotification("SertificatCost");
                }
            }
            get { return mysertificatcost; }
        }
        private decimal? mysertificatpay;
        public decimal? SertificatPay
        {
            set
            {
                if (mysertificatpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mysertificatpay.Value, value.Value)))
                {
                    mysertificatpay = value;
                    this.PropertyChangedNotification("SertificatPay");
                }
            }
            get { return mysertificatpay; }
        }
        private decimal? myvolume;
        public decimal? Volume
        {
            set
            {
                if (myvolume.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myvolume.Value, value.Value)))
                {
                    myvolume = value;
                    this.PropertyChangedNotification("ConVolume");
                }
            }
            get { return myvolume; }
        }
        private decimal? myweight;
        public decimal? Weight
        {
            set
            {
                if ((myweight.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myweight.Value, value.Value))))
                {
                    myweight = value;
                    this.PropertyChangedNotification("Weight");
                }
            }
            get { return myweight; }
        }
        #endregion

        public override bool SaveDataChanges()
        {
            bool isSuccess = true;
            if (myparcel?.Status.Id < 500 && !string.IsNullOrEmpty(mygroup) & myalgorithm != null) //con algorithm is calculated
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                err.AppendLine("Изменения не сохранены");
                myvdbm.Errors.Clear();
                if (!myvdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    err.AppendLine(myvdbm.ErrorMessage);
                }
                myalgorithm.DomainState = lib.DomainObjectState.Modified;
                myalgdbm.ParcelId = myparcel.Id;
                myalgdbm.Group = myrequest.Consolidate;
                myalgdbm.Errors.Clear();
                if (!myalgdbm.SaveItemChanches(myalgorithm))
                {
                    isSuccess = false;
                    err.AppendLine(myalgdbm.ErrorMessage);
                }
                if (!isSuccess)
                    this.PopupText = err.ToString();
            }
            return isSuccess;
        }
        protected override bool CanSaveDataChanges()
        {
            return !this.IsReadOnly && myparcel.Status.Id < 500;
        }
        protected override void AddData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override bool CanAddData(object parametr)
        {
            return false;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return false;
        }
        protected override void DeleteData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override void RefreshData(object parametr)
        {
            if (myrequest.ParcelId.HasValue & !string.IsNullOrEmpty(myrequest.Consolidate))
            {
                this.LoadData();
                if (this.PopupText == "Изменения сохранены") this.PopupText = string.Empty;
            }
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override bool CanRejectChanges()
        {
            return !this.IsReadOnly;
        }

        protected new AlgorithmValuesRequestCon AlgorithmValuesCreate(Algorithm algorithm, Formula formula)
        {
            AlgorithmValuesRequestCon values = new AlgorithmValuesRequestCon(algorithm, formula, this);
            myvdbm.Collection.Add(values);
            //myvaluesstorage.UpdateItem(values);
            return values;
        }
        private void AlgorithmValuesPlus()
        {
            AlgorithmValuesRequest[] valuess = new AlgorithmValuesRequest[] {
            new AlgorithmValuesRequestCon(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "X1", "", 200, "П12/П9", null), this),
            new AlgorithmValuesRequestCon(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "X2", "", 200, "П17/П9", null), this),
            new AlgorithmValuesRequestCon(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "X3", "", 200, "П18/П9", null), this),
            new AlgorithmValuesRequestCon(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "X4", "", 200, "П19/П9", null), this),
            new AlgorithmValuesRequestCon(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "X5", "", 200, "П20/П9", null), this),
            //new AlgorithmValuesRequestCon(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "P2", "", 200, "П23+П22+П24+П25+П27+П28+П29", null), myrequest),
            //new AlgorithmValuesRequestCon(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "P3", "", 200, "П23+П22+П24+П35+П25+П27+П28+П29", null), myrequest)
            };
            foreach(AlgorithmValuesRequest item in valuess)
                item.FormulaInit();
        }
        protected void Request_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Request request = sender as Request; // для прослушивания всех request алгоритма
            switch (e.PropertyName)
            {
                case "CellNumber":
                    if (!string.IsNullOrEmpty(request.Consolidate))
                    {
                        mygwdbm.Execute();
                        this.CellNumber = (mygwdbm.CellNumber ?? 0M);
                    }
                    break;
                //case "ConCorr":
                //    if (!string.IsNullOrEmpty(request.Consolidate))
                //        foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                //            if ((request.ServiceType == "ТЭО" && item.Formula.Code == "П30") || (request.ServiceType == "ТД" && item.Formula.Code == "П39"))
                //            {
                //                if (!(item.Value1 == request.ConCorr)) // null !
                //                    item.Value1 = request.ConCorr;
                //                break;
                //            }
                //    break;
                //case "ConCustomsPay":
                //    if (!string.IsNullOrEmpty(request.Consolidate))
                //        foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                //            if (item.Formula.Code == "П12") // null !
                //            {
                //                if (!(item.Value1 == request.ConCustomsPay))
                //                    item.Value1 = request.ConCustomsPay;
                //                break;
                //            }
                //    break;
                case "Consolidate":
                        this.Group = request.Consolidate;
                        this.LoadData();
                    break;
                case "Invoice":
                    if (!string.IsNullOrEmpty(request.Consolidate))
                    {
                        mygwdbm.Execute();
                        this.Invoice = (mygwdbm.Invoice ?? 0M);
                    }
                    break;
                case "InvoiceDiscount":
                    if (!string.IsNullOrEmpty(request.Consolidate))
                        foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                            if (item.Formula.Code == "П9")
                            {
                                mygwdbm.Execute();
                                item.Value1Templ = (mygwdbm.InvoiceDiscount ?? 0M); // менять только в алгоритме
                                break;
                            }
                    break;
                case "OfficialWeight":
                    if (!string.IsNullOrEmpty(request.Consolidate))
                        foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                            if (item.Formula.Code == "П10")
                            {
                                mygwdbm.Execute();
                                item.Value1Templ = (mygwdbm.Weight ?? 0M);
                                break;
                            }
                    break;
                case "ServiceType":
                    if (!string.IsNullOrEmpty(request.Consolidate))
                    {
                        this.ServiceType = request.ServiceType;
                        this.UpdateServiceType(request);
                        foreach (Request req in this.Requests) // don't turn off event e.t. all request algorithms must updated
                            req.ServiceType = request.ServiceType;
                    }
                    break;
                case "Volume":
                    if (!string.IsNullOrEmpty(request.Consolidate))
                        mygwdbm.Execute();
                    this.Volume = (mygwdbm.Volume ?? 0M);
                    break;
            }
        }
        private void LoadData()
        {
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            // удаление значений и ссылок на Request
            if (myvdbm.Collection?.Count > 0)
                foreach (AlgorithmValuesRequest vals in myvdbm.Collection)
                {
                    vals.Dispose();
                }
            // определение веса
            mygwdbm.Execute();
            if (mygwdbm.Errors.Count > 0)
                err.AppendLine(mygwdbm.ErrorMessage);
            else
            {
                this.Invoice = (mygwdbm.Invoice ?? 0M);
                this.Volume = (mygwdbm.Volume ?? 0M);
                this.CellNumber = (mygwdbm.CellNumber ?? 0M);
            }
            if (myrequest.Status.Id < 500)
            {
                // получаем алгоритм
                mywdbm.Weight = (mygwdbm.Weight ?? 0M);
                if (mywdbm.Errors.Count > 0) err.AppendLine(mywdbm.ErrorMessage);
                myadbm.Errors.Clear();
                myadbm.ItemId = mywdbm.AlgorithmId;
                myalgorithm = myadbm.GetFirst();
                if (myadbm.Errors.Count > 0) err.AppendLine(myadbm.ErrorMessage);
                // загружаем функции
                myfdbm.Errors.Clear();
                myfdbm.Fill();
                if (myfdbm.Errors.Count > 0) err.AppendLine(myfdbm.ErrorMessage);
                myformulasynchronizer.DomainCollection = myfdbm.Collection;
            }
            else
            {
                myalgorithm = myalgdbm.GetFirst();
                if (myformulasynchronizer.DomainCollection == null)
                    myformulasynchronizer.DomainCollection = new ObservableCollection<Formula>();
                else
                    myformulasynchronizer.DomainCollection?.Clear();
            }
            myalgorithms.Clear();
            if (myalgorithm != null) myalgorithms.Add(myalgorithm);
            this.PropertyChangedNotification("Algorithms");
            //Загружаем значения
            myvdbm.Formulas = myformulasynchronizer.DomainCollection;
            myvdbm.Errors.Clear();
            myvdbm.Fill();
            if (myvdbm.Errors.Count > 0) err.AppendLine(myvdbm.ErrorMessage);
            // формируем коллекцию для отображения
            myalgorithmformulas.Clear();
            AlgorithmValuesRequest values = null;
            foreach (FormulaVM frm in myformulasynchronizer.ViewModelCollection)
            {
                AlgorithmFormula algfrm = new AlgorithmFormula(frm, lib.DomainObjectState.Unchanged);
                myalgorithmformulas.Add(algfrm);
                values = null;
                foreach (AlgorithmValuesRequest vals in myvdbm.Collection)
                {
                    if (vals.Formula == frm.DomainObject)
                    {
                        values = vals;
                        break;
                    }
                }
                if (values == null) values = AlgorithmValuesCreate(myalgorithm, frm.DomainObject);
                if (myrequest.Status.Id < 500)
                {
                    switch(frm.Code)
                    {
                        //default:
                        //    values.FormulaInit();
                        //    break;
                        case "П9":
                           values.Value1Templ = (mygwdbm.InvoiceDiscount ?? 0M);
                           break;
                        case "П10":
                            values.Value1Templ = (mygwdbm.Weight ?? 0M);
                            break;
                        case "П11":
                            if (this.ServiceType == "ТД")
                                values.Value1Templ = 3;
                            else
                                values.Value1Templ = null;
                            break;
                    }
                    values.FormulaInit();
                }
                algfrm.Algorithms.Add(new AlgorithmValuesRequestVM(values));
            }
            AlgorithmValuesPlus();
            #region Fields Update not dependent from ServiceType
            //this.Pay = this.Algorithm.Formulas.FirstOrDefault((AlgorithmValues v) => { });
            #endregion

            myview1.MoveCurrentToPosition(-1);
            myview2.MoveCurrentToPosition(-1);
            if (err.Length > 0)
                this.PopupText = err.ToString();
            else
            {
                this.Save.Execute(null);
                foreach (Request request in this.Requests)
                    if (request.AlgorithmCMD != null) request.AlgorithmCMD.Save.Execute(null);
            }
        }
        internal bool RequestAttached(Request request)
        {
            if (request.Consolidate != this.Group || request.ParcelId != myparcel.Id) throw new Exception("Присоединение заявки к чужой консолидации.\nЗаявка " + request.Id + " консолидация " + mygroup + " перевозка " + myparcel.Id);
            if (myrequests.Contains(request)) return true;
            if (myrequests.Count == 0)
            {
                this.ServiceType = request.ServiceType;
                this.UpdateServiceType(request);
            }
            else if (this.ServiceType != request.ServiceType)
            {
                this.OpenPopup("Обнаружено не совпадение Услуги в заявке " + request.StorePointDate + " и группе консолидации \"" + mygroup + "\"!\nПеревозка " + myparcel.ParcelNumberEntire, true);
            }

            myrequests.Add(request);
            request.PropertyChanged += Request_PropertyChanged;
            return true;
        }
        private void UpdateServiceType(Request request)
        {
            this.ServiceType = request.ServiceType;
            foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                switch (item.Formula.Code)
                {
                    case "П11":
                        if (request.ServiceType == "ТД")
                            item.Value1Templ = 3;
                        else
                            item.Value1Templ = null;
                        break;
                    case "П21":
                        if (request.ServiceType == "ТЭО")
                        {
                            this.Cost = item.Value1;
                            if (item.Value2.HasValue)
                                this.CostPer = decimal.Divide(item.Value2.Value, 100M);
                            else
                                this.CostPer = item.Value2;
                        }
                        else
                        {
                            this.Cost = null;
                            this.CostPer = null;
                        }
                        break;
                    case "П30":
                        if (request.ServiceType == "ТЭО")
                        {
                            this.Corr = item.Value1;
                            if (item.Value2.HasValue)
                                this.CorrPer = decimal.Divide(item.Value2.Value, 100M);
                            else
                                this.CorrPer = item.Value2;
                        }
                        break;
                    case "П31":
                        if (request.ServiceType == "ТЭО")
                        {
                            this.Pay = item.Value1;

                            if (item.Value2.HasValue)
                                this.PayPer = decimal.Divide(item.Value2.Value, 100M);
                            else
                                this.PayPer = item.Value2;
                            if (!string.IsNullOrEmpty(request.Consolidate) & item.Value2.HasValue & request.AlgorithmCMD != null)
                            {
                                decimal? p40 = null;
                                foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                {
                                    if (values.Formula.Code == "П31")
                                    {
                                        if (values.Value1.HasValue & values.Value2.HasValue)
                                            p40 = values.Value1.Value * decimal.Divide(item.Value2.Value, values.Value2.Value) - values.Value1.Value;
                                        break;
                                    }
                                }
                                if (p40.HasValue)
                                    foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                    {
                                        if (values.Formula.Code == "П30")
                                        {
                                            values.Value1Templ = (values.Value1Templ ?? 0M) + p40;
                                            break;
                                        }
                                    }
                            }
                        }
                        break;
                    case "П39":
                        if (request.ServiceType == "ТД")
                        {
                            this.Corr = item.Value1;
                            if (item.Value2.HasValue)
                                this.CorrPer = decimal.Divide(item.Value2.Value, 100M);
                            else
                                this.CorrPer = item.Value2;
                        }
                        break;
                    case "П40":
                        if (request.ServiceType == "ТД")
                        {
                            this.Pay = item.Value1;

                            if (item.Value2.HasValue)
                                this.PayPer = decimal.Divide(item.Value2.Value, 100M);
                            else
                                this.PayPer = item.Value2;
                            if (!string.IsNullOrEmpty(request.Consolidate) & item.Value1.HasValue & item.Value2.HasValue & request.AlgorithmCMD != null)
                            {
                                decimal? p40 = null;
                                foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                {
                                    if (values.Formula.Code == "П40")
                                    {
                                        if (values.Value1.HasValue & values.Value2.HasValue)
                                            p40 = values.Value1.Value * decimal.Divide(item.Value2.Value, values.Value2.Value) - values.Value1.Value;
                                        break;
                                    }
                                }
                                if (p40.HasValue)
                                    foreach (AlgorithmValuesRequest values in request.AlgorithmCMD.Algorithm.Formulas)
                                    {
                                        if (values.Formula.Code == "П39")
                                        {
                                            values.Value1Templ = (values.Value1Templ ?? 0M) + p40;
                                            break;
                                        }
                                    }
                            }
                        }
                        break;
                }
        }
    }

    internal class AlgorithmFormulaRequestConCommandStore // проблемы с синхронизацией c изменениями в Request при отвязке от Request
    {
        internal AlgorithmFormulaRequestConCommandStore()
        { mycollection = new SortedDictionary<AlgorithmRequestCommandId, AlgorithmConsolidateCommand>(); }

        private System.Collections.Generic.SortedDictionary<AlgorithmRequestCommandId, AlgorithmConsolidateCommand> mycollection;

        internal void Clear()
        { mycollection.Clear(); }
        internal AlgorithmConsolidateCommand GetItem(AlgorithmRequestCommandId id)
        {
            AlgorithmConsolidateCommand firstitem;
            mycollection.TryGetValue(id, out firstitem);
            return firstitem;
        }
        internal AlgorithmConsolidateCommand GetItemLoad(AlgorithmRequestCommandId id)
        {
            AlgorithmConsolidateCommand firstitem = this.GetItem(id);
            if (firstitem == default(AlgorithmConsolidateCommand))
            {
                Parcel parcel = CustomBrokerWpf.References.ParcelStore.GetItemLoad(id.ObjectId,out var errors);
                firstitem = new AlgorithmConsolidateCommand(parcel, id.GroupId);
            }
            return firstitem;
        }
    }
    internal class AlgorithmRequestCommandId : System.IComparable<AlgorithmRequestCommandId>
    {
        internal AlgorithmRequestCommandId(int objectid, string groupid)
        { this.ObjectId = objectid; this.GroupId = groupid; }
        internal int ObjectId { get; }
        internal string GroupId { get; }

        public int CompareTo(AlgorithmRequestCommandId other)
        {
            if (other == null) return 1;
            return this.ObjectId.CompareTo(other.ObjectId) + this.ObjectId.CompareTo(other.ObjectId) + this.GroupId.CompareTo(other.GroupId);
        }
    }
}
