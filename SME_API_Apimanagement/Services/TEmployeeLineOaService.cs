using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Repository;

namespace SME_API_Apimanagement.Services
{
    public class TEmployeeLineOaService
    {
        private readonly TEmployeeLineOaRepository _repository;

        public TEmployeeLineOaService(TEmployeeLineOaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TEmployeeLineOa>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch
            {
                return new List<TEmployeeLineOa>();
            }
        }

        public async Task<TEmployeeLineOa?> GetByIdAsync(string id)
        {
            try
            {

                var empData =  await _repository.GetByIdAsync(id);
                if(empData == null)
                {
                    return null;
                }
                return empData;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> AddAsync(TEmployeeLineOa entity)
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

        public async Task<bool> UpdateAsync(TEmployeeLineOa entity)
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

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch
            {
                return false;
            }
        }
    }
}