using System.Data.Common;
using KyroClient.Core.Connection.Strategies;

namespace KyroClient.Core.Connection.Services;

public interface IDatabaseProvider
{
    string Name { get; }
    string DefaultPort { get; }
    DbConnection CreateConnection(IConnectionOptions options);
    DbConnectionStringBuilder CreateConnectionStringBuilder();
}