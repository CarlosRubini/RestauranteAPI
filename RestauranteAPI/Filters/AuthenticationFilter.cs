using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Net.Http.Formatting;

namespace Api.Filters
{
    public class AuthenticationFilter : IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        private bool IsAuthorized(HttpRequestMessage request)
        {
            string token = GetToken();
            if (request.Headers.Authorization?.Parameter == token) return true;
            NameValueCollection values = request.RequestUri.ParseQueryString();
            if (values.Get("token") == token) return true;
            return false;
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (IsAuthorized(context.Request))
            {
                ClaimsIdentity identity = new ClaimsIdentity(AuthenticationTypes.Basic);
                identity.AddClaim(new Claim(ClaimTypes.Name, "API RESTAURANTE"));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "API.RESTAURANTE"));
                identity.AddClaim(new Claim(ClaimTypes.Role, "USER"));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                context.Principal = principal;
                return Task.CompletedTask;
            }
            context.ErrorResult = new AuthenticationFailureResult("Acesso negado", context.Request);
            return Task.CompletedTask;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private static string GetToken()
        {
            return ConfigurationManager.AppSettings["TOKEN"];
        }
    }
}