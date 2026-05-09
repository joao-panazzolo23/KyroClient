using System.Data;
using KyroClient.Core.Connection.Services;
using KyroClient.Core.Connection.Strategies;
using Npgsql;

namespace KyroClient.PostgreSql.Connection;

public class PostgreSqlConnection : IDatabaseConnection
{
    private readonly NpgsqlConnection _inner;
    IConnectionOptions IDatabaseConnection.Options { get; }
    ConnectionState IDatabaseConnection.State { get; }

    public Task OpenAsync(CancellationToken ct = default)
        => _inner.OpenAsync(ct);

    public Task CloseAsync()
        => _inner.CloseAsync();

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

    public async ValueTask DisposeAsync() => await _inner.DisposeAsync();
}