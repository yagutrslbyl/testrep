using Eat.DAL;
using Eat.Models;
using Eat.Utilities;
using Eat.ViewModels.Story;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eat.Controllers
{
    public class StoryController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _env;

        public StoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
       
        //public async Task<IActionResult> Create(StoryCreateViewModel model)
        //{
        //    // Dropdownlar post sonrası tekrar doldurulmalı
        //    ViewBag.Categories = _context.Categories.ToList();
        //    ViewBag.Languages = new List<string> { "English", "Turkish" };

        //    if (!ModelState.IsValid)
        //        return View(model);

        //    // Image kontrolü
        //    if (model.CoverImage == null)
        //    {
        //        ModelState.AddModelError("CoverImage", "Cover image is required.");
        //        return View(model);
        //    }

        //    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        //    var extension = Path.GetExtension(model.CoverImage.FileName).ToLower();

        //    if (!allowedExtensions.Contains(extension))
        //    {
        //        ModelState.AddModelError("CoverImage", "Only JPG, PNG or WEBP images are allowed.");
        //        return View(model);
        //    }

        //    if (model.CoverImage.Length > 2 * 1024 * 1024)
        //    {
        //        ModelState.AddModelError("CoverImage", "Image size must be less than 2MB.");
        //        return View(model);
        //    }

        //    var story = new Story
        //    {
        //        Title = model.Title,
        //        Description = model.Description,
        //        MainCharacters = model.MainCharacters,
        //        CategoryId = model.CategoryId.Value,
        //        Tags = model.Tags,
        //        Language = model.Language,
        //        CreatedDate = DateTime.Now,
        //        //Status = "Draft"
        //    };

        //    string folderPath = "uploads/stories";
        //    var fileName = await model.CoverImage.SaveImage(_env, folderPath);

        //    story.CoverImageUrl = Path.Combine(folderPath, fileName).Replace("\\", "/");

        //    // story.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    _context.Stories.Add(story);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("StartWriting", new { id = story.Id });
        //}


        [HttpGet]
        public IActionResult StartWriting(int id)
        {
            var story = _context.Stories.Find(id);

            if (story == null)
                return NotFound();

            return View(story);
        }
    }
    }
