using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string TeamAdminEmails { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
