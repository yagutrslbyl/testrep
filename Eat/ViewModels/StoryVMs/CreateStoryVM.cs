using System.ComponentModel.DataAnnotations;
using Eat.Models;

namespace Eat.ViewModels.StoryVMs
{
    public class CreateStoryVM
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(2000)]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Category is required")]
        public int? CategoryId { get; set; }

        //[Required(ErrorMessage = "Language is required")]
        public string? Language { get; set; }

        [Required(ErrorMessage = "Cover image is required")]
        public IFormFile CoverImage { get; set; }

        [MaxLength(250)]
        public string? Tags { get; set; }

        [MaxLength(250)]
        public string? MainCharacters { get; set; }

        public List<Category>? Categories { get; set; }
    }
}