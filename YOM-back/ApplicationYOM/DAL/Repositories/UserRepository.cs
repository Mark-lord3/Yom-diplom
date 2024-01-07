using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using System.Linq.Expressions;
using DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly YomContext _context;

        public UserRepository(YomContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetByParamAsync(Expression<Func<ApplicationUser, bool>> filterExpression)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(filterExpression);
        }

        public async Task<int> GetCount(Expression<Func<ApplicationUser?, bool>>? filterExpression)
        {
            IQueryable<ApplicationUser> query = _context.Users;

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            return await query.CountAsync();
        }

        public void Add(ApplicationUser entity)
        {
            try
            {
                _context.Users.Add(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteById(string id)
        {
            var userToDelete = await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
            if (userToDelete is not null)
                _context.Users.Remove(userToDelete);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllByQueryAsync(FilterState query)
        {
            var result = _context.Users.AsQueryable().AsNoTracking();
            var filteredResult = result.OrderByState(query)
                .FilterByState(query).ToPagedListAsync(query);
            return await filteredResult.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllByParamsAsync(Expression<Func<ApplicationUser, bool>> filterExpression)
        {
            var resultByParameter = await _context.Users.AsNoTracking()
                .Where(filterExpression)
                .ToListAsync();
            return resultByParameter;
        }

        public async Task<int> GetCount()
        {
            return await _context.Users.CountAsync();
        }
    }
}