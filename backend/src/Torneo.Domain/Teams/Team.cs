using Torneo.Domain.Common;

namespace Torneo.Domain.Teams;

public sealed class Team : Entity
{
    public const int MaxNameLength = 80;

    private Team(Guid id, string name, DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
    }

    public Guid Id { get; }

    public string Name { get; private set; }

    public DateTimeOffset CreatedAt { get; }

    public static Result<Team> Create(string name)
    {
        var validation = ValidateName(name);
        if (validation.IsFailure)
        {
            return Result<Team>.Failure(validation.Error!.Code, validation.Error.Message);
        }

        var team = new Team(Guid.NewGuid(), name.Trim(), DateTimeOffset.UtcNow);
        team.Raise(new TeamCreated(team.Id, team.Name, team.CreatedAt));
        return Result<Team>.Success(team);
    }

    public Result Rename(string name)
    {
        var validation = ValidateName(name);
        if (validation.IsFailure)
        {
            return validation;
        }

        Name = name.Trim();
        return Result.Success();
    }

    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(ErrorCodes.Validation, "El nombre del equipo es obligatorio.");
        }

        if (name.Trim().Length > MaxNameLength)
        {
            return Result.Failure(ErrorCodes.Validation, "El nombre del equipo no puede superar 80 caracteres.");
        }

        return Result.Success();
    }
}
