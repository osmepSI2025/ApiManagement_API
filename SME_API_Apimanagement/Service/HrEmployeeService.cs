using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using SME_API_Apimanagement.Services;
using System.Globalization;
using System.Text.Json;
using System.Linq;
namespace SME_API_Apimanagement.Service
{
    public class HrEmployeeService
    {
        private readonly ICallAPIService _serviceApi;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly UserManagementRepository _repositoryUserManagement;
        public HrEmployeeService(
           IConfiguration configuration, ICallAPIService serviceApi, IApiInformationRepository repositoryApi
            , UserManagementRepository repositoryUserManagement)
        {
            _serviceApi = serviceApi;
            _repositoryApi = repositoryApi;
            _repositoryUserManagement = repositoryUserManagement;
        }

        public async Task<string> GetApiInformation(string servicename)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            try {
                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = servicename });
                var apiParam = LApi.Select(x => new MapiInformationModels
                {
                    ServiceNameCode = x.ServiceNameCode,
                    ApiKey = x.ApiKey,
                    AuthorizationType = x.AuthorizationType,
                    ContentType = x.ContentType,
                    CreateDate = x.CreateDate,
                    Id = x.Id,
                    MethodType = x.MethodType,
                    ServiceNameTh = x.ServiceNameTh,
                    Urldevelopment = x.Urldevelopment,
                    Urlproduction = x.Urlproduction,
                    Username = x.Username,
                    Password = x.Password,
                    UpdateDate = x.UpdateDate,
                    Bearer = x.Bearer,
                    AccessToken = x.AccessToken

                }).FirstOrDefault();

                var apiResponse = await _serviceApi.GetDataApiAsync(apiParam, null);

