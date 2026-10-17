using Torneo.Domain.Common;

namespace Torneo.Domain.Players;

public sealed class Player : Entity
{
    public const int MinShirtNumber = 1;
    public const int MaxShirtNumber = 99;
    public const int MaxNameLength = 80;

    private Player(Guid id, Guid teamId, string name, int shirtNumber, LineupRole role)
    {
        Id = id;
        TeamId = teamId;
        Name = name;
        ShirtNumber = shirtNumber;
        Role = role;
    }

    public Guid Id { get; }

    public Guid TeamId { get; }

    public string Name { get; private set; }

    public int ShirtNumber { get; private set; }

    public LineupRole Role { get; private set; }

    public static Result<Player> Create(Guid teamId, string name, int shirtNumber, LineupRole role = LineupRole.Defensa)
    {
        if (teamId == Guid.Empty)
        {
            return Result<Player>.Failure(ErrorCodes.Validation, "El jugador debe pertenecer a un equipo.");
        }

        var nameValidation = ValidateName(name);
        if (nameValidation.IsFailure)
        {
            return Result<Player>.Failure(nameValidation.Error!.Code, nameValidation.Error.Message);
        }

        var numberValidation = ValidateShirtNumber(shirtNumber);
        if (numberValidation.IsFailure)
        {
            return Result<Player>.Failure(numberValidation.Error!.Code, numberValidation.Error.Message);
        }

        var player = new Player(Guid.NewGuid(), teamId, name.Trim(), shirtNumber, role);
        return Result<Player>.Success(player);
    }

    public Result Update(string name, int shirtNumber, LineupRole role)
    {
        var nameValidation = ValidateName(name);
        if (nameValidation.IsFailure)
        {
            return nameValidation;
        }

        var numberValidation = ValidateShirtNumber(shirtNumber);
        if (numberValidation.IsFailure)
        {
            return numberValidation;
        }

        Name = name.Trim();
        ShirtNumber = shirtNumber;
        Role = role;
        return Result.Success();
    }

    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(ErrorCodes.Validation, "El nombre del jugador es obligatorio.");
        }

        if (name.Trim().Length > MaxNameLength)
        {
            return Result.Failure(ErrorCodes.Validation, "El nombre del jugador no puede superar 80 caracteres.");
        }

        return Result.Success();
    }

    private static Result ValidateShirtNumber(int shirtNumber)
    {
        if (shirtNumber is < MinShirtNumber or > MaxShirtNumber)
        {
            return Result.Failure(ErrorCodes.Validation, "El dorsal debe estar entre 1 y 99.");
        }

        return Result.Success();
    }
}
