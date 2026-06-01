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
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public ExchangeRateRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ExchangeRate exchangeRate)
        {
            await _context.ExchangeRates.AddAsync(exchangeRate);
        }

        public async Task<ExchangeRate?> GetLatestRateAsync(Currency baseCurrency, Currency targetCurrency)
        {
            return await _context.ExchangeRates
                .Where(x => x.BaseCurrency == baseCurrency && x.TargetCurrency == targetCurrency)
                .OrderByDescending(x => x.LastUpdated)
                .FirstOrDefaultAsync();
        }
        public async Task<List<ExchangeRate>> GetCurrentRatesAsync()
        {
            return await _context.ExchangeRates
                .Where(x => x.TargetCurrency == Currency.GEL)
                .GroupBy(x => x.BaseCurrency)
                .Select(x => x
                    .OrderByDescending(x => x.LastUpdated)
                    .First())
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
