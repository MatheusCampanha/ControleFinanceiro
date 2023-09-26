using ControleFinanceiro.Domain.Core.Data;
using ControleFinanceiro.Domain.Core.Interfaces;
using ControleFinanceiro.Domain.Core.Notifications;
using System.Net;
using System.Text.Json;

namespace ControleFinanceiro.Middleware
{
    public class CorrelationContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Settings _settings;

        public CorrelationContextMiddleware(RequestDelegate next, Settings settings)
        {
            _next = next;
            _settings = settings;
        }

        public async Task Invoke(HttpContext httpContext, ICorrelationContext correlationContext)
        {
            var correlationId = httpContext.Request.Headers[_settings.CorrelationDefaultName].ToString();

            if (string.IsNullOrEmpty(correlationId))
            {
                //se nao existir cria
                correlationId = Guid.NewGuid().ToString();
            }
            else if (!Guid.TryParse(correlationId, out var guidCorrelationId) || guidCorrelationId == Guid.Empty)
            {
                //tratando resposta caso o correlationId seja invalido (nao seja um guid)
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;

                correlationId = null;

                var notificacoes = new List<Notificacao>() { new Notificacao(_settings.CorrelationDefaultName, "inválido") };
                var retorno = JsonSerializer.Serialize(new
                {
                    Notifications = notificacoes,
                    Invalid = true,
                    Valid = false
                });

                await httpContext.Response.WriteAsync(retorno);
            }

            correlationContext.CorrelationId = new Guid(correlationId!);
            httpContext.Response.Headers.Add(_settings.CorrelationDefaultName, correlationId);

            await _next(httpContext);
        }
    }

    public static class CorrelationContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationContextMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationContextMiddleware>();
            return app;
        }
    }
}