using KyroClient.Core.Restore.Enums;

namespace KyroClient.Core.Restore.Models;

public interface IDatabaseRestoreService
{
    Task RestoreAsync(
        RestoreOptions options,
        IProgress<RestoreProgress>? progress = null,
        CancellationToken ct = default);

    bool SupportsFormat(RestoreFormat format);
}