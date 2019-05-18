using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models
{
    public class Match : BaseEntity
    {
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        public int LeagueId { get; set; }
        public virtual League League { get; set; }

        public DateTime Time { get; set; }
        public string Venue { get; set; }

        public int HomeGoalsCount { get; set; }
        public int AwayGoalsCount { get; set; }

        public ICollection<MatchPlayer> MatchPlayers { get; set; }

        public ICollection<Goal> Goals { get; set; }

    }
}
