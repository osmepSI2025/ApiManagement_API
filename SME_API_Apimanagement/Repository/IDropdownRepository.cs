using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public interface IDropdownRepository
    {
        List<DropdownModels> GetDropdownLookUp(string LookupType);
        List<DropdownModels> GetDropdownSystem();
        List<DropdownModels> GetDropdownOrganization();
        List<DropdownModels> GetDropdownOrganizationWithOutData(List<MOrganizationModels> WoModels);
        
    }
}
