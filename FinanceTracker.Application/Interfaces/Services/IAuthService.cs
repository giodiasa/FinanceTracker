using FinanceTracker.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(UserRegisterDto dto);
        Task<AuthResponseDto> LoginAsync(UserLoginDto dto);
    }
}
