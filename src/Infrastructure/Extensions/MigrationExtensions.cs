using Application.Abstractions.Data;
using Infrastructure.Databases.ApplicationDbContext;
using Infrastructure.Databases.MTDbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db1 = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db1.Database.MigrateAsync();

        var db2 = scope.ServiceProvider.GetRequiredService<RegistrationDbContext>();
        await db2.Database.MigrateAsync();
    }

}



//  Add-Migration InitialWrite -Context AppDbContext -OutputDir "Infrastructure/Persistence/Contexts/ApplicationDbContext/Migrations"
//  Add-Migration InitialWrite -Context RegistrationDbContext -OutputDir "Infrastructure/Persistence/Contexts/MTDbContext/Migrations"
// Update-Database -Context AppDbContext
// Update-Database -Context RegistrationDbContext

//# AppDbContext (Write) – gerar em Application/Infrastructure/Persistence/Migrations
//dotnet ef migrations add InitialWrite -c AppDbContext \
//  -p src/Application/Application.csproj \
//  -s src/YourApp.Api/YourApp.Api.csproj \
//  -o Infrastructure/Persistence/Migrations

//# Se tiver outra para a Saga, crie uma subpasta separada:
//dotnet ef migrations add InitialSaga -c RegistrationDbContext \
//  -p src/Application/Application.csproj \
//  -s src/YourApp.Api/YourApp.Api.csproj \
//  -o Infrastructure/Persistence/Migrations/Saga

