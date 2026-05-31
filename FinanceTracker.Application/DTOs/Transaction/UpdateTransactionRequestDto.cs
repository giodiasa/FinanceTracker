using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinanceTracker.Application.DTOs.Transaction
{
    public class UpdateTransactionRequestDto
    {
        public decimal Amount { get; set; }
        [Required]
        public Currency Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public RecurrencePeriod? RecurrencePeriod { get; set; }
        public int CategoryId { get; set; }
    }
}
