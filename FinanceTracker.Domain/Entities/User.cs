using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Domain.Entities
{
    public class User : IdentityUser
    {
        public DateTime RegisterDate { get; set; }
        public List<Category> Categories { get; set; } = [];
        public List<Transaction> Transactions { get; set; } = [];
    }
}
