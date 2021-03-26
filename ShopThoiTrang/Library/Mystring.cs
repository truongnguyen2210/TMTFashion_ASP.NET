using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ShopThoiTrang.Library
{
    public static class Mystring
    {
        public static string ToMD5(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
        public static string str_slug(string s)
        {
            string[][] symbols = { 
                new string[] {"[áàảãạăắằẵặẳâấầẩẫậ]", "a"},
                new string[] {"[đ]", "d"},
                new string[] {"[éèẽẹẻêểềếệễ]", "e"},
                new string[] {"[íìịỉĩ]", "i"},
                new string[] {"[òóỏõọôốồộổỗơớờỡởợ]", "o"},
                new string[] {"[ùúụủũưứừựửữ]", "u"},
                new string[] {"[ỳýỷỹỵ]", "y"},
                new string[] {"[\\s'\";,]", "-"},
            };
            s = s.ToLower();
            foreach(var ss in symbols)
            {
                s = Regex.Replace(s, ss[0], ss[1]);
            }
            return s;
        }
    }
}