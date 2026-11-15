using ExoApi.Contexts;
using Microsoft.EntityFrameworkCore;
using ExoApi.Interfaces;
using ExoApi.Repositories;
using ExoApi.Middlewares;
using ExoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = contexto =>
        {
            var detalhes = contexto.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return new BadRequestObjectResult(
                new ErroResposta(StatusCodes.Status400BadRequest, "Dados inválidos.", detalhes));
        };
    });
builder.Services.AddDbContext<ExoApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ExoApi",
        Version = "v1",
        Description = "API para gerenciamento de projetos da ExoApi."
    });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ExoApi.xml"));
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
