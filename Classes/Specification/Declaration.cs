using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using lib = KirillPolyanskiy.DataModelClassLibrary;
namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class Declaration : lib.DomainBaseStamp
    {
        public Declaration(int id, long stamp, lib.DomainObjectState mstate
            , decimal? cbrate, decimal? fee, string number,DateTime? spddate, decimal? tax, decimal? totalsum, decimal? vat
            ) : base(id, stamp, null, null, mstate)
        {
            mycbrate = cbrate;
            myfee = fee;
            mynumber = number;
            mytax = tax;
            mytotalsum = totalsum;
            myspddate = spddate;
            myvat = vat;
        }
        public Declaration() : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added
            , null, null, null, null,null,null,null
            ) { }

        private decimal? mycbrate;
        public decimal? CBRate
        {
            set { SetProperty<decimal?>(ref mycbrate, value);}
            get { return mycbrate; }
        }
        private decimal? myfee;
        public decimal? Fee
        {
            set { SetProperty<decimal?>(ref myfee, value); }
            get { return myfee; }
        }
        private string mynumber;
        public string Number
        { set { SetProperty<string>(ref mynumber, value); } get { return mynumber; } }
        private decimal? mytax;
        public decimal? Tax
        {
            set { SetProperty<decimal?>(ref mytax, value); }
            get { return mytax; }
        }
        private decimal? mytotalsum;
        public decimal? TotalSum
        {
            set { SetProperty<decimal?>(ref mytotalsum, value); }
            get { return mytotalsum; }
        }
        private DateTime? myspddate;
        public DateTime? SPDDate
        { set { SetProperty<DateTime?>(ref myspddate, value); } get { return myspddate; } }
        private decimal? myvat;
        public decimal? VAT
        {
            set { SetProperty<decimal?>(ref myvat, value); }
            get { return myvat; }
        }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Declaration templ= sample as Declaration;
            this.CBRate = templ.CBRate;
            this.Fee = templ.Fee;
            this.Number = templ.Number;
            this.Tax = templ.Tax;
            this.TotalSum = templ.TotalSum;
            this.SPDDate = templ.SPDDate;
            this.VAT = templ.VAT;
        }
        protected override void RejectProperty(string property, object value)
        {
        }

        internal string LoadDeclaration(string filepath)
        {
            string errmsg = string.Empty;
            decimal invoice=0,rate=0;
            try
            {
                XElement rateelement = null, CUMainContractTerms = null;
                XDocument xdoc = XDocument.Load(filepath);
                XElement element = xdoc.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("ED_Container")); // из за меняющегося пространства имен
                if(element != null)
                    element = element.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("ContainerDoc"));
                if (element != null)
                    element = element.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("DocBody"));
                if (element != null)
                    element = element.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("ESADout_CU"));
                if (element != null)
                    element = element.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("ESADout_CUGoodsShipment"));
                if (element != null)
                {
                    CUMainContractTerms = element.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("ESADout_CUMainContractTerms"));
                    if (CUMainContractTerms != null)
                    {
                        rateelement = CUMainContractTerms.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("ContractCurrencyRate"));
                        CUMainContractTerms = CUMainContractTerms.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("TotalInvoiceAmount"));
                        if (rateelement != null)
                        {
                            decimal.TryParse(rateelement.Value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), out rate);
                            this.CBRate = rate;
                        }
                        if (CUMainContractTerms != null)
                        {
                            decimal.TryParse(CUMainContractTerms.Value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), out invoice);
                            this.TotalSum = invoice;
                        }
                    }
                    element = element.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("ESADout_CUPayments"));
                    if (element != null)
                        foreach(XElement CUCustomsPayment in element.Elements().Where(e => e.Name.LocalName.Equals("ESADout_CUCustomsPayment")))
                            switch(CUCustomsPayment.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("PaymentModeCode"))?.Value)
                            {
                                case "1010":
                                    decimal.TryParse(CUCustomsPayment.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("PaymentAmount"))?.Value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), out invoice);
                                    this.Fee = invoice;
                                    break;
                                case "2010":
                                    decimal.TryParse(CUCustomsPayment.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("PaymentAmount"))?.Value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), out invoice);
                                    this.Tax = invoice;
                                    break;
                                case "5010":
                                    decimal.TryParse(CUCustomsPayment.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("PaymentAmount"))?.Value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), out invoice);
                                    this.VAT = invoice;
                                    break;
                            }
                }
                XComment comment = xdoc.Nodes().FirstOrDefault(e => e.NodeType == System.Xml.XmlNodeType.Comment) as XComment;
                this.Number = comment?.Value.Substring(3);
            }
            catch(Exception ex) { errmsg = ex.Message; }
            return errmsg;
        }
    }

    internal class DeclarationDBM : lib.DBManagerStamp<Declaration>
    {
        internal DeclarationDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "spec.CustomDeclaration_sp";
            InsertCommandText = "spec.CustomDeclarationAdd_sp";
            UpdateCommandText = "spec.CustomDeclarationUpd_sp";
            DeleteCommandText = "spec.CustomDeclarationDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int) };
            UpdateParams = new SqlParameter[] {UpdateParams[0]
                ,new SqlParameter("@cbrateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@feeupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@numberupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@spddateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@taxupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@totalsumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@vatupd", System.Data.SqlDbType.Bit)
            };
            InsertUpdateParams = new SqlParameter[] {InsertUpdateParams[0]
                ,new SqlParameter("@cbrate", System.Data.SqlDbType.Money)
                ,new SqlParameter("@fee", System.Data.SqlDbType.Money)
                ,new SqlParameter("@number", System.Data.SqlDbType.NVarChar,25)
                ,new SqlParameter("@spddate", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@tax", System.Data.SqlDbType.Money)
                ,new SqlParameter("@totalsum", System.Data.SqlDbType.Money)
                ,new SqlParameter("@vat", System.Data.SqlDbType.Money)
            };
        }

        protected override Declaration CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new Declaration(reader.GetInt32(0), reader.GetInt64(reader.GetOrdinal("stamp")), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("cbrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("cbrate"))
                , reader.IsDBNull(reader.GetOrdinal("fee")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("fee"))
                , reader.IsDBNull(reader.GetOrdinal("number")) ? null : reader.GetString(reader.GetOrdinal("number"))
                , reader.IsDBNull(reader.GetOrdinal("spddate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("spddate"))
                , reader.IsDBNull(reader.GetOrdinal("tax")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("tax"))
                , reader.IsDBNull(reader.GetOrdinal("totalsum")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("totalsum"))
                , reader.IsDBNull(reader.GetOrdinal("vat")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("vat")));
        }
        protected override void GetOutputSpecificParametersValue(Declaration item)
        {
        }
        protected override bool LoadObjects()
        {
            return true;
        }
        protected override void LoadObjects(Declaration item)
        {
        }
        protected override bool SaveChildObjects(Declaration item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Declaration item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue()
        {
        }
        protected override bool SetSpecificParametersValue(Declaration item)
        {
            foreach (SqlParameter par in UpdateParams)
                switch(par.ParameterName)
                {
                    case "@cbrateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.CBRate));
                        break;
                    case "@feeupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Fee));
                        break;
                    case "@numberupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Number));
                        break;
                    case "@spddateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.SPDDate));
                        break;
                    case "@taxupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Declaration.Tax));
                        break;
                    case "@totalsumupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.TotalSum));
                        break;
                    case "@vatupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Declaration.VAT));
                        break;
                }
            foreach (SqlParameter par in InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@cbrate":
                        par.Value = item.CBRate;
                        break;
                    case "@fee":
                        par.Value = item.Fee;
                        break;
                    case "@number":
                        par.Value = item.Number;
                        break;
                    case "@spddate":
                        par.Value = item.SPDDate;
                        break;
                    case "@tax":
                        par.Value = item.Tax;
                        break;
                    case "@totalsum":
                        par.Value = item.TotalSum;
                        break;
                    case "@vat":
                        par.Value = item.VAT;
                        break;
                }
            return true;
        }
    }

    public class DeclarationVM : lib.ViewModelErrorNotifyItem<Declaration>
    {
        public DeclarationVM(Declaration model):base(model)
        {

        }

        public decimal? Fee
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Fee.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Fee.Value, value.Value))))
                {
                    string name = nameof(this.Fee);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Fee);
                    ChangingDomainProperty = name; this.DomainObject.Fee = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Fee : null; }
        }
        public decimal? Tax
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Tax.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Tax.Value, value.Value))))
                {
                    string name = nameof(this.Tax);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Tax);
                    ChangingDomainProperty = name; this.DomainObject.Tax = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Tax : null; }
        }
        public decimal? VAT
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.VAT.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.VAT.Value, value.Value))))
                {
                    string name = nameof(this.VAT);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.VAT);
                    ChangingDomainProperty = name; this.DomainObject.VAT = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.VAT : null; }
        }
        
        protected override bool DirtyCheckProperty()
        {
            throw new NotImplementedException();
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            throw new NotImplementedException();
        }

        protected override void InitProperties()
        {
            throw new NotImplementedException();
        }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }

        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            throw new NotImplementedException();
        }
    }
}
