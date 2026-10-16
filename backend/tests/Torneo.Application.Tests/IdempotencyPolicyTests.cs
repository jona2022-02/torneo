using Torneo.Application.Idempotency;
using Torneo.Domain.Common;

namespace Torneo.Application.Tests;

public class IdempotencyPolicyTests
{
    private const string Hash = "abc123";

    [Fact]
    public void Sin_clave_responde_que_falta()
    {
        var result = IdempotencyPolicy.Evaluate(null, Hash, null);

        Assert.Equal(ErrorCodes.IdempotencyMissingKey, result.Error!.Code);
    }

    [Fact]
    public void Clave_nueva_deja_continuar()
    {
        var result = IdempotencyPolicy.Evaluate(Guid.NewGuid().ToString(), Hash, null);

        Assert.True(result.IsSuccess);
        Assert.False(result.Value.Replay);
    }

    [Fact]
    public void Misma_clave_y_mismo_cuerpo_repite_la_respuesta()
    {
        var key = Guid.NewGuid().ToString();
        var stored = new StoredResponse(key, Hash, 201, """{"id":"1"}""");

        var result = IdempotencyPolicy.Evaluate(key, Hash, stored);

        Assert.True(result.Value.Replay);
        Assert.Equal(201, result.Value.StatusCode);
        Assert.Equal(stored.ResponseBody, result.Value.ResponseBody);
    }

    [Fact]
    public void Misma_clave_y_otro_cuerpo_entra_en_conflicto()
    {
        var key = Guid.NewGuid().ToString();
        var stored = new StoredResponse(key, Hash, 201, """{"id":"1"}""");

        var result = IdempotencyPolicy.Evaluate(key, "otro-hash", stored);

        Assert.Equal(ErrorCodes.IdempotencyKeyReused, result.Error!.Code);
    }
}
