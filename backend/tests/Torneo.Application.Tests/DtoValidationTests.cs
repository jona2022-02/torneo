using Torneo.Api.Matches;
using Torneo.Api.Teams;

namespace Torneo.Application.Tests;

public class DtoValidationTests
{
    [Fact]
    public void Equipo_sin_nombre_falla_en_el_dto()
    {
        var result = new CreateTeamDtoValidator().Validate(new CreateTeamDto { Name = "  " });

        Assert.False(result.IsValid);
        Assert.Equal("El nombre del equipo es obligatorio.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Gol_con_minuto_fuera_de_rango_falla_en_el_dto()
    {
        var result = new RegisterResultDtoValidator().Validate(new RegisterResultDto
        {
            HomeScore = 1,
            AwayScore = 0,
            Goals = [new GoalLineDto { PlayerId = Guid.NewGuid(), Minute = 0 }]
        });

        Assert.False(result.IsValid);
        Assert.Equal("Cada gol necesita un jugador y un minuto entre 1 y 120.", result.Errors[0].ErrorMessage);
    }
}
