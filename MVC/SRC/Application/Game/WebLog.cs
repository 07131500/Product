using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class WebLog : System.Web.UI.Page
    {
        public string mUserID = "";
        /// <summary>
        /// 寫log
        /// </summary>
        /// <param name="strMsg">log訊息</param>
        public void WriteWebFormLog(string strMsg)
        {
            string strPath = this.Server.MapPath("~") + "\\Log\\";
            string strFileName = System.DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-");
            string strFilePath = strPath + strFileName + ".txt";

            string strTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            string strIP = Request.UserHostAddress;
            if (strIP == "::1")
            {
                strIP = "127.0.0.1";
            }
            strIP += ":" + Request.Url.Port;

            string strMemo = strTime + " [" + strIP + "]" + "[" + mUserID + "]" + "\r\n" + strMsg + "\r\n";
            if (System.IO.Directory.Exists(strPath) == false)
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter(strFilePath, true);
            file.WriteLine(strMemo);
            file.Close();
            file.Dispose();
        }



    }
}
