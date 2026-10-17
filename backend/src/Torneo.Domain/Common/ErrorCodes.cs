namespace Torneo.Domain.Common;

public static class ErrorCodes
{
    public const string Validation = "validation";
    public const string AuthInvalid = "auth.invalid";
    public const string RateLimit = "rate_limit";
    public const string NotFound = "not_found";
    public const string Conflict = "conflict";
    public const string IdempotencyMissingKey = "idempotency.missing_key";
    public const string IdempotencyKeyReused = "idempotency.key_reused";
    public const string TeamNameDuplicate = "team.name.duplicate";
    public const string TeamHasMatches = "team.has_matches";
    public const string PlayerDuplicateNumber = "player.duplicate_number";
    public const string PlayerHasGoals = "player.has_goals";
    public const string MatchSameTeam = "match.same_team";
    public const string MatchResultGoalMismatch = "match.result.goal_mismatch";
    public const string MatchResultPlayerNotInMatch = "match.result.player_not_in_match";
}
