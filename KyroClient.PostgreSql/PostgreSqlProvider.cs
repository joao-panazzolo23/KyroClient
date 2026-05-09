using System.Data.Common;
using KyroClient.Core.Connection.Services;
using KyroClient.Core.Connection.Strategies;
using Npgsql;

namespace KyroClient.PostgreSql;

public class PostgreSqlProvider : IDatabaseProvider
{
    public string Name => "PostgreSql";
    public string DefaultPort => "5432";

    public DbConnection CreateConnection(IConnectionOptions options)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = options.Host,
            Port = int.Parse(options.Port),
            Database = options.Database,
            Username = options.Username,
            Password = options.Password,
        };
        return new NpgsqlConnection(builder.ConnectionString);
    }

    public DbConnectionStringBuilder CreateConnectionStringBuilder()
        => new NpgsqlConnectionStringBuilder();
}