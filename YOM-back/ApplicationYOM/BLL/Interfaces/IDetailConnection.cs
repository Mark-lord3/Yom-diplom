using BLL.Models.SessionDetail;

namespace BLL.Interfaces
{
    public interface IDetailConnection
    {
        Task<SessionDetailModel> GetDetailOfSession();

        Task<TimeSpan> GetAverageSessionTime();

        Task<int> GetMonthRegistration();

        Task<SessionChartData> GetSessionChart();
    }
}
