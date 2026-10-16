namespace Torneo.Infrastructure.Persistence;

internal sealed record SeedGoal(string Team, int Shirt, int Minute);

internal sealed record SeedFixture(
    string Home,
    string Away,
    DateTimeOffset Kickoff,
    bool Finished,
    int HomeScore,
    int AwayScore,
    SeedGoal[] Goals);

internal static class TournamentCatalog
{
    public static readonly string[] Teams =
        ["Real Madrid", "Barcelona", "Atlético de Madrid", "Bayern Múnich"];

    public static readonly string[][] Players =
    [
        ["Thibaut Courtois", "Dani Carvajal", "Éder Militão", "Federico Valverde", "Kylian Mbappé"],
        ["Marc-André ter Stegen", "Jules Koundé", "Ronald Araújo", "Pedri", "Robert Lewandowski"],
        ["Jan Oblak", "Antoine Griezmann", "José María Giménez", "Rodrigo De Paul", "Julián Álvarez"],
        ["Manuel Neuer", "Joshua Kimmich", "Alphonso Davies", "Harry Kane", "Jamal Musiala"]
    ];

    public static readonly SeedFixture[] Fixtures =
    [
        new("Real Madrid", "Barcelona", Kickoff(11, 15), true, 2, 1,
            [new("Real Madrid", 5, 18), new("Real Madrid", 5, 61), new("Barcelona", 3, 74)]),
        new("Atlético de Madrid", "Bayern Múnich", Kickoff(11, 17), true, 1, 1,
            [new("Atlético de Madrid", 2, 33), new("Bayern Múnich", 4, 80)]),
        new("Real Madrid", "Atlético de Madrid", Kickoff(12, 16), true, 3, 0,
            [new("Real Madrid", 5, 8), new("Real Madrid", 3, 51), new("Real Madrid", 5, 77)]),
        new("Barcelona", "Bayern Múnich", Kickoff(18, 15), false, 0, 0, []),
        new("Real Madrid", "Bayern Múnich", Kickoff(18, 17), false, 0, 0, []),
        new("Barcelona", "Atlético de Madrid", Kickoff(19, 16), false, 0, 0, [])
    ];

    private static DateTimeOffset Kickoff(int day, int hour) =>
        new(2026, 10, day, hour, 0, 0, TimeSpan.Zero);
}
