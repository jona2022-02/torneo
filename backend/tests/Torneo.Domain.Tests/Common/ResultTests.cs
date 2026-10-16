using Torneo.Domain.Common;

namespace Torneo.Domain.Tests.Common;

public class ResultTests
{
    [Fact]
    public void Success_no_lleva_error()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_guarda_codigo_y_mensaje()
    {
        var result = Result.Failure(ErrorCodes.NotFound, "No se encontró el equipo.");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCodes.NotFound, result.Error!.Code);
        Assert.Equal("No se encontró el equipo.", result.Error.Message);
    }

    [Fact]
    public void Result_generico_expone_el_valor_cuando_es_exitoso()
    {
        var result = Result<string>.Success("Torneo");

        Assert.True(result.IsSuccess);
        Assert.Equal("Torneo", result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Result_generico_fallido_guarda_el_error_y_no_expone_valor()
    {
        var result = Result<string>.Failure(ErrorCodes.Validation, "El nombre es obligatorio.");

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCodes.Validation, result.Error!.Code);
        Assert.Equal("El nombre es obligatorio.", result.Error.Message);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
