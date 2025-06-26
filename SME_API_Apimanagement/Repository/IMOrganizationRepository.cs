using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public interface IMOrganizationRepository
    {
        Task<IEnumerable<MOrganization>> GetAllAsync();
        Task<MOrganization> GetByIdAsync(int id);
        Task AddAsync(MOrganization organization);
        Task UpdateAsync(MOrganization organization);
        Task DeleteAsync(int id);
        Task<List<MOrganizationModels>> GetOrgBySeach(MOrganizationModels xModels);
        Task<int> UpsertOrg(MOrganizationModels xModels);
    }
}
