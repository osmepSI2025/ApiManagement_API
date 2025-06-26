using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public interface IApiInformationRepository
    {
        Task<IEnumerable<MApiInformation>> GetAllAsync(MapiInformationModels param);
        Task<MApiInformation> GetByIdAsync(string servicecode);
        Task AddAsync(MApiInformation service);
        Task UpdateAsync(MApiInformation service);
        Task DeleteAsync(int id);
        Task UpdateAllBearerTokensAsync(string newBearerToken);
    }
}
