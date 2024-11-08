namespace Api.App.Features.Support.Types.Excpetions;

public class TypeError(string message): System.Exception($"TypeError: {message}") {}