using Eat.DAL;
using Eat.Models;
using Eat.ViewModels.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eat.Controllers
{
    public class NotificationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public NotificationController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            // Kullanıcının hikayeleri
            var myStoryIds = await _context.Stories
                .Where(s => s.UserId == user.Id)
                .Select(s => s.Id)
                .ToListAsync();

            // Votes
            var votes = await _context.StoryVotes
                .Include(v => v.User)
                .Include(v => v.Story)
                .Where(v => myStoryIds.Contains(v.StoryId))
                .OrderByDescending(v => v.Id)
                .ToListAsync();

            // Comments
            var comments = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Story)
                .Where(c => myStoryIds.Contains(c.StoryId))
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            var model = new NotificationViewModel
            {
                Votes = votes,
                Comments = comments
            };

            return View(model);
        }
    }
}
