using FinanceTracker.Application.DTOs.Category;
using FinanceTracker.Application.DTOs.Transaction;
using FinanceTracker.Application.Interfaces.Services;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers
{
    [Authorize]
    [Route("transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
        [HttpPost]
        public async Task<IActionResult> AddTransaction(CreateTransactionRequestDto model)
        {
            int userId = GetUserId();
            await _transactionService.AddTransactionAsync(userId, model);
            return Created();
        }
        [HttpGet]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] int? categoryId,
            [FromQuery] CategoryType? type,
            [FromQuery] Currency? currency,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int page,
            [FromQuery] int pageSize)
        {
            int userId = GetUserId();
            var transactions = await _transactionService.GetTransactionsAsync(userId, categoryId, type, currency, from, to, page, pageSize);
            return Ok(transactions);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, UpdateTransactionRequestDto model)
        {
            int userId = GetUserId();
            await _transactionService.UpdateTransactionAsync(id, userId, model);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            int userId = GetUserId();
            await _transactionService.DeleteTransactionAsync(id, userId);
            return NoContent();
        }
        [HttpGet("recurring")]
        public async Task<IActionResult> GetRecurringTransactions()
        {
            int userId = GetUserId();
            var transactions = await _transactionService.GetRecurringTransactionsAsync(userId);
            return Ok(transactions);
        }
        [HttpPost("{id}/generate-next")]
        public async Task<IActionResult> GenerateNext(int id)
        {
            int userId = GetUserId();
            await _transactionService.GenerateNextAsync(id, userId);
            return NoContent();
        }
    }
}
