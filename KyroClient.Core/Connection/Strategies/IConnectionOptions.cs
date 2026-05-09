namespace KyroClient.Core.Connection.Strategies;

public interface IConnectionOptions
{
    string ToDisplayString();
    IConnectionOptions WithPassword(string? password);
}