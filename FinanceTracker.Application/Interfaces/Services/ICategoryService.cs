using FinanceTracker.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllCategoriesByUserId(int userId);
        Task AddCategoryAsync(int userId, CreateCategoryRequestDto category);
        Task UpdateCategoryAsync(int id, int userId, UpdateCategoryRequestDto category);
        Task DeleteCategoryAsync(int id, int userId);
    }
}
