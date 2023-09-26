using ControleFinanceiro.Configurations.Swagger.Filters;
using ControleFinanceiro.Configurations.Swagger.HeaderParameters;
using Microsoft.OpenApi.Models;

namespace ControleFinanceiro.Configurations.Swagger
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services, string applicationName)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = applicationName });

                options.OperationFilter<AuthenticationHeaderParameter>();
                options.OperationFilter<CorrelationIdHeaderParameter>();
                options.OperationFilter<NotifiablePropertyFilter>();
            });
        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            if (app is null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(sw =>
            {
                sw.SwaggerEndpoint("../swagger/v1/swagger.json", "v1");
            });
        }
    }
}