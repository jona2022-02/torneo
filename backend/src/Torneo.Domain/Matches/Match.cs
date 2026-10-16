using Torneo.Domain.Common;
using Torneo.Domain.Goals;

namespace Torneo.Domain.Matches;

public sealed class Match : Entity
{
    private readonly List<Goal> _goals = [];

    private Match(Guid id, Guid homeTeamId, Guid awayTeamId, DateTimeOffset kickoff)
    {
        Id = id;
        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;
        Kickoff = kickoff;
        Status = MatchStatus.Programado;
    }

    public Guid Id { get; }

    public Guid HomeTeamId { get; private set; }

    public Guid AwayTeamId { get; private set; }

    public DateTimeOffset Kickoff { get; private set; }

    public MatchStatus Status { get; private set; }

    public int? HomeScore { get; private set; }

    public int? AwayScore { get; private set; }

    public IReadOnlyList<Goal> Goals => _goals;

    public static Result<Match> Create(Guid homeTeamId, Guid awayTeamId, DateTimeOffset kickoff)
    {
        if (homeTeamId == Guid.Empty || awayTeamId == Guid.Empty)
        {
            return Result<Match>.Failure(ErrorCodes.Validation, "El partido necesita equipo local y visitante.");
        }

        if (homeTeamId == awayTeamId)
        {
            return Result<Match>.Failure(ErrorCodes.MatchSameTeam, "El equipo local y el visitante no pueden ser el mismo.");
        }

        return Result<Match>.Success(new Match(Guid.NewGuid(), homeTeamId, awayTeamId, kickoff));
    }

    public Result ReplaceSchedule(Guid homeTeamId, Guid awayTeamId, DateTimeOffset kickoff)
    {
        if (Status == MatchStatus.Finalizado)
        {
            return Result.Failure(ErrorCodes.Conflict, "Un partido finalizado no cambia de calendario.");
        }

        if (homeTeamId == Guid.Empty || awayTeamId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El partido necesita equipo local y visitante.");
        }

        if (homeTeamId == awayTeamId)
        {
            return Result.Failure(ErrorCodes.MatchSameTeam, "El equipo local y el visitante no pueden ser el mismo.");
        }

        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;
        Kickoff = kickoff;
        return Result.Success();
    }

    public Result RegisterResult(int homeScore, int awayScore, IReadOnlyList<GoalRegistration> goals)
    {
        if (homeScore < 0 || awayScore < 0)
        {
            return Result.Failure(ErrorCodes.Validation, "El marcador no puede ser negativo.");
        }

        var createdGoals = new List<Goal>(goals.Count);
        foreach (var goal in goals)
        {
            if (goal.TeamId != HomeTeamId && goal.TeamId != AwayTeamId)
            {
                return Result.Failure(
                    ErrorCodes.MatchResultPlayerNotInMatch,
                    "El jugador no pertenece a ninguno de los equipos del partido.");
            }

            var created = Goal.Create(Id, goal.PlayerId, goal.TeamId, goal.Minute);
            if (created.IsFailure)
            {
                return Result.Failure(created.Error!.Code, created.Error.Message);
            }

            createdGoals.Add(created.Value);
        }

        var homeGoals = createdGoals.Count(goal => goal.TeamId == HomeTeamId);
        var awayGoals = createdGoals.Count(goal => goal.TeamId == AwayTeamId);
        if (homeGoals != homeScore || awayGoals != awayScore)
        {
            return Result.Failure(
                ErrorCodes.MatchResultGoalMismatch,
                "Los goles cargados no coinciden con el marcador.");
        }

        HomeScore = homeScore;
        AwayScore = awayScore;
        Status = MatchStatus.Finalizado;
        _goals.Clear();
        _goals.AddRange(createdGoals);
        Raise(new MatchResultRegistered(Id, homeScore, awayScore, DateTimeOffset.UtcNow));
        return Result.Success();
    }
}
