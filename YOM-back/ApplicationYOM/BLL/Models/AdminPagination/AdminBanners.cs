namespace BLL.Models.AdminPagination
{
    public class AdminBanners
    {
        public IEnumerable<Banner> Banners { get; set; }
        public int TotalPages { get; set; }
    }
}
