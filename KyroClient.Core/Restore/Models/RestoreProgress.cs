namespace KyroClient.Core.Restore.Models;

public sealed record RestoreProgress(
    long BytesRead,
    long TotalBytes,
    int StatementsExecuted,
    string CurrentStatement);