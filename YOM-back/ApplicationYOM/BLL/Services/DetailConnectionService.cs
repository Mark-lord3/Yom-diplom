using AutoMapper;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models.Enums;
using BLL.Models.SessionDetail;
using DAL.Interfaces;
using UserDevice = DAL.Entities.UserDevice;

namespace BLL.Services
{
    public class DetailConnectionService : IDetailConnection
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DetailConnectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SessionDetailModel> GetDetailOfSession()
        {
            var count = await _unitOfWork.UserConnectionRepository.GetCount(null);
            var countOfDesktop = await _unitOfWork.UserConnectionRepository.GetCount(u=> u.UserDevice == UserDevice.Desktop);
            var countOfTablet = await _unitOfWork.UserConnectionRepository.GetCount(u => u.UserDevice == UserDevice.Tablet);
            var countOfMobile = await _unitOfWork.UserConnectionRepository.GetCount(u => u.UserDevice == UserDevice.Mobile);
            var countOfUnknown = await _unitOfWork.UserConnectionRepository.GetCount(u => u.UserDevice == UserDevice.Unknown);
            return new SessionDetailModel
            {
                Desktop = (double)countOfDesktop / count * 100,
                Tablet = (double)countOfTablet / count * 100,
                Mobile = (double)countOfMobile / count * 100,
                Unknown = (double)countOfUnknown / count * 100,
            };
        }

        public async Task<TimeSpan> GetAverageSessionTime()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

            string formattedDate = startDate.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            var filterState = FilterExtension.CreateSingleFilter("DateLogin", GridFilterOperator.gt, formattedDate, // filter
                GridSortDirection.desc, "DateLogin", 1, 100); // sort
            var mappedFilterState = _mapper.Map<DAL.Helpers.FilterState>(filterState);
            var result = await _unitOfWork.UserConnectionRepository.GetAllByQueryAsync(mappedFilterState);
            if (result is null) return TimeSpan.MinValue;
            List<TimeSpan> times = result.Select(conn => conn.DateLogout - conn.DateLogin).ToList();
            var averageMilliseconds = times.Select(t => t.TotalMilliseconds).Average();
            var averageTime = TimeSpan.FromMilliseconds(averageMilliseconds);

            return averageTime;
        }

        public async Task<int> GetMonthRegistration()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            var endDate = startDate.AddMonths(1);
            var count = await _unitOfWork.UserConnectionRepository.GetCount(u => u.DateCreated >= startDate && u.DateCreated < endDate);
            return count;
        }

        public async Task<SessionChartData> GetSessionChart()
        {
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(-1);
            string formattedDate = endDate.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            var count = await _unitOfWork.UserConnectionRepository.GetCount(u => u.DateLogin >= endDate && u.DateCreated < startDate);
            var filterState = FilterExtension.CreateSingleFilter("DateLogin", GridFilterOperator.gt, formattedDate, // filter
                GridSortDirection.desc, "DateLogin", 1, count); // sort
            var mappedFilterState = _mapper.Map<DAL.Helpers.FilterState>(filterState);
            var result = await _unitOfWork.UserConnectionRepository.GetAllByQueryAsync(mappedFilterState);
            return new SessionChartData { Count = count, DateLogins = result.Select(u => u.DateLogin) };
        }
    }
}
