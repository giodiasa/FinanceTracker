using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Errors
{
    public class ErrorResponse
    {
        public string Code { get; set; } = null!;
        public string Message { get; set; } = null!;
        public object? Details { get; set; }
    }
}
