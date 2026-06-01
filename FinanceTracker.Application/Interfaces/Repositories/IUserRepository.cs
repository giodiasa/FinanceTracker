using FinanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
