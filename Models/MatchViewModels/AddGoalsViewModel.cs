using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rekreatistika.Models.MatchViewModels
{
    public class AddGoalsViewModel
    {
        public int MatchId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public Match Match { get; set; }
        public SelectList HomePlayersSelectList { get; set; }
        public SelectList AwayPlayersSelectList { get; set; }
        public string HomeScorerIds { get; set; }
        public string AwayScorerIds { get; set; }
        public int LeagueId { get; set; }
		public bool IsHomeTeamAdmin { get; set; }
	    public bool IsAwayTeamAdmin { get; set; }
	}
}
