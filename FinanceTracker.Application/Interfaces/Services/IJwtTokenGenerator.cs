using FinanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
