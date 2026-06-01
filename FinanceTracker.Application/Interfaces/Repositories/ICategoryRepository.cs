using FinanceTracker.Application.DTOs.Category;
using FinanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesByUserId(int userId);
        Task<Category?> GetCategoryByIdAsync(int Id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
        Task<bool> HasTransactionsAsync(int categoryId);
        Task SaveChangesAsync();
    }
}
