using Api.App.Features.Policies;

namespace Api.App.Policies;

public class AuthenticationPolicy : IPolicy
{
    public string Role { get; } = "personal";
    public List<string> Rules { get; set; } =
    [
        "profile.upload.avatar",
        "profile.update.avatar",
        "profile.update.account",
        "account.confirm",
        "personal.session.logged",
        "personal.session.refresh",
        "reset.password",
        "reset.password",
        "account.reset.password",
        "account.email.code"
    ];
}