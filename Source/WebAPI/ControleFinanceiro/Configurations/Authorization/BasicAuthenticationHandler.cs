using ControleFinanceiro.Domain.Core.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ControleFinanceiro.Configurations.Authorization
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly Settings _settings;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            Settings settings) : base(options, logger, encoder, clock)
        {
            _settings = settings;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Path.HasValue && Array.Exists(_settings.PathException, x => Request.Path.Value.Contains(x)))
                return await Task.FromResult(AuthenticateResult.Success(GetAuthenticationTicket()));

            if (!Request.Headers.ContainsKey(nameof(_settings.Token)))
                return await Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse($"Basic {Request.Headers[nameof(_settings.Token)]}");
                if (authHeader.Parameter != _settings.Token)
                    return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
            catch
            {
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }

            return await Task.FromResult(AuthenticateResult.Success(GetAuthenticationTicket()));
        }

        private AuthenticationTicket GetAuthenticationTicket()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, ""),
                new Claim(ClaimTypes.Name, "")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationTicket(principal, Scheme.Name);
        }
    }
}