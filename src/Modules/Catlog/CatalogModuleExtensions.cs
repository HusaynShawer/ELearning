using ELearning.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning.Catalog;

public static class CatalogModuleExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<CatalogDbContext>(o =>
            o.UseNpgsql(config.GetConnectionString("Default"),
                s => s.MigrationsHistoryTable("__EFMigrationsHistory", CatalogDbContext.Schema)));

        return services;
    }
}
