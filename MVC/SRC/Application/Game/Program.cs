using Game;
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
    class Program
    {
        static SQL ms = new SQL(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\練習20230703\practice2015\20240902(MVC登入)\MVC\SRC\Application\Game\APP_DATA\login.mdf;Integrated Security=True");

        //取得目前程式的名稱
        public static string ProcessName = Process.GetCurrentProcess().ProcessName;

        public class UserInfo
        {

        }

        static void Main(string[] args)
        {
            #region 判斷程式是否正在執行.若正在執行則不跑
            string ProcesName = Process.GetCurrentProcess().ProcessName;
            string ProcessName2 = ProcessName.Split('.')[0].ToLower();
            Process[] ps = Process.GetProcessesByName(ProcessName);

            if (ps.Length > 1)
            {
                Console.WriteLine("Batch is running, Please wait.");
                return;
            }
            #endregion

            Program p = new Program();
            Role r = new Role();
            Role.Status status = Role.Status.live;

            #region 註冊/登入
            //p.ChooseWantDo();
            #endregion

            #region 創造一到一百的資料
            NPOI_Helper_Utility npoi_helper = new NPOI_Helper_Utility();
            npoi_helper.CreateNewExcel_Foreach_XLSX("ExpTable");
            #endregion

            #region 創造怪物
            //string MonsterName = p.stringInput("怪物");
            //int HP = p.InputToInt("HP");
            //int Attack = p.InputToInt("Attack");
            //int Defense = p.InputToInt("Defense");
            //string AId = ms.GET_NEWID();
            //ms.CreateMonster(MonsterName, AId, HP, Attack, Defense);
            #endregion

            #region 裝備
            //string EquipName = p.stringInput("裝備");
            //int HP = p.InputToInt("HP");
            //int Attack = p.InputToInt("Attack");
            //int Defense = p.InputToInt("Defense");
            //string EquipDescribe = p.stringInput("裝備描述");
            //ms.CreateEquip(EquipName, HP, Attack, Defense, EquipDescribe);
            #endregion



            #region 登入成功後取得人物資料
            //用GetRole取得Level [Power]     [Agile] [Intellect]  
            //
            //
            
            #endregion


            Console.WriteLine((int)status);
            Console.ReadLine();
        }


        /// <summary>
        /// 主線/主要情節
        /// </summary>
        public void MainPlot()
        {
            //出現Role
            Console.WriteLine();
            Console.WriteLine("請選擇接下來的行動:數字1(冒險)、數字2(商店)、數字3(登出)");
            string b = Console.ReadLine();
            switch (b) {
                case "1":
                    Adventure();
                    break;
                case "2":
                    //商店資料表還沒建
                    break;
                case "3":
                    Console.WriteLine("已登出");
                    break;
            }
          
            

        }
        /// <summary>
        /// 冒險
        /// </summary>
        public void Adventure()
        {
            string a = "";
            //當
            do
            {
                #region 隨機生成怪物
                Monster randomMonster = Monster.GenerateRandomMonster(ms);
                if (randomMonster != null)
                {
                    Console.WriteLine(randomMonster);
                }
                else
                {
                    Console.WriteLine("沒有怪物資料可供生成。");
                }
                #endregion
                
                Console.WriteLine("請輸入(1:繼續/2:返回)");
                a = Console.ReadLine();

            }
            while (a == "1");
        }


        public void ChooseWantDo()
        {
            Console.WriteLine("請輸入數字1(註冊) / 2(登入) ");
            string s = Console.ReadLine();
            int UserWantDo = Convert.ToInt32(s) - 1;
            // 將整數轉換為 Behavior 列舉
            User.Behavior action = (User.Behavior)UserWantDo;
            //註冊
            if (action == User.Behavior.register)
            {
                UserRegister();
            }
            //登入
            if (action == User.Behavior.login)
            {
                UserLogin();
            }

        }


        public void UserRegister()
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

        public void UserLogin()
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
                DataTable dt = ms.GetUserInfo(account);
                Console.WriteLine("登入成功");
                
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


        public int InputToInt(string input)
        {
            Console.WriteLine("請輸入" + input + "數值:");
            string s = Console.ReadLine();
            int a = Convert.ToInt32(s);
            return a;
        }

        public string stringInput(string input)
        {
            Console.WriteLine("請輸入" + input + "名稱:");
            string s = Console.ReadLine();
            return s;
        }


    }
}
