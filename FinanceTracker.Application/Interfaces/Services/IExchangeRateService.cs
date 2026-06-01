using FinanceTracker.Application.DTOs.ExchaneRate;
using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Services
{
    public interface IExchangeRateService
    {
        Task<decimal> GetRateToGelAsync(Currency currency);
        Task<decimal> ConvertToGelAsync(decimal amount, Currency currency);
        Task <List<ExchangeRateResponseDto>>GetCurrentRatesAsync();
    }
}
