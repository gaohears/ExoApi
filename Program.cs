using ExoApi.Contexts;
using Microsoft.EntityFrameworkCore;
using ExoApi.Interfaces;
using ExoApi.Repositories;
using ExoApi.Middlewares;
using ExoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using ExoApi.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ExoApi.Swagger;

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
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };

        // Resposta 401 no mesmo formato padrão (ErroResposta) da Atividade 3
        options.Events = new JwtBearerEvents
        {
            OnChallenge = contexto =>
            {
                contexto.HandleResponse();
                return ExceptionHandlingMiddleware.EscreverAsync(
                    contexto.HttpContext,
                    StatusCodes.Status401Unauthorized,
                    "Acesso negado. Informe um token JWT válido.");
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ExoApi",
        Version = "v1",
        Description = "API para gerenciamento de projetos da ExoApi."
    });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ExoApi.xml"));
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Informe apenas o token JWT obtido em /api/login."
    });

    c.OperationFilter<AuthorizeOperationFilter>();
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
