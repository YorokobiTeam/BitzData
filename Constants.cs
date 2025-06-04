using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitzData
{
    public class Constants
    {
        public static readonly string APPLICATION_DATA = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string CACHE_METADATA = Path.Combine(APPLICATION_DATA, "cache", "metadata");
        public static readonly string CACHE_FILES = Path.Combine(APPLICATION_DATA, "cache", "files");


    }
}
