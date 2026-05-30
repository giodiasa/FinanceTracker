using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public required Enums.Type Type { get; set; }
        public required Currency Currency { get; set; }
        public decimal? BudgetLimit { get; set; }
        public List<Transaction> Transactions { get; set; } = [];
    }
}
