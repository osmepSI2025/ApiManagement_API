using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Repository;

namespace SME_API_Apimanagement.Services
{
    public class TEmployeeMapSystemService
    {
        private readonly TEmployeeMapSystemRepository _repository;

        public TEmployeeMapSystemService(TEmployeeMapSystemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TEmployeeMapSystem>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch
            {
                return new List<TEmployeeMapSystem>();
            }
        }

        public async Task<TEmployeeMapSystem?> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<TEmployeeMapSystem?>> GetByEmpIdAsync(string id)
        {
            try
            {
                return await _repository.GetByEmpIdAsync(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> AddAsync(TEmployeeMapSystem entity)
        {
            try
            {
                return await _repository.AddAsync(entity);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TEmployeeMapSystem entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string Empid)
        {
            try
            {
                return await _repository.DeleteAsync(Empid);
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddRangeAsync(List<TEmployeeMapSystem> entity)
        {
            try
            {
                return await _repository.AddRangeAsync(entity);
            }
            catch
            {
                return false;
            }
        }
    }
}