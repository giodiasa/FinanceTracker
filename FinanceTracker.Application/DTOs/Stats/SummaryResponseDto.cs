using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.Stats
{
    public class SummaryResponseDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
        public int TransactionCount { get; set; }
    }
}
