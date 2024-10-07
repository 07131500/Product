using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Game
{
    class User
    {
        public Guid Logintoken;

        public enum Behavior
        {
            register=0,
            login=1
        }

        private string userId;

        private string password;

        public string UserId
        {
            get {return userId; }
            set { userId=value; }
        }

        public string Password
        {
            get {return password; }
            set {password=value; }
        }

        public double HP { get; set; }
        public double Attack { get; set; }
        public double Defense { get; set; }

      

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




        // 方法直接返回能力值
        //屬性值換算能力值HP Attack Defense   [Power]     [Agile] [Intellect]   
        public void CalculateAbilities(int power, int agile, int intellect)
        {
            // 計算 HP、Attack 和 Defense
            HP = CalculateHP(power, agile, intellect);
            Attack = CalculateAttack(power, agile, intellect);
            Defense = CalculateDefense(power, agile, intellect);
        }

        // 將計算邏輯提取到單獨的方法中
        //1點力量 1 HP 0.75 Attack 0.25 Denfense
        private double CalculateHP(int power, int agile, int intellect)
        {
            return power + (agile * 0.5) + (intellect * 0.5);
        }
        //1點敏捷 0.5 HP 1 Attack 0.5 Denfense
        private double CalculateAttack(int power, int agile, int intellect)
        {
            return (power * 0.75) + agile + (intellect * 0.5);
        }
        //1點智慧 0.5 HP 0.5 Attack 1 Denfense
        private double CalculateDefense(int power, int agile, int intellect)
        {
            return (power * 0.25) + (agile * 0.5) + intellect;
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
