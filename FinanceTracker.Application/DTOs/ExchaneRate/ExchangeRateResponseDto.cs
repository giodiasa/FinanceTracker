using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.ExchaneRate
{
    public class ExchangeRateResponseDto
    {
        public Currency BaseCurrency { get; set; }
        public Currency TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
