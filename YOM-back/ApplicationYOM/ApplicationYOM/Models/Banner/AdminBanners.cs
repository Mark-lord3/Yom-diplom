namespace PL.Models.Banner
{
    public class AdminBanners
    {
        public IEnumerable<AdminBannerModel> Banners { get; set; }
        public int TotalPages { get; set; }
    }
}
