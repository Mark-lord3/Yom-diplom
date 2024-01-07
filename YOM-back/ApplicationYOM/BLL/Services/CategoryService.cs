using AutoMapper;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using BLL.Models.AdminPagination;
using BLL.Models.Enums;
using BLL.Validation;
using BLL.Validation.Exceptions;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task AddCategoryAsync(Category category, IFormFile photo)
        {
            try
            {
                await CheckCategoryTitle(category.Title, null);

                var mappedCategory = _mapper.Map<DAL.Entities.Category>(category);
                var path = await _photoService.UploadPhotoAsync(new List<IFormFile> { photo }, mappedCategory);
                mappedCategory.PhotoPath = path;
                _unitOfWork.CategoryRepository.Add(mappedCategory);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task DeleteCategoryByIdAsync(ulong categoryId)
        {
            try
            {
                var category =
                    await _unitOfWork.CategoryRepository.GetByParamAsync(u => u.Id == (int)categoryId) ?? throw new YOMException($"Category with this id {categoryId}"); ;
                await _unitOfWork.CategoryRepository.DeleteById(categoryId)!;
                await _photoService.DeletePhotoByPathAsync(category.PhotoPath);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task AddClicks(int categoryId)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByParamAsync(u => u.Id == categoryId) ?? throw new YOMException("Category with this id not exist");
                category.Clicks++;
                _unitOfWork.CategoryRepository.Update(category);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task<IEnumerable<Category>> GetPopularCategories()
        {
            var filterState = FilterExtension.CreateSingleFilter("Id", GridFilterOperator.isnotnull, null, // filter
                GridSortDirection.desc, "Clicks", 1, 3); // sort
            var mappedFilterState = _mapper.Map<DAL.Helpers.FilterState>(filterState);
            var categories = await _unitOfWork.CategoryRepository.GetAllByQueryAsync(mappedFilterState);
            var mappedResult = _mapper.Map<IEnumerable<Category>>(categories);

            foreach (var cat in mappedResult)
            {
                if (cat.PhotoPath is not null)
                    cat.PhotoPath = await _photoService.GetUrlPhotoPath(cat.PhotoPath);
            }
            return mappedResult;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var unmappedResult = await _unitOfWork.CategoryRepository.GetAll();
            var mappedResult = _mapper.Map<IEnumerable<Category>>(unmappedResult);

            foreach (var cat in mappedResult)
            {
                if (cat.PhotoPath is not null)
                    cat.PhotoPath = await _photoService.GetUrlPhotoPath(cat.PhotoPath);
            }
            return mappedResult;
        }

        public async Task<AdminCategories> GetAllAdminPagedCategories(int pageNumber, int? pageSize)
        {
            pageSize ??= 20;
            if (pageNumber <= 0 || pageSize <= 0)
                throw new YOMException("Incorrect input data");

            var totalCount = await _unitOfWork.CategoryRepository.GetCount(null);
            var totalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);

            var categories = await _unitOfWork.CategoryRepository.GetAllByQueryAsync(new DAL.Helpers.FilterState { PageNumber = pageNumber, PageSize = pageSize });
            foreach (var category in categories)
            {
                category.PhotoPath = await _photoService.GetUrlPhotoPath(category.PhotoPath);
            }

            return new AdminCategories { Categories = _mapper.Map<IEnumerable<Category>>(categories), TotalPages = totalPages };
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            var unmappedResult = await _unitOfWork.CategoryRepository.GetByParamAsync(u => u.Id == categoryId);
            var category = _mapper.Map<Category>(unmappedResult);
            if (category.PhotoPath is not null)
                category.PhotoPath = await _photoService.GetUrlPhotoPath(category.PhotoPath);
            return category;
        }

        public async Task UpdateCategoryAsync(Category model, IFormFile? photo)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByParamAsync(u => u.Id == (int)model.Id) ?? throw new YOMException("SubCategory with this id not exist");
                await CheckCategoryTitle(model.Title, model.Id);
                category.Title = model.Title;
                if (photo is not null && ImageValidator.IsImage(photo))
                {
                    await _photoService.DeletePhotoByPathAsync(category.PhotoPath);
                    var path = await _photoService.UploadPhotoAsync(new List<IFormFile> { photo }, category);
                    if (path is not null)
                        category.PhotoPath = path;
                }
                
                _unitOfWork.CategoryRepository.Update(category);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        private async Task CheckCategoryTitle(string title, ulong? categoryId)
        {
            var categoryTitleExist =
                await _unitOfWork.CategoryRepository.GetByParamAsync(u => u.Title == title);
            if (categoryId is not null)
                if (categoryTitleExist is not null && (int)categoryId != categoryTitleExist.Id)
                    throw new YOMException($"Category with this title {title}");
            if (categoryTitleExist is not null)
                throw new YOMException($"Category with this title {title}");
        }
    }
}
