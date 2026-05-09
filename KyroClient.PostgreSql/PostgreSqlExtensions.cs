using KyroClient.Core.Connection.Services;
using KyroClient.PostgreSql.Connection;
using Microsoft.Extensions.DependencyInjection;

namespace KyroClient.PostgreSql;

public static class PostgreSqlExtensions
{
    public static IServiceCollection AddPostgreSql(this IServiceCollection services)
    {
        return services.AddScoped<IDatabaseConnection, PostgreSqlConnection>();
    }
}