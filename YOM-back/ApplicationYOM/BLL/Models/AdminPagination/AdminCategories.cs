namespace BLL.Models.AdminPagination
{
    public class AdminCategories
    {
        public IEnumerable<Category> Categories { get; set; }
        public int TotalPages { get; set; }
    }
}
