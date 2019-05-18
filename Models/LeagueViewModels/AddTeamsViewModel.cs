using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rekreatistika.Models.TeamViewModels;

namespace Rekreatistika.Models.LeagueViewModels
{
    public class AddTeamsViewModel
    {
        public int LeagueId { get; set; }
        public League League { get; set; }
        public IList<TeamDetailsViewModel> TeamsToSelect { get; set; }

        public IList<TeamDetailsViewModel> CurrentTeams { get; set; }
        public string SelectedTeamIds { get; set; }
    }
}
