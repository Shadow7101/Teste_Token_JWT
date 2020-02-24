using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace FDE.Api
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            try
            {
                var user = context.UserName;
                var password = context.Password;

                if (user != "user721" && password != "pswd722")
                {
                    context.SetError("invalid_grant", "Usuário ou senha inválidos!");
                    return;
                }
                var roles = new string[] { "User", "Admin" };
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user));
                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                GenericPrincipal principal = new GenericPrincipal(identity, roles);
                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
            }
        }
    }
}