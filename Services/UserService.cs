using SpacefinderOff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacefinderOff.Services
{
    public static class UserService
    {
        public static List<User> Users = new List<User>();

        public static bool IsEmailRegistered(string email)
        {
            return Users.Any(u => u.Email == email);
        }

        public static bool ValidateUser(string email, string password)
        {
            return Users.Any(u => u.Email == email && u.Password == password);
        }

        public static void AddUser(User user)
        {
            Users.Add(user);
        }
    }
}
