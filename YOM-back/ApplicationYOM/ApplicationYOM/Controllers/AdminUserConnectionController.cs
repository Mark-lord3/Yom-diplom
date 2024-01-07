using AutoMapper;
using BLL.Interfaces;
using BLL.Models.SessionDetail;
using BLL.Models.Users;
using BLL.Validation.Exceptions;
using Microsoft.AspNetCore.Mvc;
using PL.Models.UserConnection;

namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminUserConnectionController : Controller
    {
        private readonly IUserConnectionService _userConnectionService;
        private readonly IDetailConnection _detailConnection;
        private readonly IMapper _mapper;

        public AdminUserConnectionController(IUserConnectionService userConnectionService, IMapper mapper, IDetailConnection detailConnection)
        {
            _userConnectionService = userConnectionService;
            _mapper = mapper;
            _detailConnection = detailConnection;
        }

        [HttpGet]
        public async Task<UserConnectionModel> GetUserConnectionInfoAsync(string userId)
        {
            var userConnection = await _userConnectionService.GetUserConnectionInfo(userId);
            return _mapper.Map<UserConnectionModel>(userConnection);
        }

        [HttpGet("All")]
        public async Task<IEnumerable<UserConnectionModel>> GetUsersConnectionInfoAsync()
        {
            var userConnection = await _userConnectionService.GetAllUsersInfo();
            return _mapper.Map<IEnumerable<UserConnectionModel>>(userConnection);
        }

        [HttpGet("SessionDetail")]
        public async Task<SessionDetailModel> GetSessionDetail()
        {
            var sessionDetail = await _detailConnection.GetDetailOfSession();
            return sessionDetail;
        }

        [HttpGet("AverageSessionTime")]
        public async Task<TimeSpan> GetAverageSessionTime()
        {
            return await _detailConnection.GetAverageSessionTime();
        }

        [HttpGet("MonthRegistration")]
        public async Task<int> GetRegistrationPerMonth()
        {
            return await _detailConnection.GetMonthRegistration();
        }

        [HttpGet("SessionChart")]
        public async Task<SessionChartData> GetSessionChart()
        {
            return await _detailConnection.GetSessionChart();
        }
    }
}
