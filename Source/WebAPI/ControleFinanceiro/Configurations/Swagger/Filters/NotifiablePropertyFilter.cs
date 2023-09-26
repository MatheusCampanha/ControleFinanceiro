using ControleFinanceiro.Domain.Core.Notifications;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ControleFinanceiro.Configurations.Swagger.Filters
{
    public class NotifiablePropertyFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var type = typeof(Notifiable);
            var ignoredProperties = type.GetProperties().Select(x => x.Name);

            // find all properties by name which need to be removed
            // and not shown on the swagger sepc.
            operation.Parameters
                .Where(param => ignoredProperties.Any(exc => string.Equals(exc, param.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(prExclude => prExclude)
                .ToList()
                .ForEach(key => operation.Parameters.Remove(key));
        }
    }
}