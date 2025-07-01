using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BitzData
{
    public class Utilities
    {
        public static string GetExtension(string fileName)
        {
            string[] strings = fileName.Split(".");
            if (strings.Length < 2) return "";
            return strings.Last<string>();
        }

        public static string ExtractFileNameFromSupabasePath(string filePath)
        {
            string[] strings = filePath.Split("/");
            return strings.Last<string>();
        }

        public static string GetFileMD5(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            var hashBytes = MD5.HashData(bytes);
            if (hashBytes is null) throw new InvalidDataException();
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2")); // Format as hexadecimal
            }
            return sb.ToString();
        }


    }
}
