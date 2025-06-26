using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public class ErrorApiLogRepository : IErrorApiLogRepository
    {
        private readonly ApiMangeDBContext _context;

        public ErrorApiLogRepository(ApiMangeDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TErrorApiLog>> GetAllAsync()
        {
            return await _context.TErrorApiLogs.ToListAsync();
        }

        public async Task<TErrorApiLog?> GetByIdAsync(int id)
        {
            return await _context.TErrorApiLogs.FindAsync(id);
        }

        public async Task AddAsync(TErrorApiLog errorLog)
        {
            await _context.TErrorApiLogs.AddAsync(errorLog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TErrorApiLog errorLog)
        {
            _context.TErrorApiLogs.Update(errorLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var errorLog = await GetByIdAsync(id);
            if (errorLog != null)
            {
                _context.TErrorApiLogs.Remove(errorLog);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<TErrorApiLog>> SearchAsync(TErrorApiLogModels searchModel)
        {
            try
            {
                var query = _context.TErrorApiLogs.AsQueryable();

                if (!string.IsNullOrEmpty(searchModel.SystemCode))
                    query = query.Where(x => x.SystemCode.Contains(searchModel.SystemCode));

                if (!string.IsNullOrEmpty(searchModel.Message))
                    query = query.Where(x => x.Message.Contains(searchModel.Message));

                if (!string.IsNullOrEmpty(searchModel.UserName))
                    query = query.Where(x => x.UserName.Contains(searchModel.UserName));

                if (searchModel.ErrorDate.HasValue)
                    query = query.Where(x => x.ErrorDate == searchModel.ErrorDate);

                if (!string.IsNullOrEmpty(searchModel.HttpMethod))
                    query = query.Where(x => x.HttpMethod.Contains(searchModel.HttpMethod));

                // Add more conditions as needed for other fields in the model

                // Pagination
                if (searchModel.rowOFFSet > 0 && searchModel.rowFetch > 0)
                    query = query.Skip(searchModel.rowOFFSet).Take(searchModel.rowFetch);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                return new List<TErrorApiLog>(); // Return an empty list instead of null

            }
        }
    }
}
