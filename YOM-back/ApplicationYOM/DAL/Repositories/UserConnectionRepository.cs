using System.Linq.Expressions;
using DAL.Data;
using DAL.Entities;
using DAL.Helpers;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserConnectionRepository : IRepository<UserConnectionInfo>
    {
        private readonly YomContext _context;

        public UserConnectionRepository(YomContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserConnectionInfo>> GetAll()
        {
            return await _context.UserConnections.ToListAsync();
        }

        public async Task<UserConnectionInfo?> GetByParamAsync(Expression<Func<UserConnectionInfo?, bool>> filterExpression)
        {
            return await _context.UserConnections.AsNoTracking().FirstOrDefaultAsync(filterExpression) ?? null;
        }

        public void Add(UserConnectionInfo entity)
        {
            try
            {
                _context.UserConnections.Add(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteById(ulong id)
        {
            var adToDelete = await _context.UserConnections.FirstOrDefaultAsync(ad => ad.Id == id);
            if (adToDelete is not null)
                _context.UserConnections.Remove(adToDelete);
        }

        public void Update(UserConnectionInfo entity)
        {
            _context.UserConnections.Update(entity);
        }

        public async Task<IEnumerable<UserConnectionInfo>> GetAllByQueryAsync(FilterState query)
        {
            var result = _context.UserConnections.AsNoTracking().AsQueryable();
            var filteredResult = result.OrderByState(query)
                .FilterByState(query).ToPagedListAsync(query);
            return await filteredResult.ToListAsync();
        }

        public async Task<int> GetCount(Expression<Func<UserConnectionInfo?, bool>>? filterExpression)
        {
            IQueryable<UserConnectionInfo> query = _context.UserConnections;

            if (filterExpression is not null)
            {
                query = query.Where(filterExpression);
            }

            return await query.CountAsync();
        }
    }
}
