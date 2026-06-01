using FinanceTracker.Application.Interfaces.Repositories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public UserRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
