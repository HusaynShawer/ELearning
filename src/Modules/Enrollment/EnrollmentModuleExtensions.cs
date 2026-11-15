using ELearning.Enrollment.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning.Enrollment;

public static class EnrollmentModuleExtensions
{
    public static IServiceCollection AddEnrollmentModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<EnrollmentDbContext>(o =>
            o.UseNpgsql(config.GetConnectionString("Default"),
                s => s.MigrationsHistoryTable("__EFMigrationsHistory", EnrollmentDbContext.Schema)));

        return services;
    }
}
