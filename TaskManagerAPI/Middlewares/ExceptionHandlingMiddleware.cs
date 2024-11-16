using System.Net;
using System.Text.Json;

namespace TaskManagerAPI.Middlewares;

/// <summary>
/// Middleware for handling exceptions in the ASP.NET Core pipeline.
/// </summary>
public class ExceptionHandlingMiddleware
{
    /// <summary>
    /// Delegate to control the flow between middlewares in the HTTP request pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Middleware for handling exceptions and providing a consistent error response.
    /// </summary>
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Handles exceptions that occur during the request pipeline execution and ensures standard error response format.
    /// </summary>
    /// <param name="context">HttpContext object that holds the HTTP request and response information.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Handles exceptions that occur during the execution of the request pipeline and formats
    /// the response to return standardized error details in JSON format.
    /// </summary>
    /// <param name="context">The HttpContext which provides access to HTTP-specific information about the individual HTTP request.</param>
    /// <param name="exception">The exception that was thrown during request processing.</param>
    /// <return>
    /// A task representing the asynchronous operation that completes when the response
    /// has been written and sent to the client.
    /// </return>
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