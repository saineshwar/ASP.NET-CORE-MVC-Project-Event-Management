using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventApplicationCore.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /<controller>/
        public IActionResult Error()
        {
            HttpContext.Session.Clear();
            return View("Error");
        }
    }
}
