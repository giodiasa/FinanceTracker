using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.Stats
{
    public class MonthlyTrendResponseDto
    {
        public int Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Balance { get; set; }
    }
}
