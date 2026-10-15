using System;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;

namespace TaskFlow.Api.Extensions;

public static class DataExtensions
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionsString = builder.Configuration.GetConnectionString("Default");

        if (String.IsNullOrEmpty(connectionsString))
        {
            throw new Exception("The database connection string is missing");
        }

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionsString);
        });
    }

    public static void MigrateDatabase(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
