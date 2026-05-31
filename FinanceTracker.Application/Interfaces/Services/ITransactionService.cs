using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.DTOs.Category;
using FinanceTracker.Application.DTOs.Transaction;
using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Services
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(int userId, CreateTransactionRequestDto transaction);
        Task DeleteTransactionAsync(int id, int userId);
        Task<List<TransactionResponseDto>> GetRecurringTransactionsAsync(int userId);
        Task<PagedResult<TransactionResponseDto>> GetTransactionsAsync(int userId, int? categoryId, CategoryType? type, Currency? currency, DateTime? from, DateTime? to, int page, int pageSize);
        Task UpdateTransactionAsync(int id, int userId, UpdateTransactionRequestDto transaction);
        Task GenerateNextAsync(int id, int userId);
    }
}
