using System.Runtime.CompilerServices;
using KyroClient.PostgreSql;
using Microsoft.Extensions.DependencyInjection;

namespace KyroClient.Desktop.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddPostgreSql();
    }
}