using Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Program
    {
       static SQL s = new SQL(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\練習20230703\practice2015\20240902(MVC登入)\MVC\SRC\Application\Game\APP_DATA\login.mdf;Integrated Security=True");
        static void Main(string[] args)
        {
            Role r = new Role();
            Role.Status status = Role.Status.live;

            
            Console.WriteLine("After assert");

            Console.WriteLine("登入系統:");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("註冊:");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("請輸入帳號:");
            string account = Console.ReadLine();
            if(account.Length>8)
            {
                Console.WriteLine("請重新輸入8位數帳號:");
                account = Console.ReadLine();
            }
            Console.WriteLine("請輸入密碼:");
            string password = Console.ReadLine();
            if(password.Length > 8)
            {
                Console.WriteLine("請重新輸入8位數密碼:");
                password = Console.ReadLine();
            }
            bool count=s.IsUser(account,password);
            if (count)
            {
                DataTable dt = s.GetUserInfo(account);
                Console.WriteLine("登入成功");
            }
            else
            {
                Console.WriteLine("登入失敗");
            }
            
           
            Console.ReadLine();
            Program p = new Program();
           
            Console.WriteLine((int)status);
            Console.ReadLine();
        }

       




    }
}
