using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostgreSqlDotnetCore.Models
{
    public class MatchDetailsViewModel
    {

        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string HomeTeamLogoAddress { get; set; }
        public string AwayTeamLogoAddress { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public IList<string> HomeScorersNames { get; set; }
        public IList<string> AwayScorerNames { get; set; }
        public string DateOfMatch { get; set; }
        public bool IsHomeTeamWinner => HomeTeamGoals > AwayTeamGoals;
        public bool IsAwayTeamWinner => AwayTeamGoals > HomeTeamGoals;
    }
}
