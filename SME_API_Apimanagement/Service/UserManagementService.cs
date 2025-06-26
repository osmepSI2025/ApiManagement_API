using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Repository;

namespace SME_API_Apimanagement.Service

{
    public class UserManagementService
    {
        private readonly UserManagementRepository _repository;

        public UserManagementService(UserManagementRepository repository)
        {
            _repository = repository;
        }

        public Task<List<TEmployeeRole>> GetAllAsync() => _repository.GetAllAsync();
        public Task<TEmployeeRole?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task AddAsync(TEmployeeRole entity) => _repository.AddAsync(entity);
        public Task UpdateAsync(TEmployeeRole entity) => _repository.UpdateAsync(entity);
        public Task<int> DeleteAsync(int id) => _repository.DeleteAsync(id);

    }
}