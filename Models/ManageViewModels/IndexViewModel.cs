using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Rekreatistika.Models;

namespace PostgreSqlDotnetCore.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public List<Match> LatestMatches { get; set; } 

        public string StatusMessage { get; set; }

        public IList<League> UserLeagues { get; set; }
        public IList<Team> UserTeams { get; set; }
        public IList<Player> UserPlayers { get; set; }

        public IndexViewModel()
        {
            UserLeagues = new List<League>();
            UserTeams = new List<Team>();
            UserPlayers = new List<Player>();
        }
    }
}
