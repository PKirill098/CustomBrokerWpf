using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Collections.ObjectModel;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Algorithm
{
    public class Algorithm : lib.DomainBaseReject
    {
        public Algorithm(int id, lib.DomainObjectState state, string name, byte index) : base(id, state)
        {
            myname = name;
            myindex = index;
            myformulas = new ObservableCollection<AlgorithmValues>();
        }
        public Algorithm() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added, null,0) { }

        private string myname;
        public string Name
        { set { SetProperty<string>(ref myname, value); } get { return myname; } }
        private byte myindex;
        public byte Index
        { set { SetProperty<byte>(ref myindex, value); } get { return myindex; } }
        private List<AlgorithmFuncValue> mylistfunc;
        internal List<AlgorithmFuncValue> ListFunc { set { mylistfunc = value; } get { return mylistfunc; } }

        private ObservableCollection<AlgorithmValues> myformulas;
        internal ObservableCollection<AlgorithmValues> Formulas
        {
            get
            {
                return myformulas;
            }
        }

        internal void FormulasInit()
        {
            foreach(AlgorithmValues item in myformulas.OrderBy(v=>v.Formula.Code))
            {
                item.FormulaInit();
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            Algorithm newitem = (Algorithm)sample;
            this.Name = newitem.Name;
        }
        protected override void RejectProperty(string property, object value)
        {
            if(property== "Name") myname = (string)value;
        }
    }

    public class AlgorithmDBM : lib.DBManagerId<Algorithm,Algorithm>
    {
        public AlgorithmDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;
            SelectCommandText = "[dbo].[Algorithm_sp]";
            InsertCommandText = "[dbo].[AlgorithmAdd_sp]";
            UpdateCommandText = "[dbo].[AlgorithmUpd_sp]";
            DeleteCommandText = "[dbo].[AlgorithmDel_sp]";

            SqlParameter paridout = new SqlParameter("@param1", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@param1", System.Data.SqlDbType.Int);

            SelectParams = new SqlParameter[] { new SqlParameter("@param1", System.Data.SqlDbType.Int) };
            myinsertparams = new SqlParameter[]
            {
                paridout
            };
            myupdateparams = new SqlParameter[]
            {
                parid
            };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@param2", System.Data.SqlDbType.NVarChar,20),
                new SqlParameter("@index", System.Data.SqlDbType.TinyInt)
            };
            mydeleteparams = new SqlParameter[]
            {
                parid
            };
        }

		protected override Algorithm CreateRecord(SqlDataReader reader)
		{
            return new Algorithm(reader.GetInt32(0), lib.DomainObjectState.Unchanged, reader.GetString(1), reader.GetByte(2));
		}
        protected override Algorithm CreateModel(Algorithm reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
			return reader;
        }
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader), addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
        protected override void GetOutputParametersValue(Algorithm item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
            }
        }
        protected override void ItemAcceptChanches(Algorithm item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(Algorithm item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Algorithm item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(Algorithm item)
        {
            myupdateparams[0].Value = item.Id;
            myinsertupdateparams[0].Value = item.Name;
            myinsertupdateparams[1].Value = item.Index;
            return true;
        }
        protected override void SetSelectParametersValue()
        {
        }
    }

    public class AlgorithmWeightDBM : lib.DBMExec
    {
        internal AlgorithmWeightDBM():base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            SelectCommandText = "[dbo].[AlgorithmWeight_sp]";
            SelectParams = new SqlParameter[] { new SqlParameter("@param1", System.Data.SqlDbType.Money), new SqlParameter("@param2", System.Data.SqlDbType.Int) };
            SelectParams[1].Direction = System.Data.ParameterDirection.Output;
        }

        private decimal? myweight;
        internal decimal? Weight
        {
            set { myweight = value; base.Execute(); }
            get { return myweight; }
        }
        public int? AlgorithmId
        { get { return DBNull.Value == this.SelectParams[1].Value ? (int?)null : (int)this.SelectParams[1].Value; } }

        protected override void PrepareFill()
        {
            this.SelectParams[0].Value= myweight;
        }
    }

    internal class AlgorithmFuncValue
    {
        internal AlgorithmFuncValue():this(string.Empty,(string eer) => { return 0M; })
        {

        }
        internal AlgorithmFuncValue(string name, Func<string,decimal> func)
        {
            myformula = name;
            myfunc = func;
        }
        private string myformula;
        internal string Name
        { get { return myformula; } }
        private Func<string,decimal> myfunc;
        internal Func<string,decimal> FuncValue
        {
            set
            {
                myfunc = value;
            }
            get { return myfunc; }
        }
    }
}
