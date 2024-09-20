using System;
using System.Collections;
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
        private SqlTransaction dbTransaction = null;
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

            var commnadResult = comm.ExecuteNonQuery();
            conn.Close();

            //dt = ExecuteDataTable(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);

            if (commnadResult == 1)
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


            //conn.Open();
            //comm = new SqlCommand(strSql.ToString(), conn);
            //comm.Parameters.AddRange(sqlParamList.ToArray());
           dt=ExecuteDataTable(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);
            //SqlDataAdapter da = new SqlDataAdapter(comm);
            //da.Fill(dt);
           // conn.Close();
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

            var commnadResult = ExecuteDataTable(strSql.ToString(), sqlParamList.ToArray(),CommandType.Text); //comm.ExecuteScalar();

            if (commnadResult != null)
            {
                return true;
            }

            return false;
        }

        public int ExecuteNonQuery(string strCommand, CommandType CommandType)
        {
            int ModifyNum = 0;
            EnsureConnection();
            strCommand = SQLFormat.ConvertCommandToSqlFormat(strCommand);
            SqlCommand Command = new SqlCommand(strCommand, conn);
            Command.CommandType = CommandType;
            CheckTransaction(Command);
            ModifyNum += Command.ExecuteNonQuery();
            if (Command.Transaction == null)
                conn.Close();
            return ModifyNum;
        }

        public int ExecuteNonQuery(string strcommand, object[] Parameters, CommandType CommandType)
        {
            int ModifyNum = 0;
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strcommand, Parameters, CommandType);
            CheckTransaction(Command);
            ModifyNum += Command.ExecuteNonQuery();
            if (Command.Transaction == null)
                conn.Close();
            return ModifyNum;
        }

        public int ExecuteNonQuery(ArrayList AryCommand, ArrayList ArySqlParameter)
        {
            int ModifyNum = 0;
            EnsureConnection();
            SqlTransaction st = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            SqlCommand command = conn.CreateCommand();
            command.Transaction = st;
            try
            {
                for (int x = 0; x < AryCommand.Count; x++)
                {
                    command.CommandText = AryCommand[x].ToString();
                    SqlParameter[] sqParameter = ArySqlParameter[x] as SqlParameter[];
                    command.Parameters.Clear();
                    foreach (SqlParameter y in sqParameter)
                    {
                        command.Parameters.Add(y);
                    }
                    ModifyNum += command.ExecuteNonQuery();
                }
                command.Transaction.Commit();
            }
            catch (Exception Err)
            {
                command.Transaction.Rollback();
                throw Err;
            }
            conn.Close();
            return ModifyNum;
        }


        public DataTable ExecuteDataTable(string strCommand,object[] Parameters,CommandType CommandType)
        {
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strCommand, Parameters,CommandType);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Command);
            CheckTransaction(Command);
            da.Fill(dt);
            if (Command.Transaction == null)
                conn.Close();
            return dt;
        }
        private void CheckTransaction(SqlCommand cmd)
        {
            if (dbTransaction == null)
            {
                return;
            }
            else
            {
                cmd.Transaction = dbTransaction;
            }
        }
        private void EnsureConnection()
        {
            Trace.Assert(conn != null);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        private SqlCommand CreateSqlCommand(string strCommand, object[] Parameters, CommandType CommandType)
        {
            strCommand = SQLFormat.ConvertCommandToSqlFormat(strCommand);
            SqlParameter[] spArray = SQLFormat.ConvertParaToSqlPara(ref strCommand, Parameters);
            SqlCommand Command = new SqlCommand(strCommand, conn);
            Command.CommandType = CommandType;
            foreach (SqlParameter x in spArray)
            {
                Command.Parameters.Add(x);
            }
            return Command;
        }


    }
}
