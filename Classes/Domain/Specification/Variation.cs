using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class Variation:lib.DomainBaseNotifyChanged
    {
        public Variation(int id,lib.DomainObjectState state
            ,string colormark, string singular,string plural) :base(id, state)
        {
            mycolormark = colormark;
            myplural = plural;
            mysingular = singular;
        }
        public Variation() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added, "#FFFFFFFF", string.Empty, string.Empty) { }

        private string mycolormark;
        public string ColorMark
        {
            set
            {
                base.SetProperty<string>(ref mycolormark, value);
            }
            get { return mycolormark; }
        }
        private string myplural;
        public string Plural
        { set { SetProperty<string>(ref myplural, value); } get { return myplural; } }
        private string mysingular;
        public string Singular
        { set { SetProperty<string>(ref mysingular, value); } get { return mysingular; } }

        public override bool ValidateProperty(string propertyname, object value, out string errmsg, out byte messagekey)
        {
            bool isvalid = true;
            errmsg = null;
            messagekey = 0;
            switch (propertyname)
            {
                case nameof(this.Plural):
                    if (string.IsNullOrEmpty((string)value))
                    {
                        errmsg = "Множественное число не может быть пустым! ";
                        isvalid = false;
                    }
                    break;
                case nameof(this.Singular):
                    if (string.IsNullOrEmpty((string)value))
                    {
                        errmsg = "Единственное число не может быть пустым! ";
                        isvalid = false;
                    }
                    break;
            }
            return isvalid;
        }
    }

    public class VariationDBM : lib.DBManagerId<Variation>
    {
        internal VariationDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            this.NeedAddConnection = false;
            this.SelectProcedure = true;
            this.SelectCommandText = "[spec].[Variation_sp]";
            this.SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int), new SqlParameter("@plural", System.Data.SqlDbType.NVarChar, 50), new SqlParameter("@filter", System.Data.SqlDbType.Int) };
            this.InsertProcedure = true;
            this.InsertCommandText = "[spec].[VariationAdd_sp]";
            this.InsertParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int) { Direction=System.Data.ParameterDirection.Output} };
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int);
            this.UpdateProcedure = false;
            this.UpdateCommandText = "UPDATE spec.Variation_tb SET plural=IIF(@pluralupd=1,@plural,plural),singular=IIF(@singularupd=1,@singular,singular),color=IIF(@colorupd=1,@color,color) WHERE id=@id";
            this.UpdateParams = new SqlParameter[] { id, new SqlParameter("@pluralupd", System.Data.SqlDbType.Bit), new SqlParameter("@singularupd", System.Data.SqlDbType.Bit), new SqlParameter("@colorupd", System.Data.SqlDbType.Bit) };
            this.InsertUpdateParams = new SqlParameter[] { new SqlParameter("@plural", System.Data.SqlDbType.NVarChar,50), new SqlParameter("@singular", System.Data.SqlDbType.NVarChar, 50), new SqlParameter("@color", System.Data.SqlDbType.NChar, 9) };
            this.DeleteProcedure = false;
            this.DeleteCommandText = "DELETE FROM spec.Variation_tb WHERE id=@id";
            this.DeleteParams = new SqlParameter[] { id };
        }

        private int? myid;
        private string myplural;
        public string Plural
        {
            get { return myplural; }
            set { myplural = value; }
        }
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { set { myfilter = value; } get { return myfilter; } }

        protected override void CancelLoad()
        {
        }
        protected override Variation CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            return new Variation(reader.GetInt32(0),lib.DomainObjectState.Unchanged
                ,reader.GetString(this.Fields["color"]), reader.GetString(this.Fields["singular"]), reader.GetString(this.Fields["plural"]));
        }
        protected override void GetOutputParametersValue(Variation item)
        {
            if(item.DomainState==lib.DomainObjectState.Added)
                item.Id=(int)this.InsertParams[0].Value;
        }
        protected override void ItemAcceptChanches(Variation item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(Variation item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Variation item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(Variation item)
        {
            foreach (SqlParameter par in this.InsertParams)
            {
                switch (par.ParameterName)
                {
                    case "@pluralupd":
                        par.Value = true;
                        break;
                    case "@singularupd":
                        par.Value = true;
                        break;
                    case "@colorupd":
                        par.Value = true;
                        break;
                }
            }
            foreach (SqlParameter par in this.UpdateParams)
            {
                switch (par.ParameterName)
                {
                    case "@id":
                        par.Value = item.Id;
                        break;
                    case "@pluralupd":
                        par.Value = true;
                        break;
                    case "@singularupd":
                        par.Value = true;
                        break;
                    case "@colorupd":
                        par.Value = true;
                        break;
                }
            }
            foreach (SqlParameter par in this.InsertUpdateParams)
            {
                switch (par.ParameterName)
                {
                    case "@plural":
                        par.Value = item.Plural;
                        break;
                    case "@singular":
                        par.Value = item.Singular;
                        break;
                    case "@color":
                        par.Value = item.ColorMark;
                        break;
                }
            }
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[1].Value = myplural;
            this.SelectParams[2].Value = myfilter?.FilterWhereId;
        }
    }

    public class VariationVM : lib.ViewModelErrorNotifyItem<Variation>
    {
        public VariationVM(Variation model):base(model)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.Plural), nameof(this.Singular) });
            DeleteRefreshProperties.AddRange(new string[] { nameof(this.Plural), nameof(this.Singular) });
            InitProperties();
        }
        public VariationVM():this(new Variation()) { }

        private string myplural;
        public string Plural
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(myplural, value)))
                {
                    string name = nameof(this.Plural);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myplural);
                    myplural = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Plural = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? myplural : null; }
        }
        private string mysingular;
        public string Singular
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(mysingular, value)))
                {
                    string name = nameof(this.Singular);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mysingular);
                    mysingular = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Singular = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? mysingular : null; }
        }
        public string ColorMark
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.ColorMark, value)))
                {
                    string name = nameof(this.ColorMark);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ColorMark);
                    ChangingDomainProperty = name; this.DomainObject.ColorMark = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ColorMark : System.Windows.Media.Brushes.Transparent.ToString(); }
        }

        protected override bool DirtyCheckProperty()
        {
            return myplural != this.DomainObject.Plural || mysingular != this.DomainObject.Singular;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(this.Plural):
                    myplural = this.DomainObject.Plural;
                    break;
                case nameof(this.Singular):
                    mysingular = this.DomainObject.Singular;
                    break;
            }
        }
        protected override void InitProperties()
        {
            myplural = this.DomainObject.Plural;
            mysingular = this.DomainObject.Singular;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.ColorMark):
                    this.DomainObject.ColorMark = (string)value;
                    break;
                case nameof(this.Plural):
                    if (myplural != this.DomainObject.Plural)
                        myplural = this.DomainObject.Plural;
                    else
                        this.Plural = (string)value;
                    break;
                case nameof(this.Singular):
                    if (mysingular != this.DomainObject.Singular)
                        mysingular = this.DomainObject.Singular;
                    else
                        this.Singular = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            byte errcode = 0;
            switch (propertyname)
            {
                case nameof(this.Plural):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Plural, out errmsg, out errcode);
                    break;
                case nameof(this.Singular):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Singular, out errmsg, out errcode);
                    break;
            }
            if (isvalid)
                ClearErrorMessageForProperty(propertyname);
            else if (inform) AddErrorMessageForProperty(propertyname, errmsg, errcode);
            return isvalid;
        }
    }

    internal class VariationSynchronizer : lib.ModelViewCollectionsSynchronizer<Variation, VariationVM>
    {
        protected override Variation UnWrap(VariationVM wrap)
        {
            return wrap.DomainObject as Variation;
        }
        protected override VariationVM Wrap(Variation fill)
        {
            return new VariationVM(fill);
        }
    }

    public class VariationCommander : lib.ViewModelViewCommand
    {
        internal VariationCommander()
        {
            #region Filter
            myfilter = new lib.SQLFilter.SQLFilter("Variation", "AND", CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);

            mypluralfilter = new VariationPluralFilter();
            mypluralfilter.DeferredFill = true;
            mypluralfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mypluralfilter.ExecCommand2 = () => { mypluralfilter.Clear(); };
            mysingularfilter = new VariationSingularFilter();
            mysingularfilter.DeferredFill = true;
            mysingularfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mysingularfilter.ExecCommand2 = () => { mysingularfilter.Clear(); };

            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            #endregion

            mymaindbm = new VariationDBM();
            mydbm = mymaindbm;
            mymaindbm.FillAsyncCompleted = () => {
                if (mydbm.Errors.Count > 0)
                    OpenPopup(mydbm.ErrorMessage, true);
            };
            mymaindbm.Collection = new System.Collections.ObjectModel.ObservableCollection<Variation>();
            mymaindbm.Filter = myfilter;
            mymaindbm.FillAsync();
            mysync = new VariationSynchronizer();
            mysync.DomainCollection = mymaindbm.Collection;
            base.Collection = mysync.ViewModelCollection;
        }

        VariationDBM mymaindbm;
        VariationSynchronizer mysync;
        private System.Threading.Tasks.Task myrefreshtask;

        #region Filter
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { get { return myfilter; } }
        private VariationPluralFilter mypluralfilter;
        public VariationPluralFilter PluralFilter
        { get { return mypluralfilter; } }
        private VariationSingularFilter mysingularfilter;
        public VariationSingularFilter SingularFilter
        { get { return mysingularfilter; } }

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            this.EndEdit();
            FilterActualise();
            RefreshData(null);
        }
        private bool FilterRunCanExec(object parametr)
        { return true; }
        private void FilterActualise()
        {
            if (mypluralfilter.FilterOn)
            {
                string[] items = new string[mypluralfilter.SelectedItems.Count];
                for (int i = 0; i < mypluralfilter.SelectedItems.Count; i++)
                    items[i] = (string)mypluralfilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "plural", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "plural", new string[0]);
            if (mysingularfilter.FilterOn)
            {
                string[] items = new string[mysingularfilter.SelectedItems.Count];
                for (int i = 0; i < mysingularfilter.SelectedItems.Count; i++)
                    items[i] = (string)mysingularfilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "singular", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "singular", new string[0]);
        }

        private RelayCommand myfilterclear;
        public ICommand FilterClear
        {
            get { return myfilterclear; }
        }
        private void FilterClearExec(object parametr)
        {
            mypluralfilter.Clear();
            mypluralfilter.IconVisibileChangedNotification();
            mysingularfilter.Clear();
            mysingularfilter.IconVisibileChangedNotification();
            this.EndEdit();
            FilterActualise();
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }
        #endregion

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
            return (myrefreshtask == null || myrefreshtask.IsCompleted);
        }
        protected override bool CanRejectChanges()
        {
            return true;
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
            myrefreshtask = mydbm.FillAsync();
        }
        protected override void SettingView()
        {
            mypluralfilter.ItemsSource = myview.OfType<VariationVM>();
            mysingularfilter.ItemsSource = myview.OfType<VariationVM>();
        }
    }

    public class VariationPluralFilter : libui.CheckListBoxVMFill<VariationVM, string>
    {
        protected override void AddItem(VariationVM item)
        {
            if (!Items.Contains(item.Plural)) Items.Add(item.Plural);
        }
    }
    public class VariationSingularFilter : libui.CheckListBoxVMFill<VariationVM, string>
    {
        protected override void AddItem(VariationVM item)
        {
            if (!Items.Contains(item.Singular)) Items.Add(item.Singular);
        }
    }
}
