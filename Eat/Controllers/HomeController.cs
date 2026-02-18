using System.Diagnostics;
using Eat.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eat.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }


    }
}
