using KyroClient.Core.Schemas.Models;

namespace KyroClient.Core.Schemas.Services;

public interface ISchemaExplorer
{
    Task<IReadOnlyList<DatabaseInfo>> GetDatabasesAsync(CancellationToken ct = default);

    Task<IReadOnlyList<TableInfo>> GetTablesAsync(
        string database,
        CancellationToken ct = default
    );

    Task<IReadOnlyList<ColumnInfo>> GetColumnsAsync(
        string database,
        string table,
        CancellationToken ct = default
    );
}