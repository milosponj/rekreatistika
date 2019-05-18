using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models
{
    public class LeagueStats
    {
        public Team Team { get; set; }
        public int MatchesPlayed { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
        public int MatchesDrawn { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
    }
}
