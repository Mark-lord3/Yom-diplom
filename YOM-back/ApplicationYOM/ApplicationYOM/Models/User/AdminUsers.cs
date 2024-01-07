namespace PL.Models.User
{
    public class AdminUsers
    {
        public IEnumerable<User> Users { get; set; }
        public int TotalPages { get; set; }
    }
}
