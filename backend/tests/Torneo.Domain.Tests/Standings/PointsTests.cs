using Torneo.Domain.Matches;
using Torneo.Domain.Standings;
using Torneo.Domain.Teams;

namespace Torneo.Domain.Tests.Standings;

public class PointsTests
{
    [Fact]
    public void Victoria_suma_tres_puntos_y_la_derrota_cero()
    {
        var home = Team.Create("Local").Value;
        var away = Team.Create("Visita").Value;
        var match = FinishedMatchFactory.Create(home.Id, away.Id, 1, 0);

        var table = StandingsCalculator.Calculate([home, away], [match]);

        Assert.Equal(3, Row(table, home.Id).Points);
        Assert.Equal(1, Row(table, home.Id).Won);
        Assert.Equal(0, Row(table, away.Id).Points);
        Assert.Equal(1, Row(table, away.Id).Lost);
    }

    [Fact]
    public void Empate_suma_un_punto_a_cada_equipo()
    {
        var home = Team.Create("Local").Value;
        var away = Team.Create("Visita").Value;
        var match = FinishedMatchFactory.Create(home.Id, away.Id, 0, 0);

        var table = StandingsCalculator.Calculate([home, away], [match]);

        Assert.Equal(1, Row(table, home.Id).Points);
        Assert.Equal(1, Row(table, away.Id).Points);
        Assert.Equal(1, Row(table, home.Id).Drawn);
        Assert.Equal(1, Row(table, away.Id).Drawn);
    }

    [Fact]
    public void Victoria_visitante_suma_tres_puntos_al_visitante()
    {
        var home = Team.Create("Local").Value;
        var away = Team.Create("Visita").Value;
        var match = FinishedMatchFactory.Create(home.Id, away.Id, 0, 2);

        var table = StandingsCalculator.Calculate([home, away], [match]);

        Assert.Equal(0, Row(table, home.Id).Points);
        Assert.Equal(3, Row(table, away.Id).Points);
    }

    [Fact]
    public void Partido_programado_no_entra_en_la_tabla()
    {
        var home = Team.Create("Local").Value;
        var away = Team.Create("Visita").Value;
        var scheduled = Match.Create(home.Id, away.Id, new DateTimeOffset(2026, 10, 20, 15, 0, 0, TimeSpan.Zero)).Value;

        var table = StandingsCalculator.Calculate([home, away], [scheduled]);

        Assert.Equal(0, Row(table, home.Id).Played);
        Assert.Equal(0, Row(table, away.Id).Points);
    }

    private static StandingRow Row(IReadOnlyList<StandingRow> table, Guid teamId) =>
        table.Single(row => row.TeamId == teamId);
}
