using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace testPumox
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                var decodedUsernamePassword = string.Empty;
                try
                {
                    decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                }
                catch (FormatException exception)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    // Below message is well informing, but I have no idea if this meet accepted standards.
                    await context.Response.WriteAsync($"Value of 'Authorization' cookie is malformed. ({exception.Message})");
                    return;
                }
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];
                if (IsAuthorized(username, password))
                {
                    await _next.Invoke(context);
                    return;
                }
            }
            // Allow unauthorized search.
            if (context.Request.Path.Value.Contains("/company/search"))
            {
                await _next.Invoke(context);
                return;
            }

            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        private bool IsAuthorized(string username, string password)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var basicAuthUserName = config["BasicAuth:UserName"];
            var basicAuthPassword = config["BasicAuth:Password"];
            return username.Equals(basicAuthUserName, StringComparison.InvariantCultureIgnoreCase)
                   && password.Equals(basicAuthPassword);
        }
    }
}
