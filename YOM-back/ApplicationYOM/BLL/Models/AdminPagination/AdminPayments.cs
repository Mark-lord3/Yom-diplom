namespace BLL.Models.AdminPagination
{
    public class AdminPayments
    {
        public IEnumerable<Payment> Payments { get; set; }
        public int TotalPages { get; set; }
    }
}
