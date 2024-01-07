using AutoMapper;
using BLL.Interfaces;
using BLL.Models.Users;
using BLL.Validation.Exceptions;
using Microsoft.AspNetCore.Mvc;
using PL.Models.UserConnection;

namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserConnectionController : Controller
    {
        private readonly IUserConnectionService _userConnectionService;
        private readonly IMapper _mapper;

        public UserConnectionController(IUserConnectionService userConnectionService, IMapper mapper)
        {
            _userConnectionService = userConnectionService;
            _mapper = mapper;
        }

        [HttpPost("Disconnect")]
        public async Task<IActionResult> DisconnectAsync(string userId)
        {
            try
            {
                await _userConnectionService.Disconnect(userId);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
        }

        [HttpPost("Connect")]
        public async Task<IActionResult> UpdateAsync(ConnectModel model)
        {
            try
            {
                await _userConnectionService.UpdateConnection(_mapper.Map<UserConnectionInfo>(model));
                return Ok("Success");
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
        }
    }
}
