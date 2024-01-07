namespace BLL.Models.AdminPagination
{
    public class AdminHelpReports
    {
        public IEnumerable<UserHelpReport> UserHelpReports { get; set; }
        public int TotalPages { get; set; }
    }
}
