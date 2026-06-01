using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.Stats
{
    public class StatsByCategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal TotalExpense { get; set; }
        public decimal Percentage { get; set; }
        public int TransactionCount { get; set; }
    }
}
