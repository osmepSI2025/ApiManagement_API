using Quartz;

namespace SME_API_Apimanagement.Services
{
    public class MOrganizationUpsertJob : IJob
    {
        private readonly MOrganizationService _service;

        public MOrganizationUpsertJob(MOrganizationService service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _service.UpsertAsync();

            }
            catch (Exception ex) { }

        }
    }
}