using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using THE_Excel = Microsoft.Office.Interop.Excel;
using System.Threading;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.Excel.InvoiceMaker
{
    class InvoiceMaker10
    {
        private string fileName;
        THE_Excel.Application xlApp;
        THE_Excel.Workbook xlWorkbook;
        THE_Excel._Worksheet xlWorksheet;
        THE_Excel._Worksheet xlWorksheet_2;

        private string filePath;
        private string excelPath;
        private string rawPath;
        Models.Invoice my_Invoice;

        private void setFilePath(string inInvoiceNo)
        {
            filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            excelPath = filePath + "\\INVOICE\\Invoice_" + inInvoiceNo + ".xlsx";
        }
        /*
        private void setFilePath(string inInvoiceNo, string inCustomer)
        {
            filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            excelPath = filePath + "\\INVOICE\\Invoice_" + inInvoiceNo + "_" + inCustomer + ".xlsx";
        }*/

        private void copyNewExcelFile()
        {
            File.Copy(rawPath, excelPath, true);
        }

        public InvoiceMaker10(Models.Invoice in_Invoice)
        {
            my_Invoice = new Models.Invoice();
            my_Invoice = in_Invoice;

            //setFilePath(in_Invoice.Invoice_ID, in_Invoice.Clinic_Name);
            setFilePath(in_Invoice.Invoice_ID);
            rawPath = filePath + "\\RAW\\Invoice_Template_10.xlsx";

            fileName = excelPath;
            xlApp = new THE_Excel.Application();
            xlApp.DisplayAlerts = false;
        }

        /*
        public void updateFormula()
        {
            update_Invoice_Formula();
            update_InvoiceItem_Formula();
            setFooter();

            xlWorkbook.Save();
            xlWorkbook.Close();
        }
        */

        public void MakeNewInvoice()
        {
            copyNewExcelFile();

            try
            {
                update_Invoice_Value();
                update_InvoiceItem_Value();
                setFooter();

                xlWorkbook.Save();
                Thread.Sleep(1000);
            }
            finally
            {
                //xlWorkbook.Close(Type.Missing, Type.Missing, Type.Missing);
                xlWorkbook.Saved = true;
                xlWorkbook.Close(true, Type.Missing, Type.Missing);

                //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet_2);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                xlWorksheet = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                xlWorkbook = null;
                xlApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                
                xlWorksheet_2 = null;
                xlApp = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            
        }

        private void update_Invoice_Value()
        {
            xlWorkbook = xlApp.Workbooks.Open(fileName);
            xlWorksheet = xlWorkbook.Worksheets.get_Item("Invoice");
            xlWorksheet.Range["F5"].Value = my_Invoice.CustomerReferenceCode; // PO Ref.
            xlWorksheet.Range["C7"].Value = my_Invoice.Clinic_Name; // Clinic Name
            xlWorksheet.Range["C8"].Value = my_Invoice.Address; // Address
            xlWorksheet.Range["C10"].Value = my_Invoice.ContactPerson; // Attn
            xlWorksheet.Range["C11"].Value = my_Invoice.Telephone; // Tel

            xlWorksheet.Range["G7"].Value = my_Invoice.SHIPTO_Clinic_Name; // Shop to Clinic Name
            xlWorksheet.Range["G8"].Value = my_Invoice.SHIPTO_Address; // Shop to Address
            xlWorksheet.Range["G10"].Value = my_Invoice.SHIPTO_ContactPerson; // Shop to Attn
            xlWorksheet.Range["G11"].Value = my_Invoice.SHIPTO_Telephone; // Shop to Tel

            xlWorksheet.Range["F3"].Value = my_Invoice.Invoice_ID; // Invoice No.
            xlWorksheet.Range["H3"].Value = my_Invoice.Invoice_Date; // Date
            xlWorksheet.Range["H5"].Value = my_Invoice.Clinic_ID; // Customer ID

            xlWorksheet.Range["A16"].Value = my_Invoice.Sales_Person; // Sales Person
            xlWorksheet.Range["D16"].Value = my_Invoice.Invoice_Date; // Delivery Date
            xlWorksheet.Range["E16"].Value = my_Invoice.Delivery; // Delivery Method
            xlWorksheet.Range["F16"].Value = my_Invoice.Payment_Terms; // Payment Terms
            xlWorksheet.Range["G16"].Value = "CASH/CHEQUE"; // Payment Method
            xlWorksheet.Range["I16"].Formula = "=EDATE(D16,1)"; // Payment Date

            xlWorksheet.Range["A14"].Value = my_Invoice.Remark; // Remark
        }


        private void update_InvoiceItem_Value()
        {
            List<Models.Invoice_Item> tempInvoiceItem = new List<Models.Invoice_Item>();
            tempInvoiceItem = my_Invoice.ItemList;

            int NumberOfRows = tempInvoiceItem.Count;
            int start_Row = 18;

            //if (NumberOfRows >= 2)
            //{
            for (int i = 1; i <= NumberOfRows; i++)
            {
                xlWorksheet.Range["A" + (start_Row + i)].Value = i + ") "; // Number of record
                xlWorksheet.Range["B" + (start_Row + i)].Value = tempInvoiceItem[i - 1].Model; // Product ID
                if (!string.IsNullOrWhiteSpace(tempInvoiceItem[i - 1].Item_Remark.Trim()))
                {
                    xlWorksheet.Range["D" + (start_Row + i)].Value = tempInvoiceItem[i - 1].Brand + " " + tempInvoiceItem[i - 1].Product_Descruibtions + " " + tempInvoiceItem[i - 1].Item_Remark; // Describtion
                }
                else
                {
                    xlWorksheet.Range["D" + (start_Row + i)].Value = tempInvoiceItem[i - 1].Brand + " " + tempInvoiceItem[i - 1].Product_Descruibtions;
                }

                xlWorksheet.Range["F" + (start_Row + i)].Value = tempInvoiceItem[i - 1].Qty; // Qty
                xlWorksheet.Range["G" + (start_Row + i)].Value = tempInvoiceItem[i - 1].Unit_Price; // Unit Price
                xlWorksheet.Range["H" + (start_Row + i)].Value = tempInvoiceItem[i - 1].Net_Price; // Net Price
                xlWorksheet.Range["I" + (start_Row + i)].Value = tempInvoiceItem[i - 1].Total; // Amount
            }
            //}
        }

        public InvoiceMaker10(string inFileName)
        {
            fileName = inFileName;
            xlApp = new THE_Excel.Application();
        }

        private void update_Invoice_Formula()
        {
            xlWorkbook = xlApp.Workbooks.Open(fileName);
            xlWorksheet = xlWorkbook.Worksheets.get_Item("Invoice");
            xlWorksheet.Range["C7"].Formula = "=IF(INV_Data!B2 <> \"\",INV_Data!B2,\" \")"; // Clinic Name
            xlWorksheet.Range["C8"].Formula = "=IF(INV_Data!C2 <> \"\",INV_Data!C2,\" \")"; // Address
            xlWorksheet.Range["C10"].Formula = "=IF(INV_Data!D2 <> \"\",INV_Data!D2,\" \")"; // Attn
            xlWorksheet.Range["C11"].Formula = "=IF(INV_Data!E2 <> \"\",INV_Data!E2,\" \")"; // Tel

            xlWorksheet.Range["G7"].Formula = "=IF(INV_Data!N2 <> \"\",INV_Data!N2,\" \")"; // Shop to Clinic Name
            xlWorksheet.Range["G8"].Formula = "=IF(INV_Data!O2 <> \"\",INV_Data!O2,\" \")"; // Shop to Address
            xlWorksheet.Range["G10"].Formula = "=IF(INV_Data!P2 <> \"\",INV_Data!P2,\" \")"; // Shop to Attn
            xlWorksheet.Range["G11"].Formula = "=IF(INV_Data!Q2 <> \"\",INV_Data!Q2,\" \")"; // Shop to Tel

            xlWorksheet.Range["F3"].Formula = "=IF(INV_Data!F2 <> \"\",INV_Data!F2,\" \")"; // Invoice No.
            xlWorksheet.Range["H3"].Formula = "=IF(INV_Data!G2 <> \"\",INV_Data!G2,\" \")"; // Date
            xlWorksheet.Range["H5"].Formula = "=IF(INV_Data!A2 <> \"\",INV_Data!A2,\" \")"; // Customer ID

            xlWorksheet.Range["A16"].Formula = "=IF(INV_Data!H2 <> \"\",INV_Data!H2,\" \")"; // Sales Person
            xlWorksheet.Range["D16"].Formula = "=IF(INV_Data!G2 <> \"\",INV_Data!G2,\" \")"; // Delivery Date
            xlWorksheet.Range["E16"].Formula = "=IF(INV_Data!I2 <> \"\",INV_Data!I2,\" \")"; // Delivery Method
            xlWorksheet.Range["F16"].Formula = "=IF(INV_Data!J2 <> \"\",INV_Data!J2,\" \")"; // Payment Terms
            xlWorksheet.Range["G16"].Formula = "=IF(INV_Data!K2 <> \"\",INV_Data!K2,\" \")"; // Payment Method
            xlWorksheet.Range["I16"].Formula = "=IF(INV_Data!L2 <> \"\",INV_Data!L2,\" \")"; // Payment Date
        }

        private void update_InvoiceItem_Formula()
        {
            xlWorksheet_2 = xlWorkbook.Worksheets.get_Item("INV_Product_Data");
            THE_Excel.Range xlTestRange;
            xlTestRange = xlWorksheet_2.UsedRange;
            int NumberOfRows = xlTestRange.Rows.Count;
            int start_Row = 18;
            if (NumberOfRows >= 2)
            {
                for (int i = 1; i < NumberOfRows; i++)
                {
                    xlWorksheet.Range["A" + (18 + i)].Value = i + ") "; // Number of record
                    xlWorksheet.Range["B" + (18 + i)].Formula = "=IF(INV_Product_Data!B" + (i + 1) + " <> \"\",INV_Product_Data!B" + (i + 1) + ",\" \")"; // Product ID
                    xlWorksheet.Range["D" + (18 + i)].Formula = "=IF(INV_Product_Data!E" + (i + 1) + " <> \"\",INV_Product_Data!C" + (i + 1) + "& \" \" &INV_Product_Data!D" + (i + 1) + "& \" \" &INV_Product_Data!E" + (i + 1) + "& \" \" &INV_Product_Data!K" + (i + 1) + ",\"\")"; // Describtion
                    xlWorksheet.Range["F" + (18 + i)].Formula = "=IF(INV_Product_Data!G" + (i + 1) + " <> \"\",INV_Product_Data!G" + (i + 1) + ",\" \")"; // Qty
                    xlWorksheet.Range["G" + (18 + i)].Formula = "=IF(INV_Product_Data!H" + (i + 1) + " <> \"\",INV_Product_Data!H" + (i + 1) + ",\" \")"; // Unit Price
                    xlWorksheet.Range["H" + (18 + i)].Formula = "=IF(INV_Product_Data!I" + (i + 1) + " <> \"\",INV_Product_Data!I" + (i + 1) + ",\" \")"; // Net Price
                    xlWorksheet.Range["I" + (18 + i)].Formula = "=IF(INV_Product_Data!J" + (i + 1) + " <> \"\",INV_Product_Data!J" + (i + 1) + ",\" \")"; // Amount
                }
            }
        }
        
        private void setFooter()
        {
            if (!string.IsNullOrWhiteSpace(my_Invoice.Discount.Trim()))
            {
                xlWorksheet.Range["H35"].Value = "-" + my_Invoice.Discount; // Discount
            }
            else
            {
                xlWorksheet.Range["H35"].Value = 0;
            }

            xlWorksheet.Range["H36"].Formula = "=H34+H35"; //Total

            xlWorksheet.Range["B44"].Formula = "=IF(C7 <> \"\",C7,\" \")"; //Clinic Name
            xlWorksheet.Range["B51"].Formula = "=IF(C10 <> \"\",C10,\" \")"; //Clinic Name
            xlWorksheet.Range["G51"].Formula = "=IF(A16 <> \"\",A16,\" \")"; //Clinic Name


        }

        

    }
}