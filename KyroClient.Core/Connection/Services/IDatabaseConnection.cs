using System.Data;
using KyroClient.Core.Connection.Strategies;

namespace KyroClient.Core.Connection.Services;

public interface IDatabaseConnection : IAsyncDisposable
{
    IConnectionOptions Options { get; }
    ConnectionState State { get; }

    Task OpenAsync(CancellationToken ct = default);
    Task CloseAsync();
    Task<bool> TestAsync(CancellationToken ct = default);
}