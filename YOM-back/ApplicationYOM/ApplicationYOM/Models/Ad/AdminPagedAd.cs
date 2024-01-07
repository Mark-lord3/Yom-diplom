namespace PL.Models.Ad
{
    public class AdminPagedAd
    {
        public IEnumerable<AdModel> Ads { get; set; }
        public int TotalPages { get; set; }
    }
}
