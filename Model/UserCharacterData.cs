using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Model
{
    public class UserCharacterData
    {
        public string UserId { get; set; } = string.Empty;
        public List<BaldurCharacter> Characters { get; set; } = new();
    }

}
