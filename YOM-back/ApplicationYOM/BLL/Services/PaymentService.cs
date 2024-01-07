using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Models.AdminPagination;
using BLL.Validation.Exceptions;
using DAL.Interfaces;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Pay(Payment model)
        {
            try
            {
                var payment = _mapper.Map<DAL.Entities.Payment>(model);
                payment.PaymentTime = DateTime.Now;
                payment.PaidUntil = DateTime.Now.AddMonths(1); 
                _unitOfWork.PaymentRepository.Pay(payment);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task<AdminPayments> GetAllPayments(int pageNumber, int? pageSize)
        {
            pageSize ??= 20;
            if (pageNumber <= 0 || pageSize <= 0)
                throw new YOMException("Incorrect input data");

            var totalCount = await _unitOfWork.PaymentRepository.GetCount(null);
            var totalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);


            var payments = await _unitOfWork.PaymentRepository.GetAllByQueryAsync
                (new DAL.Helpers.FilterState { PageNumber = pageNumber, PageSize = pageSize });

            return new AdminPayments { Payments = _mapper.Map<IEnumerable<Payment>>(payments), TotalPages = totalPages };
        }

        public async Task<AdminPayments> GetPaymentsByMonth(int pageNumber, int? pageSize)
        {
            pageSize ??= 20;
            if (pageNumber <= 0 || pageSize <= 0)
                throw new YOMException("Incorrect input data");

            var totalCount = await _unitOfWork.PaymentRepository.GetCount(null);
            var totalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);
            var payments = await _unitOfWork.PaymentRepository.GetPaymentsByMonth(pageNumber, pageSize);

            return new AdminPayments { Payments = _mapper.Map<IEnumerable<Payment>>(payments), TotalPages = totalPages };
        }

        public async Task<IEnumerable<Payment>> GetUserPayment(string userId)
        {
            var result = await _unitOfWork.PaymentRepository.GetUserPayment(userId);
            return _mapper.Map<IEnumerable<Payment>>(result);
        }

        public async Task<double> GetTotalMonthPayments()
        {
            return await _unitOfWork.PaymentRepository.GetTotalMonthPayments();
        }

        public async Task<double> GetTotalPayments()
        {
            return await _unitOfWork.PaymentRepository.GetTotalPayments();
        }

        public async Task<double> GetTotalBannerMonthPayments()
        {
            return await _unitOfWork.PaymentRepository.GetTotalBannerMonthPayments();
        }

        public async Task<double> GetTotalPromotionMonthPayments()
        {
            return await _unitOfWork.PaymentRepository.GetTotalPromotionMonthPayments();
        }

        public async Task<double> GetAllAverage()
        {
            var total = await _unitOfWork.PaymentRepository.GetTotalPayments();
            var count = await _unitOfWork.PaymentRepository.GetCount(null);
            return total / count;
        }

        public async Task<double> GetMonthAverage()
        {
            var total = await _unitOfWork.PaymentRepository.GetTotalMonthPayments();

            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            var endDate = startDate.AddMonths(1);
            var count = await _unitOfWork.PaymentRepository.GetCount(p =>
                    p.PaymentTime >= startDate && p.PaymentTime < endDate);
            return total / count;
        }
    }
}
