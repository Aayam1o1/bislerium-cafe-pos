using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bislerium_cafe_pos.Utils
{
    public class AppUtils
    {
        public static string GetDesktopDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Bislerium_cw_db");
        }

        public static string GetAppUsersFilePath()
        {
            return Path.Combine(GetDesktopDirectoryPath(), "users.json");
        }

        public static string GetCoffeeFilePath()
        {
            return Path.Combine(GetDesktopDirectoryPath(), "coffee.json");
        }
        public static string GetAddOnTableFilePath()
        {
            return Path.Combine(GetDesktopDirectoryPath(), "addOn.json");
        }
        public static string GetCustomersListFilePath()
        {
            return Path.Combine(GetDesktopDirectoryPath(), "customers.json");
        }

        public static string GetOrderListFilePath()
        {
            return Path.Combine(GetDesktopDirectoryPath(), "orders.json");
        }
        //for checking numeric values in a string
        public static bool IsNumeric(string input)
        {
            Regex numericRegex = new Regex("^[0-9]+$");
            return numericRegex.IsMatch(input);
        }
    }
}
