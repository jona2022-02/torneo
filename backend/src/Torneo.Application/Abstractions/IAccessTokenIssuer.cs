namespace Torneo.Application.Abstractions;

public interface IAccessTokenIssuer
{
    string Issue(Guid operatorId, string username, string displayName);
}
