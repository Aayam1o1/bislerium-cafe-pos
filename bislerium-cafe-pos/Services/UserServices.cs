using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bislerium_cafe_pos.Models;
using bislerium_cafe_pos.Utils;

namespace bislerium_cafe_pos.Services

{
    public class UserServices
    {
        private List<User> _users = new()
        {
            new User()
            {
                Username = "admin",
                Password = "admin",
                Role = "admin"
            },
            new User()
            {
                Username = "staff",
                Password = "staff",
                Role = "staff"
            }
        };

        //For Sign In
        public User SignInUser(string password, string username)
        {
            const String errorMessage = "Entered Credentials are wrong";
            User user = _users.FirstOrDefault(u => u.Password == password && u.Username == username);
            
            return user ?? throw new Exception(errorMessage);
        }
    }
}
