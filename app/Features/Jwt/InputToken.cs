namespace Api.App.Features.Jwt;

public record InputToken(Guid Id, string Name, string Email, int Timestamp, string[] Rules);