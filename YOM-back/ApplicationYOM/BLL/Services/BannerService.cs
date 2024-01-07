using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Models.AdminPagination;
using BLL.Models.Enums;
using BLL.Validation.Exceptions;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using BannerAdvertisementPlan = DAL.Entities.BannerEnums.BannerAdvertisementPlan;
using BannerPage = DAL.Entities.BannerEnums.BannerPage;
using BannerSize = DAL.Entities.BannerEnums.BannerSize;
using BannerState = BLL.Models.Enums.BannerState;

namespace BLL.Services
{
    public class BannerService : IBannerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public BannerService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<Banner> GetBannerByIdAsync(ulong id)
        {
            var banner = await _unitOfWork.BannerRepository.GetByParamAsync(u => u.Id == id) ?? throw new YOMException("Banner with this id not exist");
            if (banner.PhotoPaths is not null)
                banner.PhotoPaths = await _photoService.GetUrlPhotoPath(banner.PhotoPaths);
            return _mapper.Map<Banner>(banner);
        }

        public async Task<AdminBanners> GetAllBanners(int pageNumber, int? pageSize)
        {
            pageSize ??= 20;
            if (pageNumber <= 0 || pageSize <= 0)
                throw new YOMException("Incorrect input data");

            var totalCount = await _unitOfWork.BannerRepository.GetCount(null);
            var totalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);


            var banners = await _unitOfWork.BannerRepository.GetAllByQueryAsync(new DAL.Helpers.FilterState{PageNumber = pageNumber, PageSize = pageSize} );
            foreach (var banner in banners)
            {
                banner.PhotoPaths = await _photoService.GetUrlPhotoPath(banner.PhotoPaths);
            }

            return new AdminBanners { Banners = _mapper.Map<IEnumerable<Banner>>(banners), TotalPages = totalPages };
        }

        public async Task<Banner> GetBannerByCompanyAsync(string company)
        {
            var banner = await _unitOfWork.BannerRepository.GetByParamAsync(u => u.CompanyName == company) ?? throw new YOMException("Banner with this id not exist");
            if (banner.PhotoPaths is not null)
                banner.PhotoPaths = await _photoService.GetUrlPhotoPath(banner.PhotoPaths);
            return _mapper.Map<Banner>(banner);
        }

        public async Task UpdateBannerAsync(Banner model)
        {
            try
            {
                var banner = await _unitOfWork.BannerRepository.GetByParamAsync(u => u.Id == model.Id) ?? throw new YOMException("Banner with this id not exist");
                _unitOfWork.BannerRepository.Update(UpdateBanner(model, banner));
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task UpdateBannerWithPhotoAsync(Banner model, IFormFile photo)
        {
            try
            {
                var banner = await _unitOfWork.BannerRepository.GetByParamAsync(u => u.Id == model.Id) ?? throw new YOMException("Banner with this id not exist");

                await _photoService.DeletePhotoByPathAsync(model.PhotoPaths);
                model.PhotoPaths = await _photoService.UploadPhotoAsync(photo, model);
                
                _unitOfWork.BannerRepository.Update(UpdateBanner(model, banner));
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task DeleteBannerAsync(ulong id)
        {
            try
            {
                var banner = await _unitOfWork.BannerRepository.GetByParamAsync(b => b.Id == id) ??
                             throw new YOMException("Banner with this id not exist");
                if (banner.PhotoPaths is not null)
                    await _photoService.DeletePhotoByPathAsync(banner.PhotoPaths);
                await _unitOfWork.BannerRepository.DeleteById(id);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task CreateBannerAsync(Banner banner, IFormFile photo)
        {
            try
            {
                banner.PhotoPaths = await _photoService.UploadPhotoAsync(photo, banner);
                banner.DateExpired = DateTime.Now.AddMonths(1);
                banner.BannerState = BannerState.OnReview;
                var mappedBanner = _mapper.Map<DAL.Entities.Banner>(banner);
                _unitOfWork.BannerRepository.Add(mappedBanner);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task UpdateBannerState(ulong bannerId, BannerState bannerState)
        {
            try
            {
                var banner = await _unitOfWork.BannerRepository.GetByParamAsync(u => u.Id == bannerId) ?? throw new YOMException("Banner with this id not exist");
                banner.BannerState = _mapper.Map<DAL.Entities.BannerEnums.BannerState>(bannerState);
                _unitOfWork.BannerRepository.Update(banner);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task AddClicksToBanner(ulong id)
        {
            try
            {
                var banner = await _unitOfWork.BannerRepository.GetByParamAsync(b => b.Id == id);
                if (banner is null)
                    throw new YOMException("Banner with this id not exits");

                banner.ClicksCount++;
                _unitOfWork.BannerRepository.Update(banner);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task<IEnumerable<Banner>> GetBannerByPageAndSize(Models.Enums.BannerPage bannerPage, Models.Enums.BannerSize bannerSize)
        {
            var filters = new List<Filter> { new Filter
                {
                    Field = "BannerPage", Operator = GridFilterOperator.eq, Value = ((int)bannerPage).ToString()
                },
                new Filter
                    {
                        Field = "BannerSize", Operator = GridFilterOperator.eq, Value = ((int)bannerSize).ToString()
                    },
                new Filter
                {
                    Field = "BannerState", Operator = GridFilterOperator.eq, Value = ((int)BannerState.Active).ToString()
                }
                };
            var filter = new FilterState { Filter = new GridFilterState { Filters = filters, Logic = GridFilterLogic.and } };
            var banners =
                await _unitOfWork.BannerRepository.GetAllByQueryAsync(_mapper.Map<DAL.Helpers.FilterState>(filter));
            foreach (var banner in banners)
            {
                banner.PhotoPaths = await _photoService.GetUrlPhotoPath(banner.PhotoPaths);
            }
            
            return _mapper.Map<IEnumerable<Banner>>(banners);
        }

        private DAL.Entities.Banner UpdateBanner(Banner model, DAL.Entities.Banner banner)
        {
            banner.CompanyName = model.CompanyName;
            banner.Description = model.Description;
            banner.BannerAdvertisementPlan = _mapper.Map<BannerAdvertisementPlan>(banner.BannerAdvertisementPlan);
            banner.Header = model.Header;
            banner.Email = model.Email;
            banner.PhoneNumber = model.PhoneNumber;
            banner.LinkToCompany = model.LinkToCompany;
            banner.BannerPage = _mapper.Map<BannerPage>(banner.BannerAdvertisementPlan);
            banner.BannerSize = _mapper.Map<BannerSize>(banner.BannerAdvertisementPlan);
            return banner;
        }
    }
}
