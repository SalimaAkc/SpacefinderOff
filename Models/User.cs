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
        public int UserId { get; set; }  
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public int? NumberId { get; set; }

        // Navigation property
        public Number? Number { get; set; }

        // Helper to access type and number directly
        public UserType? UserType => Number?.UserType;
        public string? UserNumber => Number?.Value;
    }

    
}


