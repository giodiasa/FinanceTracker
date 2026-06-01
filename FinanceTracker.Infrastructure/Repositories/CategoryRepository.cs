using FinanceTracker.Application.Interfaces.Repositories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public CategoryRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }
        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<List<Category>> GetAllCategoriesByUserId(int userId)
        {
            return await _context.Categories.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int Id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<bool> HasTransactionsAsync(int categoryId)
        {
            return await _context.Transactions.AnyAsync(t => t.CategoryId == categoryId);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
