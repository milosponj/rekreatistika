using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models
{
    public class MatchPlayer
    {
        public int Id { get; set; }
        
        public int MatchId { get; set; }
        public Match Match { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
