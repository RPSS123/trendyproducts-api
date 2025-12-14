using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace TrendyProducts.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next) { _next = next; }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var payload = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(payload);
            }
        }
    }
}