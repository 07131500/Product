using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Data;

namespace Game
{
    class User
    {
        static SQL ms = new SQL(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\練習20230703\practice2015\20240902(MVC登入)\MVC\SRC\Application\Game\APP_DATA\login.mdf;Integrated Security=True");

        public enum Behavior
        {
            register = 0,
            login = 1
        }

        private string userId;

        private string password;

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string TypeName { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Power { get; set; }
        public int Agile { get; set; }
        public int Intellect { get; set; }
        public int CurrentExp { get; set; }
        public int RequiredExp { get; set; }






        /// <summary>
        /// 註冊
        /// </summary>
        public void Register()
        {
            Console.WriteLine("註冊系統:");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("註冊:");
            Console.WriteLine("-----------------------------------------------------");

            string account = CheckInputNumber("帳號", 8);
            string password = CheckInputNumber("密碼", 8);

            int count = ms.CreateUser(account, password);
            if (count > 0)
            {
                DataTable dt = ms.GetUserInfo(account);
                Console.WriteLine("註冊成功");
            }
            else
            {
                Console.WriteLine("註冊失敗");
            }
        }
        /// <summary>
        /// 登入
        /// </summary>
        public void Login()
        {
            Console.WriteLine("登入系統:");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("登入:");
            Console.WriteLine("-----------------------------------------------------");

            string account = CheckInputNumber("帳號", 8);
            string password = CheckInputNumber("密碼", 8);

            bool count = ms.IsUser(account, password);
            if (count)
            {
                DataTable dtUser = ms.GetUserInfo(account);
                string UserId = dtUser.Rows[0]["RoleId"].ToString();
                Console.WriteLine("登入成功");
                #region 登入成功後取得人物資料
                //用UserUd查詢 GetRole取得Level [Power]     [Agile] [Intellect]   跟當前屬性
                DataTable dtRole = ms.GetRole(UserId);
                //用迴圈把所有腳色資料列出來，並進行選擇
                for (int i = 0; i < dtRole.Rows.Count; i++)
                {
                    // 取得每一行的資料
                    DataRow row = dtRole.Rows[i];

                    //虛構一個Id用來關聯id
                    string chooseid = (i + 1).ToString();
                    // 取得所需的欄位值
                    string id = row["Id"].ToString();
                    string name = row["Name"].ToString();
                    string typeName = row["TypeName"].ToString();
                    int level = Convert.ToInt32(row["Level"]);
                    int hp = Convert.ToInt32(row["HP"]);
                    int attack = Convert.ToInt32(row["Attack"]);
                    int defense = Convert.ToInt32(row["Defense"]);
                    int power = Convert.ToInt32(row["Power"]);
                    int agile = Convert.ToInt32(row["Agile"]);
                    int intellect = Convert.ToInt32(row["Intellect"]);
                    // 列印角色資訊
                    Console.WriteLine($"角色ID: {id}, 名稱: {name}, 類型: {typeName}, 等級: {level}, HP: {hp}, 攻擊: {attack}, 防禦: {defense}, 力量: {power}, 敏捷: {agile}, 智力: {intellect}");
                    // 在這裡可以加入選擇邏輯，例如詢問使用者是否選擇該角色
                    Console.WriteLine("選擇此角色？(按數字" + chooseid + ")");

                }
                string choice = Console.ReadLine();
                // 根據選擇的ID取得角色資料，存進class
                DataRow selectedRow = dtRole.Rows[Convert.ToInt32(choice) - 1];
                RoleId = selectedRow["Id"].ToString();
                RoleName = selectedRow["Name"].ToString();
                TypeName = selectedRow["TypeName"].ToString();
                Level = Convert.ToInt32(selectedRow["Level"]);
                HP = Convert.ToInt32(selectedRow["HP"]);
                Attack = Convert.ToInt32(selectedRow["Attack"]);
                Defense = Convert.ToInt32(selectedRow["Defense"]);
                Power = Convert.ToInt32(selectedRow["Power"]);
                Agile = Convert.ToInt32(selectedRow["Agile"]);
                Intellect = Convert.ToInt32(selectedRow["Intellect"]);
                CurrentExp = Convert.ToInt32(selectedRow["CurrentExp"]);
                RequiredExp = Convert.ToInt32(selectedRow["RequiredExp"]);
                #endregion

                //顯示主畫面 名稱屬性 商店 冒險 登出 
                Console.WriteLine($"角色名稱: {RoleName}");
                Console.WriteLine($"角色職業: {TypeName}");
                Console.WriteLine($"等級: {Level}");
                Console.WriteLine($"HP: {HP}");
                Console.WriteLine($"攻擊: {Attack}");
                Console.WriteLine($"防禦: {Defense}");
                Console.WriteLine($"力量: {Power}");
                Console.WriteLine($"敏捷: {Agile}");
                Console.WriteLine($"智力: {Intellect}");
                Console.WriteLine($"當前經驗: {CurrentExp}");
                Console.WriteLine($"下一級所需經驗: {RequiredExp}");
                Console.WriteLine("---------------------------------------------------------------------------------------------------");
                Console.WriteLine("{冒險}(按數字1) {商店}(按數字2)  {登出}(按數字3)");

                CurrentExp = 1000;
                RoleLevelUP();

                #region 冒險

                #endregion

                #region 商店

                #endregion

                #region 冒險

                #endregion

                #region 登出
                Logout();
                #endregion

            }
            else
            {
                Console.WriteLine("登入失敗");
            }
        }

        /// <summary>
        /// 處理輸入
        /// </summary>
        /// <param name="InputType">帳號/密碼</param>
        /// <param name="MaxLength">最大數量限制</param>
        /// <returns></returns>
        public string CheckInputNumber(string InputType, int MaxLength)
        {
            string UserInput;
            do
            {
                Console.WriteLine("請輸入" + InputType + ":");
                UserInput = Console.ReadLine();
                if (UserInput.Length > 8)
                {
                    Console.WriteLine("輸入失敗，請重新輸入" + MaxLength.ToString() + "位數以內的" + InputType);
                }

            }
            while (UserInput.Length > 8);


            return UserInput;
        }

        public void Logout()
        {

        }


        public void RoleLevelUP()
        {
            int CurExp = CurrentExp;
            int ReqExp = RequiredExp;
            int Lv = Level;
            //升級
            while (CurExp >= ReqExp)
            {
                Lv = Lv + 1;
                CurExp -= ReqExp;
                ReqExp = RequiredExperience(Lv);
            }
            Power = Lv;
            Agile = Lv;
            Intellect = Lv;
            Level = Lv;
            CurrentExp = CurExp < 0 ? CurExp + ReqExp : CurExp; //小於0加回去減掉的部分
            CalculateAbilities(Power, Agile, Intellect);
            ms.UpdateRole(RoleId, Level, HP, Attack, Defense, CurrentExp);
        }

        #region 屬性換算 升等或穿裝備時
        // 屬性值換算能力值HP Attack Defense   [Power]     [Agile] [Intellect]   
        /// <summary>
        /// 屬性換算能力值
        /// </summary>
        /// <param name="power"></param>
        /// <param name="agile"></param>
        /// <param name="intellect"></param>
        public void CalculateAbilities(int power, int agile, int intellect)
        {
            // 計算 HP、Attack 和 Defense
            HP = CalculateHP(power, agile, intellect) + 1;
            Attack = CalculateAttack(power, agile, intellect) + 1;
            Defense = CalculateDefense(power, agile, intellect) + 1;

        }

        // 將計算邏輯提取到單獨的方法中
        //1點力量 1 HP 0.75 Attack 0.25 Denfense
        private int CalculateHP(int power, int agile, int intellect)
        {
            double a = power + (agile * 0.5) + (intellect * 0.5);
            int b = (int)Math.Round(a);
            return b;
        }
        //1點敏捷 0.5 HP 1 Attack 0.5 Denfense
        private int CalculateAttack(int power, int agile, int intellect)
        {
            double a = (power * 0.75) + agile + (intellect * 0.5);
            int b = (int)Math.Round(a);
            return b;
        }
        //1點智慧 0.5 HP 0.5 Attack 1 Denfense
        private int CalculateDefense(int power, int agile, int intellect)
        {
            double a = (power * 0.25) + (agile * 0.5) + intellect;
            int b = (int)Math.Round(a);
            return b;
        }
        #endregion

        /// <summary>
        /// 升級經驗公式
        /// </summary>
        /// <param name="Level"></param>
        /// <returns></returns>
        public int RequiredExperience(int Level)
        {
            //等級的平方乘以 1
            int n = 1;
            int m = n * (Level * Level);
            return m;
        }
        /// <summary>
        /// 每次升級屬性提升
        /// </summary>
        /// <returns></returns>
        public int PropertyPromote(int Level)
        {
            //每項屬性都提升Level值
            return Level;
        }

        /// <summary>
        /// 加密字串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        //public string Encrypt(string text)
        //{
        //    try
        //    {
        //        byte[] buffer = Encoding.Default.GetBytes(text);
        //        MemoryStream ms = new MemoryStream();
        //        //AesCryptoServiceProvider tdes = new AesCryptoServiceProvider();
        //        //des加密
        //        DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
        //        CryptoStream encStream = new CryptoStream(ms, tdes.CreateEncryptor(Encoding.Default.GetBytes(mstrKey), Encoding.Default.GetBytes(mstrIV)), CryptoStreamMode.Write);
        //        encStream.Write(buffer, 0, buffer.Length);
        //        encStream.FlushFinalBlock();
        //        return Convert.ToBase64String(ms.ToArray());
        //    }
        //    catch (Exception err)
        //    {
        //        //throw new Exception("");
        //        return "";
        //    }

        //}

    }
}
