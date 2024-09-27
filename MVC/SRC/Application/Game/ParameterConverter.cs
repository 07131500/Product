using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class ParameterConverter
    {
        /// <summary>
        /// 把"轉成'，把`轉成空白
        /// </summary>
        /// <param name="Command">字串語法</param>
        /// <returns></returns>
        static public string ConvertCommandToSqlFormat(string Command)
        {
            String ret = Command;
            ret = ret.Replace("\"", "'");
            ret = ret.Replace("`", " ");
            return ret.ToString();
        }
        /// <summary>
        /// 如果不是NULL返回變數參數，是NULL使用ConvertParaToSql方法
        /// </summary>
        /// <param name="cmd">字串語法</param>
        /// <param name="Para">變數參數</param>
        /// <returns></returns>
        public static SqlParameter[] ConvertParaToSqlPara(ref String cmd, object[] Para)
        {
            Trace.Assert(Para != null);
            SqlParameter[] ret = null;
            try
            {
                ret = (SqlParameter[])Para;
            }
            catch (Exception e)
            {
            }
            if (ret != null)
            {
                return ret;
            }
            SqlParameter[] par = (SqlParameter[])Para;
            ret = ConvertParaToSql(ref cmd, par);
            return ret;
        }
        /// <summary>
        /// 如果參數命名為:而不是@開頭，改成@開頭
        /// </summary>
        /// <param name="cmd">字串語法</param>
        /// <param name="Para">變數參數</param>
        /// <returns></returns>
        public static SqlParameter[] ConvertParaToSql(ref String cmd, SqlParameter[] Para)
        {
            Trace.Assert(Para != null);
            SqlParameter[] ret = new SqlParameter[Para.Length];
            for (int i = 0; i < Para.Length; i++)
            {
                string pName = Para[i].ParameterName.ToString();
                string pValue = Para[i].Value.ToString();
                string pNameA = pName.Replace(":", "@");
                cmd = cmd.Replace(pName, pNameA);
                ret[i] = new SqlParameter(pNameA, pValue);
            }
            return ret;
        }
    }
}
