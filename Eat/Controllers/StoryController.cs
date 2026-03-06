using Eat.DAL;
using Eat.Models;
using Eat.Utilities;
using Eat.ViewModels.StoryVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Eat.Controllers
{
   
    public class StoryController : Controller
    {
        private const string V = "Notfound";
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        UserManager<AppUser> _userManager;

        public StoryController(AppDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var stories = await _context.Stories
                 .Where(s => s.IsPublished)
                .Include(s => s.Category)
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            return View(stories);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> StartChapter(int id)
        {
            // Story’i ve mevcut chapter’ları al
            var story = await _context.Stories
                                      .Include(s => s.Chapters)
                                      .FirstOrDefaultAsync(s => s.Id == id);
            if (story == null) return NotFound();

            // Yeni chapter oluştur (otomatik olarak en son sıraya gelir)
            var chapter = new Chapter
            {
                StoryId = id,
                Title = $"Chapter {story.Chapters.Count + 1}", // Yeni bölüm başlığı
                Content = "", // Boş içerik
                CreatedDate = DateTime.UtcNow
            };

            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();

            // Kullanıcıyı Write sayfasına yönlendir
            return RedirectToAction("Write", new { storyId = id, chapterId = chapter.Id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var story = await _context.Stories
                .Include(s => s.Category)
                .Include(s => s.User)
                 .FirstOrDefaultAsync(s => s.Id == id && s.IsPublished);

            if (story == null)
                return NotFound();

            var vm = new StoryDetailsVM
            {
                Story = story,
                Chapters = await _context.Chapters
                     .Where(c => c.StoryId == id && c.IsPublished)
                    .OrderBy(c => c.CreatedDate)
                    .ToListAsync(),

                Comments = await _context.Comments
                    .Where(c => c.StoryId == id)
                    .Include(c => c.User)
                    .OrderByDescending(c => c.CreatedDate)
                    .ToListAsync()
            };

            return View(vm);
        }
        public IActionResult Create()
        {
            var vm = new CreateStoryVM
            {
                Categories = _context.Categories.ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStoryVM vm)
        {

            if (vm.CoverImage == null)
            {
                ModelState.AddModelError("CoverImage", "Cover image is required.");
            }
            if (!ModelState.IsValid)
            {
                vm.Categories = _context.Categories.ToList();
                return View(vm);
            }

            string fileName = null;

            if (vm.CoverImage != null)
            {
                fileName = await vm.CoverImage.SaveImage(_env, "uploads/stories");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var story = new Story
            {
                Title = vm.Title,
                Description = vm.Description,
                CategoryId = vm.CategoryId,
                Language = vm.Language,
                CoverImageUrl = fileName,
                Tags = vm.Tags,
                MainCharacters = vm.MainCharacters,
                UserId = userId,
                CreatedDate = DateTime.Now
            };

            _context.Stories.Add(story);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Write), new { storyId = story.Id });
        }


        public async Task<IActionResult> Edit(int id)
        {
            var story = await _context.Stories.FindAsync(id);

            if (story == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", story.CategoryId);

            return View(story);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Story model, IFormFile newCover)
        {
            var story = await _context.Stories.FindAsync(model.Id);

            if (story == null)
                return NotFound();

            story.Title = model.Title;
            story.Description = model.Description;
            story.Language = model.Language;
            story.CategoryId = model.CategoryId;
            story.Tags = model.Tags;
            story.MainCharacters = model.MainCharacters;

            if (newCover != null)
            {
                story.CoverImageUrl = await newCover.SaveImage(_env, "uploads/stories");
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = story.Id });
        }
        public async Task<IActionResult> Delete(int id)
        {
            var story = await _context.Stories.FindAsync(id);

            if (story == null)
                return NotFound();

            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyStories));
        }

        public async Task<IActionResult> Write(int storyId, int? chapterId)
        {
            var story = await _context.Stories
                .Include(s => s.Chapters)
                .FirstOrDefaultAsync(s => s.Id == storyId);

            if (story == null)
                return NotFound();

            var chapters = story.Chapters
                .OrderBy(c => c.CreatedDate)
                .ToList();

            Chapter? selectedChapter = null;

            if (chapterId != null)
                selectedChapter = chapters.FirstOrDefault(c => c.Id == chapterId);

            var vm = new CreateChapterVM
            {
                StoryId = storyId,
                Id = selectedChapter?.Id,
                Title = selectedChapter?.Title,
                Content = selectedChapter?.Content,
                Chapters = chapters
            };

            ViewBag.StoryTitle = story.Title;

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveChapter(CreateChapterVM vm)
        {
            // 1️⃣ Story mövcuddurmu?
            var story = await _context.Stories
                .Include(s => s.Chapters)
                .FirstOrDefaultAsync(s => s.Id == vm.StoryId);

            if (story == null)
                return NotFound();

            // 2️⃣ Validation səhvi varsa → siyahını doldur və geri qaytar
            if (!ModelState.IsValid)
            {
                vm.Chapters = story.Chapters
                    .OrderBy(c => c.CreatedDate)
                    .ToList();

                ViewBag.StoryTitle = story.Title;

                return View("Write", vm);
            }

            Chapter chapter;

            // 3️⃣ Yeni chapter
            if (vm.Id == null || vm.Id == 0)
            {
                chapter = new Chapter
                {
                    StoryId = vm.StoryId,
                    Title = vm.Title.Trim(),
                    Content = vm.Content.Trim(),
                    CreatedDate = DateTime.UtcNow
                };

                _context.Chapters.Add(chapter);
            }
            // 4️⃣ Mövcud chapter update
            else
            {
                chapter = await _context.Chapters
                    .FirstOrDefaultAsync(c => c.Id == vm.Id && c.StoryId == vm.StoryId);

                if (chapter == null)
                    return NotFound();

                chapter.Title = vm.Title.Trim();
                chapter.Content = vm.Content.Trim();
            }

            // 5️⃣ DB-yə yaz
            await _context.SaveChangesAsync();

            // 6️⃣ Siyahını yenidən yüklə (dropdown üçün)
            var updatedChapters = await _context.Chapters
                .Where(c => c.StoryId == vm.StoryId)
                .OrderBy(c => c.CreatedDate)
                .ToListAsync();

            // 7️⃣ ViewModel-i yenilə
            vm.Id = chapter.Id;
            vm.Chapters = updatedChapters;

            ViewBag.StoryTitle = story.Title;

            // 8️⃣ Eyni səhifədə qal
            return View("Write", vm);
        }

        


        public IActionResult MyStories()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var stories = _context.Stories
                .Where(s => s.UserId == userId)
                .Select(s => new StoryDashboardVM
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Language = s.Language,
                    //IsMature = s.IsMature,
                    CoverImageUrl = s.CoverImageUrl,
                    //Notes = s.Notes,
                    Chapters = s.Chapters.Select(c => new ChapterListItemVM
                    {
                        Id = c.Id,
                        Title = c.Title,
                        CreatedDate = c.CreatedDate,
                        //Views = c.Views,
                        //Votes = c.Votes,
                        //CommentsCount = c.Comments.Count
                    }).ToList()
                }).ToList();

            return View(stories);
        }
        [HttpGet]
        public async Task<IActionResult> StoryDetails(int id)
        {
            if (id == null)
            {
                return Content("not found");
            }

            var story = await _context.Stories
       .Include(s => s.Chapters)
       .Include(s => s.Comments)
       .Include(s => s.User)
       .FirstOrDefaultAsync(s => s.Id == id);

           

            var vm = new StoryDashboardVM
            {
                Id = story.Id,
                Title = story.Title,
                Description = story.Description,
                Language = story.Language,
                CoverImageUrl = story.CoverImageUrl,
                // Kullanıcının toplam Views'u, Chapters toplamı
                TotalViews = story.Chapters.Sum(c => c.Views),
                Votes = story.Votes,
                CommentCount = story.Comments?.Count ?? 0, // null değilse direkt count

                // Chapters detayları
                Chapters = story.Chapters
                    .OrderByDescending(c => c.CreatedDate)
                    .Select(c => new ChapterListItemVM
                    {
                        Id = c.Id,
                        Title = c.Title,
                        CreatedDate = c.CreatedDate,
                        Views = c.Views,
                        Votes = c.Votes.Count,
                        CommentsCount = c.Comments?.Count ?? 0
                    })
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoryDetails(StoryDashboardVM model, IFormFile newCover)
        {
            var story = await _context.Stories.FindAsync(model.Id);
            if (story == null) return NotFound();

            story.Title = model.Title ?? story.Title;
            story.Description = model.Description ?? story.Description; // null gelirse eski değer kalır
            story.Language = model.Language ?? story.Language;

            if (newCover != null)
            {
                story.CoverImageUrl = await newCover.SaveImage(_env, "uploads/stories");
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(StoryDetails), new { id = story.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishChapter(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Story)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chapter == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 🔐 Təhlükəsizlik
            if (chapter.Story.UserId != userId)
                return Forbid();

            // Chapter publish et
            chapter.IsPublished = true;

            // Əgər story publish deyilsə və ilk published chapter-dırsa
            if (!chapter.Story.IsPublished)
            {
                chapter.Story.IsPublished = true;
            }

            await _context.SaveChangesAsync();
            //return RedirectToAction("Write", new { storyId = chapter.StoryId });
            return RedirectToAction(nameof(Write), new { storyId = chapter.StoryId, chapterId = chapter.Id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> ReadChapter(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Story)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsPublished);

            if (chapter == null) return NotFound();

            // Views artır
            chapter.Views++;
            await _context.SaveChangesAsync();

            // Previous və Next
            var previousChapter = await _context.Chapters
                .Where(c => c.StoryId == chapter.StoryId && c.Id < id && c.IsPublished)
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();

            var nextChapter = await _context.Chapters
                .Where(c => c.StoryId == chapter.StoryId && c.Id > id && c.IsPublished)
                .OrderBy(c => c.Id)
                .FirstOrDefaultAsync();

            // Vote-lar
            var upvotes = await _context.StoryVotes
                .CountAsync(v => v.StoryId == chapter.StoryId && v.Upvote);

            var downvotes = await _context.StoryVotes
                .CountAsync(v => v.StoryId == chapter.StoryId && !v.Upvote);

            bool? userVote = null;
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = _userManager.GetUserId(User);
                var vote = await _context.StoryVotes
                    .FirstOrDefaultAsync(v => v.StoryId == chapter.StoryId && v.UserId == currentUserId);
                if (vote != null) userVote = vote.Upvote;
            }

            // Comment-lər
            var comments = await _context.Comments
                .Where(c => c.StoryId == chapter.StoryId)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new CommentItemVM
                {
                    UserName = c.User.UserName,
                    Content = c.Content,
                    CreatedDate = c.CreatedDate
                })
                .ToListAsync();

            var vm = new ReaderChapterVM
            {
                Id = chapter.Id,
                StoryId = chapter.StoryId,
                Title = chapter.Title,
                Content = chapter.Content,
                Views = chapter.Views,
                CreatedDate = chapter.CreatedDate,
                StoryTitle = chapter.Story.Title,
                AuthorId = chapter.Story.User.Id,
                AuthorName = chapter.Story.User.UserName,
                AuthorProfileImage = chapter.Story.User.ProfileImageUrl,

                CoverImage = chapter.Story.CoverImageUrl,
                PreviousChapterId = previousChapter?.Id,
                NextChapterId = nextChapter?.Id,
                Upvotes = upvotes,
                Downvotes = downvotes,
                Score = upvotes - downvotes,
                UserVote = userVote,
                CommentCount = comments.Count,
                Comments = comments
            };

            return View(vm);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ReaderDetails(int id)
        {
            var story = await _context.Stories
                .Include(s => s.User)
                .Include(s => s.Chapters)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsPublished);

            if (story == null) return NotFound();

           

            var relatedStories = await _context.Stories
        .Where(s => s.CategoryId == story.CategoryId &&
                    s.Id != story.Id &&
                    s.IsPublished)
        .OrderByDescending(s => s.Views)
        .Take(5)
        .Select(s => new RelatedStoryVM
        {
            Id = s.Id,
            Title = s.Title,
            CoverImageUrl = s.CoverImageUrl,
            Views = s.Views,
            Votes = s.Votes
        })
        .ToListAsync();

            var vm = new ReaderStoryDetailsVM
            {
                Id = story.Id,
                Title = story.Title,
                CoverImageUrl = story.CoverImageUrl,
                Description = story.Description,
                AuthorId=story.User.Id,
                AuthorName = story.User.UserName,
                Language = story.Language,
                Tags = story.Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                TotalViews = story.Chapters.Sum(c => c.Views),
                Chapters = story.Chapters
                    .Where(c => c.IsPublished)
                    .OrderBy(c => c.CreatedDate)
                    .Select(c => new ReaderChapterVM
                    {
                        Id = c.Id,
                        Title = c.Title,
                        CreatedDate = c.CreatedDate,
                        Views = c.Views
                    }).ToList(),
                RelatedStories = relatedStories
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Vote(int storyId, bool isUpvote)
        {
            var userId = _userManager.GetUserId(User);

            var existingVote = await _context.StoryVotes
                .FirstOrDefaultAsync(v => v.StoryId == storyId && v.UserId == userId);

            if (existingVote == null)
            {
                // Yeni vote
                var vote = new StoryVote
                {
                    StoryId = storyId,
                    UserId = userId,
                    Upvote = isUpvote
                };

                _context.StoryVotes.Add(vote);
            }
            else
            {
                if (existingVote.Upvote == isUpvote)
                {
                    // Eyni vote yenidən basılıb → sil
                    _context.StoryVotes.Remove(existingVote);
                }
                else
                {
                    // Vote dəyişdirilir
                    existingVote.Upvote = isUpvote;
                }
            }

            await _context.SaveChangesAsync();

            // Yeni score hesabla
            var upvotes = await _context.StoryVotes
                .CountAsync(v => v.StoryId == storyId && v.Upvote);

            var downvotes = await _context.StoryVotes
                .CountAsync(v => v.StoryId == storyId && !v.Upvote);

            return Json(new
            {
                success = true,
                score = upvotes - downvotes,
                upvotes,
                downvotes
            });
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Comment boş ola bilməz.");

            var userId = _userManager.GetUserId(User);

            var comment = new Comment
            {
                StoryId = request.StoryId,
                UserId = userId,
                Content = request.Content,
                CreatedDate = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var commentVM = new CommentItemVM
            {
                UserName = User.Identity.Name,
                Content = request.Content,
                CreatedDate = comment.CreatedDate
            };

            return Json(commentVM);
        }

        // JSON-u bind etmək üçün
        public class CommentRequest
        {
            public int StoryId { get; set; }
            public string Content { get; set; }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteChapter(int chapterId, int storyId)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Story)
                .FirstOrDefaultAsync(c => c.Id == chapterId);

            if (chapter == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Güvenlik kontrolü
            if (chapter.Story.UserId != userId)
                return Forbid();

            _context.Chapters.Remove(chapter);

            await _context.SaveChangesAsync();

            return RedirectToAction("StoryDetails", new { id = storyId });
        }
    }

}

