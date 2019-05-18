using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rekreatistika.Models.PlayerViewModels;

namespace Rekreatistika.Models.AccountViewModels
{
    public class MyStatisticsViewModel
    {
        public string Email { get; set; }
        public IList<Match> MyMatches { get; set; }
        public IList<PlayerDetailsViewModel> MyPlayers { get; set; }
        public PlayerDetailsViewModel Player { get; set; }
        public IList<Team> MyTeams { get; set; }
        public IList<League> MyLeagues { get; set; }

        public MyStatisticsViewModel()
        {
            MyMatches = new List<Match>();
            MyPlayers = new List<PlayerDetailsViewModel>();
            MyTeams = new List<Team>();
            MyLeagues = new List<League>();
        }
    }
}
