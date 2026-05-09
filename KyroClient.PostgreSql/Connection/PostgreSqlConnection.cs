using System.Data;
using KyroClient.Core.Connection.Services;
using KyroClient.Core.Connection.Strategies;
using Npgsql;

namespace KyroClient.PostgreSql.Connection;

public class PostgreSqlConnection : IDatabaseConnection
{
    public readonly NpgsqlConnection Conn;
    IConnectionOptions IDatabaseConnection.Options { get; }
    ConnectionState IDatabaseConnection.State { get; }

    public PostgreSqlConnection(IConnectionOptions options)
    {
        //TODO: REMOVE HARDCODE 
        Conn = new NpgsqlConnection(
            "Host=localhost;Port=5432;Database=breadboard;Username=postgres;Password=postgres;"
        );
    }

    public Task OpenAsync(CancellationToken ct = default)
        => Conn.OpenAsync(ct);

    public Task CloseAsync()
        => Conn.CloseAsync();

    /// <summary>
    /// Todo: this is returning just a bool. It could be MUCH MORE detailed than that,
    /// exposing error at Interface
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<bool> TestAsync(CancellationToken ct = default)
    {
        try
        {
            await OpenAsync(ct);
            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
        finally
        {
            await CloseAsync();
        }
    }

    public IDbCommand CreateCommand() => Conn.CreateCommand();

    public async ValueTask DisposeAsync() => await Conn.DisposeAsync();
}