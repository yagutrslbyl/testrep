using Eat.DAL;
using Eat.Models;
using Eat.ViewModels.StoryVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eat.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(new List<ReaderStoryDetailsVM>());
        }

        [HttpGet]
        public async Task<IActionResult> Results(
           string query,
           string lengthFilter,       // örn: "1-10", "10-20"
           string lastUpdatedFilter)  // örn: "Today", "ThisWeek", "ThisMonth"
        {
            if (string.IsNullOrEmpty(query))
                return View("Index", new List<ReaderStoryDetailsVM>());

            // query'nin ilk 3 harfini al
            string searchKey = query.Length >= 3 ? query.Substring(0, 3) : query;

            var stories = await _context.Stories
                .Include(s => s.User)
                .Include(s => s.Chapters)
                .Include(s => s.Category)
                .Where(s => s.IsPublished &&
        (
            s.Title.ToLower().Contains(query.ToLower()) ||
            (s.Description != null && s.Description.ToLower().Contains(query.ToLower())) ||
            (s.Category != null && s.Category.Name.ToLower().Contains(query.ToLower())) ||
            (s.Tags != null && s.Tags.ToLower().Contains(query.ToLower()))
        ))
    .ToListAsync();

            // Length filtreleme
            if (!string.IsNullOrEmpty(lengthFilter) && lengthFilter != "Any")
            {
                stories = lengthFilter switch
                {
                    "1-10" => stories.Where(s => s.Chapters.Count >= 1 && s.Chapters.Count <= 10).ToList(),
                    "10-20" => stories.Where(s => s.Chapters.Count >= 10 && s.Chapters.Count <= 20).ToList(),
                    "20-50" => stories.Where(s => s.Chapters.Count >= 20 && s.Chapters.Count <= 50).ToList(),
                    "50+" => stories.Where(s => s.Chapters.Count > 50).ToList(),
                    _ => stories
                };
            }

            // Last Updated filtreleme
            if (!string.IsNullOrEmpty(lastUpdatedFilter) && lastUpdatedFilter != "Anytime")
            {
                var now = DateTime.Now;
                stories = lastUpdatedFilter switch
                {
                    "Today" => stories.Where(s => s.CreatedDate.Date == now.Date).ToList(),
                    "ThisWeek" => stories.Where(s => s.CreatedDate >= now.AddDays(-7)).ToList(),
                    "ThisMonth" => stories.Where(s => s.CreatedDate >= now.AddMonths(-1)).ToList(),
                    _ => stories
                };
            }

            // Null kontrolü ve VM dönüşümü
            var vmList = stories.Select(s => new ReaderStoryDetailsVM
            {
                Id = s.Id,
                Title = s.Title ?? "Untitled",
                CoverImageUrl = s.CoverImageUrl ?? "/images/default-cover.jpg",
                Description = s.Description ?? "",
                AuthorName = s.User?.UserName ?? "Unknown",
                Language = s.Language ?? "Unknown",
                TotalViews = s.Views,
                Votes = s.Votes,
                Chapters = s.Chapters?.Select(c => new ReaderChapterVM
                {
                    Id = c?.Id ?? 0,
                    Title = c?.Title ?? "Untitled Chapter"
                }).ToList() ?? new List<ReaderChapterVM>(),
                Tags = s.Tags?.Split(',').ToList() ?? new List<string>(),
                RelatedStories = new List<RelatedStoryVM>()
            }).ToList();

            ViewBag.SearchQuery = query;
            ViewBag.LengthFilter = lengthFilter ?? "Any";
            ViewBag.LastUpdatedFilter = lastUpdatedFilter ?? "Anytime";

            return View("Index", vmList);
        }
    } 
}
