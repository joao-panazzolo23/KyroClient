using System.Data;
using KyroClient.Core.Connection.Models;
using KyroClient.Core.Schemas.Models;

namespace KyroClient.Core.Schemas.Services;

public interface ISchemaExplorer
{
    Task<IReadOnlyList<DatabaseInfo>> GetDatabases(CancellationToken ct = default);

    Task<IReadOnlyList<TableInfo>> GetTables(
        string database,
        CancellationToken ct = default
    );

    Task<IReadOnlyList<ColumnInfo>> GetColumns(
        string database,
        string schema,
        string table,
        CancellationToken ct = default
    );

    //todo: find a better way to use returns
    Task<QueryResult> ExecuteQuery(
        string database,
        string sqlCommand,
        CancellationToken ct = default
    );
}