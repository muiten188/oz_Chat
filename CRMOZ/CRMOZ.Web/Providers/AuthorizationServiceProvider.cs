using CRMOZ.Data;
using CRMOZ.Model.Models;
using CRMOZ.Web;
using CRMOZ.Web.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace WebApisTokenAuth
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); // 
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {

            // Change authentication ticket for refresh token requests  
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);

        }

        public static AuthenticationProperties CreateProperties(ApplicationUser user, bool isAdmin)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "username", user.UserName },
            };
            if (isAdmin)
            {
                data.Add("isAdmin", "true");
            }
            data.Add("userID", user.Id);
            return new AuthenticationProperties(data);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var userManager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(new OZChatDbContext()));

            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user,
                context.Options.AuthenticationType);

            ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user,
                CookieAuthenticationDefaults.AuthenticationType);
  
            //var oauthIdentity = await user.GenerateUserIdentityAsync(userManager);
            //var cookiesIdentity = await user.GenerateUserIdentityAsync(userManager);

            bool isAdmin = await userManager.IsInRoleAsync(user.Id, "Administrator");
            AuthenticationProperties properties = CreateProperties(user, isAdmin);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);


            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            ////Accounts acc = new Accounts();

            //AccountController accController = new AccountController();
            ////Authenticate the user credentials
            //if (true)//thêm hàm login cho mobile vào đây
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            //    identity.AddClaim(new Claim("username", context.UserName));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //    context.Validated(identity);
            //}
            //else
            //{
            //    context.SetError("invalid_grant", "Provided username and password is incorrect");
            //    return;
            //}
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }

    

    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {

            var guid = Guid.NewGuid().ToString();

            // copy all properties and set the desired lifetime of refresh token  
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
            };

            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            _refreshTokens.TryAdd(guid, refreshTokenTicket);

            // consider storing only the hash of the handle  
            context.SetToken(guid);
        }


        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            // context.DeserializeTicket(context.Token);
            AuthenticationTicket ticket;
            string header = context.OwinContext.Request.Headers["Authorization"];

            if (_refreshTokens.TryRemove(context.Token, out ticket))
            {
                context.SetTicket(ticket);
            }
        }
    }
}