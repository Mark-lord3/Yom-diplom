using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FullName { get; set; }
        public string? RefreshToken { get; set; }
        public string? AvatarPath { get; set; }
        public bool IsBlocked { get; set; } = false;
        public ulong? UserInfoId { get; set; }
        public UserConnectionInfo? UserInfo { get; set; }
        public DateTime RefreshTokenExpiryInDays { get; set; }
        public ICollection<FavoriteAd> FavoriteAds { get; set; }
        public ICollection<LastViewedHistory> LastViewedHistories { get; set; }
        public ICollection<Purchase> PurchasesAsBuyer { get; set; }
        public ICollection<Purchase> PurchasesAsSeller { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<UserConversation> UserConversations { get; set; }
        public ICollection<Ad> Ads { get; set; }
        public ICollection<ChatMessage> ChatMessagesSent { get; set; }
    }
}
