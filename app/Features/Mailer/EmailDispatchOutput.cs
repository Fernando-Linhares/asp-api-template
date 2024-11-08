namespace Api.App.Features.Mailer;

public record EmailDispatchOutput(string To, string Subject, long Time);