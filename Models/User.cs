using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacefinderOff.Models;

namespace SpacefinderOff.Models
{
    public class User
    {
        public int UserID { get; set; }  
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }

        public string? FullName { get; set; }
        public DateTime RegistrationDate { get; set; }

        public byte[] ProfilePicture { get; set; }
        public string Status { get; set; }
        public string Language { get; set; }

    }

    
}


