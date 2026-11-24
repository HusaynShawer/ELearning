using ELearning.Identity.Domain;
using ELearning.Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Buffers;

namespace ELearning.Identity;

public static class IdentityModuleExtensions
{


    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<AppIdentityDbContext>(o =>
            o.UseNpgsql(
                config.GetConnectionString("Default"),
                s => s.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    AppIdentityDbContext.Schema)));

        services.AddDataProtection();

        services.AddIdentityCore<ApplicationUser>(o =>
        {
            o.User.RequireUniqueEmail = true;
            o.Password.RequiredLength = 8;
            o.Password.RequireDigit = true;
            o.Password.RequireUppercase = true;
            o.Password.RequireNonAlphanumeric = false;
            o.Lockout.MaxFailedAccessAttempts = 5;
            o.Lockout.DefaultLockoutTimeSpan =
                TimeSpan.FromMinutes(10);
        })
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

        // Register JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            config["Jwt:SecretKey"]!)),
                    
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly",p =>p.RequireRole(Roles.Admin));
            options.AddPolicy("InstructorOrAdmin",p =>p.RequireRole(Roles.Instructor,Roles.Admin));
            options.AddPolicy("StudentOnly",p =>p.RequireRole(Roles.Student));

        });

        services.AddScoped<AuthService>();
        services.AddScoped<TokenService>();
        services.AddScoped<UserService>();
        services.AddScoped<LeariningService>();
        // repositories
        services.AddScoped<UserRepository>();

        return services;
    }

}
