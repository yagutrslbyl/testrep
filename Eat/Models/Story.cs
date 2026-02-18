namespace Eat.Models
{
    public class Story
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Language { get; set; }

        public string CoverImageUrl { get; set; }

        public string Tags { get; set; }
        public string MainCharacters  { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign Key
        public int UserId { get; set; }

        // Navigation Property
        public AppUser User { get; set; }

        public ICollection<Chapter> Chapters { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
