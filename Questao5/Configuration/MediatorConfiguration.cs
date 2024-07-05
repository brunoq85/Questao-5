using MediatR;
using System.Reflection;

namespace Questao5.Configuration
{
    public static class MediatorConfiguration
    {
        public static void AddMediatorConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
