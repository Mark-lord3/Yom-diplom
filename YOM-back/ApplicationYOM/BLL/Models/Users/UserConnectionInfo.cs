using BLL.Models.Enums;

namespace BLL.Models.Users
{
    public class UserConnectionInfo
    {
        public ulong Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateLogin { get; set; }
        public DateTime DateLogout { get; set; }
        public DateTime DateCreated { get; set; }
        public UserDevice UserDevice { get; set; }
    }
}
