using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Game
{
    class Login
    {
        public Guid Logintoken;


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
