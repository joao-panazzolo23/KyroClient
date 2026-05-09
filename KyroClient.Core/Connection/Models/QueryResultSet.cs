namespace KyroClient.Core.Connection.Models;

public sealed record QueryResultSet
{
    public required IReadOnlyList<ColumnDefinition> Columns { get; init; }
    public required IAsyncEnumerable<IReadOnlyList<object?>> Rows { get; init; }
    public int? RowsAffected { get; init; }
    public TimeSpan Elapsed { get; init; }
}