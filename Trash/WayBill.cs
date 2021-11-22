using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    class WayBill
    {
		private static WayBill waybill;
		private WayBill()
		{
			customers = new List<CustomerWayBill>();
		}
		internal static WayBill GetWayBill()
		{
			if (waybill == null)
				waybill = new WayBill();
			return waybill;
		}

		int nstart, nstop;
        string cname,filename,filepath;

        BackgroundWorker mybw;
        ExcelImportWin myExcelImportWin;
		WayBillClientWin myWayBillClientWin;
		List<CustomerWayBill> customers;

		internal void CreateWayBillFromSpec()
        {
			if (mybw == null)
			{
				mybw = new BackgroundWorker();
				mybw.DoWork += BackgroundWorker_DoWork;
				mybw.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
				mybw.WorkerReportsProgress = true;
				mybw.ProgressChanged += BackgroundWorker_ProgressChanged;
			}
			if (mybw.IsBusy)
			{
				MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "СФ и ТТН", MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
			if (!(System.IO.File.Exists(Environment.CurrentDirectory + @"\Templates\ТТН.xltx") & System.IO.File.Exists(Environment.CurrentDirectory + @"\Templates\СФ.xltx")))
            {
                MessageBox.Show("Шаблон не найден!", "СФ и ТТН", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.customers.Clear();
            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
            fd.CheckPathExists = true;
            fd.CheckFileExists = true;
            fd.Multiselect = true;
            fd.Title = "Выбор файла с данными";
            fd.Filter = "Файл Excel |*.xls;*.xlsx";
            fd.ShowDialog();
            if (fd.FileNames.Length > 0)
            {
				CustomerWayBill customer;
				for(int i=0;i<fd.FileNames.Length;i++)
				{
                    cname = string.Empty;
                    filepath = fd.FileNames[i];
                    filename = filepath.Substring(filepath.LastIndexOf('\\') + 1);
                    nstart = filename.IndexOf('_') + 1;
                    if (nstart > 0)
                    {
                        nstop = filename.IndexOf('_', nstart);
                        if (nstop < 0) nstop = filename.IndexOf('.', nstart);
                        cname = filename.Substring(nstart, nstop - nstart);
                    }
                    customer = GetCustomerWayBill(cname);
                    customer.Path = filepath;
                    customer.FileName = filename;
					customer.PropertyChanged += Customer_PropertyChanged;
                    customers.Add(customer);
                }
                customers.Sort((CustomerWayBill x, CustomerWayBill y) => { int q;q = string.Compare(x.Name, y.Name);if(q==0) q=string.Compare(x.FileName, y.FileName); return q; });
                // открытие формы выбора клиента, код по вызову обработки из ее представления
                if (myWayBillClientWin != null && myWayBillClientWin.IsVisible)
                {
                    myWayBillClientWin.DataCancelEdit();
                    (System.Windows.Data.CollectionViewSource.GetDefaultView(customers) as System.Windows.Data.CollectionView).Refresh();
                }
                else
                {
                    if (myWayBillClientWin != null)
                        myWayBillClientWin.DataContext = null;
                    myWayBillClientWin = new WayBillClientWin();
                    CustomerWayBillListVM cvm = new CustomerWayBillListVM(customers);
                    cvm.EndEdit = myWayBillClientWin.DataEndEdit;
                    cvm.CancelEdit = myWayBillClientWin.DataCancelEdit;
                    myWayBillClientWin.DataContext = cvm;
                    myWayBillClientWin.Show();
                }
            }
        }

		private void Customer_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			CustomerWayBill curc=sender as CustomerWayBill;
			CustomerWayBill newc = GetCustomerWayBill(curc.Name);
			curc.Address = newc.Address;
			curc.BankBic = newc.BankBic;
			curc.BankName = newc.BankName;
			curc.ContractDate = newc.ContractDate;
			curc.ContractNum = newc.ContractNum;
			curc.CorAccount = newc.CorAccount;
			curc.FullName = newc.FullName;
			curc.INN = newc.INN;
			curc.RAccount = newc.RAccount;
			newc = null;
		}

		private CustomerWayBill GetCustomerWayBill(string customername)
		{
			CustomerWayBill cstm = new CustomerWayBill();
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
			{
				SqlCommand cmd = new SqlCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "dbo.CustomerWayBill_sp";
                cmd.Parameters.Add(new SqlParameter("@name", customername));
				cmd.Connection = conn;
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
				if (reader.Read())
				{
					if(!reader.IsDBNull(0)) cstm.Name = reader.GetString(0);
					if (!reader.IsDBNull(1)) cstm.FullName = reader.GetString(1);
					if (!reader.IsDBNull(2)) cstm.INN = reader.GetString(2);
					if (!reader.IsDBNull(3)) cstm.BankBic = reader.GetString(3);
					if (!reader.IsDBNull(4)) cstm.BankName = reader.GetString(4);
					if (!reader.IsDBNull(5)) cstm.RAccount = reader.GetString(5);
					if (!reader.IsDBNull(6)) cstm.CorAccount = reader.GetString(6);
					if (!reader.IsDBNull(7)) cstm.ContractNum = reader.GetString(7);
					if (!reader.IsDBNull(8)) cstm.ContractDate = reader.GetDateTime(8);
					if (!reader.IsDBNull(9)) cstm.Address = reader.GetString(9);
				}
				reader.Close();
			}
			return cstm;
		}
		internal void RunDoWork()
		{
            if (mybw.IsBusy)
            {
                MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "СФ и ТТН", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            if (myExcelImportWin != null && myExcelImportWin.IsVisible) myExcelImportWin.Close();
			myExcelImportWin = new ExcelImportWin();
			myExcelImportWin.Show();
			mybw.RunWorkerAsync();
		}

		private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
			bool waytruble, prctruble;

			int maxr, wayr, prcr;
            string str, customername;
            decimal dmval;

            BackgroundWorker worker = sender as BackgroundWorker;
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;

            try
            {
                customers.Sort((CustomerWayBill x, CustomerWayBill y) => { return string.Compare(x.Name, y.Name); });
                CustomBrokerWpf.Domain.References.Country country;

                Excel.Workbook exWbSpc;
                Excel.Worksheet exWhSpc;
                Excel.Workbook exWbWay;
                Excel.Worksheet exWhWay=null;
                Excel.Workbook exWbPrc;
                Excel.Worksheet exWhPrc=null;
                Excel.Range rangetemplway = null, rangetemplprc = null;

                waytruble = false;
                prctruble = false;
                wayr = 19;
                prcr = 19;
                customername = @"#_start_#";

                foreach (CustomerWayBill customer in customers)
                {
                    if (customername != customer.Name)
                    {
                        if (exWhWay != null)
                        {
                            CloseWayBill(exWhWay, exWhPrc, wayr, prcr);
                            worker.ReportProgress(100, (waytruble ? "В ТТН для " + customername + " не удалось заполнить все ячейки." : string.Empty)
                                + (waytruble & prctruble ? "\n" : string.Empty)
                                + (prctruble ? "В СФ для " + customername + " не удалось заполнить все ячейки." : string.Empty));
                        }

                        customername = customer.Name;
                        exWbWay = exApp.Workbooks.Add(Environment.CurrentDirectory + @"\Templates\ТТН.xltx");
                        exWhWay = exWbWay.Sheets[1];
                        exWbPrc = exApp.Workbooks.Add(Environment.CurrentDirectory + @"\Templates\СФ.xltx");
                        exWhPrc = exWbPrc.Sheets[1];
                        wayr = 19;
                        prcr = 19;

                        if (rangetemplway == null)
                        {
                            rangetemplway = exWhWay.Range[exWhWay.Cells[wayr, 1], exWhWay.Cells[wayr, 16]];
                            rangetemplprc = exWhPrc.Range[exWhPrc.Cells[wayr, 1], exWhPrc.Cells[wayr, 14]];
                        }
                        waytruble = string.IsNullOrEmpty(customer.Address) | string.IsNullOrEmpty(customer.BankBic) | string.IsNullOrEmpty(customer.BankName) | string.IsNullOrEmpty(customer.ContractNum) | string.IsNullOrEmpty(customer.CorAccount) | string.IsNullOrEmpty(customer.INN) | (string.IsNullOrEmpty(customer.Name) & string.IsNullOrEmpty(customer.FullName)) | string.IsNullOrEmpty(customer.RAccount) | !customer.ContractDate.HasValue;
                        prctruble = string.IsNullOrEmpty(customer.Address) | (string.IsNullOrEmpty(customer.Name) & string.IsNullOrEmpty(customer.FullName)) | string.IsNullOrEmpty(customer.INN);

                        worker.ReportProgress(0);

                        exWhWay.Cells[13, 9] = DateTime.Today.ToShortDateString();
                        exWhWay.Cells[38, 3] = DateTime.Today.ToLongDateString();
                        str = (string.IsNullOrEmpty(customer.FullName) ? customer.Name : customer.FullName) + (string.IsNullOrEmpty(customer.FullName) ? string.Empty : ", ") + customer.Address + ", ИНН " + customer.INN + ", р/с " + customer.RAccount + " в " + customer.BankName + ", БИК " + customer.BankBic + ", корр/с " + customer.CorAccount;
                        exWhWay.Cells[7, 3] = str;
                        exWhWay.Cells[9, 3] = str;

                        exWhWay.Cells[10, 3] = "Договор № " + customer.ContractNum + " от " + (customer.ContractDate.HasValue ? customer.ContractDate.Value.ToShortDateString() : string.Empty);

                        exWhPrc.Cells[4, 2] = "Счет - фактура №  от " + DateTime.Today.ToLongDateString();
                        exWhPrc.Cells[10, 2] = "Грузополучатель и его адрес: " + (string.IsNullOrEmpty(customer.FullName) ? customer.Name : customer.FullName) + ", " + customer.Address;
                        exWhPrc.Cells[12, 2] = "Покупатель: " + (string.IsNullOrEmpty(customer.FullName) ? customer.Name : customer.FullName);
                        exWhPrc.Cells[13, 2] = "Адрес: " + customer.Address;
                        exWhPrc.Cells[14, 2] = "ИНН/КПП " + customer.INN;
                    }
                    exWbSpc = exApp.Workbooks.Open(customer.Path, false, true);
                    exWhSpc = exWbSpc.Sheets["Для расчета"];
                    if (exWhSpc == null) throw new Exception("В файле " + customer.FileName + " не найден лист \"Для расчета\".");

                    maxr = exWhSpc.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                    int r = 7;
                    str = (exWhSpc.Cells[r, 3].Text as string).Trim();
                    while (str.Length > 0 & str.ToLower().IndexOf("итого") < 0)
                    {
                        if (wayr != 19)
                        {
                            exWhWay.Rows[wayr].Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                            exWhPrc.Rows[prcr].Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                            rangetemplway.Copy();
                            exWhWay.Range[exWhWay.Cells[wayr, 1], exWhWay.Cells[wayr, 16]].PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone);
                            exWhWay.Cells[wayr, 1] = 1 + int.Parse(exWhWay.Cells[wayr - 1, 1].Text as string);
                            rangetemplprc.Copy();
                            exWhPrc.Range[exWhPrc.Cells[prcr, 1], exWhPrc.Cells[prcr, 14]].PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone);

                        }

                        str = (exWhSpc.Cells[r, 3].Text as string).Trim();
                        if (str.Contains("жен")) str = str.Remove(str.IndexOf(" ", str.IndexOf("жен")));
                        if (str.Contains("муж")) str = str.Remove(str.IndexOf(" ", str.IndexOf("муж")));
                        str = str + " " + (exWhSpc.Cells[r, 24].Text as string).Trim();
                        exWhWay.Cells[wayr, 2] = str;
                        exWhPrc.Cells[prcr, 2] = str;

                        exWhWay.Cells[wayr, 11] = exWhSpc.Cells[r, 5];
                        exWhPrc.Cells[prcr, 5] = exWhSpc.Cells[r, 5];

                        str = (exWhSpc.Cells[r, 30].Text as string).Trim();
                        if (decimal.TryParse(str, out dmval))
                        {
                            dmval = decimal.Divide(dmval, 1.18M);
                            exWhWay.Cells[wayr, 12] = dmval;
                            exWhPrc.Cells[prcr, 6] = dmval;
                        }
                        else
                        {
                            exWhWay.Cells[wayr, 12] = str;
                            exWhPrc.Cells[prcr, 6] = str;
                            exWhWay.Cells[wayr, 11].Interior.Color = 255;
                            exWhPrc.Cells[prcr, 6].Interior.Color = 255;
							waytruble = true;
							prctruble = true;
						}

                        exWhPrc.Cells[prcr, 13] = exWhSpc.Cells[r, 28];
                        str = (exWhSpc.Cells[r, 28].Text as string).Trim();
						country = References.Countries.FindFirstItem("FullName", str);
						if(country==null) country = References.Countries.FindFirstItem("ShortName", str);
						if (country != null)
                        {
                            exWhPrc.Cells[prcr, 12] = country.Code;
                            exWhPrc.Cells[prcr, 12].Interior.Pattern = Excel.Constants.xlNone;
                        }
                        else
                        {
                            exWhPrc.Cells[prcr, 12].Interior.Pattern = Excel.Constants.xlSolid;
                            exWhPrc.Cells[prcr, 12].Interior.Color = 255;
							prctruble = true;
						}
						r++; wayr++; prcr++;
                        str = (exWhSpc.Cells[r, 3].Text as string).Trim();
                        worker.ReportProgress((int)(decimal.Divide(r, maxr) * 100));
                    }
                    exWbSpc.Close();
                }
                if (exWhWay != null) CloseWayBill(exWhWay, exWhPrc, wayr, prcr);
                worker.ReportProgress(100, (waytruble ? "В ТТН для " + customers[customers.Count - 1].Name + " не удалось заполнить все ячейки." : string.Empty)
                    + (waytruble & prctruble ? "\n" : string.Empty)
                    + (prctruble ? "В СФ для " + customers[customers.Count - 1].Name + " не удалось заполнить все ячейки." : string.Empty));
            }
            finally
            {
                if (exApp != null)
                {
                    exApp.Visible = true;
                    exApp.DisplayAlerts = true;
                    exApp.ScreenUpdating = true;
                    exApp = null;
                }
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                myExcelImportWin.MessageTextBlock.Text = "Загрузка прервана из-за ошибки" + "\n" + e.Error.Message;
            }
            else
            {
				if (string.IsNullOrEmpty(myExcelImportWin.MessageTextBlock.Text))
				{
					myExcelImportWin.Close();
					myExcelImportWin = null;
				}
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            myExcelImportWin.ProgressBar1.Value = e.ProgressPercentage;
			string msg = (string)e.UserState;
			if (!string.IsNullOrEmpty(msg))
			{
				if (!string.IsNullOrEmpty(myExcelImportWin.MessageTextBlock.Text)) msg = "\n" + msg;
				myExcelImportWin.MessageTextBlock.Text = myExcelImportWin.MessageTextBlock.Text + msg;
			}
        }

        private void CloseWayBill(Excel.Worksheet exWhWay, Excel.Worksheet exWhPrc, int wayr, int prcr)
        {
            int waybreaknext = 1, waystart = 19;
            string str;
            int[] pagesum = new int[30];
            Excel.Range rangeheader;

            rangeheader = exWhWay.Range[exWhWay.Cells[16, 1], exWhWay.Cells[18, 16]];
            exWhWay.Parent.Windows[1].SmallScroll(wayr);
            for (int i = 1; i < exWhWay.HPageBreaks.Count + 1; i++)
            {
                waybreaknext = exWhWay.HPageBreaks[i].Location.Row - 1;
                if (waybreaknext+2 > wayr) break;
                exWhWay.Rows[waybreaknext].Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                exWhWay.Rows[waybreaknext].Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                exWhWay.Rows[waybreaknext].Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                exWhWay.Rows[waybreaknext].Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                exWhWay.Rows[waybreaknext].Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                wayr = wayr + 5;
                exWhWay.Parent.Windows[1].SmallScroll(5);
                exWhWay.Range[exWhWay.Cells[wayr, 1], exWhWay.Cells[wayr, 16]].Copy();
                exWhWay.Range[exWhWay.Cells[waybreaknext, 1], exWhWay.Cells[waybreaknext, 16]].PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone);
                str = "=SUM(R[-" + (waybreaknext - waystart).ToString() + "]C:R[-1]C)";
                exWhWay.Cells[waybreaknext, 11].Formula = str;
                exWhWay.Cells[waybreaknext, 13].Formula = str;
                exWhWay.Cells[waybreaknext, 15].Formula = str;
                exWhWay.Cells[waybreaknext, 16].Formula = str;
                exWhWay.Rows[waybreaknext].AutoFit();
                exWhWay.Range[exWhWay.Cells[waybreaknext, 1], exWhWay.Cells[waybreaknext, 7]].Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.Constants.xlNone;
                pagesum[i - 1] = waybreaknext; //для общей суммы, последняя страница не учитывается
                waybreaknext++;
                exWhWay.Cells[waybreaknext, 16] = "Страница № " + (i + 1).ToString();
                exWhWay.Rows[waybreaknext].AutoFit();
                exWhWay.Range[exWhWay.Cells[waybreaknext, 1], exWhWay.Cells[waybreaknext, 16]].Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.Constants.xlNone;
                exWhWay.Range[exWhWay.Cells[waybreaknext, 1], exWhWay.Cells[waybreaknext, 16]].Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.Constants.xlNone;
                exWhWay.Range[exWhWay.Cells[waybreaknext, 1], exWhWay.Cells[waybreaknext, 16]].Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.Constants.xlNone;
                exWhWay.Range[exWhWay.Cells[waybreaknext, 1], exWhWay.Cells[waybreaknext, 16]].Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.Constants.xlNone;
                while (waybreaknext < exWhWay.HPageBreaks[i].Location.Row)
                {
                    exWhWay.Rows[waybreaknext].Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                    exWhWay.Range[exWhWay.Cells[waybreaknext, 1], exWhWay.Cells[waybreaknext, 16]].Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.Constants.xlNone;
                    exWhWay.Parent.Windows[1].SmallScroll(1);
                    waybreaknext++;
                    wayr++;
                }
                waybreaknext++;
                rangeheader.Copy();
                exWhWay.Range[exWhWay.Cells[waybreaknext, 1], exWhWay.Cells[waybreaknext + 2, 16]].PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone);
                exWhWay.Range[exWhWay.Rows[waybreaknext], exWhWay.Rows[waybreaknext + 2]].AutoFit();
                waystart = waybreaknext + 3;
            }
            str = "=SUM(R[-" + (wayr - waystart).ToString() + "]C:R[-1]C)";
            exWhWay.Cells[wayr, 11].Formula = str;
            exWhWay.Cells[wayr, 13].Formula = str;
            exWhWay.Cells[wayr, 15].Formula = str;
            exWhWay.Cells[wayr, 16].Formula = str;
            exWhWay.Rows[wayr].AutoFit();
            wayr++;
            waybreaknext = 0;
            str = "=R[-1]C";
            while (pagesum[waybreaknext] > 0)
            {
                str = str + "+R[-" + (wayr - pagesum[waybreaknext]).ToString() + "]C";
                waybreaknext++;
            }
            exWhWay.Cells[wayr, 11].Formula = str;
            exWhWay.Cells[wayr, 13].Formula = str;
            exWhWay.Cells[wayr, 15].Formula = str;
            exWhWay.Cells[wayr, 16].Formula = str;

            wayr = wayr + 3;
            CurrAmountWords.SingularPlural sp;
            CurrAmountWords amountwords = new CurrAmountWords();
            str = amountwords.AmountWords(exWhWay.Cells[wayr - 5, 1].Text, out sp, CurrAmountWords.GenderWord.Masculine, true);
            exWhWay.Cells[wayr, 4] = str;
            if (sp == CurrAmountWords.SingularPlural.Many)
                exWhWay.Cells[wayr, 13] = "порядковых номеров записей";
            else if (sp == CurrAmountWords.SingularPlural.Some)
                exWhWay.Cells[wayr, 13] = "порядковых номера записей";
            else
                exWhWay.Cells[wayr, 13] = "порядковый номер записей";
            str = amountwords.AmountWords(exWhWay.Cells[wayr - 5, 1].Text, out sp, CurrAmountWords.GenderWord.Neuter, true);
            str = "Всего отпущено " + str;
            if (sp == CurrAmountWords.SingularPlural.Many)
                str = str + " наименований на сумму ";
            else if (sp == CurrAmountWords.SingularPlural.Some)
                str = str + " наименования на сумму ";
            else
                str = str + " наименование на сумму ";
            str = str + amountwords.AmountCurrWords(decimal.Parse(exWhWay.Cells[wayr - 3, 16].Text), CurrAmountWords.CurrencyName.RUB);
            exWhWay.Cells[wayr + 6, 1] = str;
            exWhWay.Parent.Windows[1].SmallScroll(0, wayr);

            str = "=SUM(R[-" + (prcr - 19).ToString() + "]C:R[-1]C)";
            exWhPrc.Cells[prcr, 7] = str;
            exWhPrc.Cells[prcr, 10] = str;
            exWhPrc.Cells[prcr, 11] = str;
        }
    }

    public class CustomerWayBill:INotifyPropertyChanged
	{
		private string mypath;
		public string Path
		{
			set { mypath = value; }
			get { return mypath; }
		}

		private string myfilename;
		public string FileName
		{
			set { myfilename = value; }
			get { return myfilename; }
		}

		private string myname;
		public string Name
		{
			set
			{
				myname = value;
				PropertyChangedNotification("Name");
			}
			get { return myname; }
		}

		private string myfullname;
		public string FullName
		{
			set { myfullname = value; }
			get { return myfullname; }
		}

		private string myinn;
		public string INN
		{
			set { myinn = value; }
			get { return myinn; }
		}

		private string mybankbic;
		public string BankBic
		{
			set { mybankbic = value; }
			get { return mybankbic; }
		}

		private string mybankname;
		public string BankName
		{
			set { mybankname = value; }
			get { return mybankname; }
		}

		private string myraccount;
		public string RAccount
		{
			set { myraccount = value; }
			get { return myraccount; }
		}

		private string mycoraccount;
		public string CorAccount
		{
			set { mycoraccount = value; }
			get { return mycoraccount; }
		}

		private string myaddress;
		public string Address
		{
			set { myaddress = value; }
			get { return myaddress; }
		}

		private string mycontractnum;
		public string ContractNum
		{
			set { mycontractnum = value; }
			get { return mycontractnum; }
		}

		private DateTime? mycontractdate;
		public DateTime? ContractDate
		{
			set { mycontractdate = value; }
			get { return mycontractdate; }
		}

		public CustomerWayBill():this(string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,string.Empty,null) { }
		internal CustomerWayBill(string path,string filename,string name,string fullname,string inn,string bankbic,string bankname,string raccount,string coraccount,string address,string contractnum,DateTime? contractdate): base()
		{
			mypath = path;
			myfilename = filename;
			myname = name;
			myfullname = fullname;
			myinn = inn;
			mybankbic = bankbic;
			mybankname = bankname;
			myraccount = raccount;
			mycoraccount = coraccount;
			myaddress = address;
			mycontractnum = contractnum;
			mycontractdate = contractdate;
		}

		//INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void PropertyChangedNotification(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}

	}
	internal class CustomerWayBillListVM
	{
		private List<CustomerWayBill> mycustomers;
		internal CustomerWayBillListVM(List<CustomerWayBill> customers)
		{
			mycustomers = customers;
            myendedit = () => { return true; };
            mycanceledit = () => { return; };
            myrun = new RelayCommand(RunExec, RunCanExec);
		}

		public List<CustomerWayBill> Customers
		{ get { return mycustomers; } }
		public object CustomersName
		{
			get
			{
				ReferenceDS refds = ((ReferenceDS)(App.Current.FindResource("keyReferenceDS")));
				if (refds.tableCustomerName.Count == 0) refds.CustomerNameRefresh();
				return refds.tableCustomerName.DefaultView;

			}
		}

        protected Func<bool> myendedit; // вызов для окна закончить редактирование
        internal Func<bool> EndEdit { set { myendedit = value; } }
        private Action mycanceledit; // вызов для окна отменить изменения
        internal Action CancelEdit { set { mycanceledit = value; } }

        private RelayCommand myrun;
		public ICommand Run { get { return myrun; } }
		private void RunExec(object parametr)
		{
            if(this.myendedit())
			    WayBill.GetWayBill().RunDoWork();
		}
		private bool RunCanExec(object parametr)
		{
			return true;
		}

	}
}
