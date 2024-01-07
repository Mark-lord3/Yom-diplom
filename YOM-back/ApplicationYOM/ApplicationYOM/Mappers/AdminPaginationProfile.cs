using AutoMapper;

namespace PL.Mappers
{
    public class AdminPaginationProfile : Profile
    {
        public AdminPaginationProfile()
        {
            CreateMap<BLL.Models.Ads.AdminPagedAd, Models.Ad.AdminPagedAd>();
            CreateMap<BLL.Models.AdminPagination.AdminBanners, Models.Banner.AdminBanners>();
            CreateMap<BLL.Models.AdminPagination.AdminHelpReports, Models.HelpReport.AdminHelpReports>();
            CreateMap<BLL.Models.AdminPagination.AdminCategories, Models.Category.AdminCategories>();
            CreateMap<BLL.Models.AdminPagination.AdminSubCategories, Models.Category.AdminSubCategories>();
            CreateMap<BLL.Models.AdminPagination.AdminPayments, Models.Payment.AdminPayments>();
            CreateMap<BLL.Models.AdminPagination.AdminUsers, Models.User.AdminUsers>();
        }
    }
}
