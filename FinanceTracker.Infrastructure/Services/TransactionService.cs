using AutoMapper;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.DTOs.Category;
using FinanceTracker.Application.DTOs.Transaction;
using FinanceTracker.Application.Exceptions;
using FinanceTracker.Application.Interfaces.Repositories;
using FinanceTracker.Application.Interfaces.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }
        public async Task AddTransactionAsync(int userId, CreateTransactionRequestDto transaction)
        {
            if (transaction.Amount <= 0)
            {
                throw new AppException("AMOUNT_IS_NEGATIVE", "ოდენობა უნდა იყოს 0-ზე მეტი", 400);
            }

            if (transaction.TransactionDate.Date > DateTime.UtcNow.Date)
            {
                throw new AppException("DATE_IS_IN_FUTURE", "თარიღი არ უნდა იყოს მომავალში", 400);
            }
            if (transaction.IsRecurring && transaction.RecurrencePeriod == null)
            {
                throw new AppException("RECURRENCE_PERIOD_NOT_SPECIFIED", "განმეორებად ტრანზაქციებზე უნდა იყოს მითითებული გამეორების წესი", 400);
            }
            var transactionEntity = _mapper.Map<Transaction>(transaction);
            if (transaction.IsRecurring)
            {
                transactionEntity.NextOccurrence =
                    CalculateNextOccurrence(
                        transactionEntity.TransactionDate,
                        transaction.RecurrencePeriod!.Value);
            }
            transactionEntity.UserId = userId;
            await _transactionRepository.AddTransactionAsync(transactionEntity);
        }

        public async Task DeleteTransactionAsync(int id, int userId)
        {
            Transaction? transactionToDelete = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transactionToDelete == null || transactionToDelete.UserId != userId)
            {
                throw new AppException("TRANSACTION_NOT_FOUND", "ტრანზაქცია არ მოიძებნა", 404);
            }
            await _transactionRepository.DeleteTransactionAsync(transactionToDelete);
        }

        public async Task GenerateNextAsync(int id, int userId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null || transaction.UserId != userId)
            {
                throw new AppException("TRANSACTION_NOT_FOUND", "ტრანზაქცია არ მოიძებნა", 404);
            }
            if (!transaction.IsRecurring)
            {
                throw new AppException("NOT_RECURRING_TRANSACTION", "ტრანზაქცია არ არის განმეორებადი", 400);
            }
            var nextUpdate = transaction.NextOccurrence;
            var duplicateExists =
                await _transactionRepository.ExistsByCategoryAndDateAsync(
                    userId,
                    transaction.CategoryId,
                    nextUpdate!.Value);
            if (duplicateExists)
            {
                throw new AppException(
                    "DUPLICATE_TRANSACTION",
                    "ამ თარიღით ტრანზაქცია უკვე არსებობს",
                    409);
            }
            var newTransaction = new Transaction
            {
                UserId = transaction.UserId,
                CategoryId = transaction.CategoryId,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                TransactionDate = nextUpdate.Value,
                Description = transaction.Description,
                IsRecurring = false
            };
            transaction.NextOccurrence = CalculateNextOccurrence(nextUpdate.Value, transaction.RecurrencePeriod!.Value);
            await _transactionRepository.UpdateTransactionAsync(transaction);
            await _transactionRepository.AddTransactionAsync(newTransaction);
        }

        public async Task<List<TransactionResponseDto>> GetRecurringTransactionsAsync(int userId)
        {
            var transactions = await _transactionRepository.GetRecurringTransactionsAsync(userId);
            return transactions.Select(t => _mapper.Map<TransactionResponseDto>(t)).ToList();
        }

        public async Task<PagedResult<TransactionResponseDto>> GetTransactionsAsync(int userId, int? categoryId, CategoryType? type, Currency? currency, DateTime? from, DateTime? to, int page, int pageSize)
        {
            var transactions = await _transactionRepository.GetTransactionsAsync(userId, categoryId, type, currency, from, to, page, pageSize);
            var totalCount = await _transactionRepository.GetTransactionsCountAsync(userId, categoryId, type, currency, from, to);
            var result = new PagedResult<TransactionResponseDto>
            {
                Items = _mapper.Map<List<TransactionResponseDto>>(transactions),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
            return result;
        }

        public async Task UpdateTransactionAsync(int id, int userId, UpdateTransactionRequestDto transaction)
        {
            Transaction? transactionToUpdate = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transactionToUpdate == null || transactionToUpdate.UserId != userId)
            {
                throw new AppException("TRANSACTION_NOT_FOUND", "ტრანზაქცია არ მოიძებნა", 404);
            }
            if (transaction.Amount <= 0)
            {
                throw new AppException("AMOUNT_IS_NEGATIVE", "ოდენობა უნდა იყოს 0-ზე მეტი", 400);
            }
            if (transaction.TransactionDate.Date > DateTime.UtcNow.Date)
            {
                throw new AppException("DATE_IS_IN_FUTURE", "თარიღი არ უნდა იყოს მომავალში", 400);
            }
            if (transaction.IsRecurring && transaction.RecurrencePeriod == null)
            {
                throw new AppException("RECURRENCE_PERIOD_NOT_SPECIFIED", "განმეორებად ტრანზაქციებზე უნდა იყოს მითითებული გამეორების წესი", 400);
            }
            var category = await _categoryRepository.GetCategoryByIdAsync(transaction.CategoryId);

            if (category == null || category.UserId != userId)
            {
                throw new AppException("CATEGORY_NOT_FOUND", "კატეგორია ვერ მოიძებნა", 404);
            }
            _mapper.Map(transaction, transactionToUpdate);
            if (transactionToUpdate.IsRecurring)
            {
                transactionToUpdate.NextOccurrence =
                    CalculateNextOccurrence(
                        transactionToUpdate.TransactionDate,
                        transactionToUpdate.RecurrencePeriod!.Value);
            }
            else
            {
                transactionToUpdate.NextOccurrence = null;
                transactionToUpdate.RecurrencePeriod = null;
            }
            await _transactionRepository.UpdateTransactionAsync(transactionToUpdate);
        }
        private DateTime CalculateNextOccurrence(DateTime transactionDate,RecurrencePeriod recurrencePeriod)
        {
            return recurrencePeriod switch
            {
                RecurrencePeriod.Daily => transactionDate.AddDays(1),
                RecurrencePeriod.Weekly => transactionDate.AddDays(7),
                RecurrencePeriod.Monthly => transactionDate.AddMonths(1),
                _ => throw new AppException(
                    "INVALID_RECURRENCE_PERIOD",
                    "Invalid recurrence period.",
                    400)
            };
        }
    }
}
