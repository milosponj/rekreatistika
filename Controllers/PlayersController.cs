using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rekreatistika.Data;
using Rekreatistika.Models;
using Rekreatistika.Models.PlayerViewModels;
using Rekreatistika.Services;

namespace Rekreatistika.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPlayersService _playersService;
        private readonly IImagesService _imagesService;
        private readonly IOptions<AppSettings> _appSettings;

        public PlayersController(ApplicationDbContext context, IPlayersService playersService, IImagesService imagesService, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _playersService = playersService;
            _imagesService = imagesService;
            _appSettings = appSettings;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
	        var isSuperAdmin = Utilities.IsSuperAdmin(User);
			var players = await _context.Players.ToListAsync();
			var playerDetailsModels = players.Select(x => new PlayerDetailsViewModel()
			{
				Id = x.Id,
				Name = x.Name,
				DateOfBirth = x.DateOfBirth,
				Nickname = x.Nickname
			});
			return View(playerDetailsModels);
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            var model = new PlayerDetailsViewModel()
            {
                Name = player.Name,
                DateOfBirth = player.DateOfBirth,
                Email = player.Email,
                Id = player.Id,
                Nickname = player.Nickname,
				IsAdmin = User.Identity.Name != null && player.Email == User.Identity.Name
            };

            return View(model);
        }

        // GET: Players/Create
		[Authorize]
        public IActionResult Create()
        {
            var model = new EditPlayerViewModel()
            {
                Email = User.Identity.Name
            };
            return View(model);
        }

        // POST: Players/Create
        [HttpPost]
		[Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditPlayerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var playerId = _playersService.Save(model.Name, User.Identity.Name, DateTime.Now, model.Nickname, model.Email, model.DateOfBirth);

                if (model.Avatar != null && model.Avatar.Length > 0)
                {
                    await _imagesService.SavePicture(model.Avatar, _appSettings.Value.StorageFolder + @"/players", playerId.ToString());
                }

                return RedirectToAction(nameof(Details), new {id = playerId });
            }

            return View(model);
        }

        // GET: Players/Edit/5
		[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            var model = new EditPlayerViewModel()
            {
                Id = player.Id,
                Name = player.Name,
                Nickname = player.Nickname,
                DateOfBirth = player.DateOfBirth,
                Email = player.Email
            };
            
            return View(model);
        }

        // POST: Players/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPlayerViewModel model)
        {
			var player = await _context.Players.FindAsync(model.Id);
			if (player.Email != User.Identity.Name &&  !Utilities.IsSuperAdmin(User))
		        return NotFound();

			if (ModelState.IsValid)
            {
				await _playersService.Update(model.Id, model.Name, User.Identity.Name, DateTime.Now, model.Nickname, model.Email, model.DateOfBirth);

                if (model.Avatar != null && model.Avatar.Length > 0)
                {
                    await _imagesService.SavePicture(model.Avatar, _appSettings.Value.StorageFolder + @"/players", model.Id.ToString());
                }
            }
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var player = await _context.Players.FindAsync(id);
            //_context.Players.Remove(player);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
