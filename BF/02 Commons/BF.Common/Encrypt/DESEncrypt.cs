using BF.Common.Helper;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BF.Common.Encrypt
{
    public sealed class DESEncrypt
    {
        private DESEncrypt()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private static string _key = ConfigHelper.GetConfigValue("EncryptKey", "123456");

        /**/
        /// <summary>
        /// 对称加密解密的密钥
        /// </summary>
        public static string Key
        {
            get
            {
                if (string.IsNullOrEmpty(_key))
                {
                    return "zhoufoxc";
                }
                else

                    return _key;
            }
            set
            {
                _key = value;
            }
        }


        /**/
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string DesEncrypt(string encryptString)
        {
            return DesEncrypt(encryptString, null);
        }

        public static string DesEncrypt(string encryptString, string encrptKey)
        {
            if (string.IsNullOrEmpty(encrptKey)) encrptKey = Key;
            byte[] keyBytes = Encoding.UTF8.GetBytes(encrptKey);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /**/
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString)
        {
            return DesDecrypt(decryptString, null);
        }

        /**/
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString, string encrptKey)
        {
            if (string.IsNullOrEmpty(encrptKey)) encrptKey = Key;
            byte[] keyBytes = Encoding.UTF8.GetBytes(encrptKey);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
    }
}
