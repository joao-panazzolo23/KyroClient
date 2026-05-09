using KyroClient.Core.Restore.Enums;
using KyroClient.Core.Restore.Models;

namespace KyroClient.Core.Restore.Services;

public interface IDatabaseRestoreService
{
    Task RestoreAsync(
        RestoreOptions options,
        IProgress<RestoreProgress>? progress = null,
        CancellationToken ct = default);

    bool SupportsFormat(RestoreFormat format);
}