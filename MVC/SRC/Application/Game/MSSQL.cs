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
        /// <summary>
        /// 連接
        /// </summary>
        /// <param name="strConn">資料庫連接字串</param>
        public MSSQL(string strConn)
        {
            conn = new SqlConnection(strConn);
        }
        /// <summary>
        /// 取得交易
        /// </summary>
        /// <returns></returns>
        public DbTransaction GetTransaction()
        {
            return dbTransaction;
        }
        /// <summary>
        /// 取得目前資料庫連接
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection()
        {
            return conn;
        }
        /// <summary>
        /// 確認交易，沒設交易，直接返回
        /// </summary>
        /// <param name="cmd">資料庫命令訊息</param>
        private void CheckTransaction(SqlCommand cmd)
        {
            //沒設交易，直接返回
            if (dbTransaction == null)
            {
                return;
            }
            else
            {
                cmd.Transaction = dbTransaction;
            }
        }
        /// <summary>
        /// 打開資料庫連接
        /// </summary>
        private void EnsureConnection()
        {
            Trace.Assert(conn != null);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        /// <summary>
        /// 關閉資料庫
        /// </summary>
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
        /// <summary>
        /// 交易關閉
        /// </summary>
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
        /// <summary>
        /// 開始交易
        /// </summary>
        public void TransactionStart()
        {
            Trace.Assert(conn != null);
            EnsureConnection();
            this.dbTransaction = this.conn.BeginTransaction();
        }
        /// <summary>
        /// 交易確認
        /// </summary>
        public void TransactionCommit()
        {
            this.dbTransaction.Commit();
            TransactionClose();
        }
        /// <summary>
        /// 交易回滾
        /// </summary>
        public void TransactionRollback()
        {
            this.dbTransaction.Rollback();
            TransactionClose();
        }
        /// <summary>
        /// 創建資料庫命令訊息
        /// </summary>
        /// <param name="strCommand">字串命令</param>
        /// <param name="Parameters">參數變數</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 把資料填進DataSet
        /// </summary>
        /// <param name="strCommand">字串命令</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 把資料填進DataSet
        /// </summary>
        /// <param name="strcommand">字串命令</param>
        /// <param name="Parameters">參數變數</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 把資料填進DataTable
        /// </summary>
        /// <param name="strCommand">字串命令</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 把資料填進DataTable
        /// </summary>
        /// <param name="strcommand">字串命令</param>
        /// <param name="Parameters">參數變數</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 查詢第一個資料列的第一個資料行
        /// </summary>
        /// <param name="strCommand">字串命令</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 查詢第一個資料列的第一個資料行
        /// </summary>
        /// <param name="strcommand">字串命令</param>
        /// <param name="Parameters">參數變數</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 查詢有加解密的第一個資料列的第一個資料行
        /// </summary>
        /// <param name="strcommand">字串命令</param>
        /// <param name="Parameters">參數變數</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 查詢有加解密的第一個資料列的第一個資料行
        /// </summary>
        /// <param name="strCommand">字串命令</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 傳回資料讀取
        /// </summary>
        /// <param name="strCommand">字串命令</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string strCommand, CommandType CommandType)
        {
            EnsureConnection();
            SqlCommand SqlCommand = new SqlCommand(strCommand, conn);
            SqlCommand.CommandType = CommandType;
            SqlDataReader dr = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
        /// <summary>
        /// 傳回資料讀取
        /// </summary>
        /// <param name="strcommand">字串命令</param>
        /// <param name="Parameters">參數變數</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string strcommand, object[] Parameter, CommandType CommandType)
        {
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strcommand, Parameter, CommandType);
            CheckTransaction(Command);
            SqlDataReader dr = Command.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
        /// <summary>
        /// 執行非查詢
        /// </summary>
        /// <param name="strCommand">字串命令</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 執行非查詢
        /// </summary>
        /// <param name="strcommand">字串命令</param>
        /// <param name="Parameters">參數變數</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 執行非查詢
        /// </summary>
        /// <param name="AryCommand">用動態陣列存取所有字串命令</param>
        /// <returns></returns>
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
        /// <summary>
        /// 執行非查詢
        /// </summary>
        /// <param name="AryCommand">用動態陣列存取所有字串命令</param>
        /// <param name="ArySqlParameter">用動態陣列存取所有變數參數</param>
        /// <returns></returns>
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
        /// <summary>
        /// 執行後取得查詢命令
        /// </summary>
        /// <param name="strcommand">字串命令</param>
        /// <param name="Parameters">參數變數</param>
        /// <param name="CommandType">命令類型</param>
        /// <returns></returns>
        public DbCommand ExecuteQueryGetCmd(string strcommand, object[] Parameters, CommandType CommandType)
        {
            int ModifyNum = 0;
            EnsureConnection();
            SqlCommand Command = CreateSqlCommand(strcommand, Parameters, CommandType);
            CheckTransaction(Command);
            ModifyNum += Command.ExecuteNonQuery();
            return Command;
        }
        /// <summary>
        /// 插入批量複製
        /// </summary>
        /// <param name="srcdata">資料表</param>
        /// <param name="strTable">資料表名稱</param>
        /// <returns></returns>
        public int InsertbulkCopy(DataTable srcdata, string strTable)
        {
            int ModifyNum = 0;
            EnsureConnection();
            SqlTransaction trans = conn.BeginTransaction();
            // SqlCommand command = conn.CreateCommand();
            try
            {
                //保留原本Identity，timeout設為無限期等待
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
