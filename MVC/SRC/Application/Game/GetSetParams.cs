using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Game
{
    class GetSetParams
    {
        public string SetFilePath()
        {
            string FilePath = "";

            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                //WebForm
                FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "bin\\" + "mssql.xml";
            }
            else
            {
                //WinForm
                FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "mssql.xml";
            }

            return FilePath;
        }

        /// <summary>
        /// 取得連線字串
        /// </summary> 
        public string GetSqlConnectionStr()
        {
            string FilePath = SetFilePath();

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(FilePath);

               // Common.Security oSec = new Common.Security();
                string strSQL = ds.Tables[0].Rows[0]["MSSQL"].ToString();
                //oSec.strKey = "";
                //oSec.strIV = "";
                return strSQL;
                //return oSec.Decrypt(strSQL);
            }
            catch (Exception ex)
            {
                return "";
                //throw new Exception("");
            }
        }

        /// <summary>
        /// 取得連線字串
        /// </summary> 
        public string GetSqlConnectionStrDS()
        {
            string FilePath = SetFilePath();

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(FilePath);

                //Common.Security oSec = new Common.Security();
                string strSQL = ds.Tables[0].Rows[0]["MSSQL_DS"].ToString();
                //oSec.strKey = "";
                //oSec.strIV = "";
                return strSQL;
                //return oSec.Decrypt(strSQL);
            }
            catch (Exception ex)
            {
                return "";
                //throw new Exception("");
            }
        }

        public string GetSqlConnectionStrJBM()
        {
            string FilePath = SetFilePath();

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(FilePath);

                //Common.Security oSec = new Common.Security();
                string strSQL = ds.Tables[0].Rows[0]["MSSQL_JBM"].ToString();
                //oSec.strKey = "";
                //oSec.strIV = "";
                return strSQL;
                //return oSec.Decrypt(strSQL);
            }
            catch (Exception ex)
            {
                return "";
                //throw new Exception("");
            }
        }

        /// <summary>
        /// 資料庫共用加密
        /// </summary> 
        public string dbEncrypt(string sValue)
        {
            string FilePath = SetFilePath();

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(FilePath);

                //Common.Security oSec = new Common.Security();
                //oSec.strKey = "";
                //oSec.strIV = "";
                return sValue;
                //return oSec.Encrypt(sValue);
            }
            catch (Exception ex)
            {
                return "";
                //throw new Exception("");
            }
        }

        /// <summary>
        /// 資料庫共用解密
        /// </summary> 
        public string dbDecrypt(string sValue)
        {
            string FilePath = SetFilePath();

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(FilePath);

                //Common.Security oSec = new Common.Security();
                //oSec.strKey = "";
                //oSec.strIV = "";
                return sValue;
                //return oSec.Decrypt(sValue);
            }
            catch (Exception ex)
            {
                return "";
                //throw new Exception("");
            }
        }
    }
}
