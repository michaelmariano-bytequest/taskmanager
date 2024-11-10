using System.Net;
using System.Text.Json;

namespace TaskManagerAPI.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Passa para o próximo middleware
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = new
        {
            context.Response.StatusCode,
            Message = "An unexpected error occurred. Please try again later.",
            Detailed = exception.Message //TODO Remova em produção para não expor detalhes
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
}