using FinanceTracker.Application.DTOs.Stats;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Services
{
    public interface IStatsService
    {
        Task<SummaryResponseDto> GetSummaryAsync(int userId, int month, int year);
        Task<List<StatsByCategoryResponseDto>> GetByCategoryAsync(int userId, int month, int year);
        Task<List<MonthlyTrendResponseDto>> GetMonthlyTrendAsync(int userId, int year);
        Task<List<BudgetStatusResponseDto>> GetBudgetStatusAsync(int userId);
    }
}
