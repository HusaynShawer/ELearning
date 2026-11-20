using ELearning.Catalog;
using ELearning.Enrollment;
using ELearning.Identity;
using ELearning.Identity.Domain;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddEnrollmentModule(builder.Configuration);


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<ApplicationRole>>();

    await RoleSeeder.SeedAsync(roleManager);
}
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
