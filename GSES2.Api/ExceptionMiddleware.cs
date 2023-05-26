using GSES2.Domain.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace GSES2.Api;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch(DomainException ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)ex.InternalStatusCode!;

            var errorResponse = new ErrorResponse
            {
                Message = ex.Message,
                Code = ex.InternalStatusCode
            };

            var json = JsonConvert.SerializeObject(errorResponse);

            await httpContext.Response.WriteAsync(json);

        }
        catch (Exception ex)
        {

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new ErrorResponse
            {
                Message = "Something went wrong."
            };

            var json = JsonConvert.SerializeObject(errorResponse);

            await httpContext.Response.WriteAsync(json);
        }
    }
}

internal class ErrorResponse
{
    public string Message { get; set; }
    public int? Code { get; set; }
}