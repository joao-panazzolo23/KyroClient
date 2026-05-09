using KyroClient.Core.Restore.Enums;

namespace KyroClient.Core.Restore.Models;

public sealed record RestoreOptions
{
    public required string FilePath { get; init; }
    public required string TargetDatabase { get; init; }
    public RestoreFormat Format { get; init; } = RestoreFormat.Sql;
    public bool DropAndRecreate { get; init; } = false;
}