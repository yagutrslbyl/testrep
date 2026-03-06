using Eat.Models;

namespace Eat.ViewModels.StoryVMs
{
    public class StoryDashboardVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public bool IsMature { get; set; }

        public string? CoverImageUrl { get; set; }

        public string? Notes { get; set; }

        public List<ChapterListItemVM> Chapters { get; set; }
        public int TotalViews { get; set; }       // Tüm chapterların view toplamı
        public int Votes { get; set; }            // StoryVotes tablosundan toplam oy
        public int CommentCount { get; set; }
    }
}
