using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Game
{
    class Log 
    {
       


        /// <summary>
        /// 寫Trace Log
        /// </summary>
        /// <param name="TxtName">要建的txt文件名稱</param>
        /// <param name="Conditions">判斷式真假</param>
        /// <param name="TraceName">這個事件名稱</param>
        public void WriteTraceLogTxt(string TxtName, bool Conditions, string TraceName)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "Log\\";
            string strFileName = System.DateTime.Now.ToString("yyyyMMdd");
            string strFilePath= strPath + strFileName;
            string strTime= System.DateTime.Now.ToString("HH:mm:ss");
            string strMemo = "[" + strTime + "] " + TxtName;
            // 添加 TraceListener  txtName
            Trace.Listeners.Add(new TextWriterTraceListener(TxtName));
            Trace.Listeners.Add(new ConsoleTraceListener());
            // 斷言 false出現TraceName
            Trace.Assert(Conditions, TraceName);
            // 确保所有輸出都被寫入到文件中
            Trace.Flush();
            // 移除所有 TraceListener
            Trace.Listeners.Clear();
        }
     

        /// <summary>
        /// 寫ConsoleLog
        /// </summary>
        /// <param name="strMsg"></param>
        public void WriteConsoleWindowLog(string strMsg)
        {
            try
            {
                //資料夾名稱
                string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";
                //檔案名稱，可以加入ID顯示不同
                string strFileName =  System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string strFilePath = strPath + strFileName;
                //生成日期
                string strTime = System.DateTime.Now.ToString("HH:mm:ss");
                //組合
                string strMemo = "[" + strTime + "] " + strMsg;

                if (System.IO.Directory.Exists(strPath) == false)
                {
                    System.IO.Directory.CreateDirectory(strPath);
                }
                //System.IO.StreamWriter file = new System.IO.StreamWriter(strFilePath, true);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFilePath, true))
                {
                    file.WriteLine(strMemo);
                    //file.Close();
                    //file.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 寫WindowForm Log
        /// </summary>
        /// <param name="strMsg"></param>
        public void WriteWindowFormLog(string strMsg)
        {
            try
            { 
                //資料夾名稱
                string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "Log\\";
                //檔案名稱，可以加入ID顯示不同
                string strFileName = System.DateTime.Now.ToString("yyyyMMdd");
                string strFilePath = strPath + strFileName + ".txt";
                //生成日期、組合
                string strTime = System.DateTime.Now.ToString("HH:mm:ss");
                string strMemo = "[" + strTime + "] " + strMsg;

                if (System.IO.Directory.Exists(strPath) == false)
                {
                    System.IO.Directory.CreateDirectory(strPath);
                }
                //System.IO.StreamWriter file = new System.IO.StreamWriter(strFilePath, true);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFilePath, true))
                {
                    file.WriteLine(strMemo);
                    //file.Close();
                    //file.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LOG_CONTENT"></param>
        /// <param name="LOG_PARM"></param>
        public void WriteLogDB(string LOG_CONTENT, string LOG_PARM = "")
        {
            try
            {
                //寫入資料庫 INSERT_INTO SYS_LOG
                //SQL s.SYS_LOG(USER_ID,LOG_CONTENT,LOG_PARM); 
            }
            catch (Exception ex)
            {
                //WriteLogTXT("寫入DB失敗");
            }
        }

        /// <summary>
        /// 刪除大於30天的LOG
        /// </summary>
        public static void DelLogTXT()
        {
            try
            {
                string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";

                if (System.IO.Directory.Exists(strPath) == true)
                {
                    DirectoryInfo diF = new DirectoryInfo(strPath);
                    FileInfo[] fiF = diF.GetFiles("*.txt");
                    foreach (FileInfo tmpfi in fiF)
                    {
                        TimeSpan ts = DateTime.Now.Subtract(tmpfi.LastWriteTime);
                        if (ts.Days > 30)
                        {
                            tmpfi.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="strMsg"></param>
        //public void WriteMVCLog(string strMsg)
        //{

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="strMsg"></param>
        //public void WriteDotNetCoreLog(string strMsg)
        //{

        //}


    }
}
