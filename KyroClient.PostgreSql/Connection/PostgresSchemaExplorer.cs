using System.Data;
using KyroClient.Core.Schemas.Enums;
using KyroClient.Core.Schemas.Models;
using KyroClient.Core.Schemas.Services;
using Npgsql;

namespace KyroClient.PostgreSql.Connection;

public class PostgresSchemaExplorer(NpgsqlConnection connection) : ISchemaExplorer, IDisposable, IAsyncDisposable
{
    public async Task<IReadOnlyList<DatabaseInfo>> GetDatabases(CancellationToken ct = default)
    {
        const string sql = """
                           SELECT
                               d.datname AS name,
                               pg_catalog.pg_get_userbyid(d.datdba) AS owner,
                               pg_catalog.pg_encoding_to_char(d.encoding) AS encoding,
                               pg_catalog.pg_database_size(d.datname) AS size_bytes
                           FROM pg_catalog.pg_database d
                           WHERE d.datistemplate = false
                           ORDER BY d.datname;
                           """;
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(ct);

        try
        {
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = sql;

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            var result = new List<DatabaseInfo>();
            while (await reader.ReadAsync(ct))
                result.Add(new DatabaseInfo
                {
                    Name = reader.GetString(0),
                    Owner = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Encoding = reader.IsDBNull(2) ? null : reader.GetString(2),
                    SizeBytes = reader.IsDBNull(3) ? null : reader.GetInt64(3),
                });
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IReadOnlyList<TableInfo>> GetTables(string database, CancellationToken ct = default)
    {
        //todo: remove hardcode, create new Connection string, etc...
        var connStr = $"Host=localhost;Port=5432;Username=postgres;Password=postgres;Database={database}";
        await using var dbConn = new NpgsqlConnection(connStr);
        await dbConn.OpenAsync(CancellationToken.None);
        Console.WriteLine($"Connected to: {dbConn.Database}");

        const string sql = """
                           SELECT t.table_schema, t.table_name, t.table_type
                           FROM information_schema.tables t
                           WHERE t.table_schema NOT IN ('pg_catalog', 'information_schema')
                           ORDER BY t.table_schema, t.table_name;
                           """;

        await using var cmd = new NpgsqlCommand(sql, dbConn);
        await using var reader = await cmd.ExecuteReaderAsync(CancellationToken.None);

        var result = new List<TableInfo>();
        while (await reader.ReadAsync(CancellationToken.None))
            result.Add(new TableInfo
            {
                Schema = reader.GetString(0),
                Name = reader.GetString(1),
                Kind = reader.GetString(2) == "VIEW" ? TableKind.View : TableKind.Table
            });

        return result;
    }

    // public async Task<IReadOnlyList<TableInfo>> GetTables(string database, CancellationToken ct = default)
    // {
    //     if (connection.State != ConnectionState.Open)
    //         await connection.OpenAsync(ct);
    //
    //     const string sql = """
    //                        SELECT
    //                            t.table_schema,
    //                            t.table_name,
    //                            t.table_type
    //                        FROM information_schema.tables t
    //                        JOIN pg_catalog.pg_class c ON c.relname = t.table_name
    //                        JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace AND n.nspname = t.table_schema
    //                        WHERE t.table_schema NOT IN ('pg_catalog', 'information_schema')
    //                        ORDER BY t.table_schema, t.table_name;
    //                        """;
    //     try
    //     {
    //         await using var cmd = connection.CreateCommand();
    //         cmd.CommandText = sql;
    //
    //         await using var reader = await cmd.ExecuteReaderAsync(ct);
    //         var result = new List<TableInfo>();
    //         while (await reader.ReadAsync(ct))
    //             result.Add(new TableInfo
    //             {
    //                 Schema = reader.GetString(0),
    //                 Name = reader.GetString(1),
    //                 Kind = reader.GetString(2) == "VIEW" ? TableKind.View : TableKind.Table
    //             });
    //
    //         return result;
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    // }

    public async Task<IReadOnlyList<ColumnInfo>> GetColumns(
        string database,
        string table,
        CancellationToken ct = default
    )
    {
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(ct);

        const string sql = """
                           SELECT
                               c.column_name,
                               c.data_type,
                               c.is_nullable = 'YES',
                               c.column_default,
                               c.ordinal_position,
                               EXISTS (
                                   SELECT 1
                                   FROM information_schema.table_constraints tc
                                   JOIN information_schema.key_column_usage kcu
                                       ON kcu.constraint_name = tc.constraint_name
                                       AND kcu.table_schema = tc.table_schema
                                   WHERE tc.constraint_type = 'PRIMARY KEY'
                                     AND tc.table_name = c.table_name
                                     AND kcu.column_name = c.column_name
                               ) AS is_pk
                           FROM information_schema.columns c
                           WHERE c.table_name = @table
                             AND c.table_schema NOT IN ('pg_catalog', 'information_schema')
                           ORDER BY c.ordinal_position;
                           """;
        //todo: find a better way to open connections. that sucks.
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(ct);

        try
        {
            await using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("table", table);

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            var result = new List<ColumnInfo>();
            while (await reader.ReadAsync(ct))
                result.Add(new ColumnInfo
                {
                    Name = reader.GetString(0),
                    DbTypeName = reader.GetString(1),
                    IsNullable = reader.GetBoolean(2),
                    DefaultValue = reader.IsDBNull(3) ? null : reader.GetString(3),
                    OrdinalPosition = reader.GetInt32(4),
                    IsPrimaryKey = reader.GetBoolean(5),
                });

            return result;
        }
        catch
        {
        }

        return null;
    }

    public void Dispose() => connection.Dispose();

    public async ValueTask DisposeAsync() => await connection.DisposeAsync();
}