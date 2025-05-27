using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacefinderOff.Models;

namespace SpacefinderOff.Models
{
    public class Number
    {
        public int NumberId { get; set; }
        public string Value { get; set; } = string.Empty; // e.g., "r123456" or "u654321"
        public UserType UserType { get; set; }

        // Navigation property for EF Core
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
