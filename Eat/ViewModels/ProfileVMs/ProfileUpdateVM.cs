using System.ComponentModel.DataAnnotations;

namespace Eat.ViewModels.ProfileVMs
{
    public class ProfileUpdateVM
    {
        [Required]
        public string UserName { get; set; }

        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? Pronoun { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public IFormFile? CoverImage { get; set; }

        public string? ExistingProfileImage { get; set; }
        public string? ExistingCoverImage { get; set; }
    }
}
