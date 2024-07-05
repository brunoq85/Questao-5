using Microsoft.Data.Sqlite;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Repository;
using System.Data;

namespace Questao5.Configuration
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>(sp => new SqliteConnection(configuration["DatabaseName"]));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMovimentoRepository, MovimentoRepository>();
            services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
            services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();
        }
    }
}
