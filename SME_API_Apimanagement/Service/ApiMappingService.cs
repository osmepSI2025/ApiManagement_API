using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using SME_API_Apimanagement.Services;

namespace SME_API_Apimanagement.Service
{

    public class ApiMappingService
    {
        private readonly HttpClient _httpClient;
        private readonly string Api_ErrorLog;
        private readonly string Api_SysCode;
        private readonly ITAPIMappingRepository _tAPIMappingRepository;
        private readonly ITErrorApiLogService _tErrorApiLogService;  

        public ApiMappingService(HttpClient httpClient, IConfiguration configuration, ITAPIMappingRepository tAPIMappingRepository
            , ITErrorApiLogService tErrorApiLogService)
        {
            _httpClient = httpClient;
            Api_ErrorLog = configuration["Information:Api_ErrorLog"] ?? throw new ArgumentNullException("Api_ErrorLog is missing in appsettings.json");
            Api_SysCode = configuration["Information:Api_SysCode"] ?? throw new ArgumentNullException("Api_SysCode is missing in appsettings.json");


            _tAPIMappingRepository = tAPIMappingRepository;
            _tErrorApiLogService = tErrorApiLogService;
        }
        public async Task<ApiPermisionApiRespone> GetAllByBusinessIdAsync(searchApiPermisionRespone models)
        {
            ApiPermisionApiRespone data = new ApiPermisionApiRespone();
            try
            {
                var xresult = _tAPIMappingRepository.GetAllByBusinessIdAsync(models);
                if (xresult.Result == null || xresult.Result.Count == 0)
                {
                  

                    data.responseMsg = "Success";
                    data.responseCode = "200";

                    data.result = new List<TApiPermisionRespone>();
            }
                else
            {
                    var datalist = xresult.Result.Select(x => new TApiPermisionRespone
                    {
                        Owner_System_Code = x.OrganizationCode,
                        Owner_System_Name = x.SystemName,
                        API_Service_Name = x.ServiceName,
                        API_Method = x.ApiMethod,
                        API_URL_UAT_Outbound = x.ApiUrlUatOundbound,// Or another property that matches your business id
                        API_URL_PROD_Outbound = x.ApiUrlProdOundbound,                       
                        FlagActive = x.FlagActive
                    }).ToList();
                    data.responseMsg = "Success";
                data.responseCode = "200";
                data.result = datalist;
            }
            return data;
            }
            catch (Exception ex)
            {
                var errorLog = new TErrorApiLog
                {
                    Message = "Function " + "GetAllByBusinessIdAsync" + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = "", // ดึงจาก context หรือ session
                    Path = "",
                    HttpMethod = "GET",
                    RequestData = "", // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = "SYS-API",
                    CreatedBy = "system"

                };
                   await _tErrorApiLogService.AddAsync(errorLog);
                data.responseMsg = ex.Message;
                data.responseCode = "500";
                data.result = new List<TApiPermisionRespone>();
                // Log the exception or handle it as needed
                return data; // Return an empty list in case of error

            }
        }
    }
}
