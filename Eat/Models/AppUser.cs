using Microsoft.AspNetCore.Identity;

namespace Eat.Models
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? Pronoun { get; set; }

        public string? ProfileImageUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public ICollection<Story> Stories { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
