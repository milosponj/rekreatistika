using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using PostgreSqlDotnetCore.Models.ManageViewModels;
using Rekreatistika.Data;
using Rekreatistika.Models;
using Rekreatistika.Services;

namespace Rekreatistika.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ILogger<HomeController> _logger;
        private readonly IMatchesService _matchesService;

        public HomeController(IStringLocalizer<HomeController> localizer, ILogger<HomeController> logger, IMatchesService matchesService)
        {
            _localizer = localizer;
            _logger = logger;
            _matchesService = matchesService;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel()
            {
                LatestMatches = _matchesService.GetLatestMatches()
            };
            return View(model);
        }

        public IActionResult About()
        {


            ViewData["Message"] = _localizer["Your application description page."];
            string x = _localizer["Hello"];
            ViewData["hello"] = x;
            return View();
        }

        [HttpGet]
        public IActionResult MatchDetailsResult()
        {
            return ViewComponent("MatchDetails", 7);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
