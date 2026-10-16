using Torneo.Domain.Standings;
using Torneo.Domain.Teams;

namespace Torneo.Domain.Tests.Standings;

public class GoalDifferenceTests
{
    [Fact]
    public void Diferencia_de_gol_es_goles_a_favor_menos_goles_en_contra()
    {
        var home = Team.Create("Local").Value;
        var away = Team.Create("Visita").Value;
        var match = FinishedMatchFactory.Create(home.Id, away.Id, 2, 0);

        var table = StandingsCalculator.Calculate([home, away], [match]);

        var homeRow = table.Single(row => row.TeamId == home.Id);
        var awayRow = table.Single(row => row.TeamId == away.Id);

        Assert.Equal(2, homeRow.GoalsFor);
        Assert.Equal(0, homeRow.GoalsAgainst);
        Assert.Equal(2, homeRow.GoalDifference);
        Assert.Equal(0, awayRow.GoalsFor);
        Assert.Equal(2, awayRow.GoalsAgainst);
        Assert.Equal(-2, awayRow.GoalDifference);
    }
}
