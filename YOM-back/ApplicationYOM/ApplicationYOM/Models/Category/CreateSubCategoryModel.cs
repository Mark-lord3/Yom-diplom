namespace PL.Models.Category
{
    public class CreateSubCategoryModel
    {
        public int CategoryId { get; set; }
        public string? Title { get; set; }
        public string? Section { get; set; }
        public ulong Clicks { get; set; }
    }
}
