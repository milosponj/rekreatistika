using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rekreatistika.Models.LeagueViewModels
{
    public class CreateMatchViewModel
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        public int LeagueId { get; set; }
        public string LeagueName { get; set; }

        public DateTime Time { get; set; }
        public string Venue { get; set; }

        [Required(ErrorMessage = "Broj golova se mora postaviti za novi meč!")]
        public int? HomeGoalsCount { get; set; }
        [Required(ErrorMessage = "Broj golova se mora postaviti za novi meč!")]
        public int? AwayGoalsCount { get; set; }

        public SelectList TeamsSelectList { get; set; }

        public string FlatpickrDateOfMatchFormat => String.Format("{0}-{1}-{2}", Time.Year,
            Time.Month, Time.Day);
    }
}
