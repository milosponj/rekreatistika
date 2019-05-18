using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rekreatistika.Data;
using Rekreatistika.Models;
using Rekreatistika.Models.LeagueViewModels;
using Rekreatistika.Models.TeamViewModels;
using Rekreatistika.Services;

namespace Rekreatistika.Controllers
{
    public class LeaguesController : Controller
    {
        private readonly IMatchesService _matchesService;
        private readonly ILeaguesService _leaguesService;
        private readonly ITeamsService _teamsService;
	    private readonly ILogger _logger;

		public LeaguesController(IMatchesService matchesService, ILeaguesService leaguesService, ILogger<AccountController> logger, ITeamsService teamsService)
        {
            _matchesService = matchesService;
            _leaguesService = leaguesService;
            _teamsService = teamsService;
	        _logger = logger;
        }

        // GET: Leagues
        public async Task<IActionResult> Index()
        {
            var model = new LeagueIndexViewModel();
            _logger.LogError("hads");

            model.Leagues = _leaguesService.GetLeagues();
            return View(model);
        }

        // GET: Leagues/Details/5
        public async Task<IActionResult> Details(int id)
        {

            League league = _leaguesService.GetLeague(id);
            if (league == null)
            {
                return NotFound();
            }

            var model = new LeagueDetailsViewModel()
            {
                LeagueId = league.Id,
                LeagueName = league.Name,
                Matches = _matchesService.GetMatchesForLeague(league.Id),
                LeagueStats = _leaguesService.GetLeagueStats(league.Id),
				IsAdmin = User.Identity.Name != null && league.LeagueAdminEmails.Contains(User.Identity.Name)
            };

            return View(model);
        }

        // GET: Leagues/AddTeams
        public IActionResult AddTeams(int leagueId)
        {
	        var league = _leaguesService.GetLeague(leagueId);
			//if he's not admin he can't add teams
			if (!league.LeagueAdminEmails.Contains(User.Identity.Name)) { 
				_logger.LogWarning("User with email - " + User?.Identity?.Name + " tried to edit league with id " + leagueId);
				return NotFound();
			}
            AddTeamsViewModel model = new AddTeamsViewModel()
            {
                LeagueId = leagueId,
                League = league,
                CurrentTeams = league.Teams.Select(x => new Models.TeamViewModels.TeamDetailsViewModel()
                {
                    TeamName = x.Name,
                    TeamId = x.Id
                }).ToList()
			};

            model.TeamsToSelect = _teamsService.GetTeamsThatAreNotInLeague(leagueId).Select(x=>new Models.TeamViewModels.TeamDetailsViewModel()
            {
                TeamName = x.Name,
                TeamId = x.Id
            }).ToList();

            return View("AddTeams", model);
        }

