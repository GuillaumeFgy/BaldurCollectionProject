using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Model
{
    public static class Session
    {
        public static UserAccount? CurrentUser { get; set; }
    }
}
