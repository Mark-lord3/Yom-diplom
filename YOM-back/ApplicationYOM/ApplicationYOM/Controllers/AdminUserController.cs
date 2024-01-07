using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using PL.Models.User;
using static IdentityServer4.Models.IdentityResources;

namespace PL.Controllers
{
    [ApiController]
    [Route("api/Admin/User")]
    public class AdminUserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AdminUserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("AllUsers")]
        public async Task<AdminUsers> GetAllUsers(int pageNumber, int? pageSize)
        {
            var result = await _userService.GetAllPagination(pageNumber, pageSize);
            return _mapper.Map<AdminUsers>(result);
        }

        [HttpGet("AllBlockedUsers")]
        public async Task<AdminUsers> GetAllBlockedUsers(int pageNumber, int? pageSize)
        {
            var result = await _userService.GetAllBlockedUsers(pageNumber, pageSize);
            return _mapper.Map<AdminUsers>(result);
        }

        [HttpGet("UserCount")]
        public async Task<int> GetUsersCount()
        {
            return await _userService.GetUsersCountAsync();
        }

        [HttpGet("BlockedUserCount")]
        public async Task<int> GetBlockedUsersCount()
        {
            return await _userService.GetUsersBlockedCountAsync();
        }

        [HttpPost("Block/{UserId}")]
        public async Task<IActionResult> BlockUser(string UserId)
        {
            try
            {
                await _userService.BlockUserAsync(UserId);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UnBlock/{UserId}")]
        public async Task<IActionResult> UnBlockUser(string UserId)
        {
            try
            {
                await _userService.UnBlockUserAsync(UserId);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{UserId}")]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            try
            {
                await _userService.DeleteAsync(UserId);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
