using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rekreatistika.Data;
using Rekreatistika.Models;

namespace Rekreatistika.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMatchesService _matchesService;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Player> GetUserPlayers(string email)
        {
            var players = _context.Players.Where(x => x.Email == email);
            return players.ToList();
        }

        public IList<Team> GetUserTeams(string email)
        {
            var teams = _context.Teams.Where(x => x.TeamAdminEmails.Contains(email));
            return teams.ToList();
        }

        public IList<League> GetUserLeagues(string email)
        {
            var leagues = _context.Leagues.Where(x => x.LeagueAdminEmails.Contains(email));
            return leagues.ToList();
        }

        public IList<Match> GetUserMatches(string email)
        {
            var myPlayers = GetUserPlayers(email);
            List<Team> teamsIAmPlayingFor = new List<Team>();
            foreach (var player in myPlayers)
            {
                var teams = _context.Teams.Include(x => x.Players).Where(x => x.Players.Contains(player));
                teamsIAmPlayingFor.AddRange(teams.ToList());
            }
            var teamIds = teamsIAmPlayingFor.Select(x => x.Id);
            var matches = _context.Matches.Include(x=>x.HomeTeam).Include(x=>x.AwayTeam).Where(x => teamIds.Contains(x.HomeTeamId) || teamIds.Contains(x.AwayTeamId))
                .Take(100).ToList();
            return matches;
        }
    }
}
