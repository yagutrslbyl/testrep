using Eat.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eat.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoriesController : Controller
    {
        private readonly AppDbContext _context;

        public StoriesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stories = await _context.Stories
                .Where(s => s.IsPublished)
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            return View(stories);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var story = await _context.Stories.FindAsync(id);

            if (story == null)
                return NotFound();

            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
