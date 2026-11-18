using ELearning.Catalog;
using ELearning.Enrollment;
using ELearning.Identity;
using ELearning.Identity.Domain;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddEnrollmentModule(builder.Configuration);


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<ApplicationRole>>();

    await RoleSeeder.SeedAsync(roleManager);
}

app.MapGet("/", () => "Hello World!");

app.Run();
