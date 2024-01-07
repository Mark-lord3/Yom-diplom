namespace BLL.Models.AdminPagination
{
    public class AdminSubCategories
    {
        public IEnumerable<SubCategory> SubCategories { get; set; }
        public int TotalPages { get; set; }
    }
}
