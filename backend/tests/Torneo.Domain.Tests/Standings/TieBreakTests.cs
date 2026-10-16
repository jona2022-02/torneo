using Torneo.Domain.Standings;
using Torneo.Domain.Teams;

namespace Torneo.Domain.Tests.Standings;

public class TieBreakTests
{
    [Fact]
    public void A_igual_puntos_gana_quien_tiene_mejor_diferencia()
    {
        var atlas = Team.Create("Atlas").Value;
        var boca = Team.Create("Boca").Value;
        var cuzco = Team.Create("Cuzco").Value;
        var danubio = Team.Create("Danubio").Value;
        var wide = FinishedMatchFactory.Create(atlas.Id, boca.Id, 3, 0);
        var narrow = FinishedMatchFactory.Create(cuzco.Id, danubio.Id, 1, 0);

        var table = StandingsCalculator.Calculate(
            [atlas, boca, cuzco, danubio],
            [wide, narrow]);

        Assert.Equal([atlas.Id, cuzco.Id], table.Take(2).Select(row => row.TeamId));
        Assert.Equal(3, table[0].Points);
        Assert.Equal(3, table[1].Points);
        Assert.True(table[0].GoalDifference > table[1].GoalDifference);
    }

    [Fact]
    public void A_igual_puntos_y_diferencia_gana_quien_tiene_mas_goles_a_favor()
    {
        var atlas = Team.Create("Atlas").Value;
        var boca = Team.Create("Boca").Value;
        var cuzco = Team.Create("Cuzco").Value;
        var danubio = Team.Create("Danubio").Value;
        var highScoring = FinishedMatchFactory.Create(atlas.Id, boca.Id, 2, 2);
        var goalless = FinishedMatchFactory.Create(cuzco.Id, danubio.Id, 0, 0);

        var table = StandingsCalculator.Calculate(
            [cuzco, danubio, atlas, boca],
            [highScoring, goalless]);

        Assert.Equal(1, table[0].Points);
        Assert.Equal(0, table[0].GoalDifference);
        Assert.Equal(2, table[0].GoalsFor);
        Assert.Equal(0, table[2].GoalsFor);
        Assert.Equal([atlas.Id, boca.Id], table.Take(2).Select(row => row.TeamId));
    }

    [Fact]
    public void A_igualdad_total_ordena_por_nombre()
    {
        var zulia = Team.Create("Zulia").Value;
        var atlas = Team.Create("Atlas").Value;
        var draw = FinishedMatchFactory.Create(zulia.Id, atlas.Id, 1, 1);

        var table = StandingsCalculator.Calculate([zulia, atlas], [draw]);

        Assert.Equal(atlas.Id, table[0].TeamId);
        Assert.Equal(zulia.Id, table[1].TeamId);
        Assert.Equal(table[0].Points, table[1].Points);
        Assert.Equal(table[0].GoalDifference, table[1].GoalDifference);
        Assert.Equal(table[0].GoalsFor, table[1].GoalsFor);
    }
}
