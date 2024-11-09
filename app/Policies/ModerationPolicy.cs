using Api.App.Features.Policies;

namespace Api.App.Policies;

public class ModerationPolicy: IPolicy
{
    public List<string> Rules { get; set; } =
    [
        "admin.moderation.users"
    ];
}