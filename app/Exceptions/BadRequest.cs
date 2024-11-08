using Api.App.Features.Exception;

namespace Api.App.Exceptions;

public class BadRequest(string message) : Exception(message), IHttpException
{
    public int StatusCode { get; set; } = 400;
    public string TextMessage => Message;
    public override string Message { get; } = message;
    public string Details { get; set; } = "";
}