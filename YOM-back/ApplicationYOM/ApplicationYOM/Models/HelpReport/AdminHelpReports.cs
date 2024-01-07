namespace PL.Models.HelpReport
{
    public class AdminHelpReports
    {
        public IEnumerable<UserHelpReportModel> UserHelpReports { get; set; }
        public int TotalPages { get; set; }
    }
}
