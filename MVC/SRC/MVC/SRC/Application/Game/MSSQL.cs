using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class MSSQL
    {
        private SqlConnection conn;
        protected string strConn;
        private SqlTransaction dbTransaction = null;

        public MSSQL(string strConn)
        {
            conn = new SqlConnection(strConn);
        }

        public DbTransaction GetTransaction()
        {
            return dbTransaction;
        }

        public DbConnection GetConnection()
        {
            return conn;
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

        public void Close()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Dispose();
                dbTransaction = null;
            }
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                conn = null;
            }
        }

        public void TransactionClose()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Dispose();
                dbTransaction = null;
            }
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public void TransactionStart()
        {
            Trace.Assert(conn != null);
            EnsureConnection();
            this.dbTransaction = this.conn.BeginTransaction();
        }

        public void TransactionCommit()
        {
            this.dbTransaction.Commit();
            TransactionClose();
        }

        public void TransactionRollback()
        {
            this.dbTransaction.Rollback();
            TransactionClose();
        }

        private SqlCommand CreateSqlCommand(string strCommand, object[] Parameters, CommandType CommandType)
        {
            strCommand = ParameterConverter.ConvertCommandToSqlFormat(strCommand);
            SqlParameter[] spArray = ParameterConverter.ConvertParaToSqlPara(ref strCommand, Parameters);
            SqlCommand Command = new SqlCommand(strCommand, conn);
            Command.CommandType = CommandType;
            foreach (SqlParameter x in spArray)
            {
                Command.Parameters.Add(x);
            }
            return Command;
        }

        public DataSet ExecuteDataSet(string strCommand, CommandType CommandType)
        {
            try
            {
                EnsureConnection();
                strCommand = ParameterConverter.ConvertCommandToSqlFormat(strCommand);
                SqlCommand Command = new SqlCommand(strCommand, conn);
                Command.CommandText = strCommand;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(Command);
                CheckTransaction(Command);
                da.Fill(ds);
                conn.Close();
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet ExecuteDataSet(string strcommand, object[] Parameters, CommandType CommandType)
        {
            try
            {
                EnsureConnection();
                SqlCommand Command = CreateSqlCommand(strcommand, Parameters, CommandType);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(Command);
                CheckTransaction(Command);
                da.Fill(ds);
                conn.Close();
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ExecuteDataTable(string strCommand, CommandType CommandType)
        {
            try
            {
                EnsureConnection();
                strCommand = ParameterConverter.ConvertCommandToSqlFormat(strCommand);
                SqlCommand Command = new SqlCommand(strCommand, conn);
                Command.CommandType = CommandType;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(Command);
                CheckTransaction(Command);
                da.Fill(dt);
                if (Command.Transaction == null)
                    conn.Close();
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ExecuteDataTable(string strcommand, object[] Parameters, CommandType CommandType)
        {
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strcommand, Parameters, CommandType);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Command);
            CheckTransaction(Command);
            da.Fill(dt);
            if (Command.Transaction == null)
                conn.Close();
            return dt;
        }

        public string ExecuteScalar(string strCommand, CommandType CommandType)
        {
            try
            {
                EnsureConnection();
                strCommand = ParameterConverter.ConvertCommandToSqlFormat(strCommand);
                SqlCommand Command = new SqlCommand(strCommand, conn);
                Command.CommandType = CommandType;
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

        public string ExecuteScalar(string strcommand, object[] Parameters, CommandType CommandType)
        {
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strcommand, Parameters, CommandType);
            CheckTransaction(Command);
            string strRtn = Command.ExecuteScalar() == null ? "" : Command.ExecuteScalar().ToString();
            if (Command.Transaction == null)
                conn.Close();
            return strRtn;
        }

        public SecureString ExecuteScalarSecure(string strcommand, object[] Parameters, CommandType CommandType)
        {
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strcommand, Parameters, CommandType);
            CheckTransaction(Command);
            SecureString strRtn = new NetworkCredential("", (Command.ExecuteScalar() == null ? "" : Command.ExecuteScalar().ToString())).SecurePassword;
            if (Command.Transaction == null)
                conn.Close();
            return strRtn;
        }
        public SecureString ExecuteScalarSecure(string strCommand, CommandType CommandType)
        {
            EnsureConnection();
            strCommand = ParameterConverter.ConvertCommandToSqlFormat(strCommand);
            SqlCommand Command = new SqlCommand(strCommand, conn);
            Command.CommandType = CommandType;
            CheckTransaction(Command);
            SecureString strRtn = new NetworkCredential("", (Command.ExecuteScalar() == null ? "" : Command.ExecuteScalar().ToString())).SecurePassword;
            if (Command.Transaction == null)
                conn.Close();
            return strRtn;
        }

        public DbDataReader ExecuteReader(string strCommand, CommandType CommandType)
        {
            EnsureConnection();
            SqlCommand SqlCommand = new SqlCommand(strCommand, conn);
            SqlCommand.CommandType = CommandType;
            SqlDataReader dr = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }

        public DbDataReader ExecuteReader(string strcommand, object[] Parameter, CommandType CommandType)
        {
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strcommand, Parameter, CommandType);
            CheckTransaction(Command);
            SqlDataReader dr = Command.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }

        public int ExecuteNonQuery(string strCommand, CommandType CommandType)
        {
            int ModifyNum = 0;
            EnsureConnection();
            strCommand = ParameterConverter.ConvertCommandToSqlFormat(strCommand);
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

        public int ExecuteNonQuery(ArrayList AryCommand)
        {
            int ModifyNum = 0;
            EnsureConnection();
            conn.BeginTransaction(IsolationLevel.ReadCommitted);
            SqlCommand command = conn.CreateCommand();
            try
            {
                foreach (string x in AryCommand)
                {
                    command.CommandText = x;
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

        public DbCommand ExecuteQueryGetCmd(string strcommand, object[] Parameters, CommandType CommandType)
        {
            int ModifyNum = 0;
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strcommand, Parameters, CommandType);
            CheckTransaction(Command);
            ModifyNum += Command.ExecuteNonQuery();
            return Command;
        }

        public int InsertbulkCopy(DataTable srcdata, string strTable)
        {
            int ModifyNum = 0;
            EnsureConnection();
            SqlTransaction trans = conn.BeginTransaction();
            // SqlCommand command = conn.CreateCommand();
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, trans))
                {
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.DestinationTableName = "dbo." + strTable;
                    bulkCopy.WriteToServer(srcdata);
                }

                trans.Commit();

            }
            catch (Exception Err)
            {
                trans.Rollback();
                throw Err;
            }
            return ModifyNum;

        }
    }
}
