using Rekreatistika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Services
{
    public interface ILeaguesService
    {
        IList<LeagueStats> GetLeagueStats(int leagueID);
        Task<bool> AddTeamsToLeague(List<int> teamId, int leagueId);
        void RemoveTeamFromLeague(int teamId, int leagueId);
        void AddGoals(int teamId, int matchId, List<int> homeScorerIds);
        string GetScorerIds(int matchId, int teamId);
        int Save(League league);
        League GetLeague(int id);
        IList<League> GetLeagues();
        IList<Team> GetTeams(int leagueId);
        void AddMatch(Match match);
    }
}
