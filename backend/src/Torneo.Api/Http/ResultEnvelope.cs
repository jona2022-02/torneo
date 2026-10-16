using Torneo.Domain.Common;

namespace Torneo.Api.Http;

public sealed record ResultEnvelope(Result Result, int SuccessStatus, object? Body, string? Location);
