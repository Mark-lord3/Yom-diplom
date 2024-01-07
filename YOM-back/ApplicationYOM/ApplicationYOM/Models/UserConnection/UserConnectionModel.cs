namespace PL.Models.UserConnection
{
    public class UserConnectionModel
    {
        public string UserId { get; set; }
        public DateTime DateLogin { get; set; }
        public DateTime DateLogout { get; set; }
        public DateTime DateCreated { get; set; }

        public UserDevice UserDevice { get; set; }
    }
}
