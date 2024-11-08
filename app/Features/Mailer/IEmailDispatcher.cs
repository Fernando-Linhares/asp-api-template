namespace Api.App.Features.Mailer;

public interface IEmailDispatcher
{
    public Task<EmailDispatchOutput> SendAsync(MailBase mailBase);
}