/* To work eith EPPlus library */
using OfficeOpenXml;
using System.Collections.Generic;
/* For I/O purpose */
using System.IO;
using System.Linq;

/* For Diagnostics */

namespace TopStar_Invoice_Maker_SQLSever.Controllers.Excel.OpeningStock
{
    class OpeningStock
    {
        public string Supplier { get; set; }
        public string Brand { get; set; }
        public string ProductCode { get; set; }
        public string Unit_Price { get; set; }
        public string Qty { get; set; }
    }

    class OpeningStock_Table : OpeningStock
    {
        public string TableName { get; set; }

        public OpeningStock_Table()
        {
            TableName = "Inventory_Opening_2019";
            Supplier = "Supplier";
            Brand = "Brand";
            ProductCode = "Product_Code";
            Unit_Price = "Cost";
            Qty = "Qty";
        }
    }

    class OpeningStockReader
    {
        private string filePath;
        private string excelPath;
        //private List<OpeningStock> openingStock_List;

        public OpeningStockReader()
        {
            string theFile = "Opening_Stock_2019";
            setFilePath(theFile);
        }

        private void setFilePath(string infileName)
        {
            filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            excelPath = filePath + "\\RAW\\" + infileName + ".xlsx";
        }

        public List<OpeningStock> ReadFromExcel()
        {
            List<OpeningStock> openingStock_List = new List<OpeningStock>();
            Opening_Stock.Stock_Item_Convertor stock_Item_Convertor = new Opening_Stock.Stock_Item_Convertor();

            using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //載入Excel檔案
                using (ExcelPackage ep = new ExcelPackage(fs))
                {
                    ExcelWorksheet sheet = ep.Workbook.Worksheets[1];//取得Sheet1
                    int startRowNumber = 4; //sheet.Dimension.Start.Row;//起始列編號，從1算起
                    int endRowNumber = sheet.Dimension.End.Row;//結束列編號，從1算起
                    int startColumn = sheet.Dimension.Start.Column;//開始欄編號，從1算起
                    int endColumn = sheet.Dimension.End.Column;//結束欄編號，從1算起


                    bool isHeader = true;
                    if (isHeader)//有包含標題
                    {
                        startRowNumber += 1;
                    }

                    ////寫入標題文字
                    //sheet.Cells[1, 1].Value = "第1欄";
                    //sheet.Cells[1, 2].Value = "第2欄";
                    for (int currentRow = startRowNumber; currentRow <= endRowNumber; currentRow++)
                    {
                        ExcelRange range = sheet.Cells[currentRow, startColumn, currentRow, endColumn];//抓出目前的Excel列
                        if (range.Any(c => !string.IsNullOrEmpty(c.Text)) == false)//這是一個完全空白列(使用者用Delete鍵刪除動作)
                        {
                            continue;//略過此列
                        }
                        //讀值
                        string myQty = sheet.Cells[currentRow, 39].Text;

                        if (!string.IsNullOrEmpty(myQty))
                        {
                            if (myQty != "0")
                            {
                                openingStock_List.Add(new OpeningStock
                                {
                                    Supplier = stock_Item_Convertor.Convert_Supplier(sheet.Cells[currentRow, 2].Text),
                                    Brand = sheet.Cells[currentRow, 2].Text,
                                    ProductCode = sheet.Cells[currentRow, 3].Text,
                                    Unit_Price = zeroDector(sheet.Cells[currentRow, 5].Text),
                                    Qty = myQty
                                });
                            }                            
                        }
                    }
                }//end   using 
            }//end using 
            return openingStock_List;
        }


        private string zeroDector(string inString)
        {
            if (inString == "-")
                return "0";
            else
                return inString;
        }

    }
}
