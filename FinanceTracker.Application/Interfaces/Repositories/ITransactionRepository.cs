using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetTransactionByIdAsync(int Id);
        Task<List<Transaction>> GetTransactionsAsync(int userId,int? categoryId,CategoryType? type,Currency? currency,DateTime? from,DateTime? to,int page,int pageSize);
        Task AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetRecurringTransactionsAsync(int userId);
        Task<int> GetTransactionsCountAsync(int userId,int? categoryId,CategoryType? type,Currency? currency,DateTime? from,DateTime? to);
        Task<bool> ExistsByCategoryAndDateAsync(int userId, int categoryId, DateTime date);
    }
}
