using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models.MatchViewModels
{
    public class MatchFullDetailsJsonModel
    {
        public MatchFullDetailsJsonModel()
        {
            HomeTeamScorers = new List<string>();
            AwayTeamScorers = new List<string>();
        }

        public string HomeTeamName { get; set; }
        public string HomeTeamLogoUrl { get; set; }
        public int HomeTeamGoalsScored { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamLogoUrl { get; set; }
        public int AwayTeamGoalsScored { get; set; }
        public IList<string> HomeTeamScorers { get; set; }
        public IList<string> AwayTeamScorers { get; set; }
        public DateTime Time { get; set; }
    }
}
