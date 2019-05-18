using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rekreatistika.Data;
using Rekreatistika.Models;
using Rekreatistika.Models.LeagueViewModels;
using Rekreatistika.Models.MatchViewModels;
using Rekreatistika.Services;

namespace Rekreatistika.Controllers
{
    public class MatchesController : Controller
    {

        private readonly ILeaguesService _leaguesService;
        private readonly IMatchesService _matchesService;
	    private readonly ITeamsService _teamsService;

        public MatchesController(ApplicationDbContext context, ILeaguesService leaguesService, IMatchesService matchesService, ITeamsService teamsService)
        {
            _leaguesService = leaguesService;
            _matchesService = matchesService;
	        _teamsService = teamsService;
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Matches.Include(m => m.AwayTeam).Include(m => m.HomeTeam);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Matches/FullDetails/id
        public async Task<MatchFullDetailsJsonModel> FullDetails(int id)
        {
            MatchFullDetailsJsonModel fullDetails = _matchesService.GetFullMatchDetails(id);
            var match = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return null;
            }

            var matchDetails = new MatchFullDetailsJsonModel()
            {
                HomeTeamName = match.HomeTeam.Name,
                HomeTeamGoalsScored = match.HomeGoalsCount,
                HomeTeamLogoUrl = "TODO",
                AwayTeamName = match.AwayTeam.Name,
                AwayTeamLogoUrl = "TODO",
                AwayTeamGoalsScored = match.AwayGoalsCount,
                Time = match.Time,
                AwayTeamScorers = new List<string>() { "mirko", "svirko" },
                HomeTeamScorers = new List<string>() { "mirko", "svirko" }
            };
            return fullDetails;
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View("Details", match);
        }

        // GET: Matches/Create
        //public IActionResult Create()
        //{
        //    ViewData["AwayTeamId"] = new SelectList(_context.Teams, "Id", "Id");
        //    ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Id");
        //    return View();
        //}

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HomeTeamId,AwayTeamId,Time,Venue,HomeGoalsCount,AwayGoalsCount")] Match match)
        {
	        var league = _leaguesService.GetLeague(match.LeagueId);
	        if (!league.LeagueAdminEmails.Split(",").Contains(User.Identity.Name))
		        return NotFound();

			if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamId"] = new SelectList(_context.Teams, "Id", "Id", match.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Id", match.HomeTeamId);
            return View(match);
        }

        // GET: Matches/AddGoals/5
        public async Task<IActionResult> AddScorers(int id)
        {
            var match = await _context.Matches.FindAsync(id);

            if (match == null)
            {
                return NotFound();
            }

            var model = new AddGoalsViewModel()
            {
                LeagueId = match.LeagueId,
                HomeTeamId = match.HomeTeamId,
                AwayTeamId = match.AwayTeamId,
                MatchId = match.Id,
                Match = match,
                AwayPlayersSelectList = new SelectList(_context.Teams.Include(x => x.Players).SingleOrDefault(x => x.Id == match.AwayTeamId)?.Players, "Id", "Name"),
                HomePlayersSelectList = new SelectList(_context.Teams.Include(x => x.Players).SingleOrDefault(x => x.Id == match.HomeTeamId)?.Players, "Id", "Name"),
                HomeScorerIds = _leaguesService.GetScorerIds(match.Id, match.HomeTeamId),
                AwayScorerIds = _leaguesService.GetScorerIds(match.Id, match.AwayTeamId)
            };
	        var homeTeam = _teamsService.GetTeam(match.HomeTeamId);
	        var awayTeam = _teamsService.GetTeam(match.AwayTeamId);
	        if (homeTeam.TeamAdminEmails.Contains(User.Identity.Name))
		        model.IsHomeTeamAdmin = true;
	        if (awayTeam.TeamAdminEmails.Contains(User.Identity.Name))
		        model.IsAwayTeamAdmin = true;
	        if (!model.IsHomeTeamAdmin && !model.IsAwayTeamAdmin)
		        return NotFound();
			
			return View(model);
        }

        // POST Matches/AddGoals/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddScorers(AddGoalsViewModel model)
        {
	        var homeTeam = _teamsService.GetTeam(model.HomeTeamId);
	        var awayTeam = _teamsService.GetTeam(model.AwayTeamId);
	        string user = User.Identity.Name;
	        if (!homeTeam.TeamAdminEmails.Contains(user) && !awayTeam.TeamAdminEmails.Contains(user))
	        {
		        return NotFound();
	        }

            var homeScorerIdStrings = model.HomeScorerIds?.Split(',');
            var awayScorerIdStrings = model.AwayScorerIds?.Split(',');
            List<int> homeScorerIds = new List<int>();
            if (homeScorerIdStrings != null)
            {
                foreach (string idString in homeScorerIdStrings)
                {
                    if (int.TryParse(idString, out int id))
                    {
                        homeScorerIds.Add(id);
                    }
                }
            }

            List<int> awayScorerIds = new List<int>();
            if (awayScorerIdStrings != null)
            {
                foreach (string idString in awayScorerIdStrings)
                {
                    if (int.TryParse(idString, out int id))
                    {
                        awayScorerIds.Add(id);
                    }
                }
            }
            if (homeScorerIds.Count > 0)
            {
                _leaguesService.AddGoals(model.HomeTeamId, model.MatchId, homeScorerIds);
            }
            if (awayScorerIds.Count > 0)
            {
                _leaguesService.AddGoals(model.AwayTeamId, model.MatchId, awayScorerIds);
            }

            return RedirectToAction(nameof(Details), "Leagues", new { id = model.LeagueId});
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
			
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }

			var league = _leaguesService.GetLeague(match.LeagueId);
	        if (!league.LeagueAdminEmails.Contains(User.Identity.Name))
		        return NotFound();

			var model = new CreateMatchViewModel();

            model.TeamsSelectList = new SelectList(_context.Leagues.Include(x => x.Teams).SingleOrDefault(x => x.Id == match.LeagueId)?.Teams, "Id", "Name");

            model.LeagueId = match.LeagueId;
            model.LeagueName = _context.Leagues.SingleOrDefault(x => x.Id == match.LeagueId)?.Name;
            model.Time = DateTime.Now.Date;
            model.HomeTeamId = match.HomeTeamId;
            model.HomeGoalsCount = match.HomeGoalsCount;
            model.AwayGoalsCount = match.AwayGoalsCount;
            model.AwayTeamId = match.AwayTeamId;
            return View(model);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HomeTeamId,AwayTeamId,Time,Venue,HomeGoalsCount,AwayGoalsCount,LeagueId")] Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (match.HomeTeamId == match.AwayTeamId)
            {
                ModelState.AddModelError("SameTeamsError", "Tim ne može igrati sam protiv sebe.");
            }

	        var league = _leaguesService.GetLeague(match.LeagueId);
	        if (!league.LeagueAdminEmails.Contains(User.Identity.Name))
		        return NotFound();

			if (ModelState.IsValid)
            {
                try
                {
					match.ModifiedBy = User.Identity.Name;
					match.ModifiedOn = DateTime.Now;

					_context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Leagues", new { id = match.LeagueId });
            }
            ViewData["AwayTeamId"] = new SelectList(_context.Teams, "Id", "Id", match.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Id", match.HomeTeamId);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var match = await _context.Matches.FindAsync(id);
            //_context.Matches.Remove(match);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
