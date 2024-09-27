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
    class SQLParameter
    {
        private SqlConnection conn;
        private SqlTransaction dbTransaction = null;


        public SQLParameter(string strConn)
        {
            conn = new SqlConnection(strConn);
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


        public DataTable ExecuteDataTable(string strCommand, object[] Parameters, CommandType CommandType)
        {
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strCommand, Parameters, CommandType);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Command);
            CheckTransaction(Command);
            da.Fill(dt);
            if (Command.Transaction == null)
                conn.Close();
            return dt;
        }
      
        public string ExecuteScalar(string strCommand,CommandType commandType)
        {
            try
            {
                EnsureConnection();
                strCommand = SQLFormat.ConvertCommandToSqlFormat(strCommand);
                SqlCommand Command = new SqlCommand(strCommand, conn);
                Command.CommandType = commandType;
                CheckTransaction(Command);
                string strRtn = Command.ExecuteScalar() == null ? "" : Command.ExecuteScalar().ToString();
                if (Command.Transaction == null)
                    conn.Close();
                return strRtn;
            }
            catch (Exception)
            {
                throw;
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
