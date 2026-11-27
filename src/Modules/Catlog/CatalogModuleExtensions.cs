using ELearning.Catalog.Application;
using ELearning.Catalog.Application.Service;
using ELearning.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Catalog;

public static class CatalogModuleExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<CatalogDbContext>(o =>
            o.UseNpgsql(config.GetConnectionString("Default"),
                s => s.MigrationsHistoryTable("__EFMigrationsHistory", CatalogDbContext.Schema)));
        // repos
        services.AddScoped<CourseRepository>();
        services.AddScoped<SectionRepository>();
        services.AddScoped<CategoryRepository>();
        services.AddScoped<LessonRepository>();
//      services
        services.AddScoped<CourseService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<SectionService>();
        services.AddScoped<LessonService>();
        services.AddScoped<UnitOfWork>();
        services.AddScoped<IVideoStorageService , LocalVideoStorageService>();
        return services;
    }
}
