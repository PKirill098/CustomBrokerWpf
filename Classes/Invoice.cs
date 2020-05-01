using System;
using System.Data;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class Invoice
    {
        
        internal void CreateInvoiceExcel(InvoiceDS.tableInvoiceRow row)
        {
            int rowdel,dr;
            decimal sumtot = 0M, dept = 0M;

            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            try
            {
                Excel.Workbook exWb = exApp.Workbooks.Add(Environment.CurrentDirectory + @"\Templates\Счет.xlt");
                Excel.Worksheet exWh = exWb.Sheets[1];

                rowdel = 0;
                exWh.Cells[3, 12] = row.invoiceid.ToString() + " от " + row.invoicedate.ToShortDateString();
                InvoiceDS.tableCustomerNameRow customer = row.tableCustomerNameRow;
                exWh.Cells[6, 6] = customer.IscustomerFullNameNull() ? customer.customerName : customer.customerFullName;
                //exWh.Cells[8, 6] = row.legalName;
                exWh.Rows[8].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                exWh.Rows[8].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                exWh.Rows[8].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                exWh.Rows[8].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                exWh.Rows[8].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                rowdel = 5;
                dr = 0;
                Excel.Range range;
                foreach(InvoiceDS.tableInvoiceDetailRow detrow in row.GettableInvoiceDetailRows())
                {
                    if (dr > 0)
                    {
                        range = exWh.Rows[15 - rowdel + dr];
                        range.Insert(Excel.XlInsertShiftDirection.xlShiftDown, false);

                        exWh.Range[exWh.Cells[15 - rowdel + dr, 2], exWh.Cells[15 - rowdel + dr, 3]].MergeCells = true;
                        exWh.Range[exWh.Cells[15 - rowdel + dr, 4], exWh.Cells[15 - rowdel + dr, 17]].MergeCells = true;
                    }
                    exWh.Cells[15 - rowdel + dr, 2] = dr + 1;
                    exWh.Cells[15 - rowdel+dr, 4] = detrow.detdescription;
                    exWh.Cells[15 - rowdel + dr, 18] = detrow.detamount;
                    exWh.Cells[15 - rowdel + dr, 19] = decimal.Divide(decimal.Ceiling(detrow.detprice * 100M), 100M);
                    exWh.Cells[15 - rowdel+dr, 20].Value = decimal.Divide(decimal.Ceiling(detrow.detsum * 100M), 100M);
                    sumtot = sumtot + detrow.detsum;
                    dr++;
                }
                if (dr == 0) dr++;
                range = exWh.Range[exWh.Cells[15 - rowdel, 2], exWh.Cells[15 - rowdel + dr-1, 20]];
                range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
                range.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;

                exWh.Cells[17 - rowdel + dr-1, 20].Formula = "=SUM(T" + (15 - rowdel).ToString() + ":T" + (15 - rowdel+dr-1).ToString() + ")";

                range = exWh.Range[exWh.Rows[18 - rowdel+dr-1], exWh.Rows[26 - rowdel+dr-1]];
                range.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                rowdel = 14;

                dept = CurrentDebt(row.invoiceid);
                sumtot = sumtot + dept;
                if (dept != 0M)
                {
                    exWh.Cells[28 - rowdel + dr - 1, 20] = decimal.Ceiling(dept * 100M) / 100M;
                }
                else
                {
                    exWh.Rows[28 - rowdel + dr - 1].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                    rowdel++;
                }

                CurrAmountWords words = new CurrAmountWords();
                exWh.Cells[29 - rowdel + dr - 1, 20] = decimal.Round(sumtot, 0);
                exWh.Cells[30 - rowdel + dr - 1, 10] = dr;
                exWh.Cells[31 - rowdel + dr - 1, 2] = words.AmountCurrWords((decimal)exWh.Cells[29 - rowdel + dr - 1, 20].Value, (CurrAmountWords.CurrencyName)Enum.Parse(typeof(CurrAmountWords.CurrencyName), "RUB", true));

                exApp.Visible = true;
            }
            catch (Exception ex)
            {
                if (exApp != null)
                {
                    foreach (Excel.Workbook itemBook in exApp.Workbooks)
                    {
                        itemBook.Close(false);
                    }
                    exApp.Quit();
                }
                throw ex;
            }
            finally
            {
                exApp.DisplayAlerts = true;
                exApp.ScreenUpdating = true;
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }

        private decimal CurrentDebt(int invoiceid)
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "account.InvoiceCurrentDebt";
                SqlParameter id = new SqlParameter("@invoiceid", invoiceid);
                SqlParameter debt = new SqlParameter();
                debt.Direction = ParameterDirection.Output;
                debt.ParameterName = "@debt";
                debt.SqlDbType = SqlDbType.Money; debt.DbType = DbType.Decimal;
                cmd.Parameters.Add(id); cmd.Parameters.Add(debt);
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return (decimal)debt.Value;
            }
        }
    }
}
