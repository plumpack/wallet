using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlumPack.Infrastructure.Migrations;

namespace PlumPack.Wallet
{
    public class Registrar
    {
        public static void Register(IServiceCollection services, IConfiguration configuration, string additionalConfigDirectory)
        {
            // Register our core infrastructure services.
            Infrastructure.Registrar.Register(services, configuration, new MigrationOptions(typeof(Migrations.Versions).Assembly), additionalConfigDirectory);
            
            // Add all the services in this assembly.
            Infrastructure.ServiceContext.AddServicesFromAssembly(typeof(Account).Assembly, services);
        }
    }
}