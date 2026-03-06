namespace Eat.Models
{
    public class Story
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Language { get; set; }

        public string? CoverImageUrl { get; set; }

        public string? Tags { get; set; }
        public string? MainCharacters  { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsPublished { get; set; } = false;

        // Foreign Key
        public string UserId { get; set; }

        // Navigation Property
        public AppUser User { get; set; }
        public int Views { get; set; } = 0;
        public int Votes { get; set; } = 0;

        public ICollection<Chapter> Chapters { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
