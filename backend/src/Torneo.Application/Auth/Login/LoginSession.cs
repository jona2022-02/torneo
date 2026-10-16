namespace Torneo.Application.Auth.Login;

public sealed record LoginSession(string Token, string Username, string DisplayName);
