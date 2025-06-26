namespace SME_API_Apimanagement.Models
{
    public class ViewRegisterApiModels
    {
        public vDropdownDTO vDdlSystem { get; set; }
        public vDropdownDTO vDdlOrg { get; set; }
        public vDropdownDTO vDdlStatus { get; set; }
        public PagingModel PageModel { get; set; }
        public MRegisterModels MRegister { get; set; }
        public List<TApiPermisionMappingModels> LApi { get; set; }
        public List<TApiPermisionMappingModels> LPerMapApiSelect { get; set; }
        public List<MSystemModels> LSystem { get; set; }
        public List<MRegisterModels> LRegis { get; set; }
        public int? TotalRowsList { get; set; }
        public MOrganizationModels MOrg { get; set; }
        public List<MOrganizationModels> LOrg { get; set; }
        public MOrganizationModels InsMOrg { get; set; }

    }
    public class UpSertRegisterApiModels
    {

        public MRegisterModels MRegister { get; set; } = new MRegisterModels();

        public List<MSystemModels> LSystem { get; set; } = new List<MSystemModels>();
        public List<TApiPermisionMappingModels> LPerMapApi { get; set; } = new List<TApiPermisionMappingModels>();
     

    }
    public class ViewSystemApiModels
    {
        public vDropdownDTO vDdlSystem { get; set; }
        public vDropdownDTO vDdlMethodApi { get; set; }
        public vDropdownDTO vDdlStatus { get; set; }
        public PagingModel PageModel { get; set; }
        public TSystemApiMappingModels TSystemAPI { get; set; }
        public List<TSystemApiMappingModels> LSysApi { get; set; }
        public List<MSystemModels> LSystem { get; set; }
    }
    public class UpSertSystemApiModels
    {

        public TSystemApiMappingModels TSystemAPI { get; set; }

        //   public List<MSystemModels> LSystem { get; set; }

    }
}
