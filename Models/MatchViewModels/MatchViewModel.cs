using Rekreatistika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.ViewModels
{
    public class MatchViewModel
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }

        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }

        public DateTime Time { get; set; }
        public string TimeString { get; set; }
        //public string Venue { get; set; }

        public int HomeGoalsCount { get; set; }
        public int AwayGoalsCount { get; set; }

        public ICollection<MatchPlayer> MatchPlayers { get; set; }

        public ICollection<Goal> Goals { get; set; }
    }
}
