using FinanceTracker.Application.Interfaces.Repositories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class StatsRepository : IStatsRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public StatsRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetCategoriesForBudgetStatusAsync(int userId)
        {
            return await _context.Categories
                .Where(x => x.UserId == userId && x.CategoryType == CategoryType.Expense && x.BudgetLimit.HasValue && x.BudgetLimit > 0)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionsForMonthlyTrendAsync(int userId, int year)
        {
            return await _context.Transactions
                .Where(x => x.TransactionDate.Year == year && x.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetMonthlyTransactionsAsync(int userId, int year, int month)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Where(x => x.TransactionDate.Month == month && x.TransactionDate.Year == year && x.UserId == userId)
                .ToListAsync();
        }
    }
}
