using ControleFinanceiro.Domain.Core.Data;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ControleFinanceiro.Configurations.Swagger.HeaderParameters
{
    public class CorrelationIdHeaderParameter : IOperationFilter
    {
        private readonly Settings _settings;

        public CorrelationIdHeaderParameter(Settings settings)
        {
            _settings = settings;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = _settings.CorrelationDefaultName,
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "string" },
                Required = false
            });
        }
    }
}
