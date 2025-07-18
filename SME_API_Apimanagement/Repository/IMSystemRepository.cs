﻿using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public interface IMSystemRepository
    {
        Task<IEnumerable<MSystem>> GetAllAsync();
        Task<MSystem> GetByIdAsync(int id);
        Task AddAsync(MSystem system);
        Task<int> UpdateAsync(MSystem system);
        Task<int> DeleteAsync(int id);
        Task<List<MSystemModels>> GetSystemBySearch(MSystemModels xModels);
        Task<List<MSystemModels>> GetSystemBySearchMaster(MSystemModels xModels);
        
        Task<int> UpsertSystem(MSystemModels xModels);
        Task<bool> UpdateStatus(MSystemModels models);
        Task<bool> UpdateStatusByCode(MSystemModels models);

    }
}
