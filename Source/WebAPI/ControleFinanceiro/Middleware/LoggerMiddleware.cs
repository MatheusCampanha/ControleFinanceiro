using ControleFinanceiro.Domain.Core.Data;
using ControleFinanceiro.Domain.Core.Interfaces;
using ControleFinanceiro.Domain.Core.Logger;
using ControleFinanceiro.Domain.Core.Logger.Interfaces;
using ElmahCore;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ControleFinanceiro.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Settings _settings;

        public LoggerMiddleware(RequestDelegate next, Settings settings)
        {
            _next = next;
            _settings = settings;
        }

        public async Task Invoke(HttpContext httpContext, IElmahRepository elmahLogger, ILogRequestResponseRepository logRequestResponseRepository)
        {
            DateTime dataEnvio = DateTime.Now;

            var request = httpContext.Request;
            var requestAsText = FormatRequest(request, _settings.LogRequestHeaders);

            try
            {
                var originalBodyStream = httpContext.Request.Body;

                using var responseBody = new MemoryStream();
                httpContext.Response.Body = responseBody;

                await _next(httpContext);

                var responseBodyAsText = await FormatResponse(httpContext.Response);
                await responseBody.CopyToAsync(originalBodyStream);

                SaveLog(httpContext, logRequestResponseRepository, requestAsText, dataEnvio, responseBodyAsText, (long)DateTime.Now.Subtract(dataEnvio).TotalMilliseconds, null);
            }
            catch (Exception e)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorId = elmahLogger.LogError(new Error(e, httpContext));

                SaveLog(httpContext, logRequestResponseRepository, requestAsText, dataEnvio, "Internal Server Error", (long)DateTime.Now.Subtract(dataEnvio).TotalMilliseconds, errorId);
            }
        }

        private void SaveLog(HttpContext httpContext, ILogRequestResponseRepository logRequestResponseRepository, string request, DateTime dataEnvio, string? response, long elapsedTime, string? errorId)
        {
            try
            {
                var logRequestResponse = new LogRequestResponse(Environment.MachineName, dataEnvio, DateTime.Now, httpContext.Request.Path, httpContext.Request.Method, httpContext.Response.StatusCode, request, response, errorId, httpContext.Response.Headers[_settings.CorrelationDefaultName], elapsedTime);

                if (_settings.ConsoleLog)
                    Console.WriteLine(JsonSerializer.Serialize(logRequestResponse));

                logRequestResponseRepository.Inserir(logRequestResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static string FormatRequest(HttpRequest request, string[] logRequestHeaders)
        {
            #region Read Body

            var requestBody = new StreamReader(request.BodyReader.AsStream()).ReadToEnd();
            byte[] content = Encoding.UTF8.GetBytes(requestBody.Replace("string", "NoString"));
            var bodyAsText = Encoding.Default.GetString(content);

            //Retorno body
            var requestBodyStream = new MemoryStream();
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            requestBodyStream.Write(content, 0, content.Length);
            request.Body = requestBodyStream;
            request.Body.Seek(0, SeekOrigin.Begin);

            #endregion Read Body

            return $"Http Request Information: {Environment.NewLine}" +
                    $"Schema:{request.Scheme} {Environment.NewLine}" +
                    $"Host:{request.Host} {Environment.NewLine}" +
                    $"Path:{request.Path} {Environment.NewLine}" +
                    $"Header:{ObterHeaders(request.Headers, logRequestHeaders)} {Environment.NewLine}" +
                    $"QueryString:{request.QueryString} {Environment.NewLine}" +
                    $"Request Body:{bodyAsText}";
        }

        private static async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private static string ObterHeaders(IHeaderDictionary headers, string[] logRequestHeaders)
        {
            var list = new List<string>();
            foreach (var key in logRequestHeaders)
            {
                if (!string.IsNullOrEmpty(headers[key]))
                    list.Add($"{key} = {headers[key]}");
            }

            return string.Join(',', list);
        }
    }

    public static class LoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggerMiddleware>();
            return app;
        }
    }
}