                return apiResponse;
            }
            catch (Exception ex)
            {
                return null;
            }    
          


         
        }
        public async Task<string> GetApiInformationByParam(string servicename, string id)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = servicename });
            var apiParam = LApi.Select(x => new MapiInformationModels
            {
                ServiceNameCode = x.ServiceNameCode,
                ApiKey = x.ApiKey, 
                AuthorizationType = x.AuthorizationType,
                ContentType = x.ContentType,
                CreateDate = x.CreateDate,
                Id = x.Id,
                MethodType = x.MethodType,
                ServiceNameTh = x.ServiceNameTh,
                Urldevelopment = x.Urldevelopment.Replace("{id}", id),// Use null conditional operator to avoid null reference
                Urlproduction = x.Urlproduction.Replace("{id}", id),// Use null conditional operator to avoid null reference
                Username = x.Username,
                Password = x.Password,
                UpdateDate = x.UpdateDate,
                Bearer = x.Bearer,
                AccessToken = x.AccessToken
            }).FirstOrDefault();



           var apiResponse = await _serviceApi.GetDataApiAsync(apiParam, null);

            return apiResponse;
        }
        public async Task<string> GetApiInformationByParamPost(string servicename, object models)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = servicename });
            var apiParam = LApi.Select(x => new MapiInformationModels
            {
                ServiceNameCode = x.ServiceNameCode,
                ApiKey = x.ApiKey,
                AuthorizationType = x.AuthorizationType,
                ContentType = x.ContentType,
                CreateDate = x.CreateDate,
                Id = x.Id,
                MethodType = x.MethodType,
                ServiceNameTh = x.ServiceNameTh,
                Urldevelopment = x.Urldevelopment,// Use null conditional operator to avoid null reference
                Urlproduction = x.Urlproduction,// Use null conditional operator to avoid null reference
                Username = x.Username,
                Password = x.Password,
                UpdateDate = x.UpdateDate,
                Bearer = x.Bearer,
                AccessToken = x.AccessToken
            }).FirstOrDefault();



            var apiResponse = await _serviceApi.GetDataApiAsync(apiParam, models);

            return apiResponse;
        }
        public async Task<BusinessUnitApiResponse> GetDepartment()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            var strDepartment = await GetApiInformation("business-units");
            if (!string.IsNullOrEmpty(strDepartment))
            {
                var result = JsonSerializer.Deserialize<BusinessUnitApiResponse>(strDepartment, options);
                return result;
            }
            else 
            {
            return null;
            }
        }
        public async Task<ApiListPositionResponse?> GetPosition()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            var strPosition = await GetApiInformation("position-all");
            if (!string.IsNullOrEmpty(strPosition))
            {
                var result = JsonSerializer.Deserialize<ApiListPositionResponse>(strPosition, options);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<ApiListEmployeeRoleResponse?> GetEmpByBu(string buId)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            var strPosition = await GetApiInformationByParam("employee-by-bu", buId);
            if (!string.IsNullOrEmpty(strPosition))
            {
                var result = JsonSerializer.Deserialize<ApiListEmployeeRoleResponse>(strPosition, options);
                return result;
            }
            else
            {
                return null;
            }
        }
        public async Task<ApiEmployeeResponse?> GetEmpDetail(string EmpId)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            var strEmp = await GetApiInformationByParam("employee-person", EmpId);
            if (!string.IsNullOrEmpty(strEmp))
            {
                var result = JsonSerializer.Deserialize<ApiEmployeeResponse>(strEmp, options);
                return result;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<EmployeeRoleModels>?> GetEmployeeList() 
        {
            List<EmployeeRoleModels> result = new List<EmployeeRoleModels>();
            EmployeeRoleModels employeeModels = new EmployeeRoleModels();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            var lDepartment = await GetDepartment();
            var lPosition = await GetPosition();
            var lEmployee = await _repositoryUserManagement.GetAllAsync();
            if (lEmployee !=null)
            {
             
                if (lEmployee != null && lDepartment != null && lPosition != null)
                {
                    foreach (var item in lEmployee)
                    {
                        var EmpDetail = await GetEmpDetail(item.EmployeeCode);
                        employeeModels = new EmployeeRoleModels
                        {
                            Id = item.Id,
                            EmployeeId = item.EmployeeCode,
                            FirstNameTh = EmpDetail?.Results.FirstNameTh ?? "-",
                            LastNameTh = EmpDetail?.Results.LastNameTh ?? "-",
                            BusinessUnitId = item.BusinessUnitId,
                            BusinessUnitName = lDepartment.Results.FirstOrDefault(d => d.BusinessUnitId == item.BusinessUnitId)?.NameTh ?? "-",
                            PositionId = item.PositionId,
                            PositionName = lPosition.Results.FirstOrDefault(p => p.Code == item.PositionId)?.NameTh ?? "-",
                            EmployeeRole = item.RoleCode,
                        };
                        result.Add(employeeModels);
                    }
                }
                return result;
            }
            else
            {
                return null;
            }

          
        }
        public async Task<ViewEmployeeRoleModels> GetAllEmployeeList(searchEmployeeRoleModels models)
        {
            var vm = new ViewEmployeeRoleModels();
            string buId = "";
            if (models.BusinessUnitId == null || models.BusinessUnitId == "")
            {
                buId = "SME001";
            }
            else 
            {
                buId = models.BusinessUnitId;
            }
            List<EmployeeRoleModels> result = new List<EmployeeRoleModels>();
            EmployeeRoleModels employeeModels = new EmployeeRoleModels();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            var lDepartment = await GetDepartment();
            var lPosition = await GetPosition();
            
            var lEmployee = await GetEmpByBu(buId);
            if (!string.IsNullOrEmpty(models.EmployeeName))
            {
                var emolist = lEmployee.Results
                        .Where(e => e.NameTh != null && e.NameTh.Contains(models.EmployeeName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (emolist.Count != 0)
                {
                    lEmployee.Results = emolist; // Update the Results property instead of reassigning the entire object
                }
            }
        
            vm.TotalRowsList = lEmployee.Results.Count();
            if (lEmployee != null)
            {
                // Apply paging
                var pagedResults = lEmployee.Results
                    .Skip(models.rowOFFSet)
                    .Take(models.rowFetch)
                    .ToList();
                if (lEmployee != null && lDepartment != null && lPosition != null)
                {
                    foreach (var item in pagedResults)
                    {
                        try {
                            var EmpDetail = await GetEmpDetail(item.EmployeeId);
                            if (EmpDetail != null)
                            {
                                employeeModels = new EmployeeRoleModels
                                {

                                    EmployeeId = item.EmployeeId,
                                    FirstNameTh = EmpDetail?.Results.FirstNameTh ?? "-",
                                    LastNameTh = EmpDetail?.Results.LastNameTh ?? "-",
                                    BusinessUnitId = item.BusinessUnitId,
                                    BusinessUnitName = lDepartment.Results.FirstOrDefault(d => d.BusinessUnitId == item.BusinessUnitId)?.NameTh ?? "-",
                                    PositionId = item.PositionId,
                                    PositionName = lPosition.Results.FirstOrDefault(p => p.Code == item.PositionId)?.NameTh ?? "-",
                                    EmployeeRole = "-",
                                    //rowFetch = models.rowFetch,
                                    //rowOFFSet = models.rowOFFSet,
                                    //totalPageUser = totalPageUser
                                };
                                result.Add(employeeModels);
                            }
                            else 
                            {
                                employeeModels = new EmployeeRoleModels
                                {

                                    EmployeeId = item.EmployeeId,
                                    FirstNameTh = item.FirstNameTh ?? "-",
                                    LastNameTh = item.LastNameTh ?? "-",
                                    BusinessUnitId = item.BusinessUnitId,
                                    BusinessUnitName =  "-",
                                    PositionId ="-",
                                    PositionName = "-",
                                    EmployeeRole = "-",
                                    //rowFetch = models.rowFetch,
                                    //rowOFFSet = models.rowOFFSet,
                                    //totalPageUser = totalPageUser
                                };
                                result.Add(employeeModels);
                            }
                           
                        } catch (Exception ex) 
                        {
                            employeeModels = new EmployeeRoleModels
                            {

                             
                                EmployeeId = item.EmployeeId,
                                FirstNameTh = item.FirstNameTh ?? "-",
                                LastNameTh = item.LastNameTh ?? "-",
                                BusinessUnitId = "-",
                                PositionId = "-",
                                PositionName = "-",
                                EmployeeRole = "-",
                                //rowFetch = models.rowFetch,
                                //rowOFFSet = models.rowOFFSet,
                                //totalPageUser = totalPageUser
                            };
                            result.Add(employeeModels);
                        }
                       
                   
                    }
                }
                vm.listEmployeeRoleModels = result;
                vm.SearchEmployeeRoleModels = models;
                return vm;
            }
            else
            {
                return null;
            }


        }

        public async Task<EmployeeResult> GetEmployeeDetailBySearch(EmployeeResult models)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var strEmp = await GetApiInformationByParamPost("employee-search", models);
                if (!string.IsNullOrEmpty(strEmp))
                {
                    var result = JsonSerializer.Deserialize<List<EmployeeResult>>(strEmp, options);

                    // Check if result is not null and has elements
                    if (result != null && result.Count > 0)
                    {
                        // emdetail
                        var emdetail = result.FirstOrDefault();
                        //find RoleCode in result.FirstOrDefault().RoleCode;
                        var EmroleCode = await _repositoryUserManagement.GetByEmpRoleAsync(emdetail.EmployeeId);

                        EmployeeResult newData = new EmployeeResult
                        {
                            EmployeeId = result.FirstOrDefault().EmployeeId,
                            FirstNameTh = result.FirstOrDefault().FirstNameTh,
                            LastNameTh = result.FirstOrDefault().LastNameTh,
                            Email = result.FirstOrDefault().Email,
                            Mobile = result.FirstOrDefault().Mobile,
                            EmploymentDate = result.FirstOrDefault().EmploymentDate,
                            TerminationDate = result.FirstOrDefault().TerminationDate,
                            EmployeeType = result.FirstOrDefault().EmployeeType,
                            EmployeeStatus = result.FirstOrDefault().EmployeeStatus,
                            SupervisorId = result.FirstOrDefault().SupervisorId,
                            CompanyId = result.FirstOrDefault().CompanyId,
                            BusinessUnitId = result.FirstOrDefault().BusinessUnitId,
                            PositionId = result.FirstOrDefault().PositionId,
                            NameEn = result.FirstOrDefault().NameEn,
                            NameTh = result.FirstOrDefault().NameTh,
                            RoleCode = EmroleCode?.RoleCode ?? null,
                        };
                        return newData;
                    }
                }
                return new EmployeeResult(); // Ensure a return value in case of empty or null result
            }
            catch (Exception ex)
            {
                return new EmployeeResult(); // Ensure a return value in case of an exception
            }
        }
    }
}
