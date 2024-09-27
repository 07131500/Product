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
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int CreateUser(string account, string password)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            strSql.AppendLine("INSERT INTO User_Info (userId,password)  ");
            strSql.AppendLine("VALUES ( @account , @password ) ");
            //避免SQL注入
            sqlParamList.Add(new SqlParameter("@ACCOUNT", account));
            sqlParamList.Add(new SqlParameter("@PASSWORD", password));

            var commnadResult = sp.ExecuteNonQuery(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);
            
            return commnadResult;
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

           dt= sp.ExecuteDataTable(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);

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
            return sp.ExecuteScalar(sql,CommandType.Text).ToUpper();
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
        /// <returns></returns>
        public int CreateMonster( string MonsterName,string AbilityId, int HP,int Attack,int Defense)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            strSql.AppendLine("INSERT INTO Monster (MonsterName,AbilityId,HP,Attack,Defense)  ");
            strSql.AppendLine("VALUES ( @MonsterName , @AbilityId,@HP,@Attack,@Defense ) ");
            
            sqlParamList.Add(new SqlParameter("@MonsterName", MonsterName));
            sqlParamList.Add(new SqlParameter("@HP", HP));
            sqlParamList.Add(new SqlParameter("@Attack", Attack));
            sqlParamList.Add(new SqlParameter("@Defense", Defense));
            sqlParamList.Add(new SqlParameter("@AbilityId", AbilityId));

            var commnadResult = sp.ExecuteNonQuery(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);
            return 0;
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
        public int CreateEquip(string EquipName,  int HP, int Attack, int Defense,string Describe)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            strSql.AppendLine("INSERT INTO Equip (Equip_Name,HP,Attack,Defense,Describe)  ");
            strSql.AppendLine("VALUES ( @Equip_Name ,@HP,@Attack,@Defense,@Describe ) ");

            sqlParamList.Add(new SqlParameter("@Equip_Name", EquipName));
            sqlParamList.Add(new SqlParameter("@HP", HP));
            sqlParamList.Add(new SqlParameter("@Attack", Attack));
            sqlParamList.Add(new SqlParameter("@Defense", Defense));
            sqlParamList.Add(new SqlParameter("@Describe", Describe));

            var commnadResult = sp.ExecuteNonQuery(strSql.ToString(), sqlParamList.ToArray(), CommandType.Text);
            return 0;
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
            return 0;
        }

        //註冊時自動建立戰士資料




    }
}
