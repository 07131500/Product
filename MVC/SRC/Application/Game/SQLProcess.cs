using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class SQLProcess
    {
        MSSQL objDB;
        GetSetParams setDB = new GetSetParams();

        public SQLProcess()
        {
            string strConn = "";
            strConn = setDB.GetSqlConnectionStr();
            objDB = new MSSQL(strConn);
        }



    }
}
