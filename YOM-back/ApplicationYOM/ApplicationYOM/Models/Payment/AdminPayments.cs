namespace PL.Models.Payment
{
    public class AdminPayments
    {
        public IEnumerable<PaymentModel> Payments { get; set; }
        public int TotalPages { get; set; }
    }
}
