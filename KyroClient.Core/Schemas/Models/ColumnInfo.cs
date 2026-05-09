namespace KyroClient.Core.Schemas.Models;

public sealed record ColumnInfo
{
    public required string Name { get; init; }
    public required string DbTypeName { get; init; }
    public bool IsNullable { get; init; }
    public bool IsPrimaryKey { get; init; }
    public string? DefaultValue { get; init; }
    public int OrdinalPosition { get; init; }
}