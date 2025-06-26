using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public interface IErrorApiLogRepository
    {
        Task<IEnumerable<TErrorApiLog>> GetAllAsync();
        Task<TErrorApiLog?> GetByIdAsync(int id);
        Task AddAsync(TErrorApiLog errorLog);
        Task UpdateAsync(TErrorApiLog errorLog);
        Task DeleteAsync(int id);
        Task<IEnumerable<TErrorApiLog>> SearchAsync(TErrorApiLogModels searchModel);
    }
}
