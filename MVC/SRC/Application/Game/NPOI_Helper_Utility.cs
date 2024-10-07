using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class NPOI_Helper_Utility
    {
        User us = new User();
        #region xlsx 2007 新版
        public void CreateNewExcel_Format_XLSX(string FileName)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory +"Excel\\";
            string strFileName = FileName + System.DateTime.Now.ToString("yyyyMMdd")+".xlsx";
            string strFilePath = strPath + strFileName;
            string strTime = System.DateTime.Now.ToString("HH:mm:ss");
            string strMemo = "[" + strTime + "] " + "CreateExcel File";

            if (System.IO.Directory.Exists(strPath) == false)
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
            // 創建一個工作簿
            IWorkbook workbook = new XSSFWorkbook(); // 使用 XSSFWorkbook 創建 .xlsx 文件
            ISheet sheet = workbook.CreateSheet("Sample Sheet"); // 創建一個工作表

            // 創建一行
            IRow row = sheet.CreateRow(0); // 第 0 行
                                           // 創建單元格並設置值
            row.CreateCell(0).SetCellValue("Hello");
            row.CreateCell(1).SetCellValue("World!");
            row.CreateCell(2).SetCellValue(strMemo);

            // 創建第二行
            row = sheet.CreateRow(1);  //第 1 行
            row.CreateCell(0).SetCellValue(123);
            row.CreateCell(1).SetCellValue(DateTime.Now.ToString("yyyy-MM-dd"));

            // 寫入 Excel 文件
            using (var fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs); // 將工作簿寫入檔案
            }

            Console.WriteLine("Excel 文件已創建!");

        }

        public void CreateNewExcel_Foreach_XLSX(string FileName)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "Excel\\";
            string strFileName = FileName + System.DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            string strFilePath = strPath + strFileName;
            string strTime = System.DateTime.Now.ToString("HH:mm:ss");
            string strMemo = "[" + strTime + "] " + "CreateExcel File";

            if (System.IO.Directory.Exists(strPath) == false)
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
            // 創建一個工作簿
            IWorkbook workbook = new XSSFWorkbook(); // 使用 XSSFWorkbook 創建 .xlsx 文件
            ISheet sheet = workbook.CreateSheet("Create ExpTable Data"); // 創建一個工作表

            IRow row;
            for (int i=0;i<100;i++)
            {
                string requireExp = us.RequiredExperience(i+1).ToString();
                row = sheet.CreateRow(i);
                row.CreateCell(1).SetCellValue(requireExp);
            }

            // 寫入 Excel 文件
            using (var fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs); // 將工作簿寫入檔案
            }

            Console.WriteLine("Excel 文件已創建!");

        }

        #endregion

        #region xls  2003 舊版

        #endregion
    }
}
