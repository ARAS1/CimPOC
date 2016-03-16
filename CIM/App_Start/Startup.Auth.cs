﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CIM.PolicyAuthHelpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace CIM
{
    public partial class Startup
    {

        // The ACR claim is used to indicate which policy was executed
        public const string AcrClaimType = "http://schemas.microsoft.com/claims/authnclassreference";
        public const string PolicyKey = "b2cpolicy";
        public const string OIDCMetadataSuffix = "/.well-known/openid-configuration";

        // App config settings
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        // B2C policy identifiers
        public static string SignUpPolicyId = ConfigurationManager.AppSettings["ida:SignUpPolicyId"];
        public static string SignInPolicyId = ConfigurationManager.AppSettings["ida:SignInPolicyId"];
        public static string ProfilePolicyId = ConfigurationManager.AppSettings["ida:UserProfilePolicyId"];

        public string IdToken;
        public OwinContext Context = new OwinContext();
        HttpContext HttpContext = HttpContext.Current;
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            OpenIdConnectAuthenticationOptions options = new OpenIdConnectAuthenticationOptions
            {
                // These are standard OpenID Connect parameters, with values pulled from web.config
                ClientId = clientId,
                RedirectUri = redirectUri,
                PostLogoutRedirectUri = redirectUri,
                SignInAsAuthenticationType = "Cookies",
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = AuthenticationFailed,
                    RedirectToIdentityProvider = OnRedirectToIdentityProvider,
                    SecurityTokenValidated = message =>
                      {
                          IdToken = message.ProtocolMessage.IdToken;
                          var newJwt = new JwtSecurityTokenHandler().ReadToken(IdToken) as JwtSecurityToken;
                          ClaimsIdentity claimsIdentity = new ClaimsIdentity();                          
                          claimsIdentity.AddClaims(newJwt.Claims);
                          message.OwinContext.Response.Context.Authentication.AuthenticationResponseGrant = new AuthenticationResponseGrant(claimsIdentity, new AuthenticationProperties());
                          message.OwinContext.Response.Context.Authentication.User = new ClaimsPrincipal(claimsIdentity);
                     //     message.AuthenticationTicket = new AuthenticationTicket(claimsIdentity, new AuthenticationProperties() {RedirectUri = "/Home/Claims/"});
                          return Task.FromResult(newJwt);
                      }
                },
                Scope = "openid profile address",
                ResponseType = "id_token",

                // The PolicyConfigurationManager takes care of getting the correct Azure AD authentication
                // endpoints from the OpenID Connect metadata endpoint.  It is included in the PolicyAuthHelpers folder.
                // The first parameter is the metadata URL of your B2C directory
                // The second parameter is an array of the policies that your app will use.
                ConfigurationManager = new PolicyConfigurationManager(
                    String.Format(CultureInfo.InvariantCulture, aadInstance, tenant, "/v2.0", OIDCMetadataSuffix),
                    new string[] { SignUpPolicyId, SignInPolicyId, ProfilePolicyId }),

                // This piece is optional - it is used for displaying the user's name in the navigation bar.
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                },
            };

            app.UseOpenIdConnectAuthentication(options);
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions() {AuthenticationType = "Cookie"});
        }

        // This notification can be used to manipulate the OIDC request before it is sent.  Here we use it to send the correct policy.
        private async Task OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            PolicyConfigurationManager mgr = notification.Options.ConfigurationManager as PolicyConfigurationManager;
            if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
            {
                if (notification.Request.Path.Value.ToLower().Contains("signup"))
                {
                    OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, "B2C_1_TestPolicy");
                    notification.ProtocolMessage.IssuerAddress = config.EndSessionEndpoint;
                }
                else if (notification.Request.Path.Value.ToLower().Contains("signin"))
                {
                    OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, "B2C_1_signInTestPolicy");
                    notification.ProtocolMessage.IssuerAddress = config.EndSessionEndpoint;
                }
                else if (notification.Request.Path.Value.ToLower().Contains("profile"))
                {
                    OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, "B2C_1_profileEditingTestPolicy");
                    notification.ProtocolMessage.IssuerAddress = config.EndSessionEndpoint;
                }
            }
                else
                {
                    if (notification.Request.Path.Value.ToLower().Contains("signup"))
                    {
                        OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, "B2C_1_TestPolicy");
                        notification.ProtocolMessage.IssuerAddress = config.AuthorizationEndpoint;
                    }
                    else if (notification.Request.Path.Value.ToLower().Contains("signin"))
                    {
                        OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, "B2C_1_signInTestPolicy");
                        notification.ProtocolMessage.IssuerAddress = config.AuthorizationEndpoint;
                    }
                    else if (notification.Request.Path.Value.ToLower().Contains("profile"))
                    {
                        OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, "B2C_1_profileEditingTestPolicy");
                        notification.ProtocolMessage.IssuerAddress = config.AuthorizationEndpoint;
                    }
                }
        }

        // Used for avoiding yellow-screen-of-death
        private Task AuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();
            notification.Response.Redirect("/Home/Error?message=" + notification.Exception.Message);
            return Task.FromResult(0);
        }
    }
}