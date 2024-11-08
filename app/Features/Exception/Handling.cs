using System.Buffers;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Serilog;

namespace Api.App.Features.Exception;

public class Handling : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        System.Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode = 500;

        if (exception is IHttpException)
        {
            statusCode = ((IHttpException)exception).StatusCode;
        }

        var timestamp = GetTimestamp();
        var error = exception.Message;
        var details = exception.InnerException?.Message ?? String.Empty;
    
        Log.Error($"[Error|{statusCode}|{timestamp}] .............. text: {error}. inner: {details}");

        var problemDetails = new
        {
            Status = statusCode,
            Error = error,
            Details = details,
            Timestamp = timestamp
        };

        string problemJson = JsonConvert.SerializeObject(problemDetails);
        
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.Headers.Append("Content-Type", "application/json");
        httpContext.Response.BodyWriter.Write(new ReadOnlySpan<byte>(
            Encoding.UTF8.GetBytes(problemJson)
            ));
        return ValueTask.FromResult(true);
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}