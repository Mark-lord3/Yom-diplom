namespace BLL.Models.Ads
{
    public class AdminPagedAd
    {
        public IEnumerable<Ad> Ads { get; set; }
        public int TotalPages { get; set; }
    }
}
