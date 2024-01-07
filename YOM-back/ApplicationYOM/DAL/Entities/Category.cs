namespace DAL.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public ulong Clicks { get; set; }
        public string PhotoPath { get; set; }
    }
}
