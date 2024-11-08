using Api.App.Features.Exception;

namespace Api.App.Exceptions;

public class Unthorized(string message) : Exception(message), IHttpException
{
    public int StatusCode { get; set; } = 401;
    public string TextMessage => Message;
    public override string Message { get; } = message;
    public string Details { get; set; } = "";
}