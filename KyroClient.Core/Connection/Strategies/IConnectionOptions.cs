namespace KyroClient.Core.Connection.Strategies;

public interface IConnectionOptions
{
    string Host { get; }
    string Port { get; }
    string Database { get; }
    string Username { get; }
    string? Password { get; }
    string ToDisplayString();
}