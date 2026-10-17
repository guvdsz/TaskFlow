using TaskFlow.Api.Extensions;
using TaskFlow.Api.Interfaces.Project;
using TaskFlow.Api.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.AddDatabase();
builder.AddIdentityCore();
builder.AddJwtAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MigrateDatabase();

app.MapControllers();

app.Run();
