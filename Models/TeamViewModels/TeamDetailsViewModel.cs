using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rekreatistika.Models.PlayerViewModels;

namespace Rekreatistika.Models.TeamViewModels
{
    public class TeamDetailsViewModel
    {
        public string TeamName { get; set; }
        public int TeamId { get; set; }
        public IList<PlayerDetailsViewModel> Players { get; set; }
        public string LogoPath { get; set; }
        public LeagueStats Stats { get; set; }
        public int AdminId { get; set; }
        public string AdminName { get; set; }
		public bool IsAdmin { get; set; }

        public string ThumbnailPath => string.Format("thumb_{0}.jpg", TeamId);
    }
}
