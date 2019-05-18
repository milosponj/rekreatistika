using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models.LeagueViewModels
{
    public class LeagueIndexViewModel
    {
        public IList<League> Leagues { get; set; }
        public bool IsLeagueAdmin { get; set; }
        public IList<int> LeagueIdsForAdmin { get; set; }
		public bool IsAdmin { get; set; }

        public LeagueIndexViewModel()
        {
            Leagues = new List<League>();
            LeagueIdsForAdmin = new List<int>();
        }
    }
}
