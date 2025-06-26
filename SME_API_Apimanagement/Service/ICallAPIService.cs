using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Services
{
    public interface ICallAPIService
    {
        Task<string> GetDataApiAsync(MapiInformationModels apiModels, object xdata);

    }
}
