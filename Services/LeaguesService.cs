using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Rekreatistika.Data;
using Rekreatistika.Models;

namespace Rekreatistika.Services
{
    public class LeaguesService : ILeaguesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMatchesService _matchesService;

        public LeaguesService(ApplicationDbContext context, IMatchesService matchesService)
        {
            _context = context;
            _matchesService = matchesService;
        }

        public IList<LeagueStats> GetLeagueStats(int id)
        {
            var matches = _matchesService.GetMatchesForLeague(id);
            var teams = _context.Leagues.Include(x => x.Teams).FirstOrDefault(x => x.Id == id)?.Teams;
            IList<LeagueStats> leagueStats = new List<LeagueStats>();
            foreach (var team in teams)
            {
                var matchesOfTeam = matches.Where(x => x.HomeTeamId == team.Id || x.AwayTeamId == team.Id);

                var teamLeagueStats = GetLeagueStatsForTeam(matchesOfTeam, team);
                leagueStats.Add(teamLeagueStats);
            }

            return leagueStats;
        }

        public async Task<bool> AddTeamsToLeague(List<int> teamIds, int leagueId)
        {
            var league = _context.Leagues.Include(x => x.Teams).FirstOrDefault(x => x.Id == leagueId);
            if (league != null)
            {
                foreach (var teamId in teamIds)
                {
                    var team = _context.Teams.FirstOrDefault(x => x.Id == teamId);
                    if (team != null)
                    {
                        league.Teams.Add(team);
                    }
                }

                _context.Update(league);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public void RemoveTeamFromLeague(int teamId, int leagueId)
        {
            var league = _context.Leagues.Include(x => x.Teams).FirstOrDefault(x => x.Id == leagueId);
            if (league != null)
            {
                league.Teams.Remove(league.Teams.FirstOrDefault(x => x.Id == teamId));

                _context.Update(league);
                _context.SaveChanges();
            }
        }

        public void AddGoals(int teamId, int matchId, List<int> scorerIds)
        {
            var currentGoalsOfTeam = _context.Goals.Where(x => x.MatchId == matchId && x.TeamId == teamId);
            //if there's already goals remove them
            if (currentGoalsOfTeam.Any())
            {
                _context.Goals.RemoveRange(currentGoalsOfTeam);
            }
            var scorers = _context.Players.Where(x => scorerIds.Contains(x.Id));
            foreach (var id in scorerIds)
            {
                var goal = new Goal()
                {
                    MatchId = matchId,
                    PlayerId = id,
                    TeamId = teamId,
                    PlayerName = scorers.First(x=>x.Id == id).Name
                };

              var cz =  _context.Add(goal);
            }
            _context.SaveChanges();

        }

        public string GetScorerIds(int matchId, int teamId)
        {
            var scorerIds = _context.Goals.Where(x => x.MatchId == matchId && x.TeamId == teamId).Select(x=>x.PlayerId);
            return string.Join(",", scorerIds);
        }


        private LeagueStats GetLeagueStatsForTeam(IEnumerable<Match> matches, Team team)
        {
            int goalsScored = 0;
            int goalsConceded = 0;
            int matchesPlayed = 0;
            int matchesWon = 0;
            int matchesLost = 0;
            int matchesDrawn = 0;

            foreach (Match m in matches)
            {
                matchesPlayed++;

                if (m.HomeTeamId == team.Id)
                {
                    goalsScored += m.HomeGoalsCount;
                    goalsConceded += m.AwayGoalsCount;

                    if (m.HomeGoalsCount > m.AwayGoalsCount)
                        matchesWon++;
                    else if (m.HomeGoalsCount == m.AwayGoalsCount)
                        matchesDrawn++;
                    else
                        matchesLost++;
                }
                else if (m.AwayTeamId == team.Id)
                {
                    goalsScored += m.AwayGoalsCount;
                    goalsConceded += m.HomeGoalsCount;

                    if (m.HomeGoalsCount < m.AwayGoalsCount)
                        matchesWon++;
                    else if (m.HomeGoalsCount == m.AwayGoalsCount)
                        matchesDrawn++;
                    else
                        matchesLost++;
                }
            }

            var leagueStats = new LeagueStats()
            {
                GoalsConceded = goalsConceded,
                GoalsScored = goalsScored,
                MatchesDrawn = matchesDrawn,
                MatchesLost = matchesLost,
                MatchesPlayed = matchesPlayed,
                MatchesWon = matchesWon,
                Team = team
            };

            return leagueStats;
        }

        public int Save(League league)
        {
            if (league.Id > 0)
            {
                var leagueToSave = _context.Leagues.SingleOrDefault(x => x.Id == league.Id);
                if (leagueToSave != null)
                {
                    leagueToSave.LeagueAdminEmails = league.LeagueAdminEmails;
                    leagueToSave.Name = league.Name;
                    _context.Update(leagueToSave);
                    _context.SaveChanges();
                }
            }
            else
            {
                _context.Add(league);
                _context.SaveChanges();
            }
            return league.Id;
        }

        public League GetLeague(int id)
        {
            var league = _context.Leagues.Include(x => x.Teams).FirstOrDefault(m => m.Id == id);
            return league;
        }

        public IList<League> GetLeagues()
        {
            return _context.Leagues.ToList();
        }

        public IList<Team> GetTeams(int leagueId)
        {
            var teams = _context.Leagues.Include(x => x.Teams).SingleOrDefault(x=>x.Id == leagueId).Teams;
            return teams.ToList();
        }

        public void AddMatch(Match match)
        {
            _context.Matches.Add(match);
            _context.SaveChanges();
        }
    }
}
