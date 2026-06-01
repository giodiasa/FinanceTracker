using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Repositories
{
    public interface IExchangeRateRepository
    {
        Task<ExchangeRate?> GetLatestRateAsync(Currency baseCurrency,Currency targetCurrency);
        Task AddAsync(ExchangeRate exchangeRate);
        Task SaveChangesAsync();
        Task<List<ExchangeRate>> GetCurrentRatesAsync();
    }
}
