using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.Functional
{
    class FileRenamer
    {
        string[] exitingFiles;
        string dNotePath;
        string backupPath;
        string newFilePath;
        //List<string> exitingFiles_in_Name;

        public FileRenamer()
        {
            dNotePath = getCurrentPath() + "\\DNOTE\\";
            backupPath = getCurrentPath() + "\\BACKUP\\";
            newFilePath = getCurrentPath() + "\\REVISED\\";
        }

        private string getCurrentPath()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\";
        }

        private void listAllExitingFiles()
        {
            string path = dNotePath;
            string[] fileArray = Directory.GetFiles(path, "*.XLSX");
            exitingFiles = fileArray;
        }

        public List<string> getList_in_Name(string inClinicCode)
        {
            List<string> result = new List<string>();
            //exitingFiles_in_Name = new List<string>();

            listAllExitingFiles();
            foreach (string fileName in exitingFiles)
            {
                if (fileName.IndexOf(inClinicCode) != -1)
                {
                    //string theResult = fileName.Replace(dNotePath,"");
                    result.Add(fileName);
                }
            }

            //exitingFiles_in_Name = result;
            return result;
        }

        public void copyFileAsBackUp(string fileName)
        {
            string theFileLocation = dNotePath + fileName;
            System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            string theDateofToday = DateTime.Now.ToString("yyyyMMdd", en);

            string theNEW_FileName = fileName.Replace(".xlsx", "") + "_" + theDateofToday + ".xlsx";
            string theNewFileLocation = backupPath + theNEW_FileName;

            File.Copy(theFileLocation, theNewFileLocation, true);
        }

        public void copyFileAsRevise(string OldFileName, string NewFileName)
        {
            string theOldFileLocation = dNotePath + OldFileName;
            string theNewFileLocation = newFilePath + NewFileName + ".xlsx";

            File.Copy(theOldFileLocation, theNewFileLocation, true);
        }

        /*
        public void updateREFCodeToRevisedFile(CLASS.INV_Clinic inClinic, string inFile)
        {
            CLASS.INV_Clinic_Table table = new CLASS.INV_Clinic_Table(inFile);
            table.updateCode(inClinic);
        }*/
    }
}
