using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinanceTracker.Application.DTOs.Category
{
    public class UpdateCategoryRequestDto
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public CategoryType CategoryType { get; set; }
        [Required]
        public Currency Currency { get; set; }
        public decimal? BudgetLimit { get; set; }
    }
}
