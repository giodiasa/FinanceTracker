using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.ExchaneRate
{
    public class NbgCurrencyResponse
    {
        public string Code { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public decimal Rate { get; set; }
    }
}
