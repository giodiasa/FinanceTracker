using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        public DateTime RegisterDate { get; set; }
        public List<Category> Categories { get; set; } = [];
        public List<Transaction> Transactions { get; set; } = [];
    }
}
