using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Eat.ViewModels.Story
{
    public class StoryCreateViewModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string MainCharacters { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        [Required]
        public string Language { get; set; }

        public string Tags { get; set; }

        public IFormFile CoverImage { get; set; }

        // Dropdown için
        public List<SelectListItem> Categories { get; set; }
    }

}
