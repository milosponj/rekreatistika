using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Rekreatistika.Controllers
{
    public class ErrorHandlingController : Controller
    {
        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            // handle different codes or just return the default error view
            return View();
        }
    }
}