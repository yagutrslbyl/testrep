using System.ComponentModel.DataAnnotations;

namespace Eat.ViewModels.Product
{
    public class CreateProductVM
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category is required")]

        public int CategoryId { get; set; }
        [Required(ErrorMessage = "image is required")]

        public IFormFile File { get; set; }
    }
}
