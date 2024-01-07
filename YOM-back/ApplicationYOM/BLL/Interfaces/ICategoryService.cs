using BLL.Models;
using BLL.Models.AdminPagination;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();

        public Task AddCategoryAsync(Category title, IFormFile photo);

        public Task<Category> GetCategoryAsync(int categoryId);

        public Task UpdateCategoryAsync(Category category, IFormFile? photo);

        public Task DeleteCategoryByIdAsync(ulong categoryId);

        public Task AddClicks(int categoryId);

        public Task<IEnumerable<Category>> GetPopularCategories();

        public Task<AdminCategories> GetAllAdminPagedCategories(int pageNumber, int? pageSize);
    }
}
