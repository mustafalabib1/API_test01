using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace test01.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Check if the Authorization header is present
            if (!Request.Headers.ContainsKey("Authentication"))
            {
                // If not, return no result
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            // Check if the header starts with "Basic "
            if(!AuthenticationHeaderValue.TryParse(authorizationHeader, out var authHeaderValue))
            {
                // If not, return no result
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            if(authHeaderValue.Scheme != "Basic")
            {
                // If the scheme is not Basic, return no result
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            //if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            //{
            //    // If not, return no result
            //    return Task.FromResult(AuthenticateResult.Fail("Unknow Scheme"));
            //}
            // Extract the base64 encoded credentials
            //var base64Credentials = authorizationHeader.Substring("Basic ".Length);
            var base64Credentials = authHeaderValue.Parameter;
            // Decode the base64 credentials
            var credentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Credentials));
            var Username = credentials.Split(':')[0];
            var Password = credentials.Split(':')[1];
            // Validate the credentials (this is just an example, you should use a proper user store)
            if(Username == "admine" && Password == "123")
            {
                // If the credentials are valid, create a ClaimsPrincipal
                var claims = new[] { new Claim(ClaimTypes.Name, "1"),new Claim(ClaimTypes.Name, Username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
