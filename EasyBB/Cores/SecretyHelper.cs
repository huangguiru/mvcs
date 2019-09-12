using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EasyBB.Cores
{
    public class SecretyHelper
    {
        /// <summary>
        /// c#生成MD5字符串 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncryptWithMD5(string source)
        {
            byte[] sor = Encoding.UTF8.GetBytes(source);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }

        public static string GetPassword(string pwd)
        {
            return SecretyHelper.EncryptWithMD5(SecretyHelper.EncryptWithMD5(pwd + EncryptWithMD5("bamncn")));

        }

    }
}