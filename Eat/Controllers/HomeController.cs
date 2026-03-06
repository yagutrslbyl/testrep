using Eat.DAL;
using Eat.Models;
using Eat.ViewModels.StoryVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Eat.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Publish edilmiş storyleri al, en az 1 published chapter içerenleri
            var stories = await _context.Stories
                .Include(s => s.Chapters)
                .Where(s => s.Chapters.Any(c => c.IsPublished))
                .ToListAsync();

            var vm = new PublishedStoriesVM
            {
                Stories = stories
            };

            return View(vm);
        }


    }
}
