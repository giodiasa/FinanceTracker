using FinanceTracker.Application.Interfaces.Services;
using FinanceTracker.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _service;
        public StatsController(IStatsService service) 
        {
            _service = service;
        }
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(string month)
        {
            int userId = GetUserId();
            var splitDate = month.Split('-');
            var result = await _service.GetSummaryAsync(userId, int.Parse(splitDate[1]), int.Parse(splitDate[0]));
            return Ok(result);
        }
        [HttpGet("by-category")]
        public async Task<IActionResult> GetByCategory(string month)
        {
            int userId = GetUserId();
            var splitDate = month.Split('-');
            var result = await _service.GetByCategoryAsync(userId, int.Parse(splitDate[1]), int.Parse(splitDate[0]));
            return Ok(result);
        }
        [HttpGet("monthly-trend")]
        public async Task<IActionResult> GetMonthlyTrend(int year)
        {
            int userId = GetUserId();
            var result = await _service.GetMonthlyTrendAsync(userId, year);
            return Ok(result);
        }
        [HttpGet("budget-status")]
        public async Task<IActionResult> GetBudgetStatus()
        {
            int userId = GetUserId();
            var result = await _service.GetBudgetStatusAsync(userId);
            return Ok(result);
        }
    }
}
