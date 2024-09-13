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
        static public string ConvertCommandToSqlFormat(string Command)
        {
            String ret = Command;
            ret = ret.Replace("\"", "'");
            ret = ret.Replace("`", " ");
            return ret.ToString();
        }

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
