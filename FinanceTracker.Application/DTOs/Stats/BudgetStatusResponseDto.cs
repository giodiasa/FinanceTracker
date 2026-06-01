using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.Stats
{
    public class BudgetStatusResponseDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal BudgetLimit { get; set; }
        public decimal Spent { get; set; }
        public decimal Remaining { get; set; }
        public decimal SpentPercent { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
