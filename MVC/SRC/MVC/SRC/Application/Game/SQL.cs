using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class SQL
    {
        private SqlConnection conn;
        private SqlCommand comm;

        public SQL()
        {

        }

        public SQL(string SqlConn)
        {
            conn = new SqlConnection(SqlConn);
        }


        public bool CreateUser(string account, string password)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            strSql.AppendLine("INSERT INTO User_Info (userId,password)  ");
            strSql.AppendLine("VALUES ( @account , @password ) ");

            //避免SQL注入
            sqlParamList.Add(new SqlParameter("@ACCOUNT", account));
            sqlParamList.Add(new SqlParameter("@PASSWORD", password));

            conn.Open();
            comm = new SqlCommand(strSql.ToString(), conn);
            comm.Parameters.AddRange(sqlParamList.ToArray());

            var storedPasswordHash = comm.ExecuteNonQuery();
            conn.Close();
            if (storedPasswordHash ==1)
            {
                return true;
            }

            return false;
        }

        public DataTable GetUserInfo(string account)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            strSql.AppendLine("SELECT * FROM User_Info WHERE 1=1 ");

            if (account != "")
            {
                strSql.AppendLine("AND userId=@account ");
                sqlParamList.Add(new SqlParameter ("@account", account ));
            }


            conn.Open();
            comm = new SqlCommand(strSql.ToString(), conn);
            comm.Parameters.AddRange(sqlParamList.ToArray());

            SqlDataAdapter storedPasswordHash = new SqlDataAdapter(comm);
            storedPasswordHash.Fill(dt);
            conn.Close();
            return dt;
        }


        public bool IsUser(string account, string password)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            
            strSql.AppendLine("SELECT * FROM User_Info WHERE 1=1 ");
            strSql.AppendLine("AND userId=@account AND password=@password ");

            //避免SQL注入，使用 SqlParameter 添加参数
            sqlParamList.Add(new SqlParameter("@ACCOUNT", account));
            sqlParamList.Add(new SqlParameter("@PASSWORD", password));
           

            conn.Open();
            comm = new SqlCommand(strSql.ToString(), conn);  
            comm.Parameters.AddRange(sqlParamList.ToArray());

            var storedPasswordHash = comm.ExecuteScalar();
            conn.Close();
            if (storedPasswordHash != null)
            {
                return true;
            }

            return false;
        }


        public DataTable ExecuteDataTable(string strcommand,object[] Parameters,CommandType CommandType)
        {
            SqlCommand Command = CreateSqlCommand(strcommand, Parameters,CommandType);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(dt);
            return dt;
        }

        private SqlCommand CreateSqlCommand(string strCommand, object[] Parameters, CommandType CommandType)
        {
            strCommand = ConvertCommandToSqlFormat(strCommand);
            SqlParameter[] spArray = ConvertParaToSqlPara(ref strCommand, Parameters);
            SqlCommand Command = new SqlCommand(strCommand, conn);
            Command.CommandType = CommandType;
            foreach (SqlParameter x in spArray)
            {
                Command.Parameters.Add(x);
            }
            return Command;
        }

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
