using Torneo.Domain.Matches;
using Torneo.Domain.Teams;

namespace Torneo.Domain.Tests.Teams;

public class TeamEventTests
{
    [Fact]
    public void Crear_equipo_registra_TeamCreated()
    {
        var team = Team.Create("Atlántida").Value;

        var created = Assert.IsType<TeamCreated>(Assert.Single(team.DomainEvents));
        Assert.Equal(team.Id, created.TeamId);
        Assert.Equal("Atlántida", created.Name);
    }

    [Fact]
    public void Registrar_resultado_registra_MatchResultRegistered()
    {
        var home = Guid.NewGuid();
        var away = Guid.NewGuid();
        var match = Match.Create(home, away, new DateTimeOffset(2026, 10, 19, 15, 0, 0, TimeSpan.Zero)).Value;
        var goals = new[] { new GoalRegistration(Guid.NewGuid(), home, 12) };

        var registered = match.RegisterResult(1, 0, goals);

        Assert.True(registered.IsSuccess);
        var domainEvent = Assert.IsType<MatchResultRegistered>(Assert.Single(match.DomainEvents));
        Assert.Equal(match.Id, domainEvent.MatchId);
        Assert.Equal(1, domainEvent.HomeScore);
        Assert.Equal(0, domainEvent.AwayScore);
    }
}
