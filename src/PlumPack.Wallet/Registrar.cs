using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlumPack.Wallet.Domain;
using PlumPack.Wallet.PayPal;
using SharpDataAccess.Migrations;

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

            var d = configuration.GetSection("PayPal");
            
            // Configure some options
            services.Configure<PayPalOptions>(configuration.GetSection("PayPal"));
        }
    }
}