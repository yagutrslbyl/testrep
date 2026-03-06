namespace Eat.Models
{
    public class ChapterVote
    {
        public int Id { get; set; }
        public string UserId { get; set; }    // Oy veren kullanıcı
        public int ChapterId { get; set; }    // Hangi chapter
        public bool Upvote { get; set; }      // Upvote mı, downvote mı

        public Chapter Chapter { get; set; }
    }
}
