using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rekreatistika.Data;
using Rekreatistika.Models;

namespace Rekreatistika.Services
{
    public class TeamsService : ITeamsService
    {
        private readonly ApplicationDbContext _context;

        public TeamsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PickTeamViewModel> SearchTeams(string text)
        {
            var teams = _context.Teams.Where(x => x.Name.ToLower().Contains(text.ToLower()));
            IList<PickTeamViewModel> teamViewModels = new List<PickTeamViewModel>();
            foreach (var team in teams)
            {
                var teamModel = new PickTeamViewModel()
                {
                    Id = team.Id,
                    Name = team.Name,
                };

                teamViewModels.Add(teamModel);
            }

            return teamViewModels;
        }

        public List<Player> GetPlayersForTeam(int teamId)
        {
            var team = _context.Teams.Include(x => x.Players).Single(x => x.Id == teamId);
            return team.Players.ToList();
        }

        public int Save(Team team)
        {
            if (team.Id > 0)
            {
                var teamToSave = _context.Teams.SingleOrDefault(x => x.Id == team.Id);
                if (teamToSave != null)
                {
                    teamToSave.TeamAdminEmails = team.TeamAdminEmails;
                    teamToSave.Name = team.Name;
					teamToSave.ModifiedBy = team.ModifiedBy;
					teamToSave.ModifiedOn = team.ModifiedOn;
                    _context.Update(teamToSave);
                    _context.SaveChanges();
                }
            }
            else
            {
                _context.Add(team);
                _context.SaveChanges();
            }
            return team.Id;
        }

        public Team GetTeamWithPlayers(int id)
        {
            return _context.Teams.Include(x => x.Players).FirstOrDefault(x => x.Id == id);
        }

	    public Team GetTeam(int id)
	    {
		    return _context.Teams.Include(x => x.Players).FirstOrDefault(x => x.Id == id);
	    }

		public IList<Team> GetTeamsToSelect(int id)
        {
            return _context.Teams.ToList();
        }

        public async Task<bool> AddPlayersToTeam(List<int> playerIds, int teamId)
        {
            var team = _context.Teams.Include(x => x.Players).FirstOrDefault(x => x.Id == teamId);
            if (team != null)
            {
                foreach (var playerId in playerIds)
                {
                    var player = _context.Players.FirstOrDefault(x => x.Id == playerId);
                    if (player != null)
                    {
                        team.Players.Add(player);
                    }
                }

                _context.Update(team);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IList<Team> GetAllTeams()
        {
            return _context.Teams.ToList();
        }

        public IEnumerable<Team> GetTeamsThatAreNotInLeague(int leagueId)
        {
            var league = _context.Leagues.Include(x=>x.Teams).FirstOrDefault(x => x.Id == leagueId);
            var teamIds = league.Teams.Select(x=>x.Id);
            var teams = _context.Teams.Where(x => !teamIds.Contains(x.Id));
            return teams;
        }
    }
}
