using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Tests;

public class UnitOfWorkTests
{
    [Fact]
    public async Task Commit_confirma_las_dos_escrituras()
    {
        var transaction = new FakeTransaction();
        var unitOfWork = new UnitOfWork(transaction);

        var result = await unitOfWork.CommitAsync(CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(["begin", "save-equipo", "save-jugador", "commit"], transaction.Steps);
    }

    [Fact]
    public async Task Fallo_en_la_segunda_escritura_revierte_la_primera()
    {
        var transaction = new FakeTransaction { FailOnSecondWrite = true };
        var unitOfWork = new UnitOfWork(transaction);

        var result = await unitOfWork.CommitAsync(CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCodes.Conflict, result.Error!.Code);
        Assert.Equal(["begin", "save-equipo", "save-jugador", "rollback"], transaction.Steps);
        Assert.DoesNotContain("commit", transaction.Steps);
    }

    private sealed class FakeTransaction : IApplicationTransaction
    {
        public List<string> Steps { get; } = [];

        public bool FailOnSecondWrite { get; init; }

        public Task BeginAsync(CancellationToken cancellationToken)
        {
            Steps.Add("begin");
            return Task.CompletedTask;
        }

        public Task SaveAsync(CancellationToken cancellationToken)
        {
            Steps.Add("save-equipo");
            Steps.Add("save-jugador");
            if (FailOnSecondWrite)
            {
                throw new InvalidOperationException("El segundo repositorio falló.");
            }

            return Task.CompletedTask;
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            Steps.Add("commit");
            return Task.CompletedTask;
        }

        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            Steps.Add("rollback");
            return Task.CompletedTask;
        }
    }
}
