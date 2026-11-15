using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExoApi.Swagger;

// Marca com cadeado (e envia o token) apenas os endpoints que têm [Authorize]
public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var metadados = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        var exigeAutenticacao = metadados.Any(m => m is AuthorizeAttribute);
        var permiteAnonimo = metadados.Any(m => m is AllowAnonymousAttribute);

        if (!exigeAutenticacao || permiteAnonimo)
        {
            return;
        }

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
            }
        };
    }
}