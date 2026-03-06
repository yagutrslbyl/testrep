using Eat.Models;

namespace Eat.ViewModels.ProfileVMs
{
    public class ProfileVM
    {
        public AppUser User { get; set; }

        // Kullanıcının yayınlanmış hikayeleri
        public List<Story> PublishedStories { get; set; }

        // Kullanıcının draft hikayeleri (yalnızca kendisi görür)
        public List<Story> DraftStories { get; set; }

        // Takipçiler ve takip edilenler
        public List<UserFollow> Followers { get; set; }
        public List<UserFollow> Following { get; set; }

        // Opsiyonel: Current logged in user id (Follow butonunu göstermek için)
        public string CurrentUserId { get; set; }
    }
}
