using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public interface IMRegisterRepository
    {
        Task<IEnumerable<MRegister>> GetRegistersAsync();
        Task<MRegister> GetRegisterByIdAsync(string id);
        // Task AddRegisterAsync(MRegister register);
        Task UpdateRegisterAsync(MRegister register);
        Task DeleteRegisterAsync(int id);
        Task<string> UpdateOrInsertRegister(UpSertRegisterApiModels xModels);
        Task<List<MRegister>> GetRegister(MRegisterModels Orgcode);
        Task<ViewRegisterApiModels> GetRegisterBySearch(MRegisterModels xModels);
    }
}
