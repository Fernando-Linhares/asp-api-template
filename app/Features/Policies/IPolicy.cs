namespace Api.App.Features.Policies;

public interface IPolicy
{
    public List<string> Rules { get; set; }
}