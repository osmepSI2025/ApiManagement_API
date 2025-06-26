using SME_API_Apimanagement.Entities;
using System.ComponentModel.DataAnnotations;

namespace SME_API_Apimanagement.Models
{
    public class TApiPermisionMappingModels
    {
        public int Id { get; set; }

        public string? OrganizationCode { get; set; }

        public string? SystemCode { get; set; }

        public string? ApiKey { get; set; }

        public string? ApiSystemCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? FlagActive { get; set; }

        public string? FlagDelete { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string? SystemName { get; set; }

        public int? SystemApiMappingId { get; set; }
        public string? ServiceName { get; set; }
        public bool IsSelected { get; set; }

        public string? OrganizationName { get; set; }
         public string? ApiMethod { get; set; }
        public string? ApiUrlUatOundbound { get; set; }
        public string? ApiUrlProdOundbound { get; set; }
        public string? ApiServiceCode { get; set; }
    }
    public partial class ApiPermisionApiRespone
    {
        public string? responseCode { get; set; }
        public string? responseMsg { get; set; }
        public List<TApiPermisionRespone> result { get; set; } = new List<TApiPermisionRespone>();
    }
    public class TApiPermisionRespone   
    {
     
        public string? Owner_System_Code { get; set; }
        public string? Owner_System_Name { get; set; }
        public string? API_Service_Name { get; set; }
        public string? API_Method { get; set; }
        public string? API_URL_UAT_Outbound { get; set; }
        public string? API_URL_PROD_Outbound { get; set; }
        public bool? FlagActive      { get; set; }
    }

    public class searchApiPermisionRespone
    {

        public string? System_Code  { get; set; }

        public string? System_Name { get; set; }

        public bool? FlagActive { get; set; }

       
    }
}

