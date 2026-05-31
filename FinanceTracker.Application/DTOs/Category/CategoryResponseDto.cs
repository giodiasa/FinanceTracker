using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.Category
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public CategoryType CategoryType { get; set; }

        public Currency Currency { get; set; }

        public decimal? BudgetLimit { get; set; }
    }
}
