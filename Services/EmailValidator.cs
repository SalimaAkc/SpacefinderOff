using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacefinderOff.Services
{
    public static class EmailValidator
    {
        public static bool IsValidThomasMoreEmail(string email)
        {
            return email.EndsWith("@student.thomasmore.be") || email.EndsWith("@thomasmore.be");
        }
    }
}
