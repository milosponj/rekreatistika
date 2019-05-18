using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PostgreSqlDotnetCore.Models;

namespace Rekreatistika.Models
{
    public class League : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public string LeagueAdminEmails { get; set; }
    }
}
