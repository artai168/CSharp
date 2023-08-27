/* To work eith EPPlus library */
using OfficeOpenXml;
using System;
using System.Collections.Generic;
/* For I/O purpose */
using System.IO;

/* For Diagnostics */

namespace TopStar_Invoice_Maker_SQLSever.Controllers.Excel.StatementMaker
{
    class StatementMaker_EPPlus
    {
        private string fileName;

        private string filePath;
        private string excelPath;
        private string rawPath;
        Models.Statement my_Statement;
        private string theStatementNo;

        private void setFilePath(string inStatementNo)
        {
            filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            excelPath = filePath + "\\STATEMENT\\STATEMENT_" + inStatementNo + ".xlsx";
            theStatementNo = inStatementNo;
        }

        public StatementMaker_EPPlus(Models.Statement in_Statement)
        {
            my_Statement = new Models.Statement();
            my_Statement = in_Statement;

            //setFilePath(in_Invoice.Invoice_ID, in_Invoice.Clinic_Name);

            setFilePath(in_Statement.ID);
            rawPath = filePath + "\\RAW\\STATEMENT_Template.xlsx";

            fileName = excelPath;
        }

        private void copyFileToShareDrive()
        {
            string machineName = Environment.MachineName;
            if (machineName == "TOPSTAR-KEVIN")
            {
                File.Copy(excelPath, "D:\\Google Drive\\Top Star\\Orders\\STATEMENT\\STATEMENT_" + theStatementNo + ".xlsx", true);
            }
        }

        public void MakeNewStatement()
        {
            // Taking existing file: 'Sample1.xlsx'. Here 'Sample1.xlsx' is treated as template file
            FileInfo templateFile = new FileInfo(rawPath);
            // Making a new file 'Sample2.xlsx'
            FileInfo newFile = new FileInfo(excelPath);

            // If there is any file having same name as 'Sample2.xlsx', then delete it first
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(excelPath);
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                // Openning first Worksheet of the template file i.e. 'Sample1.xlsx'
                //ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                var invoiceWorkSheet = package.Workbook.Worksheets["Statement"];

                //Set Header Information
                invoiceWorkSheet.Cells["F3"].Value = my_Statement.ID; // PO Ref.
                invoiceWorkSheet.Cells["H3"].Value = my_Statement.Statement_Date; // Clinic Name
                invoiceWorkSheet.Cells["H5"].Value = my_Statement.The_Clinic.Clinic_ID; // ClinicID
                invoiceWorkSheet.Cells["C7"].Value = my_Statement.The_Clinic.Clinic_Name; // Clinic Name
                invoiceWorkSheet.Cells["C8"].Value = my_Statement.The_Clinic.Address; // Address
                invoiceWorkSheet.Cells["C10"].Value = my_Statement.The_Clinic.ContactPerson; // Shop to Clinic Name
                invoiceWorkSheet.Cells["C11"].Value = my_Statement.The_Clinic.Telephone; // Shop to Address
                

                //Set Body
                int insertedRow = 0;
                List<Models.Invoice> tempInvoiceItems = new List<Models.Invoice>();
                tempInvoiceItems = my_Statement.Invoice_List;

                int NumberOfRows = tempInvoiceItems.Count;
                int start_Row = 16;
                int currentRow = 0;

                for (int i = 1; i <= NumberOfRows; i++)
                {
                    currentRow = start_Row + i;
                    if (currentRow > 21)
                    {
                        invoiceWorkSheet.InsertRow(currentRow, 1);
                        invoiceWorkSheet.Cells[string.Format("A{0}:I{0}", currentRow + 1)].Copy(invoiceWorkSheet.Cells[string.Format("A{0}:I{0}", currentRow)]);
                        invoiceWorkSheet.Cells[string.Format("B{0}:C{0}", currentRow)].Merge = true;
                        invoiceWorkSheet.Cells[string.Format("D{0}:E{0}", currentRow)].Merge = true;
                        invoiceWorkSheet.Cells[string.Format("F{0}:G{0}", currentRow)].Merge = true;
                        invoiceWorkSheet.Cells[string.Format("H{0}:I{0}", currentRow)].Merge = true;
                        insertedRow = insertedRow + 1;
                    }

                    invoiceWorkSheet.Cells[string.Format("A{0}", currentRow)].Value = i ; // Number of record
                    invoiceWorkSheet.Cells[string.Format("B{0}", currentRow)].Value = tempInvoiceItems[i - 1].Invoice_Date; // Invoice Date
                    invoiceWorkSheet.Cells[string.Format("D{0}", currentRow)].Value = tempInvoiceItems[i - 1].Invoice_ID; // Invoice ID
                    invoiceWorkSheet.Cells[string.Format("F{0}", currentRow)].Value = Convert.ToDouble(tempInvoiceItems[i - 1].Total); // Invoice Amount
                    invoiceWorkSheet.Cells[string.Format("H{0}", currentRow)].Value = tempInvoiceItems[i - 1].Due_Date; // Invoice Amount
                }
                //invoiceWorkSheet.DeleteRow((currentRow + 1), 1);

                //Set Footer
                int startingLocation = 23 + insertedRow;


                if (insertedRow == 0)
                {
                    invoiceWorkSheet.Cells["I23"].Formula = "=SUM(F17:G22)"; // SubTotal
                    invoiceWorkSheet.Cells["G38"].Value = my_Statement.The_Clinic.SalesPerson; // Sales Person
                }
                else
                {
                    invoiceWorkSheet.Cells["I" + (startingLocation)].Formula = string.Format("=SUM(F17:G{0})", (17 + insertedRow)); //Total
                    invoiceWorkSheet.Cells[string.Format("G{0}", (38 + insertedRow))].Value = my_Statement.The_Clinic.SalesPerson; // Sales Person
                }

                package.Save();
            }

            copyFileToShareDrive();
        }


    }
}
