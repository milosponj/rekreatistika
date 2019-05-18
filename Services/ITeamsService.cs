using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rekreatistika.Models;

namespace Rekreatistika.Services
{
    public interface ITeamsService
    {
        IList<PickTeamViewModel> SearchTeams(string text);
        List<Player> GetPlayersForTeam(int teamId);
        int Save(Team team);
        Team GetTeamWithPlayers(int id);
        IList<Team> GetTeamsToSelect(int id);
        Task<bool> AddPlayersToTeam(List<int> playerIds, int modelTeamId);
        IList<Team> GetAllTeams();
	    Team GetTeam(int id);
        IEnumerable<Team> GetTeamsThatAreNotInLeague(int leagueId);
    }
}
