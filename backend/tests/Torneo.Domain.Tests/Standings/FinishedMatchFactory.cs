using Torneo.Domain.Matches;

namespace Torneo.Domain.Tests.Standings;

internal static class FinishedMatchFactory
{
    internal static Match Create(Guid homeTeamId, Guid awayTeamId, int homeScore, int awayScore)
    {
        var match = Match.Create(homeTeamId, awayTeamId, new DateTimeOffset(2026, 10, 19, 15, 0, 0, TimeSpan.Zero)).Value;
        var goals = new List<GoalRegistration>();

        for (var minute = 1; minute <= homeScore; minute++)
        {
            goals.Add(new GoalRegistration(Guid.NewGuid(), homeTeamId, minute));
        }

        for (var minute = 1; minute <= awayScore; minute++)
        {
            goals.Add(new GoalRegistration(Guid.NewGuid(), awayTeamId, minute));
        }

        var registered = match.RegisterResult(homeScore, awayScore, goals);
        if (registered.IsFailure)
        {
            throw new InvalidOperationException(registered.Error!.Message);
        }

        return match;
    }
}
