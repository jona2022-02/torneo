using Microsoft.Data.SqlClient;

namespace Torneo.Infrastructure.Read;

internal sealed class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection Create() => new(_connectionString);
}
