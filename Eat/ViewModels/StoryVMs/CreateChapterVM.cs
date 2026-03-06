using Eat.Models;
using System.ComponentModel.DataAnnotations;

namespace Eat.ViewModels.StoryVMs
{
    public class CreateChapterVM
    {
        public int? Id { get; set; }  

        public int StoryId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public List<Chapter>? Chapters { get; set; } 
    }
}
