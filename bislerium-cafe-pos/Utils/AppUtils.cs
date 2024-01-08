using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bislerium_cafe_pos.Utils
{
    public class AppUtils
    {
        public static string GetDesktopDirectoryPath()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer), "Bislerium");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string GetAppUsersFilePath()
        {
            return Path.Combine(GetDesktopDirectoryPath(), "users.json");
        }

        public static string GetCoffeeFilePath()
        {
            return Path.Combine(GetDesktopDirectoryPath(), "coffee.json");
        }
    }
}
