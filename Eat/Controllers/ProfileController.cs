using Eat.DAL;
using Eat.Models;
using Eat.Utilities;
using Eat.ViewModels.ProfileVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace Eat.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        IWebHostEnvironment _env;

        public ProfileController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // 🔹 Profile Page
        public async Task<IActionResult> Index(string? id)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Eğer id null ise → kendi profiline giriyor
            if (id == null)
                id = currentUserId;

            var user = await _context.Users
                .Include(u => u.Stories)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            var vm = new ProfileVM
            {
                User = user,
                CurrentUserId = currentUserId,

                PublishedStories = user.Stories
                    .Where(s => s.IsPublished)
                    .ToList(),

                DraftStories = user.Stories
                    .Where(s => !s.IsPublished)
                    .ToList(),

                Followers = await _context.UserFollows
                    .Include(f => f.Follower)
                    .Where(f => f.FollowingId == id)
                    .ToListAsync(),

                Following = await _context.UserFollows
                    .Include(f => f.Following)
                    .Where(f => f.FollowerId == id)
                    .ToListAsync()
            };

            return View(vm);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Follow(string userId)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == userId)
                return BadRequest();

            var existingFollow = await _context.UserFollows
                .FirstOrDefaultAsync(f =>
                    f.FollowerId == currentUserId &&
                    f.FollowingId == userId);

            if (existingFollow == null)
            {
                _context.UserFollows.Add(new UserFollow
                {
                    FollowerId = currentUserId,
                    FollowingId = userId
                });
            }
            else
            {
                _context.UserFollows.Remove(existingFollow);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = userId });
        }
        [Authorize]
        public async Task<IActionResult> Update()
        {
            var user = await _userManager.GetUserAsync(User);

            var vm = new ProfileUpdateVM
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Bio = string.IsNullOrEmpty(user.Bio)
    ? "Merhaba! Ben storyVerce kullanıyorum!"
    : user.Bio,
                Pronoun = user.Pronoun,
                ExistingProfileImage = user.ProfileImageUrl,
                ExistingCoverImage = user.CoverImageUrl
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(ProfileUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(vm);
            }

            var user = await _userManager.GetUserAsync(User);

            // Username update (Identity olduğu için özel update gerekir)
            if (user.UserName != vm.UserName)
            {
                var result = await _userManager.SetUserNameAsync(user, vm.UserName);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username update failed");
                    return View(vm);
                }
            }

            user.FullName = vm.FullName;
            user.Bio = vm.Bio;
            user.Pronoun = vm.Pronoun;

            // 🔥 Profile Image Upload
            if (vm.ProfileImage != null)
            {
                string path = Path.Combine("uploads", "profiles");
                user.ProfileImageUrl = await vm.ProfileImage
                    .SaveImage(_env, path);
            }

            // 🔥 Cover Image Upload
            if (vm.CoverImage != null)
            {
                string path = Path.Combine("uploads", "covers");
                user.CoverImageUrl = await vm.CoverImage
                    .SaveImage(_env, path);
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", new { id = user.Id });
        }
    }
}

    
