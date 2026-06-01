using FinanceTracker.Application.DTOs.Stats;
using FinanceTracker.Application.Interfaces.Repositories;
using FinanceTracker.Application.Interfaces.Services;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Infrastructure.Services
{
    public class StatsService : IStatsService
    {
        private readonly IStatsRepository _statsRepository;
        private readonly IExchangeRateService _exchangeRateService;
        public StatsService(IStatsRepository statsRepository, IExchangeRateService exchangeRateService)
        {
            _statsRepository = statsRepository;
            _exchangeRateService = exchangeRateService;
        }
        public async Task<List<BudgetStatusResponseDto>> GetBudgetStatusAsync(int userId)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            var transactions = await _statsRepository.GetMonthlyTransactionsAsync(userId, year, month);
            var expenseTransactionsWithLimit = transactions.Where(t => t.Category.BudgetLimit.HasValue && t.Category.CategoryType == CategoryType.Expense).ToList();
            var categories = await _statsRepository.GetCategoriesForBudgetStatusAsync(userId);
            var result = new List<BudgetStatusResponseDto>();
            foreach (var category in categories) 
            {
                var budgetLimit = category.BudgetLimit;
                decimal spent = 0;

                foreach (var transaction in expenseTransactionsWithLimit
                    .Where(t => t.CategoryId == category.Id))
                {
                    spent += await _exchangeRateService.ConvertToGelAsync(
                        transaction.Amount,
                        transaction.Currency);
                }
                var remaining = budgetLimit - spent;
                var spentPercent = budgetLimit > 0 ? (spent / budgetLimit) * 100 : 0;
                var status = spentPercent switch
                {
                    >= 100 => "over",
                    >= 80 => "warning",
                    _ => "ok"
                };
                result.Add(new BudgetStatusResponseDto
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    BudgetLimit = (decimal)budgetLimit!,
                    Spent = spent,
                    Remaining = (decimal)remaining!,
                    SpentPercent = (decimal)spentPercent,
                    Status = status
                });
            }
            return result;
        }

        public async Task<List<StatsByCategoryResponseDto>> GetByCategoryAsync(int userId, int month, int year)
        {
            var transactions = await _statsRepository.GetMonthlyTransactionsAsync(userId, year, month);

            var expenseTransactions = transactions
                .Where(t => t.Category.CategoryType == CategoryType.Expense)
                .ToList();

            decimal totalExpenses = 0;

            foreach (var transaction in expenseTransactions)
            {
                totalExpenses += await _exchangeRateService.ConvertToGelAsync(
                    transaction.Amount,
                    transaction.Currency);
            }

            var result = new List<StatsByCategoryResponseDto>();

            foreach (var group in expenseTransactions.GroupBy(t => t.Category))
            {
                decimal categoryTotal = 0;

                foreach (var transaction in group)
                {
                    categoryTotal += await _exchangeRateService.ConvertToGelAsync(
                        transaction.Amount,
                        transaction.Currency);
                }

                decimal percentage = 0;

                if (totalExpenses > 0)
                {
                    percentage = categoryTotal * 100 / totalExpenses;
                }

                result.Add(new StatsByCategoryResponseDto
                {
                    CategoryId = group.Key.Id,
                    CategoryName = group.Key.Name,
                    TotalExpense = categoryTotal,
                    Percentage = percentage,
                    TransactionCount = group.Count()
                });
            }

            return result;
        }

        public async Task<List<MonthlyTrendResponseDto>> GetMonthlyTrendAsync(int userId, int year)
        {
            var transactions = await _statsRepository.GetTransactionsForMonthlyTrendAsync(userId, year);
            var result = new List<MonthlyTrendResponseDto>();
            for (int month = 1; month <= 12; month++)
            {
                var monthlyTransactions = transactions.Where(t => t.TransactionDate.Month == month).ToList();
                decimal totalIncome = 0;
                decimal totalExpense = 0;
                foreach (var transaction in monthlyTransactions)
                {
                    var amountInGel = await _exchangeRateService.ConvertToGelAsync(
                        transaction.Amount,
                        transaction.Currency);
                    if (transaction.Category.CategoryType == CategoryType.Income)
                        totalIncome += amountInGel;
                    else totalExpense += amountInGel;
                }
                result.Add(new MonthlyTrendResponseDto
                {
                    Month = month,
                    Income = totalIncome,
                    Expense = totalExpense,
                    Balance = totalIncome - totalExpense
                });
            }
            return result;
        }

        public async Task<SummaryResponseDto> GetSummaryAsync(int userId, int month, int year)
        {
            var transactions = await _statsRepository.GetMonthlyTransactionsAsync(userId, year, month);

            decimal totalIncome = 0;
            decimal totalExpense = 0;

            foreach (var transaction in transactions)
            {
                var amountInGel = await _exchangeRateService.ConvertToGelAsync(
                    transaction.Amount,
                    transaction.Currency);

                if (transaction.Category.CategoryType == CategoryType.Income)
                    totalIncome += amountInGel;
                else if (transaction.Category.CategoryType == CategoryType.Expense)
                    totalExpense += amountInGel;
            }

            return new SummaryResponseDto
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = totalIncome - totalExpense,
                TransactionCount = transactions.Count
            };
        }
    }
}
