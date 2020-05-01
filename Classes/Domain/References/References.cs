using KirillPolyanskiy.CustomBrokerWpf.Domain.References;
using System;
using System.Data.SqlClient;
using System.Windows;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    static internal class References
    {
        internal static void Init()
        {
            SqlConnection con;
            try
            {
                con = new SqlConnection(CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString1 + ";User ID=farlogin;Password=df(*&G$WXPOIN6S87g786rayo56358G65R(G6%#2ES^o8;Connect Timeout=1");
                con.Open();
                con.Close();
                myconnectionstring = CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString1 + ";User ID=farlogin;Password=df(*&G$WXPOIN6S87g786rayo56358G65R(G6%#2ES^o8";
            }
            catch
            {
                try
                {
                    con = new SqlConnection(CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString2 + ";User ID=farlogin;Password=df(*&G$WXPOIN6S87g786rayo56358G65R(G6%#2ES^o8;Connect Timeout=1");
                    con.Open();
                    con.Close();
                    myconnectionstring = CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString2 + ";User ID=farlogin;Password=df(*&G$WXPOIN6S87g786rayo56358G65R(G6%#2ES^o8";
                }
                catch
                {
                    try
                    {
                        con = new SqlConnection(CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString3 + ";User ID=farlogin;Password=df(*&G$WXPOIN6S87g786rayo56358G65R(G6%#2ES^o8;");
                        con.Open();
                        con.Close();
                        myconnectionstring = CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString3 + ";User ID=farlogin;Password=df(*&G$WXPOIN6S87g786rayo56358G65R(G6%#2ES^o8;";
                    }
                    catch
                    {
                        MessageBox.Show("Неудалось подключиться к базе данных.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        Application.Current.Shutdown();
                        return;
                    }
                }
            }
            HotelWpf.LoginWin logwin = new HotelWpf.LoginWin();
            logwin.ShowDialog();
            if (logwin.DialogResult.HasValue && logwin.DialogResult.Value)
            {
                myconnectionstring = myconnectionstring.Substring(0, myconnectionstring.IndexOf(";User ID=")) + ";User ID=" + logwin.Result1 + ";Password=" + logwin.Result2;
            }
            else
            {
                try
                {
                    con = new SqlConnection(con.ConnectionString.Replace("Integrated Security=False;", "Integrated Security=True;"));
                    con.Open();
                    con.Close();
                    myconnectionstring = CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString2;
                }
                catch
                {
                    Application.Current.Shutdown();
                    return;
                }
            }
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            if (CurrentUserRoles.Contains("Accounts"))
            {
                App.Current.MainWindow = new AccountMainWin();
            }
            else
            {
                App.Current.MainWindow = new MainWindow();
                mystartAsync = new Classes.StartAsyncProgram();
                System.Threading.Tasks.Task task = mystartAsync.StartAsync();
            }
            StoreInit();
            CollectionsInit();
            ReferencesInit();
            App.Current.MainWindow.Show();
        }

        private static String myconnectionstring = CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString2;
        internal static string ConnectionString { get { return myconnectionstring; } }

        static private Classes.StartAsyncProgram mystartAsync;
        static private Classes.EventLogTypeList myeventlogtype;
        static internal Classes.EventLogTypeList EventLogTypes
        {
            get
            {
                if (myeventlogtype == null) myeventlogtype = new Classes.EventLogTypeList();
                return myeventlogtype;
            }
        }

        static private ContractorList myContractors;
        static internal ContractorList Contractors
        {
            get
            {
                if (myContractors == null)
                {
                    try
                    {
                        myContractors = new ContractorList();
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Data.SqlClient.SqlException)
                        {
                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                return myContractors;
            }
        }

        static private Classes.PrincipalList myusers;
        static internal Classes.PrincipalList Users
        {
            get
            {
                if (myusers == null)
                {
                    try
                    {
                        myusers = new Classes.PrincipalList(false);
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Data.SqlClient.SqlException)
                        {
                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                return myusers;
            }
        }
        static private Classes.PrincipalList myroles;
        static internal Classes.PrincipalList Roles
        {
            get
            {
                if (myroles == null)
                {
                    try
                    {
                        myroles = new Classes.PrincipalList(true);
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Data.SqlClient.SqlException)
                        {
                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                return myroles;
            }
        }

        static private CurrentUserRoleList myuserroles;
        static internal CurrentUserRoleList CurrentUserRoles
        {
            get
            {
                if (myuserroles == null)
                {
                    try
                    {
                        myuserroles = new CurrentUserRoleList();
                    }
                    catch { }
                }
                return myuserroles;
            }
        }

        static private CountryList mycountrylist;
        static internal CountryList Countries
        {
            get
            {
                if (mycountrylist == null)
                {
                    try
                    {
                        mycountrylist = new CountryList();
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Data.SqlClient.SqlException)
                        {
                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                return mycountrylist;
            }
        }
        static private Classes.Domain.References.PriceCategoryCollection mypricecategories;
        static public Classes.Domain.References.PriceCategoryCollection PriceCategories
        {
            get
            {
                if (mypricecategories == null)
                    mypricecategories = new Classes.Domain.References.PriceCategoryCollection();
                return mypricecategories;
            }
        }

        #region References
        static private ReferenceDS myreferenceds;
        static public ReferenceDS ReferenceDS
        {
            get
            {
                if (myreferenceds == null)
                    myreferenceds = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
                return myreferenceds;
            }
        }

        static private void ReferencesInit()
        {
            lib.ReferenceCollectionSimpleItem newref;
            newref = References.ManagerGroups;
            newref = References.RequestStates;
            newref = References.RowStates;
        }
        static private lib.ReferenceCollectionSimpleItem myagentnames;
        static public lib.ReferenceCollectionSimpleItem AgentNames
        {
            get
            {
                if (myagentnames == null)
                {
                    myagentnames = new lib.ReferenceCollectionSimpleItem();
                    myagentnames.CommandText = "SELECT [agentID],[agentName],[isactual],[isdefault] FROM [CustomBroker].[dbo].[AgentName_vw] ORDER BY [agentName]";
                    myagentnames.TableName = "dbo.AgentName_vw";
                    myagentnames.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    myagentnames.DataLoad();
                }
                return myagentnames;
            }
        }
        static private lib.ReferenceCollectionSimpleItem mycustomersname;
        static public lib.ReferenceCollectionSimpleItem CustomersName
        {
            get
            {
                if (mycustomersname == null)
                {
                    mycustomersname = new lib.ReferenceCollectionSimpleItem();
                    mycustomersname.CommandText = "SELECT [customerID],ISNULL(customerName,customerFullName),CONVERT(bit,1),CONVERT(bit,0) FROM [dbo].[CustomerName_vw] ORDER BY [customerID]";
                    mycustomerrowstates.TableName = "dbo.CustomerName_vw";
                    mycustomerrowstates.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    mycustomerrowstates.DataLoad();
                }
                return mycustomerrowstates;
            }
        }
        static private lib.ReferenceCollectionSimpleItem mycustomerrowstates;
        static public lib.ReferenceCollectionSimpleItem CustomerRowStates
        {
            get
            {
                if (mycustomerrowstates == null)
                {
                    mycustomerrowstates = new lib.ReferenceCollectionSimpleItem();
                    mycustomerrowstates.CommandText = "SELECT [staterowId],[staterowName],CONVERT(bit,1),CONVERT(bit,0) FROM [dbo].[StateRow_tb] WHERE (staterowTable = N'All' OR staterowTable = N'Customer') AND staterowId<200 ORDER BY [staterowId]";
                    mycustomerrowstates.TableName = "dbo.StateRow_tb";
                    mycustomerrowstates.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    mycustomerrowstates.DataLoad();
                }
                return mycustomerrowstates;
            }
        }
        static private lib.ReferenceCollectionSimpleItem mydeliverycarstates;
        static public lib.ReferenceCollectionSimpleItem DeliveryCarStates
        {
            get
            {
                if (mydeliverycarstates == null)
                {
                    mydeliverycarstates = new lib.ReferenceCollectionSimpleItem();
                    mydeliverycarstates.CommandText = "SELECT id,name,isactual,isdefault FROM delivery.DeliveryCarState_tb";
                    mydeliverycarstates.TableName = "delivery.DeliveryCarState_tb";
                    mydeliverycarstates.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    mydeliverycarstates.DataLoad();
                }
                return mydeliverycarstates;
            }
        }
        static private lib.ReferenceCollectionSimpleItem mydeliverytypes;
        static public lib.ReferenceCollectionSimpleItem DeliveryTypes
        {
            get
            {
                if (mydeliverytypes == null)
                {
                    mydeliverytypes = new lib.ReferenceCollectionSimpleItem();
                    mydeliverytypes.CommandText = "SELECT deliverytypeID as id,deliverytypeName as name,isactual,isdefault FROM dbo.DeliveryType_tb";
                    mydeliverytypes.TableName = "dbo.DeliveryType_tb";
                    mydeliverytypes.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    mydeliverytypes.DataLoad();
                }
                return mydeliverytypes;
            }
        }
        static private lib.ReferenceCollectionSimpleItem mygoodstypeparcel;
        static public lib.ReferenceCollectionSimpleItem GoodsTypesParcel
        {
            get
            {
                if (mygoodstypeparcel == null)
                {
                    mygoodstypeparcel = new lib.ReferenceCollectionSimpleItem();
                    mygoodstypeparcel.CommandText = "SELECT CONVERT(int,[Iditem]),[Nameitem],CONVERT(bit,1) as isactual,CONVERT(bit,0) as isdefault FROM [dbo].[GoodsType_tb] ORDER BY [Iditem]";
                    mygoodstypeparcel.TableName = "dbo.GoodsType_tb";
                    mygoodstypeparcel.ConnectionString = References.ConnectionString;
                    mygoodstypeparcel.DataLoad();
                }
                return mygoodstypeparcel;
            }
        }
        static private lib.ReferenceCollectionSimpleItem mymanagergroups;
        static public lib.ReferenceCollectionSimpleItem ManagerGroups
        {
            get
            {
                if (mymanagergroups == null)
                {
                    mymanagergroups = new lib.ReferenceCollectionSimpleItem();
                    mymanagergroups.CommandText = "SELECT managergroupID,managergroupName,CONVERT(bit,1) as isactual,CONVERT(bit,0) as isdefault FROM dbo.ManagerGroup_tb ORDER BY managergroupID";
                    mymanagergroups.TableName = "dbo.ManagerGroup_tb";
                    mymanagergroups.ConnectionString = References.ConnectionString;
                    mymanagergroups.DataLoad();
                }
                return mymanagergroups;
            }
        }
        static private lib.ReferenceCollectionSimpleItem myparceltypes;
        static public lib.ReferenceCollectionSimpleItem ParcelTypes
        {
            get
            {
                if (myparceltypes == null)
                {
                    myparceltypes = new lib.ReferenceCollectionSimpleItem();
                    myparceltypes.TableName = "dbo.ParcelType_tb";
                    myparceltypes.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    myparceltypes.DataLoad();
                }
                return myparceltypes;
            }
        }
        static private lib.ReferenceCollectionSimpleItem myparticipants;
        static public lib.ReferenceCollectionSimpleItem Participants
        {
            get
            {
                if (myparticipants == null)
                {
                    myparticipants = new lib.ReferenceCollectionSimpleItem();
                    myparticipants.TableName = "dbo.ParticipantNames_vw";
                    myparticipants.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    myparticipants.DataLoad();
                }
                return myparticipants;
            }
        }
        static private lib.ReferenceCollectionSimpleItem myrowstates;
        static public lib.ReferenceCollectionSimpleItem RowStates
        {
            get
            {
                if (myrowstates == null)
                {
                    myrowstates = new lib.ReferenceCollectionSimpleItem();
                    myrowstates.CommandText = "SELECT [staterowId],[staterowName],CONVERT(bit,1),CONVERT(bit,0) FROM [dbo].[StateRow_tb] WHERE staterowId<200 ORDER BY [staterowId]";
                    myrowstates.TableName = "dbo.StateRow_tb";
                    myrowstates.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    myrowstates.DataLoad();
                }
                return myrowstates;
            }
        }
        static private lib.ReferenceCollectionSimpleItem myrequeststate;
        static public lib.ReferenceCollectionSimpleItem RequestStates
        {
            get
            {
                if (myrequeststate == null)
                {
                    myrequeststate = new lib.ReferenceCollectionSimpleItem();
                    myrequeststate.CommandText = "SELECT id,name,isactual,isdefault FROM dbo.RequestStatus_tb ORDER BY id";
                    myrequeststate.TableName = "dbo.RequestStatus_tb";
                    myrequeststate.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    myrequeststate.DataLoad();
                }
                return myrequeststate;
            }
        }
        static private lib.ReferenceCollectionSimpleItem myservicetypes;
        static public lib.ReferenceCollectionSimpleItem ServiceTypes
        {
            get
            {
                if (myservicetypes == null)
                {
                    myservicetypes = new lib.ReferenceCollectionSimpleItem();
                    myservicetypes.TableName = "dbo.ServiceType_tb";
                    myservicetypes.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    myservicetypes.DataLoad();
                }
                return myservicetypes;
            }
        }
        static private lib.ReferenceCollectionSimpleItem mystore;
        static public lib.ReferenceCollectionSimpleItem Stores
        {
            get
            {
                if (mystore == null)
                {
                    mystore = new lib.ReferenceCollectionSimpleItem();
                    mystore.CommandText = "SELECT storeId,storeName,CONVERT(bit,1),CONVERT(bit,0) FROM dbo.Store_tb ORDER BY storeId";
                    mystore.TableName = "dbo.Store_tb";
                    mystore.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    mystore.DataLoad();
                }
                return mystore;
            }
        }
        static private lib.ReferenceCollectionSimpleItem myunits;
        static public lib.ReferenceCollectionSimpleItem Units
        {
            get
            {
                if (myunits == null)
                {
                    myunits = new lib.ReferenceCollectionSimpleItem();
                    myunits.TableName = "spec.Unit_tb";
                    myunits.ConnectionString = CustomBrokerWpf.References.ConnectionString;
                    myunits.DataLoad();
                }
                return myunits;
            }
        }
        #endregion
        #region Collections
        static private void CollectionsInit()
        {
            myimporters = new Classes.Domain.ImporterCollection();
            myimporters.DataLoad();
            mymanagers = new Classes.Domain.ManagerCollection();
            myparcelnumbers = new Classes.Domain.ParcelNumberCollection();
        }
        private static Classes.Domain.References.ColorCollection mycolors;
        public static Classes.Domain.References.ColorCollection Colors
        {
            get
            {
                if (mycolors == null)
                {
                    mycolors = new Classes.Domain.References.ColorCollection();
                }
                return mycolors;
            }
        }
        static private System.Collections.ObjectModel.ObservableCollection<Classes.Domain.DeliveryCar> mydeliverycars;
        static internal System.Collections.ObjectModel.ObservableCollection<Classes.Domain.DeliveryCar> DeliveryCars
        {
            set { mydeliverycars = value; }
            get
            {
                if (mydeliverycars == null)
                {
                    Classes.Domain.DeliveryCarDBM cdbm = new Classes.Domain.DeliveryCarDBM();
                    cdbm.isAll = false;
                    //cdbm.FillAsyncCompleted = () => { if (cdbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); };
                    cdbm.FillAsync();
                    mydeliverycars = cdbm.Collection;
                }
                return mydeliverycars;
            }
        }
        static private Classes.Domain.GenderCollection mygenders;
        static public Classes.Domain.GenderCollection Genders
        {
            get
            {
                if (mygenders == null)
                {
                    mygenders = new Classes.Domain.GenderCollection();
                    mygenders.DataLoad();
                }
                return mygenders;
            }
        }
        static private Classes.Domain.ImporterCollection myimporters;
        static public Classes.Domain.ImporterCollection Importers
        {
            get
            {
                if (myimporters == null)
                {
                    myimporters = new Classes.Domain.ImporterCollection();
                    myimporters.DataLoad();
                }
                return myimporters;
            }
        }
        static private Classes.Domain.ManagerCollection mymanagers;
        static public Classes.Domain.ManagerCollection Managers
        {
            get
            {
                if (mymanagers == null)
                {
                    mymanagers = new Classes.Domain.ManagerCollection();
                }
                return mymanagers;
            }
        }
        static private Classes.Specification.MaterialCollection mymaterials;
        static public Classes.Specification.MaterialCollection Materials
        {
            get
            {
                if (mymaterials == null)
                {
                    mymaterials = new Classes.Specification.MaterialCollection();
                    mymaterials.DataLoad();
                }
                return mymaterials;
            }
        }
        static private Classes.Specification.MaterialSynchronizer mymaterialsinc;
        static public System.Collections.ObjectModel.ObservableCollection<Classes.Specification.MaterialVM> MaterialVMs
        {
            get
            {
                if (mymaterialsinc == null)
                {
                    mymaterialsinc = new Classes.Specification.MaterialSynchronizer();
                    mymaterialsinc.DomainCollection = References.Materials;
                }
                return mymaterialsinc.ViewModelCollection;
            }
        }
        static private Classes.Domain.ParcelNumberCollection myparcelnumbers;
        static public Classes.Domain.ParcelNumberCollection ParcelNumbers
        {
            get
            {
                if (myparcelnumbers == null)
                {
                    myparcelnumbers = new Classes.Domain.ParcelNumberCollection();
                }
                return myparcelnumbers;
            }
        }
        #endregion
        #region Store 
        static private void StoreInit() // Необходима инициализация в главном потоке чтобы dbm получал правильный Dispatcher 
        {
            myagentstore = new Classes.Domain.AgentStore(new Classes.Domain.AgentDBM());
            myalgorithmconsolidatestore = new Classes.Domain.Algorithm.AlgorithmFormulaRequestConCommandStore();
            mycustomerlegalstore = new Classes.Domain.CustomerLegalStore(new Classes.Domain.CustomerLegalDBM());
            mycustomsinvoicestore = new Classes.Domain.Account.CustomsInvoiceStore(new Classes.Domain.Account.CustomsInvoiceDBM());
            mydeliverycarrystore = new Classes.Domain.DeliveryCarryStore(new Classes.Domain.DeliveryCarryDBM());
            mydeliverycarstore = new Classes.Domain.DeliveryCarStore(new Classes.Domain.DeliveryCarDBM());
            myformulastorage = new Classes.Domain.Algorithm.FormulaStorage();
            myparcelstore = new Classes.Domain.ParcelStore(new Classes.Domain.ParcelDBM());
            myprepaystore = new Classes.Domain.Account.PrepayStore(new Classes.Domain.Account.PrepayDBM());
            myrequestcustomerlegalstore = new Classes.Domain.RequestCustomerLegalStore(new Classes.Domain.RequestCustomerLegalDBM());
            myrequeststore = new Classes.Domain.RequestStore(new Classes.Domain.RequestDBM());
            myspecificationstore = new Classes.Specification.SpecificationStore(new Classes.Specification.SpecificationDBM());
            myprepayrequeststore=new Classes.Domain.Account.PrepayCustomerRequestStore(new Classes.Domain.Account.PrepayCustomerRequestDBM());
        }
        static private Classes.Domain.AgentStore myagentstore;
        static internal Classes.Domain.AgentStore AgentStore
        {
            get
            {
                if (myagentstore == null)
                    myagentstore = new Classes.Domain.AgentStore(new Classes.Domain.AgentDBM());
                return myagentstore;
            }
        }
        static private Classes.Domain.Algorithm.AlgorithmFormulaRequestConCommandStore myalgorithmconsolidatestore;
        static internal Classes.Domain.Algorithm.AlgorithmFormulaRequestConCommandStore AlgorithmConsolidateStore
        {
            get
            {
                if (myalgorithmconsolidatestore == null)
                    myalgorithmconsolidatestore = new Classes.Domain.Algorithm.AlgorithmFormulaRequestConCommandStore();
                return myalgorithmconsolidatestore;
            }
        }
        static private Classes.Domain.BranchStore mybranchstore;
        static public Classes.Domain.BranchStore BranchStore
        {
            get
            {
                if (mybranchstore == null)
                {
                    mybranchstore = new Classes.Domain.BranchStore(new Classes.Domain.BranchDBM());
                }
                return mybranchstore;
            }
        }
        static private Classes.Domain.CustomerLegalStore mycustomerlegalstore;
        static internal Classes.Domain.CustomerLegalStore CustomerLegalStore
        {
            get
            {
                if (mycustomerlegalstore == null)
                {
                    mycustomerlegalstore = new Classes.Domain.CustomerLegalStore(new Classes.Domain.CustomerLegalDBM());
                }
                return mycustomerlegalstore;
            }
        }
        static private Classes.Domain.CustomerStore mycustomerstore;
        static internal Classes.Domain.CustomerStore CustomerStore
        {
            get
            {
                if (mycustomerstore == null)
                {
                    mycustomerstore = new Classes.Domain.CustomerStore(new Classes.Domain.CustomerDBM());
                }
                return mycustomerstore;
            }
        }
        static private Classes.Domain.Account.CustomsInvoiceStore mycustomsinvoicestore;
        static internal Classes.Domain.Account.CustomsInvoiceStore CustomsInvoiceStore
        {
            get
            {
                if (mycustomsinvoicestore == null)
                {
                    mycustomsinvoicestore = new Classes.Domain.Account.CustomsInvoiceStore(new Classes.Domain.Account.CustomsInvoiceDBM());
                }
                return mycustomsinvoicestore;
            }
        }
        static private Classes.Domain.DeliveryCarryStore mydeliverycarrystore;
        static public Classes.Domain.DeliveryCarryStore DeliveryCarryStore
        {
            get
            {
                if (mydeliverycarrystore == null)
                {
                    mydeliverycarrystore = new Classes.Domain.DeliveryCarryStore(new Classes.Domain.DeliveryCarryDBM());
                }
                return mydeliverycarrystore;
            }
        }
        static private Classes.Domain.DeliveryCarStore mydeliverycarstore;
        static public Classes.Domain.DeliveryCarStore DeliveryCarStore
        {
            get
            {
                if (mydeliverycarstore == null)
                {
                    mydeliverycarstore = new Classes.Domain.DeliveryCarStore(new Classes.Domain.DeliveryCarDBM());
                }
                return mydeliverycarstore;
            }
        }
        static private Classes.Domain.Algorithm.FormulaStorage myformulastorage;
        static public Classes.Domain.Algorithm.FormulaStorage FormulaStorage
        {
            get
            {
                if (myformulastorage == null)
                {
                    myformulastorage = new Classes.Domain.Algorithm.FormulaStorage();
                }
                return myformulastorage;
            }
        }
        static private Classes.Domain.GoodsStore mygoodsstore;
        static public Classes.Domain.GoodsStore GoodsStore
        {
            get
            {
                if (mygoodsstore == null)
                {
                    mygoodsstore = new Classes.Domain.GoodsStore(new Classes.Domain.GoodsDBM());
                }
                return mygoodsstore;
            }
        }
        static private Classes.Domain.ParcelStore myparcelstore;
        static internal Classes.Domain.ParcelStore ParcelStore
        {
            get
            {
                if (myparcelstore == null)
                {
                    myparcelstore = new Classes.Domain.ParcelStore(new Classes.Domain.ParcelDBM());
                }
                return myparcelstore;
            }
        }
        static private Classes.Domain.Account.PrepayStore myprepaystore;
        static internal Classes.Domain.Account.PrepayStore PrepayStore
        {
            get
            {
                if (myprepaystore == null)
                {
                    myprepaystore = new Classes.Domain.Account.PrepayStore(new Classes.Domain.Account.PrepayDBM());
                }
                return myprepaystore;
            }
        }
        static private Classes.Domain.Account.PrepayCustomerRequestStore myprepayrequeststore;
        static internal Classes.Domain.Account.PrepayCustomerRequestStore PrepayRequestStore
        {
            get
            {
                if (myprepayrequeststore == null)
                {
                    myprepayrequeststore = new Classes.Domain.Account.PrepayCustomerRequestStore(new Classes.Domain.Account.PrepayCustomerRequestDBM());
                }
                return myprepayrequeststore;
            }
        }
        static private Classes.Domain.RecipientStore myrecipientstore;
        static internal Classes.Domain.RecipientStore RecipientStore
        {
            get
            {
                if (myrecipientstore == null)
                {
                    myrecipientstore = new Classes.Domain.RecipientStore(new Classes.Domain.RecipientDBM());
                }
                return myrecipientstore;
            }
        }
        static private Classes.Domain.RequestStore myrequeststore;
        static internal Classes.Domain.RequestStore RequestStore
        {
            get
            {
                if (myrequeststore == null)
                {
                    myrequeststore = new Classes.Domain.RequestStore(new Classes.Domain.RequestDBM());
                }
                return myrequeststore;
            }
        }
        static private Classes.Domain.RequestCustomerLegalStore myrequestcustomerlegalstore;
        static internal Classes.Domain.RequestCustomerLegalStore RequestCustomerLegalStore
        {
            get
            {
                if (myrequestcustomerlegalstore == null)
                {
                    myrequestcustomerlegalstore = new Classes.Domain.RequestCustomerLegalStore(new Classes.Domain.RequestCustomerLegalDBM());
                }
                return myrequestcustomerlegalstore;
            }
        }
        static private Classes.Specification.SpecificationStore myspecificationstore;
        static internal Classes.Specification.SpecificationStore SpecificationStore
        {
            get
            {
                if (myspecificationstore == null)
                {
                    myspecificationstore = new Classes.Specification.SpecificationStore(new Classes.Specification.SpecificationDBM());
                }
                return myspecificationstore;
            }
        }
        #endregion
        static private lib.ViewCollector mycarviewcollector;
        static internal lib.ViewCollector CarsViewCollector
        {
            get
            {
                if (mycarviewcollector == null)
                    mycarviewcollector = new lib.ViewCollector();
                return mycarviewcollector;
            }
        }
        static private lib.ViewCollector mycarryviewcollector;
        static internal lib.ViewCollector CarryViewCollector
        {
            get
            {
                if (mycarryviewcollector == null)
                    mycarryviewcollector = new lib.ViewCollector();
                return mycarryviewcollector;
            }
        }
        static private lib.ViewCollector mycustomerviewcollector;
        static internal lib.ViewCollector CustomerViewCollector
        {
            get
            {
                if (mycustomerviewcollector == null)
                    mycustomerviewcollector = new lib.ViewCollector();
                return mycustomerviewcollector;
            }
        }

        public static double WorkAreaHight
        {
            get { return SystemParameters.WorkArea.Height / 1.3D; }
        }
        private static Classes.CurrencyRateSingleton mycurrencyrate;
        internal static Classes.CurrencyRateSingleton CurrencyRate
        { get { if (mycurrencyrate == null) mycurrencyrate = new Classes.CurrencyRateSingleton(); return mycurrencyrate; } }
        internal static DateTime EndQuarter(DateTime date)
        {
            DateTime end = DateTime.Today;
            switch (date.Month)
            {
                case 1:
                case 2:
                case 3:
                    end = new DateTime(DateTime.Today.Year, 3, 31);
                    break;
                case 4:
                case 5:
                case 6:
                    end = new DateTime(DateTime.Today.Year, 6, 30);
                    break;
                case 7:
                case 8:
                case 9:
                    end = new DateTime(DateTime.Today.Year, 9, 30);
                    break;
                case 10:
                case 11:
                case 12:
                    end = new DateTime(DateTime.Today.Year, 12, 31);
                    break;
            }
            return end;
        }
    }
}
