using Api.App.Features.Mailer;
using Newtonsoft.Json;

namespace Api.App.Features.Queue.Handlers;

public class JobQueueHandler : IQueueHandler
{
    public async Task Execute(string wrapJson)
    {
        var job = JsonConvert.DeserializeObject<Job>(wrapJson);

        if (job != null)
        {
            await job.Handle();
        }
    }
}