namespace Torneo.Application.Teams.GetTeams;

public sealed record TeamListItem(Guid Id, string Name, DateTimeOffset CreatedAt);
