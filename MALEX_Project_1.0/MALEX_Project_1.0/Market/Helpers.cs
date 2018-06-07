using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Market
{
    public static class Helpers
    {
        //-------------------------------------------------------------------------------------
        //-- Метод конвертации и преобразования данных
        //-------------------------------------------------------------------------------------

        //-- Метод хеширования ключа

        public static string Sign(string key, string message)
        {
            using (HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return ByteToString(b);
            }
        }

        //-- Метод преобразования массива с параметрами в строку

        public static string ToQueryString(IDictionary<string, string> dic)
        {
            var array = (from key in dic.Keys
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(dic[key])))
                    .ToArray();
            return string.Join("&", array);
        }

        //-- Метод преобразования массива с байтами в строку

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary).ToLowerInvariant();
        }


        //-- Метод преобразования запроса

        public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var kvp in dict)
            {
                string value = string.Empty;
                if (kvp.Value != null)
                    value = kvp.Value.ToString();

                nameValueCollection.Add(kvp.Key.ToString(), value);
            }

            return nameValueCollection;
        }

        //-- Получение текущего времени в милисекундах

        public static long GetTimestamp()
        {
            var d = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return (long)d;
        }
    }
}
