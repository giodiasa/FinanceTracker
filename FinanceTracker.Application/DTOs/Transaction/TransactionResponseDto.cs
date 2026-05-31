using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.Transaction
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public RecurrencePeriod? RecurrencePeriod { get; set; }
        public DateTime? NextOccurrence { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
