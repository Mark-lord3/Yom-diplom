using BLL.Models;
using BLL.Models.AdminPagination;
using BLL.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
    public interface IBannerService
    {
        Task<Banner> GetBannerByIdAsync(ulong id);

        Task<AdminBanners> GetAllBanners(int pageNumber, int? pageSize);

        Task<Banner> GetBannerByCompanyAsync(string company);

        Task UpdateBannerAsync(Banner banner);

        Task UpdateBannerWithPhotoAsync(Banner banner, IFormFile photo);

        Task DeleteBannerAsync(ulong id);

        Task CreateBannerAsync(Banner banner, IFormFile photo);

        Task UpdateBannerState(ulong bannerId, BannerState bannerState);

        Task AddClicksToBanner(ulong id);

        Task<IEnumerable<Banner>> GetBannerByPageAndSize(BannerPage bannerPage, BannerSize bannerSize);
    }
}
