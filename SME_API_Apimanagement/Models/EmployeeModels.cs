using System.Text.Json.Serialization;

namespace SME_API_Apimanagement.Models
{
    public class EmployeeRoleModels
    {
        public int Id { get; set; }

        public string? EmployeeId { get; set; }
        public string? NameTh { get; set; }

        public string? FirstNameTh { get; set; }

        public string? LastNameTh { get; set; }

        public string? LastNameEn { get; set; }
        public string? BusinessUnitId { get; set; }
        public string? BusinessUnitName { get; set; }
        public string? PositionId { get; set; }
        public string? PositionName { get; set; }
        public string? EmployeeRole { get; set; }

        
    }
    public class EmployeeResult
    {
        [JsonPropertyName("employeeId")]
        public string? EmployeeId { get; set; }

        [JsonPropertyName("employeeCode")]
        public string? EmployeeCode { get; set; }

        [JsonPropertyName("nameTh")]
        public string? NameTh { get; set; }

        [JsonPropertyName("nameEn")]
        public string? NameEn { get; set; }

        [JsonPropertyName("firstNameTh")]
        public string? FirstNameTh { get; set; }

        [JsonPropertyName("firstNameEn")]
        public string? FirstNameEn { get; set; }

        [JsonPropertyName("lastNameTh")]
        public string? LastNameTh { get; set; }

        [JsonPropertyName("lastNameEn")]
        public string? LastNameEn { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }

        [JsonPropertyName("employmentDate")]
        public DateTime? EmploymentDate { get; set; }

        [JsonPropertyName("terminationDate")]
        public DateTime? TerminationDate { get; set; }

        [JsonPropertyName("employeeType")]
        public string? EmployeeType { get; set; }

        [JsonPropertyName("employeeStatus")]
        public string? EmployeeStatus { get; set; }

        [JsonPropertyName("supervisorId")]
        public string? SupervisorId { get; set; }

        [JsonPropertyName("companyId")]
        public string? CompanyId { get; set; }

        [JsonPropertyName("businessUnitId")]
        public string? BusinessUnitId { get; set; }

        [JsonPropertyName("positionId")]
        public string? PositionId { get; set; }

        public string? RoleCode { get; set; }
    }


    public class ApiEmployeeResponse
    {
        [JsonPropertyName("results")]
        public EmployeeResult Results { get; set; }
    }
    public class ApiListEmployeeRoleResponse
    {
        [JsonPropertyName("results")]
        public List<EmployeeResult>  Results { get; set; }
    }

    public class searchEmployeeRoleModels
    {
        
        public string? EmployeeId { get; set; }
 
        public string? BusinessUnitId { get; set; }
     
        public string? EmployeeRole { get; set; }
        public int rowOFFSet { get; set; }
        public int rowFetch { get; set; }

        public string? EmployeeName { get; set; }
    }

    public class ViewEmployeeRoleModels
    {
        public searchEmployeeRoleModels SearchEmployeeRoleModels { get; set; }
        public EmployeeRoleModels EmployeeRoleModels { get; set; }


        public List<EmployeeRoleModels> listEmployeeRoleModels { get; set; }

        public List<EmployeeRoleModels> AdminUsers { get; set; }
        public List<EmployeeRoleModels> SuperAdminUsers { get; set; }
        public int? TotalRowsList { get; set; }
        public PagingModel PageModel { get; set; }

        public vDropdownDTO DDLStatus { get; set; }


        public vDropdownDTO DDLDepartment { get; set; }



    }
 
}
