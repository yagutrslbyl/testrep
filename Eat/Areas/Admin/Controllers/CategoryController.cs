using Eat.DAL;
using Eat.Models;
using Eat.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace Eat.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var cat = _context.Categories.ToList();
            return View(cat);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateCategoryVM vm)
        {
            Category category = new()
            {
                Name = vm.Name
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var cat = _context.Categories.FirstOrDefault(x => x.Id == id);
            UpdateCategoryVM vm = new()
            {
                Name = cat.Name
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Update(UpdateCategoryVM vm)
        {
            var existCat = _context.Categories.FirstOrDefault(x => x.Id == vm.Id);
            existCat.Name = vm.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var cat = _context.Categories.FirstOrDefault(x => x.Id == id);
            _context.Categories.Remove(cat);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
