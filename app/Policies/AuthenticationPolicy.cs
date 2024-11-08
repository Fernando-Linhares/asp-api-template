using Api.App.Features.Policies;

namespace Api.App.Policies;

public class AuthenticationPolicy : IPolicy
{
    public List<string> Rules { get; set; } =
    [
        "personal",
        "reset.password"
    ];
}