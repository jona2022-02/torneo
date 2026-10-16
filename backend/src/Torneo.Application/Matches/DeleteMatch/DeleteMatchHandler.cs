using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Matches.DeleteMatch;

public sealed class DeleteMatchHandler
{
    private readonly IMatchRepository _matches;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMatchHandler(IMatchRepository matches, IUnitOfWork unitOfWork)
    {
        _matches = matches;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteMatchCommand command, CancellationToken cancellationToken)
    {
        var validation = DeleteMatchValidator.Validate(command);
        if (validation.IsFailure)
        {
            return validation;
        }

        var match = await _matches.GetByIdAsync(command.Id, cancellationToken);
        if (match is null)
        {
            return Result.Success();
        }

        _matches.Remove(match);
        return await _unitOfWork.CommitAsync(cancellationToken);
    }
}
