using Microsoft.AspNetCore.Http;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;  

    public ExceptionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            var errorLog = new TErrorApiLog();
            await _next(httpContext);

            // ... inside your InvokeAsync method
            if (httpContext.Response.StatusCode != StatusCodes.Status200OK)
            {
                var statusCode = httpContext.Response.StatusCode;

                 errorLog = new TErrorApiLog
                {
                    HttpCode = statusCode.ToString(),
                    ErrorDate = DateTime.Now,
                    Path = httpContext.Request.Path,
                    HttpMethod = httpContext.Request.Method,
                    CreatedBy = httpContext.User.Identity?.Name ?? "system",
                    InnerException = null,
                    Source = "ExceptionMiddleware",
                    TargetSite = "InvokeAsync",
                    SystemCode = "SYS-API"
                };

                switch (statusCode)
                {
                    case StatusCodes.Status400BadRequest:
                        errorLog.Message = "Bad request";
                        errorLog.StackTrace = "The request could not be understood or was missing required parameters.";
                        break;
                    case StatusCodes.Status401Unauthorized:
                        errorLog.Message = "Unauthorized access";
                        errorLog.StackTrace = "The request requires user authentication.";
                        break;
                    case StatusCodes.Status403Forbidden:
                        errorLog.Message = "Forbidden";
                        errorLog.StackTrace = "The server understood the request, but refuses to authorize it.";
                        break;
                    case StatusCodes.Status404NotFound:
                        errorLog.Message = "Resource not found";
                        errorLog.StackTrace = "The requested resource could not be found.";
                        break;
                    case StatusCodes.Status405MethodNotAllowed:
                        errorLog.Message = "Method not allowed";
                        errorLog.StackTrace = "The HTTP method is not allowed for the requested resource.";
                        break;
                    case StatusCodes.Status500InternalServerError:
                        errorLog.Message = "Internal server error";
                        errorLog.StackTrace = "An unexpected error occurred on the server.";
                        break;
                    default:
                        return; // ไม่ต้อง log status อื่น
                }

                using var scope = _scopeFactory.CreateScope();
                var _errorApiLogRepository = scope.ServiceProvider.GetRequiredService<ITErrorApiLogRepository>();
                await _errorApiLogRepository.AddAsync(errorLog);

                // ส่ง response JSON กลับ
                var errorResponse = new ErrorResponseModels
                {
                    responseCode = statusCode.ToString(),
                    responseMsg = errorLog.Message
                };

                httpContext.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await httpContext.Response.WriteAsync(json);
            }

        }
        catch (Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsync("{}");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected

        // You can add more specific exception types here if needed
        if (exception is ArgumentException)
            code = HttpStatusCode.BadRequest;

        var result = JsonSerializer.Serialize(new
        {
            error = exception.Message,
            detail = exception.InnerException?.Message
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}