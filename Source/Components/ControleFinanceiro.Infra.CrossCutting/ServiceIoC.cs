using ControleFinanceiro.Domain.Core.Interfaces;
using ControleFinanceiro.Domain.Core.Logger.Interfaces;
using ControleFinanceiro.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ControleFinanceiro.Infra.CrossCutting
{
    public static class ServiceIoC
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region Repository

            services.AddScoped<IElmahRepository, ElmahRepository>();
            services.AddScoped<ILogRequestResponseRepository, LogRequestResponseRepository>();

            #endregion Repository

            return services;
        }
    }
}