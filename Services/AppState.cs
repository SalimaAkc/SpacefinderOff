using SpacefinderOff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacefinderOff.Services
{
    public static class AppState
    {
        public static User? CurrentUser { get; set; }
    }
}
