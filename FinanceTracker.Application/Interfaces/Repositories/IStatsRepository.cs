using FinanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Repositories
{
    public interface IStatsRepository
    {
        Task<List<Transaction>> GetMonthlyTransactionsAsync(int userId, int year, int month);

        Task<List<Transaction>> GetTransactionsForMonthlyTrendAsync(int userId, int year);

        Task<List<Category>> GetCategoriesForBudgetStatusAsync(int userId);
    }
}
