using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.Exceptions
{
    public class AppException : Exception
    {
        public string Code { get; }
        public int StatusCode { get; }
        public object? Details { get; }

        public AppException(string code, string message, int statusCode = 400, object? details = null)
            : base(message)
        {
            Code = code;
            StatusCode = statusCode;
            Details = details;
        }
    }
}
