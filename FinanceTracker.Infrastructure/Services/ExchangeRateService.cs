using AutoMapper;
using FinanceTracker.Application.DTOs.ExchaneRate;
using FinanceTracker.Application.Exceptions;
using FinanceTracker.Application.Interfaces.Repositories;
using FinanceTracker.Application.Interfaces.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FinanceTracker.Infrastructure.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public ExchangeRateService(IExchangeRateRepository exchangeRateRepository, IConfiguration config, IMapper mapper)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _config = config;
            _mapper = mapper;
        }

        public async Task<decimal> ConvertToGelAsync(decimal amount, Currency currency)
        {
            var rate = await GetRateToGelAsync(currency);

            return amount * rate;
        }

        public async Task<List<ExchangeRateResponseDto>> GetCurrentRatesAsync()
        {
            await GetRateToGelAsync(Currency.USD);
            await GetRateToGelAsync(Currency.EUR);

            var usdRate = await _exchangeRateRepository.GetLatestRateAsync(Currency.USD, Currency.GEL);
            var eurRate = await _exchangeRateRepository.GetLatestRateAsync(Currency.EUR, Currency.GEL);

            var rates = new List<ExchangeRate>();

            if (usdRate != null) rates.Add(usdRate);
            if (eurRate != null) rates.Add(eurRate);

            return _mapper.Map<List<ExchangeRateResponseDto>>(rates);
        }

        public async Task<decimal> GetRateToGelAsync(Currency currency)
        {
            if (currency == Currency.GEL)
            {
                return 1;
            }

            var cachedRate = await _exchangeRateRepository.GetLatestRateAsync(currency, Currency.GEL);

            if (cachedRate != null &&
                cachedRate.LastUpdated >= DateTime.Now.AddHours(-24))
            {
                return cachedRate.Rate;
            }
            var apiUrl = _config["NbgSettings:Url"];
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new AppException(
                    "EXCHANGE_RATE_API_ERROR",
                    "მოხდა შეცდომა გარე სერვისიდან კურსის წამოღებისას",
                    503);
            }
            var json = await response.Content.ReadAsStringAsync();
            var nbgResponse = JsonSerializer.Deserialize<List<NbgExchangeRateResponse>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (nbgResponse == null || nbgResponse.Count == 0)
            {
                throw new AppException(
                    "EXCHANGE_RATE_RESPONSE_EMPTY",
                    "Exchange rate API returned empty response",
                    503);
            }
            var latestRates = nbgResponse.First();

            var currencyRate = latestRates.Currencies
                .FirstOrDefault(c => c.Code == currency.ToString());
            if (currencyRate == null)
            {
                throw new AppException(
                    "EXCHANGE_RATE_NOT_FOUND",
                    $"Exchange rate was not found",
                    404);
            }
            var rateToGel = currencyRate.Rate / currencyRate.Quantity;

            var exchangeRate = new ExchangeRate
            {
                BaseCurrency = currency,
                TargetCurrency = Currency.GEL,
                Rate = rateToGel,
                LastUpdated = DateTime.Now
            };

            await _exchangeRateRepository.AddAsync(exchangeRate);
            await _exchangeRateRepository.SaveChangesAsync();

            return rateToGel;
        }
    }
}
