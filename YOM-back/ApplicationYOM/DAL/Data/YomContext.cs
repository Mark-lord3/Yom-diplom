using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using DAL.Migrations;

namespace DAL.Data
{
    public partial class YomContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public YomContext(DbContextOptions<YomContext> options) : base(options)
        {
        }

        public virtual DbSet<Ad> Ads { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<FavoriteAd> FavoriteAds { get; set; }
        public virtual DbSet<UserReview> UserReviews { get; set; }
        public virtual DbSet<UserHelpReport> UserHelpReports { get; set; }
        public virtual DbSet<LastViewedHistory> LastViewedHistories { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<Auto> Autos { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<UserConversation> UserConversations { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<UserConnectionInfo> UserConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ApplicationUser>().Property(e => e.FullName).HasMaxLength(256);
            modelBuilder.Entity<UserConversation>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<FavoriteAd>()
                .HasOne(fa => fa.User)
                .WithMany(u => u.FavoriteAds)
                .HasForeignKey(fa => fa.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FavoriteAd>()
                .HasOne(fa => fa.Ad)
                .WithMany()
                .HasForeignKey(fa => fa.AdId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.UserInfo)
                .WithOne()
                .HasForeignKey<ApplicationUser>(a => a.UserInfoId);

            modelBuilder.Entity<LastViewedHistory>()
                .HasOne(lvh => lvh.User)
                .WithMany(u => u.LastViewedHistories)
                .HasForeignKey(lvh => lvh.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LastViewedHistory>()
                .HasOne(lvh => lvh.Ad)
                .WithMany(u => u.LastViewedHistories)
                .HasForeignKey(lvh => lvh.AdId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Purchase>()
                .HasOne(pr => pr.Buyer)
                .WithMany(u => u.PurchasesAsBuyer)
                .HasForeignKey(pr => pr.BuyerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Purchase>()
                .HasOne(pr => pr.Seller)
                .WithMany(u => u.PurchasesAsSeller)
                .HasForeignKey(pr => pr.SellerId)
                .OnDelete(DeleteBehavior.NoAction);

            
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserConversations)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.Conversation)
                .WithMany(c => c.UserConversations)
                .HasForeignKey(uc => uc.ConversationId);

            modelBuilder.Entity<LastViewedHistory>()
                .HasOne(lvh => lvh.Ad)
                .WithMany(ad => ad.LastViewedHistories)
                .HasForeignKey(lvh => lvh.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Ad)
                .WithMany(ad => ad.Purchases)
                .HasForeignKey(p => p.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserReview>()
                .HasOne(ur => ur.Ad)
                .WithMany(ad => ad.UserReviews)
                .HasForeignKey(ur => ur.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ad>()
                .HasOne(ur => ur.User)
                .WithMany(ad => ad.Ads)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteAd>()
                .HasOne(fa => fa.Ad)
                .WithMany(ad => ad.FavoriteAds)
                .HasForeignKey(fa => fa.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.Sender)
                .WithMany(u => u.ChatMessagesSent)
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}