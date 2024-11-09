using Newtonsoft.Json;

namespace Api.App.Features.PubSub;

public class Message(string text)
{
    private string Text { get; set; } = text;

    public T Deserialize<T>()
    {
        return JsonConvert.DeserializeObject<T>(Text) ?? throw new InvalidOperationException();
    }
}