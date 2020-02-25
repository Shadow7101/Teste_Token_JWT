using TesteJWT.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TesteJWT.Data
{
    public static class UserRepository
    {

        public static List<User> Users
        {
            get
            {
                var l = new List<User>();
                l.Add(new User() { Id = 1, Username = "batman", Password = "123456", Role = "Admin" });
                l.Add(new User() { Id = 1, Username = "robin", Password = "123456", Role = "User" });
                return l;
            }
        }

        public static User GetUser(string username, string password)
        {
            username = username.ToLower();
            return Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
        }
    }
}