using RepositoryPattern.Api.Extensions;
using RepositoryPattern.Api.Middleware;
using RepositoryPattern.Domain.Interfaces;
using RepositoryPattern.Domain.Services;
using RepositoryPattern.Repository.Interfaces;
using RepositoryPattern.Repository.Repositories;
using YRepositoryPattern.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ?? Modular service registration
builder.Services.AddSwaggerModule();
builder.Services.AddCosmosDbModule(builder.Configuration);
builder.Services.AddJwtAuthModule(builder.Configuration);

// ?? App-specific services
builder.Services.AddSingleton<IGuitarDetails, GuitarDetails>();
builder.Services.AddSingleton<IGuitarService, GuitarService>();

builder.Services.AddControllers();

var app = builder.Build();

// ?? Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();