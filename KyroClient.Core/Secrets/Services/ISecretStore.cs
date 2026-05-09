namespace KyroClient.Core.Secrets.Services;

public interface ISecretStore
{
    Task<string?> GetAsync(string key);
    Task DeleteAsync(string key);
}