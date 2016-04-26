using System;
using System.Web.Security;

namespace BF.Common.Encrypt
{
    public class HashSignature
    {
        /// <summary>
        /// 哈希加密
        /// </summary>
        /// <param name="openID"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string Encrypt(string[] ArrTmp)
        {
            Array.Sort(ArrTmp);　　 //字典排序 
            string tmpStr = string.Join("", ArrTmp);
            string safeStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            if (!string.IsNullOrEmpty(safeStr))
            {
                safeStr = safeStr.ToLower();
            }
            return safeStr;
        }
    }
}
