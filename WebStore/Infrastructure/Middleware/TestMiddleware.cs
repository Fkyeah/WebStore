using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TestMiddleware> _logger;

        public TestMiddleware(RequestDelegate Next, ILogger<TestMiddleware> Logger)
        {
            _next = Next;
            _logger = Logger;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("Test Middleware");
            await _next(context);
        }
    }
}
