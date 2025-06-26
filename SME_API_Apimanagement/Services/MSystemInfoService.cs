using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;

namespace SME_API_Apimanagement.Services
{
    public class MSystemInfoService : IMSystemInfoService
    {
        private readonly IMSystemInfoRepository _repository;

        public MSystemInfoService(IMSystemInfoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MSystemInfo>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<MSystemInfo?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> AddAsync(MSystemInfo entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<int> UpdateAsync(MSystemInfo entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<int> CheckSysitemInfoUpsert(MSystemInfoModels xModels)
        {
            try
            {
                var systemInfo = await GetByIdAsync(xModels.SystemCode);
                if (systemInfo == null)
                {
                    var xRaw = new MSystemInfo
                    {
                        FlagActive = true,
                        FlagDelete = "N",
                        UpdateDate = DateTime.Now,
                        UpdateBy = xModels.CreateBy,
                        SystemCode = xModels.SystemCode,
                        Note = xModels.Note,
                        ApiKey = xModels.ApiKey,
                        ApiUrlProdInbound = xModels.ApiUrlProdInbound,
                        ApiUrlUatInbound = xModels.ApiUrlUatInbound,
                        ApiPassword = xModels.ApiPassword,
                        ApiUser = xModels.ApiUser,
                        CreateBy = xModels.CreateBy,
                        CreateDate = DateTime.Now,
                    };

                    var success = await AddAsync(xRaw); // เรียกใช้ AddAsync เพื่อเพิ่มข้อมูลใหม่

                    success = xRaw.Id; // ดึงค่า Id หลัง Save


                    return success; // คืนค่า Id ของข้อมูลที่เพิ่มใหม่
                }
                else
                {

                    var xRaw = new MSystemInfo
                    {
                        Id = systemInfo.Id, // ใช้ Id ที่มีอยู่แล้ว
                        FlagActive = true,
                        FlagDelete = "N",
                        UpdateDate = DateTime.Now,
                        UpdateBy = xModels.CreateBy,
                        SystemCode = xModels.SystemCode,
                        Note = xModels.Note,
                        ApiKey = xModels.ApiKey,
                        ApiUrlProdInbound = xModels.ApiUrlProdInbound,
                        ApiUrlUatInbound = xModels.ApiUrlUatInbound,
                        ApiPassword = xModels.ApiPassword,
                        ApiUser = xModels.ApiUser,

                    };
                    var updatex = await UpdateAsync(xRaw);


                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            // return await _repository.DeleteAsync(id);

            return 1; // Placeholder for actual implementation
        }
    }

    public interface IMSystemInfoService
    {
        Task<IEnumerable<MSystemInfo>> GetAllAsync();
        Task<MSystemInfo?> GetByIdAsync(string id);
        Task<int> AddAsync(MSystemInfo entity);
        Task<int> UpdateAsync(MSystemInfo entity);
        Task<int> DeleteAsync(int id);

        Task<int> CheckSysitemInfoUpsert(MSystemInfoModels xModels);
    }
}