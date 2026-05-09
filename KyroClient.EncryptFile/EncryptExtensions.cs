using KyroClient.Core.Secrets.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KyroClient.EncryptFile;

public static class EncryptExtensions
{
    public static IServiceCollection AddEncryptedFile(this IServiceCollection services)
    {
        return services.AddScoped<ISecretStore, EncryptedFileSecretStore>();
    }
}