using BLL.Models;
using BLL.Models.AdminPagination;

namespace BLL.Interfaces
{
    public interface IPaymentService
    {
        Task Pay(Payment model);

        Task<AdminPayments> GetAllPayments(int pageNumber, int? pageSize);

        Task<AdminPayments> GetPaymentsByMonth(int pageNumber, int? pageSize);

        Task<IEnumerable<Payment>> GetUserPayment(string userId);

        Task<double> GetTotalMonthPayments();

        Task<double> GetTotalPayments();

        Task<double> GetTotalBannerMonthPayments();

        Task<double> GetTotalPromotionMonthPayments();

        Task<double> GetAllAverage();

        Task<double> GetMonthAverage();
    }
}
