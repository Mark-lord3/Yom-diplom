namespace PL.Models.Category
{
    public class AdminCategories
    {
        public IEnumerable<CategoryModel> Categories { get; set; }
        public int TotalPages { get; set; }
    }
}
