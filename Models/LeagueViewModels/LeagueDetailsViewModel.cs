using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models.LeagueViewModels
{
    public class LeagueDetailsViewModel
    {
        public bool IsUserAllowedToAddScorers { get; set; }
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public string LogoUrl { get; set; }
        public IList<Match> Matches { get; set; }
        public IList<LeagueStats> LeagueStats { get; set; }
		public bool IsAdmin { get; set; }
    }
}
