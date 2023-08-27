/* To work eith EPPlus library */
using OfficeOpenXml;
using System;
using System.Collections.Generic;
/* For I/O purpose */
using System.IO;

/* For Diagnostics */

namespace TopStar_Invoice_Maker_SQLSever.Controllers.Excel.InvoiceMaker
{
    class InvoiceMaker_EPPlus
    {
        private string fileName;

        private string filePath;
        private string excelPath;
        private string rawPath;
        Models.Invoice my_Invoice;
        private string theInvoiceNo;

        private void setFilePath(string inInvoiceNo)
        {
            filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            excelPath = filePath + "\\INVOICE\\Invoice_" + inInvoiceNo + ".xlsx";
            theInvoiceNo = inInvoiceNo;
        }

        private void copyFileToShareDrive()
        {
            string machineName = Environment.MachineName;
            string str_IP = @"\\hkfile\TopStar_sales\Invoice";
            DateTime today = Convert.ToDateTime(DateTime.Now);
            string str_year = today.Year.ToString();
            if (machineName == "TOPSTAR-KEVIN")
            {
                str_IP = @"D:\Google Drive\Top Star\Invoice";
                File.Copy(excelPath, $"{str_IP}\\{str_year}\\Invoice_" + theInvoiceNo + ".xlsx", true);
            }
            else if (machineName == "LENOVO-PC")
            {
                str_IP = @"C:\Users\dennis0713\Google Drive\Top Star\Invoice";
                File.Copy(excelPath, $"{str_IP}\\{str_year}\\Invoice_" + theInvoiceNo + ".xlsx", true);
            }

            try
            {
                File.Copy(excelPath, $"{str_IP}\\{str_year}\\Invoice_" + theInvoiceNo + ".xlsx", true);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"There is an error to write the invoice excel file to the share drive: \r\n {e}" );
            }
        }

        private void copyNewExcelFile()
        {
            File.Copy(rawPath, excelPath, true);
        }

        public InvoiceMaker_EPPlus(Models.Invoice in_Invoice)
        {
            my_Invoice = new Models.Invoice();
            my_Invoice = in_Invoice;

            //setFilePath(in_Invoice.Invoice_ID, in_Invoice.Clinic_Name);

            setFilePath(in_Invoice.Invoice_ID);
            rawPath = filePath + "\\RAW\\Invoice_Template_NEW.xlsx";

            fileName = excelPath;
        }

        public void MakeNewInvoice()
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
                var invoiceWorkSheet = package.Workbook.Worksheets["Invoice"];

                //Set Header Information
                invoiceWorkSheet.Cells["F5"].Value = my_Invoice.CustomerReferenceCode; // PO Ref.
                invoiceWorkSheet.Cells["C7"].Value = my_Invoice.Clinic_Name; // Clinic Name
                invoiceWorkSheet.Cells["C8"].Value = my_Invoice.Address; // Address
                invoiceWorkSheet.Cells["C10"].Value = my_Invoice.ContactPerson; // Attn
                invoiceWorkSheet.Cells["C11"].Value = my_Invoice.Telephone; // Tel
                invoiceWorkSheet.Cells["G7"].Value = my_Invoice.SHIPTO_Clinic_Name; // Shop to Clinic Name
                invoiceWorkSheet.Cells["G8"].Value = my_Invoice.SHIPTO_Address; // Shop to Address
                invoiceWorkSheet.Cells["G10"].Value = my_Invoice.SHIPTO_ContactPerson; // Shop to Attn
                invoiceWorkSheet.Cells["G11"].Value = my_Invoice.SHIPTO_Telephone; // Shop to Tel
                invoiceWorkSheet.Cells["F3"].Value = my_Invoice.Invoice_ID; // Invoice No.
                invoiceWorkSheet.Cells["H3"].Value = my_Invoice.Invoice_Date; // Date
                invoiceWorkSheet.Cells["H5"].Value = my_Invoice.Clinic_ID; // Customer ID
                invoiceWorkSheet.Cells["A16"].Value = my_Invoice.Sales_Person; // Sales Person
                invoiceWorkSheet.Cells["D16"].Value = my_Invoice.Invoice_Date; // Delivery Date
                invoiceWorkSheet.Cells["E16"].Value = my_Invoice.Delivery; // Delivery Method
                invoiceWorkSheet.Cells["F16"].Value = my_Invoice.Payment_Terms; // Payment Terms
                invoiceWorkSheet.Cells["G16"].Value = "CASH/CHEQUE"; // Payment Method
                invoiceWorkSheet.Cells["I16"].Formula = "=EDATE(D16,1)"; // Payment Date
                invoiceWorkSheet.Cells["A14"].Value = my_Invoice.Remark; // Remark

