namespace Eat.Models
{
    public class Chapter
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        public int Views { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsPublished { get; set; } = false;
        // Foreign Key
        public int StoryId { get; set; }
        public ICollection<ChapterVote> Votes { get; set; } = new List<ChapterVote>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public Story Story { get; set; }
    }
}
