using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public interface ITSystemAPIRepository
    {
        Task<IEnumerable<TSystemApiMapping>> GetAllAsync();
        Task<TSystemApiMapping> GetByIdAsync(int id);
        Task AddAsync(TSystemApiMapping api);
        Task UpdateAsync(TSystemApiMapping api);
        Task<bool> DeleteAsync(int id);
        Task<int> UpsertSystemApi(UpSertSystemApiModels xModels);
        Task<List<TSystemApiMappingModels>> GetTSystemMappingBySearch(TSystemApiMappingModels xModels);
       
    }
}
