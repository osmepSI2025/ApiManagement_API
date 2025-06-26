using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public interface ITAPIMappingRepository
    {
        Task<IEnumerable<TApiPermisionMapping>> GetAllAsync();
        Task<TApiPermisionMapping> GetByIdAsync(int id);
        Task AddAsync(TApiPermisionMapping mapping);
        Task UpdateAsync(TApiPermisionMapping mapping);
        Task DeleteAsync(int id);
        Task<int> UpdateOrInsertTApiMapping(UpSertRegisterApiModels xModels,string apiKey);
        Task<List<TApiPermisionMappingModels>> GetAllByBusinessIdAsync(searchApiPermisionRespone models);
        Task<List<TApiPermisionMappingModels>> GetTApiMappingBySearch(TApiPermisionMappingModels xModels);
        Task<List<TApiPermisionMappingModels>> GetCheckTApiMappingBySearch(TApiPermisionMappingModels xModels);
    }
}
