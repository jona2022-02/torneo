using Torneo.Domain.Players;

namespace Torneo.Infrastructure.Persistence;

internal sealed record SeedPlayer(string Name, LineupRole Role);

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

    public static readonly SeedPlayer[][] Players =
    [
        [
            new("Thibaut Courtois", LineupRole.Portero),
            new("Dani Carvajal", LineupRole.Defensa),
            new("Éder Militão", LineupRole.Defensa),
            new("Federico Valverde", LineupRole.Volante),
            new("Kylian Mbappé", LineupRole.Delantero),
            new("Antonio Rüdiger", LineupRole.Defensa),
            new("Vinícius Júnior", LineupRole.Delantero),
            new("Jude Bellingham", LineupRole.Volante),
            new("Rodrygo", LineupRole.Delantero),
            new("Luka Modrić", LineupRole.Volante),
            new("Ferland Mendy", LineupRole.Defensa)
        ],
        [
            new("Marc-André ter Stegen", LineupRole.Portero),
            new("Jules Koundé", LineupRole.Defensa),
            new("Ronald Araújo", LineupRole.Defensa),
            new("Pedri", LineupRole.Volante),
            new("Robert Lewandowski", LineupRole.Delantero),
            new("Pau Cubarsí", LineupRole.Defensa),
            new("Lamine Yamal", LineupRole.Delantero),
            new("Frenkie de Jong", LineupRole.Volante),
            new("Raphinha", LineupRole.Delantero),
            new("Gavi", LineupRole.Volante),
            new("Alejandro Balde", LineupRole.Defensa)
        ],
        [
            new("Jan Oblak", LineupRole.Portero),
            new("Antoine Griezmann", LineupRole.Delantero),
            new("José María Giménez", LineupRole.Defensa),
            new("Rodrigo De Paul", LineupRole.Volante),
            new("Julián Álvarez", LineupRole.Delantero),
            new("Axel Witsel", LineupRole.Defensa),
            new("Nahuel Molina", LineupRole.Defensa),
            new("Koke", LineupRole.Volante),
            new("Alexander Sørloth", LineupRole.Delantero),
            new("Marcos Llorente", LineupRole.Volante),
            new("Reinildo Mandava", LineupRole.Defensa)
        ],
        [
            new("Manuel Neuer", LineupRole.Portero),
            new("Joshua Kimmich", LineupRole.Defensa),
            new("Alphonso Davies", LineupRole.Defensa),
            new("Harry Kane", LineupRole.Delantero),
            new("Jamal Musiala", LineupRole.Delantero),
            new("Dayot Upamecano", LineupRole.Defensa),
            new("Raphaël Guerreiro", LineupRole.Defensa),
            new("Leon Goretzka", LineupRole.Volante),
            new("Leroy Sané", LineupRole.Volante),
            new("Thomas Müller", LineupRole.Volante),
            new("Serge Gnabry", LineupRole.Delantero)
        ]
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
