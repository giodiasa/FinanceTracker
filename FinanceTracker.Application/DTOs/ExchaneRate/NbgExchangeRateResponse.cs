using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.ExchaneRate
{
    public class NbgExchangeRateResponse
    {
        public DateTime Date { get; set; }

        public List<NbgCurrencyResponse> Currencies { get; set; } = new();
    }
}
