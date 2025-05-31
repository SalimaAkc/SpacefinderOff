using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacefinderOff.Models
{
    public class Bookings
    {
        public int BookingID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime BookingDate { get; set; } 
        public int PeopleAmount { get; set; }
        public string Status { get; set; } = "Confirmed"; 
        public DateTime CreatedAt { get; set; }

        public int UserID { get; set; }
        public User? User { get; set; }

        public int ClassroomID { get; set; }
        public Classroom? Classroom { get; set; }
    }
}
