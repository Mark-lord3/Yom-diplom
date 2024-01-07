namespace PL.Models.Category
{
    public class AdminSubCategories
    {
        public IEnumerable<SubCategoryModel> SubCategories { get; set; }
        public int TotalPages { get; set; }
    }
}
