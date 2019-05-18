using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Rekreatistika.Models
{
    public class Goal : BaseEntity
    {
        public int MatchId { get; set; }
        public virtual Match Match { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}
