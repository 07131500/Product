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
        SQLParameter sp;
        public SQL()
        {

        }

        public SQL(string SqlConn)
        {
            sp = new SQLParameter(SqlConn);
        }

        /// <summary>
        /// 新增User_Info資料表資料
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public int CreateUser(string Account, string Password)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            strSql.AppendLine("INSERT INTO User_Info (UserId,Password)  ");
            strSql.AppendLine("VALUES ( @account , @password ) ");
            //避免SQL注入
            sqlParamList.Add(new SqlParameter("@ACCOUNT", Account));
            sqlParamList.Add(new SqlParameter("@PASSWORD", Password));

            var commnadResult = sp.ExecuteNonQuery(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);

            return commnadResult;
        }

        public DataTable GetUserInfo(string Account)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            strSql.AppendLine("SELECT * FROM User_Info WHERE 1=1 ");

            if (Account != "")
            {
                strSql.AppendLine("AND UserId=@account ");
                sqlParamList.Add(new SqlParameter("@account", Account));
            }

            dt = sp.ExecuteDataTable(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);

            return dt;
        }


        public bool IsUser(string Account, string Password)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            strSql.AppendLine("SELECT * FROM User_Info WHERE 1=1 ");
            strSql.AppendLine("AND UserId=@account AND Password=@password ");

            //避免SQL注入，使用 SqlParameter 添加参数
            sqlParamList.Add(new SqlParameter("@ACCOUNT", Account));
            sqlParamList.Add(new SqlParameter("@PASSWORD", Password));
            SqlParameter[] objAryParam = sqlParamList.ToArray();

            DataTable commnadResult = sp.ExecuteDataTable(strSql.ToString(), objAryParam, CommandType.Text); //comm.ExecuteScalar();

            if (commnadResult != null)
            {
                return true;
            }

            return false;
        }

        public string GET_NEWID()
        {
            string sql = "SELECT NEWID()";
            return sp.ExecuteScalar(sql, CommandType.Text).ToUpper();
        }




        /// <summary>
        /// 創建怪物
        /// </summary>
        /// <param name="MonsterId"></param>
        /// <param name="MonsterName"></param>
        /// <param name="AbilityId"></param>
        /// <param name="HP"></param>
        /// <param name="Attack"></param>
        /// <param name="Defense"></param>
        /// <param name="Exp"></param>
        /// <returns></returns>
        public int CreateMonster(string MonsterName, string AbilityId, int HP, int Attack, int Defense, int Exp)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            strSql.AppendLine("INSERT INTO Monster (MonsterName,AbilityId,HP,Attack,Defense,Exp)  ");
            strSql.AppendLine("VALUES ( @MonsterName , @AbilityId,@HP,@Attack,@Defense,@Exp ) ");

            sqlParamList.Add(new SqlParameter("@MonsterName", MonsterName));
            sqlParamList.Add(new SqlParameter("@HP", HP));
            sqlParamList.Add(new SqlParameter("@Attack", Attack));
            sqlParamList.Add(new SqlParameter("@Defense", Defense));
            sqlParamList.Add(new SqlParameter("@AbilityId", AbilityId));
            sqlParamList.Add(new SqlParameter("@Exp", Exp));

            var commnadResult = sp.ExecuteNonQuery(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);
            return commnadResult;
        }


        public DataTable QueryMonster()
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            strSql.AppendLine("SELECT * FROM Monster WHERE 1=1 ");


            DataTable commnadResult = sp.ExecuteDataTable(strSql.ToString(), CommandType.Text); //comm.ExecuteScalar();
            return commnadResult;
        }



        /// <summary>
        /// 創造裝備
        /// </summary>
        /// <param name="EquipName"></param>
        /// <param name="HP"></param>
        /// <param name="Attack"></param>
        /// <param name="Defense"></param>
        /// <param name="Describe"></param>
        /// <returns></returns>
        public int CreateEquip(string EquipName, int HP, int Attack, int Defense, string Describe)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            strSql.AppendLine("INSERT INTO Equip (EquipName,HP,Attack,Defense,Describe)  ");
            strSql.AppendLine("VALUES ( @EquipName ,@HP,@Attack,@Defense,@Describe ) ");

            sqlParamList.Add(new SqlParameter("@EquipName", EquipName));
            sqlParamList.Add(new SqlParameter("@HP", HP));
            sqlParamList.Add(new SqlParameter("@Attack", Attack));
            sqlParamList.Add(new SqlParameter("@Defense", Defense));
            sqlParamList.Add(new SqlParameter("@Describe", Describe));

            var commnadResult = sp.ExecuteNonQuery(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);
            return commnadResult;
        }

        /// <summary>
        /// 創造人物
        /// </summary>
        /// <param name="EquipName"></param>
        /// <param name="HP"></param>
        /// <param name="Attack"></param>
        /// <param name="Defense"></param>
        /// <param name="Describe"></param>
        /// <returns></returns>
        public int CreateRole(string Id, string Name, int HP, int Attack, int Defense)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            strSql.AppendLine("INSERT INTO Role (Id,Name,Level,HP,Attack,Defense)  ");
            strSql.AppendLine("VALUES ( @Id,@Name,@Level,@HP,@Attack,@Defense ) ");

            sqlParamList.Add(new SqlParameter("@Name", Name));
            sqlParamList.Add(new SqlParameter("@HP", HP));
            sqlParamList.Add(new SqlParameter("@Attack", Attack));
            sqlParamList.Add(new SqlParameter("@Defense", Defense));
            sqlParamList.Add(new SqlParameter("@ID", Id));

            var commnadResult = sp.ExecuteNonQuery(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);
            return commnadResult;
        }

        public DataTable GetRole(int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            strSql.AppendLine("SELECT * FROM Role WHERE 1=1 ");
            strSql.AppendLine("AND @Id=Id ");

            sqlParamList.Add(new SqlParameter("@Id", UserId));
            DataTable commnadResult = sp.ExecuteDataTable(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text); 
            return commnadResult;
        }


        //註冊時自動建立戰士資料
        public bool CreatePlayer(string Account, string Password, string Id, string Name, int HP, int Attack, int Defense)
        {
            try
            {
                StringBuilder strSql;
                List<SqlParameter> sqlParamList;
                strSql = new StringBuilder();
                sqlParamList = new List<SqlParameter>();

                strSql.AppendLine("INSERT INTO User_Info (UserId,Password)  ");
                strSql.AppendLine("VALUES ( @account , @password ) ");
                //避免SQL注入
                sqlParamList.Add(new SqlParameter("@account", Account));
                sqlParamList.Add(new SqlParameter("@password", Password));

                var commnadResult = sp.ExecuteNonQuery(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);

                strSql = new StringBuilder();
                sqlParamList = new List<SqlParameter>();
                strSql.AppendLine("INSERT INTO Role (Id,Name,Level,HP,Attack,Defense)  ");
                strSql.AppendLine("VALUES ( @Id,@Name,@Level,@HP,@Attack,@Defense ) ");

                sqlParamList.Add(new SqlParameter("@Name", Name));
                sqlParamList.Add(new SqlParameter("@HP", HP));
                sqlParamList.Add(new SqlParameter("@Attack", Attack));
                sqlParamList.Add(new SqlParameter("@Defense", Defense));
                sqlParamList.Add(new SqlParameter("@ID", Id));

                var commnadResult2 = sp.ExecuteNonQuery(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


    }
}
