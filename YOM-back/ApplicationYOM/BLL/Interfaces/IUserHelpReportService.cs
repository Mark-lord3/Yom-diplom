using BLL.Models.Enums;
using BLL.Models;
using BLL.Models.AdminPagination;

namespace BLL.Interfaces
{
    public interface IUserHelpReportService
    {
        Task CreateHelpReportAsync(UserHelpReport model);
        Task DeleteHelpReportAsync(UserHelpReport model);
        Task<UserHelpReport> GetHelpReportAsync(ulong reportId);
        Task<IEnumerable<UserHelpReport>> GetAllUserReportsAsync(string userId);

        Task<AdminHelpReports> GetAllUserReportsByStatusAsync(ReportStatus reportStatus,
            GridSortDirection gridSortDirection, int? pageNumber);
        Task UpdateReportStatusAsync(UserHelpReport model);
    }
}