        // POST: Leagues/AddTeams
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTeams(AddTeamsViewModel model)
        {
			var league = _leaguesService.GetLeague(model.LeagueId);
			if (!league.LeagueAdminEmails.Contains(User.Identity.Name))
		        return NotFound();
			if (string.IsNullOrEmpty(model.SelectedTeamIds))
            {
                ModelState.AddModelError("AddingTeamsNumber", "Morate označiti barem jedan tim kojeg želite dodati.");
                model.League = league;
                model.TeamsToSelect = _teamsService.GetTeamsThatAreNotInLeague(league.Id).Select(x => new TeamDetailsViewModel() {
                    TeamId = x.Id,
                    TeamName = x.Name
                }).ToList();
                return View(model);
            }
            var teamIdsString = model.SelectedTeamIds.Split(',').ToList();

            var teamIds = teamIdsString.Select(x => int.Parse(x)).ToList();

            await _leaguesService.AddTeamsToLeague(teamIds, model.LeagueId);

            return RedirectToAction(nameof(Details), new { id = model.LeagueId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveTeam(RemoveTeamFromLeagueViewModel model)
        {

	  //      var league = _leaguesService.GetLeague(model.LeagueId);
	  //      if (!league.LeagueAdminEmails.Split(",").Contains(User.Identity.Name))
		 //       return NotFound();
			//_leaguesService.RemoveTeamFromLeague(model.TeamId, model.LeagueId);

            return RedirectToAction(nameof(Details), new { id = model.LeagueId });
        }



        // GET: Leagues/CreateMatch
        public IActionResult CreateMatch(int leagueId)
        {
            var league = _leaguesService.GetLeague(leagueId);
	        if (!league.LeagueAdminEmails.Contains(User.Identity.Name))
		        return NotFound();

			if (league != null)
            {
                var model = new CreateMatchViewModel();
                model.TeamsSelectList = new SelectList(league.Teams, "Id", "Name");
                model.LeagueId = leagueId;
                model.LeagueName = league.Name;
                model.Time = DateTime.Now.Date;
                return View(model);
            }

            return NoContent();
        }

        // POST: Leagues/CreateMatch
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateMatch([Bind("Id,HomeTeamId,AwayTeamId,Time,Venue,HomeGoalsCount,AwayGoalsCount, LeagueId")] CreateMatchViewModel model)
        {
            var league = _leaguesService.GetLeague(model.LeagueId);

            bool isUserLeaguesAdmin = league.LeagueAdminEmails.Contains(User.Identity.Name);

            if (isUserLeaguesAdmin || User.IsInRole(CustomRoles.SuperAdmin))
            {
                if (model.HomeTeamId == model.AwayTeamId)
                {
                    ModelState.AddModelError("SameTeamsError", "Tim ne može igrati sam protiv sebe.");
                }
                if (ModelState.IsValid)
                {
                    var match = new Match()
                    {
                        AwayTeamId = model.AwayTeamId,
                        HomeTeamId = model.HomeTeamId,
                        HomeGoalsCount = model.HomeGoalsCount.Value,
                        AwayGoalsCount = model.AwayGoalsCount.Value,
                        Time = model.Time,
                        LeagueId = model.LeagueId,
                        Venue = model.Venue,
						CreatedBy = User.Identity.Name,
						CreatedOn = DateTime.Now
					};
                    _leaguesService.AddMatch(match);

                    return RedirectToAction(nameof(Details), new { id = model.LeagueId });
                }
                model.TeamsSelectList = new SelectList(_leaguesService.GetLeague(model.LeagueId).Teams, "Id", "Name");
                return View(model);
            }

            return NotFound("You had no right to do that.");

        }


        // GET: Leagues/Create
        [Authorize]
        public IActionResult Create()
        {
            var model = new EditLeagueViewModel()
            {
                CurrentUserEmail = User.Identity.Name,
            };
            return View(model);
        }
        // POST: Leagues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditLeagueViewModel model)
        {
            if (ModelState.IsValid)
            {
                _leaguesService.Save(new League()
                {
                    Name = model.Name,
					CreatedBy = User.Identity.Name,
					CreatedOn = DateTime.Now,
					LeagueAdminEmails = model.LeagueAdminEmails,
                });

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Leagues/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
	        var league = _leaguesService.GetLeague(id);

			if (!league.LeagueAdminEmails.Contains(User.Identity.Name))
		        return NotFound();
			
            if (league == null)
            {
                return NotFound();
            }
            return View(league);
        }

        // POST: Leagues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditLeagueViewModel model)
        {
	        var l = _leaguesService.GetLeague(model.Id);
	        if (!l.LeagueAdminEmails.Contains(User.Identity.Name))
		        return NotFound();

			if (ModelState.IsValid)
            {
                var league = new League()
                {
                    Name = model.Name,
                    LeagueAdminEmails = model.LeagueAdminEmails,
					ModifiedBy = User.Identity.Name,
					ModifiedOn = DateTime.Now
				};

                _leaguesService.Save(league);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Leagues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(id);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var league = await _context.Leagues.FindAsync(id);
            //_context.Leagues.Remove(league);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
