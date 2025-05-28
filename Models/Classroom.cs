using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacefinderOff.Models
{
    public class Classroom
    {
        public int ClassroomID { get; set; }
        public string? RoomNumber { get; set; }
        public int Capacity { get; set; }

        public int CampusID { get; set; }
        public Campus? Campus { get; set; }
    }
}

