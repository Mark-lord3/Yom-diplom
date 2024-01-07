using BLL.Models.Users;

namespace BLL.Interfaces
{
    public interface IUserConnectionService
    {
        public Task Disconnect(string userId);

        public Task UpdateConnection(UserConnectionInfo model);

        public Task<UserConnectionInfo> GetUserConnectionInfo(string userId);

        public Task<IEnumerable<UserConnectionInfo>> GetAllUsersInfo();
    }
}
