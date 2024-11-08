namespace Api.App.Features.Exception;

public interface IHttpException
{
    public int StatusCode { get; set; }
    public string TextMessage => "";
    public string Details { get; set; }
}