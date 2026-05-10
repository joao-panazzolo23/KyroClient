namespace KyroClient.Core.Connection.Strategies;

public record IConnectionOptions(
    string Host,
    string Port,
    string Database,
    string Username,
    string? Password
);