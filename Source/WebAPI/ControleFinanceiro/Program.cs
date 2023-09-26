using ControleFinanceiro.Configurations.Authorization;
using ControleFinanceiro.Configurations.Swagger;
using ControleFinanceiro.Domain.Core.Data;
using ControleFinanceiro.Infra.CrossCutting;
using ControleFinanceiro.Middleware;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Injecao de Dependecia
var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
builder.Services.AddSingleton(settings);
builder.Services.AddServices();
#endregion

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerSetup(settings.ApplicationName);
builder.Services.AddElmah();

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
builder.Services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });

builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseResponseCompression();
app.UsePathBase($"/{settings.ApplicationBasePath}");
app.UseSwaggerSetup();

if (builder.Configuration.GetValue("LogEnvVariables", false))
    Console.WriteLine(builder.Configuration.GetDebugView());

//segue pipeline se for alguma url interna (v1, v2..)
//desconsiderando as excecoes
app.UseWhen(context => context.Request.Path.Value!.StartsWith("/v") &&
                       !Array.Exists(settings.PathException, x => context.Request.Path.Value.Contains(x)),
                       appBuilder =>
                       {
                           appBuilder.UseLoggerMiddleware();
                           appBuilder.UseCorrelationContextMiddleware();
                       });

app.UseHttpsRedirection();

app.UseElmah();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();