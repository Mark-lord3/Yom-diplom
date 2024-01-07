using AutoMapper;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PL.Models.Payment;

namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminPaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        public AdminPaymentController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [HttpGet("AllPayments")]
        public async Task<AdminPayments> GetAllPaymentsAsync(int pageNumber, int? pageSize)
        {
            var result = await _paymentService.GetAllPayments(pageNumber, pageSize);
            return _mapper.Map<AdminPayments>(result);
        }

        [HttpGet("AllPayments/ByMonth")]
        public async Task<AdminPayments> GetAllPaymentsByMonthAsync(int pageNumber, int? pageSize)
        {
            var result = await _paymentService.GetPaymentsByMonth(pageNumber, pageSize);
            return _mapper.Map<AdminPayments>(result);
        }

        [HttpGet("AllPayments/ByUserId/{UserId}")]
        public async Task<IEnumerable<PaymentModel>> GetAllUserPaymentsAsync(string UserId)
        {
            var result = await _paymentService.GetUserPayment(UserId);
            return _mapper.Map<IEnumerable<PaymentModel>>(result);
        }

        [HttpGet("Profit/Month")]
        public async Task<ProfitModel> GetMonthProfit()
        {
            return new ProfitModel { Profit = await _paymentService.GetTotalMonthPayments() };
        }

        [HttpGet("Profit/AllTime")]
        public async Task<ProfitModel> GetTotalProfit()
        {
            return new ProfitModel { Profit = await _paymentService.GetTotalPayments() };
        }

        [HttpGet("Profit/Month/Banner")]
        public async Task<ProfitModel> GetBannerMonthProfit()
        {
            return new ProfitModel { Profit = await _paymentService.GetTotalBannerMonthPayments() };
        }

        [HttpGet("Profit/Month/Promotion")]
        public async Task<ProfitModel> GetTotalPromotionMonthProfit()
        {
            return new ProfitModel { Profit = await _paymentService.GetTotalPromotionMonthPayments() };
        }

        [HttpGet("Profit/Month/Average")]
        public async Task<double> GetAverageMonthProfit()
        {
            return await _paymentService.GetMonthAverage();
        }

        [HttpGet("Profit/AllTime/Average")]
        public async Task<double> GetAverageAllTimeProfit()
        {
            return await _paymentService.GetAllAverage();
        }
    }
}
