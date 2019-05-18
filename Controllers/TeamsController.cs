using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rekreatistika.Data;
using Rekreatistika.Models;
using Rekreatistika.Models.PlayerViewModels;
using Rekreatistika.Models.TeamViewModels;
using Rekreatistika.Services;

namespace Rekreatistika.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITeamsService _teamsService;
        private readonly IImagesService _imagesService;
        private readonly IOptions<AppSettings> _appSettings;

        public TeamsController(ApplicationDbContext context, ITeamsService teamsService,  IImagesService imagesService, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _teamsService = teamsService;
            _imagesService = imagesService;
            _appSettings = appSettings;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Team team = _teamsService.GetTeamWithPlayers(id);

            if (team == null)
            {
                return NotFound();
            }

            var model = new TeamDetailsViewModel()
            {
                TeamName = team.Name,
                TeamId = team.Id,
                Players = team.Players.Select(x => new PlayerDetailsViewModel()
                {
                    Id = x.Id,
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Email,
                    Name=x.Name,
                    Nickname = x.Nickname,
                }).ToList(),
                LogoPath = String.Format("{0}{1}", team.Id, ".png"),
				IsAdmin = User.Identity.Name != null && team.TeamAdminEmails.Contains(User.Identity.Name)
            };

            return View(model);
        }

        public IActionResult AddPlayers(int id)
        {
            var team = _teamsService.GetTeamWithPlayers(id);
            if(team.TeamAdminEmails.Contains(User.Identity.Name)){
                var model = new AddPlayersViewModel();
                model.TeamId = id;
                model.CurrentPlayers = team.Players.ToList();
                model.PlayersToSelect = _context.Players.Where(x=>!model.CurrentPlayers.Contains(x)).ToList();
                model.TeamName = team.Name;
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayers(AddPlayersViewModel model)
        {
            if (string.IsNullOrEmpty(model.SelectedPlayerIds))
            {
                ModelState.AddModelError("AddingPlayersNumber", "Morate označiti barem jednog igrača kojeg želite dodati.");
                var team = _teamsService.GetTeamWithPlayers(model.TeamId);
                model.TeamId = model.TeamId;
                model.CurrentPlayers = team.Players.ToList();
                model.PlayersToSelect = _context.Players.Where(x => !model.CurrentPlayers.Contains(x)).ToList();
                model.TeamName = team.Name;
                return View(model);
            }
            var playerIdsString = model.SelectedPlayerIds.Split(',').ToList();

            var playerIds = playerIdsString.Select(x=>x.Trim()).Select(x => int.Parse(x)).ToList();

            var isAdded = await _teamsService.AddPlayersToTeam(playerIds, model.TeamId);

            return RedirectToAction("Details","Teams",new {id=model.TeamId});



        }
        [Authorize]        // GET: Teams/Create
        public IActionResult Create()
        {
            var model = new EditTeamViewModel()
            {
                CurrentUserEmail = User.Identity.Name
            };
            return View(model);
        }

        // POST: Teams/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditTeamViewModel model)
        {
            var emails = Utilities.ExtractEmails(model.TeamAdminEmails);
            if (emails.Count < 1)
            {
                ModelState.AddModelError("CurrentUserEmail", "Morate upisati i sačuvati barem jedan validan email.");
            }
            if (ModelState.IsValid)
            {
                
                var team = new Team()
                {
                    Name = model.Name,
					CreatedBy = User.Identity.Name,
					CreatedOn = DateTime.Now,
                    TeamAdminEmails = String.Join(",", emails)
                };

                var teamId = _teamsService.Save(team);

                if (model.Logo != null && model.Logo.Length > 0)
                {
                    await _imagesService.SavePicture(model.Logo, _appSettings.Value.StorageFolder + @"/teams", teamId.ToString());
                }

                return RedirectToAction(nameof(Details), new {id=teamId});
            }
            return View(model);
        }

        
        // GET: Teams/Edit/5
		[Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _context.Teams.FindAsync(id);
	        if (!team.TeamAdminEmails.Contains(User.Identity.Name))
	        {
		        return NotFound();
	        }
            var model = new EditTeamViewModel()
            {
                Id = team.Id,
                Name = team.Name,
                TeamAdminEmails = team.TeamAdminEmails,
            };

            return View(model);
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Edit(EditTeamViewModel model)
        {
            var emails = Utilities.ExtractEmails(model.TeamAdminEmails);
	        var t = await _context.Teams.FindAsync(model.Id);
	        if (!t.TeamAdminEmails.Contains(User.Identity.Name))
	        {
		        return NotFound();
	        }
			if (emails.Count < 1)
            {
                ModelState.AddModelError("CurrentUserEmail", "Morate upisati i sačuvati barem jedan validan email.");
            }
            if (ModelState.IsValid)
            {

                var team = new Team()
                {
					Id = model.Id,
                    Name = model.Name,
					ModifiedBy = User.Identity.Name,
					ModifiedOn = DateTime.Now,
                    TeamAdminEmails = String.Join(",", emails)
                };

                _teamsService.Save(team);

                if (model.Logo != null && model.Logo.Length > 0)
                {
                    await _imagesService.SavePicture(model.Logo, _appSettings.Value.StorageFolder + @"/teams", model.Id.ToString());
                }

                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            return View(model);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var team = await _context.Teams.FindAsync(id);
            //_context.Teams.Remove(team);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pick
        public async Task<IActionResult> Pick()
        {
            var teams = _teamsService.SearchTeams("");
            return View(teams);
        }

        // GET: Pick
        public async Task<IActionResult> PickPlayerInTeam(int id)
        {
            List<Player> players = _teamsService.GetPlayersForTeam(id);
            return View(players);
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }


    }
}
