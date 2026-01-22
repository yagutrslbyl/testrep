using Eat.DAL;
using Eat.Models;
using Eat.Utilities;
using Eat.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Eat.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        AppDbContext _context;
        IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            
        }
        public IActionResult Index()
        {
            var pro = _context.Products.Include(x => x.Category).ToList();
            return View(pro);
        }

        public IActionResult Create()
        {
            ViewBag.Category = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM vm)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Category = _context.Categories.ToList();
                return View(vm);
            }
            ViewBag.Category = _context.Categories.ToList();
            var image = await vm.File.SaveImage(_env, "Upload/Product");

            Product product = new()
            {
                Name = vm.Name,
                CategoryId = vm.CategoryId,
                Category = _context.Categories.FirstOrDefault(x => x.Id == vm.CategoryId),

                Image =image
            };

            _context.Products.Add(product);
            _context.SaveChanges();


            return RedirectToAction("Index");

        }

        public IActionResult Update(int id)
        {
            var pro = _context.Products.FirstOrDefault(x => x.Id == id);

            UpdateProductVM vm = new()
            {
                Name = pro.Name,
                CategoryId = pro.CategoryId,
                
            };

            ViewBag.Category = _context.Categories.ToList();
            return View(vm);
        }

        [HttpPost]

        public async Task<IActionResult> Update(UpdateProductVM vm)
        {
            ViewBag.Category = _context.Categories.ToList();

            var existpro = _context.Products.FirstOrDefault(x => x.Id == vm.Id);

            existpro.Name = vm.Name;
            existpro.Category = _context.Categories.FirstOrDefault(x => x.Id == vm.CategoryId);
            if (vm.File != null)
            {
                var image = await vm.File.SaveImage(_env, "Upload/Product");
                existpro.Image = image;
            }
            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        public IActionResult Delete(int id)
        {
            var pro = _context.Products.FirstOrDefault(x => x.Id == id);
            _context.Products.Remove(pro);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}
