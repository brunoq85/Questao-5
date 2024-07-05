using Questao5.Infrastructure.Sqlite;
using System.Runtime.CompilerServices;

namespace Questao5.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new DatabaseConfig { Name = configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
            services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();
        }
    }
}
