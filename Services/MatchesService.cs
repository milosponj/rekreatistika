using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Rekreatistika.Data;
using Rekreatistika.Models;
using Rekreatistika.Models.MatchViewModels;

namespace Rekreatistika.Services
{
    public class MatchesService : IMatchesService
    {
        private readonly ApplicationDbContext _context;
        private IMemoryCache _cache;


        public MatchesService(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public IList<Match> GetMatchesForLeague(int leagueId)
        {
            var matches = _context.Matches.Include(x => x.HomeTeam).Include(x => x.AwayTeam).Where(x => x.LeagueId == leagueId).ToList();
            return matches;
        }

        public List<Match> GetLatestMatches()
        {
            //we cache this for an hour
            var cacheEntry = _cache.GetOrCreate("LatestMatches", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                var matches = _context.Matches.Include(x => x.HomeTeam).Include(x => x.AwayTeam).Include(x => x.League)
                    .OrderByDescending(x => x.Time).Take(5).ToList();
                return matches;
            });

            return cacheEntry;
        }

        public MatchFullDetailsJsonModel GetFullMatchDetails(int id)
        {
            var match = _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .FirstOrDefault(m => m.Id == id);

            if (match == null)
            {
                return null;
            }

            var awayScorerNames = _context.Goals.Where(x => x.MatchId == id && x.TeamId == match.AwayTeamId)
                .Select(x => x.PlayerName).ToList();

            var homeScorerNames = _context.Goals.Where(x => x.MatchId == id && x.TeamId == match.HomeTeamId)
                .Select(x => x.PlayerName).ToList();

            var matchDetails = new MatchFullDetailsJsonModel()
            {
                HomeTeamName = match.HomeTeam.Name,
                HomeTeamGoalsScored = match.HomeGoalsCount,
                HomeTeamLogoUrl = match.HomeTeamId+".jpg",
                AwayTeamName = match.AwayTeam.Name,
                AwayTeamLogoUrl = match.AwayTeamId + ".jpg",
                AwayTeamGoalsScored = match.AwayGoalsCount,
                Time = match.Time,
                AwayTeamScorers = awayScorerNames,
                HomeTeamScorers = homeScorerNames
            };

            return matchDetails;
        }
    }
}
