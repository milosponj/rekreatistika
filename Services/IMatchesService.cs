using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rekreatistika.Models;
using Rekreatistika.Models.MatchViewModels;

namespace Rekreatistika.Services
{
    public interface IMatchesService
    {
        IList<Match> GetMatchesForLeague(int leagueId);
        List<Match> GetLatestMatches();
        MatchFullDetailsJsonModel GetFullMatchDetails(int id);
    }
}
