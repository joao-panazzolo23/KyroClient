namespace KyroClient.Core.Schemas.Models;

public sealed record ColumnInfo
{
    public string Name { get; init; }
    public string DbTypeName { get; init; }
    public bool IsNullable { get; init; }
    public bool IsPrimaryKey { get; init; }
    public string? DefaultValue { get; init; }
    public int OrdinalPosition { get; init; }
}