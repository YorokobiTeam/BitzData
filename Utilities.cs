using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitzData
{
    class Utilities
    {
        public static string GetExtension(string fileName)
        {
            string[] strings = fileName.Split(".");
            if (strings.Length <= 2) return "";
            return strings.Last<string>();
        }

        public static string ExtractFileNameFromSupabasePath(string filePath)
        {
            string[] strings = filePath.Split("/");
            return strings.Last<string>();
        }
    }
}
