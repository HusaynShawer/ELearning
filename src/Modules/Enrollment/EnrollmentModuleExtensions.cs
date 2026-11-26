using ELearning.Enrollment.Infrastructure;
using ELearning.SharedKernel;
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

        //services.AddDataProtection();
        services.AddScoped<EnrollmentService>();
        services.AddScoped<InstructorService>();
        services.AddScoped<Helper>();

        // Repositories
        services.AddScoped<InstructorRepository>();
        services.AddScoped<EnrollmentRepository>();
        services.AddScoped<EnrollmentUnitOfWork>();
        return services;
    }
}
