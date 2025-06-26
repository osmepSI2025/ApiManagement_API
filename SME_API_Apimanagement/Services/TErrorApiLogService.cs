using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME_API_Apimanagement.Services
{
    public class TErrorApiLogService : ITErrorApiLogService
    {
        private readonly ITErrorApiLogRepository _repository;

        public TErrorApiLogService(ITErrorApiLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<ViewErroApiModels> GetAllAsync(TErrorApiLogModels model) => await _repository.GetAllAsync(model);

        public async Task<TErrorApiLog?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> AddAsync(TErrorApiLog log) => await _repository.AddAsync(log);

        public async Task<int> UpdateAsync(TErrorApiLog log) => await _repository.UpdateAsync(log);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

    public interface ITErrorApiLogService
    {
        Task<ViewErroApiModels> GetAllAsync(TErrorApiLogModels model);
        Task<TErrorApiLog?> GetByIdAsync(int id);
        Task<int> AddAsync(TErrorApiLog log);
        Task<int> UpdateAsync(TErrorApiLog log);
        Task<int> DeleteAsync(int id);
    }
}