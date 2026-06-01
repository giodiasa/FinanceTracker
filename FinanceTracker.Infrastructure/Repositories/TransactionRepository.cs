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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public TransactionRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }
        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public async Task DeleteTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
        }

        public async Task<bool> ExistsByCategoryAndDateAsync(int userId, int categoryId, DateTime date)
        {
            return await _context.Transactions.AnyAsync(t => t.UserId == userId && t.CategoryId == categoryId && t.TransactionDate.Date == date.Date);
        }

        public async Task<List<Transaction>> GetRecurringTransactionsAsync(int userId)
        {
            return await _context.Transactions.Where(c => c.UserId == userId && c.IsRecurring).ToListAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int Id)
        {
            return await _context.Transactions.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<List<Transaction>> GetTransactionsAsync(int userId, int? categoryId, CategoryType? type, Currency? currency, DateTime? from, DateTime? to, int page, int pageSize)
        {
            var query = _context.Transactions
                .Where(t => t.UserId == userId)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            if (type.HasValue)
            {
                query = query.Where(t => t.Category.CategoryType == type.Value);
            }

            if (currency.HasValue)
            {
                query = query.Where(t => t.Currency == currency.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(t => t.TransactionDate <= to.Value);
            }

            return await query
                .Include(t => t.Category)
                .OrderByDescending(t => t.TransactionDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTransactionsCountAsync(int userId, int? categoryId, CategoryType? type, Currency? currency, DateTime? from, DateTime? to)
        {
            var query = _context.Transactions
                .Where(t => t.UserId == userId)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            if (type.HasValue)
            {
                query = query.Where(t => t.Category.CategoryType == type.Value);
            }

            if (currency.HasValue)
            {
                query = query.Where(t => t.Currency == currency.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(t => t.TransactionDate <= to.Value);
            }

            return await query.CountAsync();
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