                //Set Body
                int insertedRow = 0;
                List<Models.Invoice_Item> tempInvoiceItem = new List<Models.Invoice_Item>();
                tempInvoiceItem = my_Invoice.ItemList;

                int NumberOfRows = tempInvoiceItem.Count;
                int start_Row = 18;
                int currentRow = 0;

                for (int i = 1; i <= NumberOfRows; i++)
                {
                    currentRow = start_Row + i;
                    if (currentRow > 20)
                    {
                        invoiceWorkSheet.InsertRow(currentRow, 1);
                        invoiceWorkSheet.Cells[string.Format("A{0}:I{0}", currentRow + 1)].Copy(invoiceWorkSheet.Cells[string.Format("A{0}:I{0}", currentRow)]);
                        invoiceWorkSheet.Cells[string.Format("B{0}:C{0}", currentRow)].Merge = true;
                        invoiceWorkSheet.Cells[string.Format("D{0}:E{0}", currentRow)].Merge = true;
                        insertedRow = insertedRow + 1;
                    }

                    invoiceWorkSheet.Cells["A" + (currentRow)].Value = i + ") "; // Number of record
                    invoiceWorkSheet.Cells["B" + (currentRow)].Value = tempInvoiceItem[i - 1].Model; // Product ID
                    if (!string.IsNullOrWhiteSpace(tempInvoiceItem[i - 1].Item_Remark.Trim()))
                    {
                        invoiceWorkSheet.Cells["D" + (currentRow)].Value = tempInvoiceItem[i - 1].Brand + " " + tempInvoiceItem[i - 1].Product_Descruibtions + " " + tempInvoiceItem[i - 1].Item_Remark; // Describtion
                    }
                    else
                    {
                        invoiceWorkSheet.Cells["D" + (currentRow)].Value = tempInvoiceItem[i - 1].Brand + " " + tempInvoiceItem[i - 1].Product_Descruibtions;
                    }

                    invoiceWorkSheet.Cells["F" + (currentRow)].Value = Convert.ToInt64(tempInvoiceItem[i - 1].Qty); // Qty
                    invoiceWorkSheet.Cells["G" + (currentRow)].Value = Convert.ToDouble(tempInvoiceItem[i - 1].Unit_Price); // Unit Price
                    invoiceWorkSheet.Cells["H" + (currentRow)].Value = Convert.ToDouble(tempInvoiceItem[i - 1].Net_Price); // Net Price
                    invoiceWorkSheet.Cells["I" + (currentRow)].Value = Convert.ToDecimal(tempInvoiceItem[i - 1].Total); // Amount
                }
                //invoiceWorkSheet.DeleteRow((currentRow + 1), 1);

                //Set Footer
                int startingLocation = 22 + insertedRow;

                if (!string.IsNullOrWhiteSpace(my_Invoice.Discount.Trim()))
                {
                    invoiceWorkSheet.Cells["H" + (startingLocation + 1)].Value = "-" + my_Invoice.Discount; // Discount
                }
                else
                {
                    invoiceWorkSheet.Cells["H" + (startingLocation + 1)].Value = "$0.0";
                }

                if (insertedRow == 0)
                {
                    invoiceWorkSheet.Cells["H" + startingLocation].Formula = "=SUM(I19:I21)"; // SubTotal
                    invoiceWorkSheet.Cells["H" + (startingLocation + 2)].Formula = "=H22+H23"; //Total
                }
                else
                {
                    invoiceWorkSheet.Cells["H" + startingLocation].Formula = "=SUM(I19:I" + (19 + insertedRow+2) + ")"; // SubTotal
                    invoiceWorkSheet.Cells["H" + (startingLocation + 2)].Formula = string.Format("=H{0}+H{1}", (22 + insertedRow),(23 + insertedRow)); //Total
                }
               
                invoiceWorkSheet.Cells["B" + (startingLocation + 10)].Formula = "=IF(C7 <> \"\",C7,\" \")"; //Clinic Name
                invoiceWorkSheet.Cells["B" + (startingLocation + 16)].Formula = "=IF(C10 <> \"\",C10,\" \")"; //Contact Person
                invoiceWorkSheet.Cells["G" + (startingLocation + 16)].Formula = "=IF(A16 <> \"\",A16,\" \")"; //Sales

                package.Save();
            }

            copyFileToShareDrive();
        }
    }
}
