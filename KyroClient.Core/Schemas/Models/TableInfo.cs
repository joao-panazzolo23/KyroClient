using KyroClient.Core.Schemas.Enums;

namespace KyroClient.Core.Schemas.Models;

public sealed record TableInfo
{
    public required string Schema { get; init; }
    public required string Name { get; init; }
    public TableKind Kind { get; init; }
    public long? RowEstimate { get; init; }
}