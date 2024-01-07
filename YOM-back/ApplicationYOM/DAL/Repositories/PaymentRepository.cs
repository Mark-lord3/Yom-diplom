using System.Linq.Expressions;
using DAL.Data;
using DAL.Entities;
using DAL.Helpers;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly YomContext _context;

        public PaymentRepository(YomContext context)
        {
            _context = context;
        }

        public void Pay(Payment model)
        {
            try
            {
                _context.Payments.Add(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByMonth(int pageNumber, int? pageSize)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            var endDate = startDate.AddMonths(1).AddSeconds(-1);



            var result = _context.Payments.AsNoTracking()
                .AsQueryable()
                .Where(x => x.PaymentTime >= startDate && x.PaymentTime <= endDate).ToPagedListAsync(new FilterState{PageNumber = pageNumber, PageSize = pageSize});

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetUserPayment(string userId)
        {
            var result = _context.Payments.AsNoTracking().AsQueryable().Where(u => u.UserId == userId);
            return await result.ToListAsync();
        }

        public async Task<double> GetTotalMonthPayments()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            var endDate = startDate.AddMonths(1);
            var result = _context.Payments.AsNoTracking().AsQueryable()
                .Where(p => p.PaymentTime >= startDate && p.PaymentTime < endDate)
                .SumAsync(p => p.PaymentAmount);
            return await result;
        }

        public async Task<double> GetTotalPayments()
        {
            return await _context.Payments
                .SumAsync(p => p.PaymentAmount);
        }

        public async Task<double> GetTotalBannerMonthPayments()
        {
            return await _context.Payments
                .Where(p=> p.PaymentType == PaymentType.Banner)
                .SumAsync(p => p.PaymentAmount);
        }

        public async Task<double> GetTotalPromotionMonthPayments()
        {
            return await _context.Payments
                .Where(p => p.PaymentType == PaymentType.AdPromotion)
                .SumAsync(p => p.PaymentAmount);
        }

        public async Task<int> GetCount(Expression<Func<Payment?, bool>>? filterExpression)
        {
            IQueryable<Payment> query = _context.Payments;

            if (filterExpression is not null)
            {
                query = query.Where(filterExpression);
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<Payment>> GetAllByQueryAsync(FilterState query)
        {
            var result = _context.Payments.AsQueryable();
            var filteredResult = result.OrderByState(query)
                .FilterByState(query).ToPagedListAsync(query);
            return await filteredResult.ToListAsync();
        }
    }
}
