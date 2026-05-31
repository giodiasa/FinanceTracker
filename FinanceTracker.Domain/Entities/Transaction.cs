using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public IsRecurring? IsRecurring { get; set; }
        public DateTime? NextOccurrence { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public int CategoryId { get; set; }
        public required Category Category { get; set; }
    }
}
