using DAL.Entities;
using DAL.Helpers;
using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface IPaymentRepository
    {
        void Pay(Payment model);
        Task<IEnumerable<Payment>> GetAllPayments();

        Task<IEnumerable<Payment>> GetPaymentsByMonth(int pageNumber, int? pageSize);

        Task<IEnumerable<Payment>> GetUserPayment(string userId);

        Task<double> GetTotalMonthPayments();

        Task<double> GetTotalPayments();

        Task<double> GetTotalBannerMonthPayments();

        Task<double> GetTotalPromotionMonthPayments();
        Task<int> GetCount(Expression<Func<Payment?, bool>>? filterExpression);
        Task<IEnumerable<Payment>> GetAllByQueryAsync(FilterState query);
    }
}
