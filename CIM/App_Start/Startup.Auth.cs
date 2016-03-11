using System;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using CIM.Model.Models.DataContexts;
using CIM.Model.Models.Login;
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
using TripGallery.MVCClient.Helpers;

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
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            /*    
                 app.CreatePerOwinContext(ApplicationDbContext.Create);
                 app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
                 app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

                 // Enable the application to use a cookie to store information for the signed in user
                 // and to use a cookie to temporarily store information about a user logging in with a third party login provider
                 // Configure the sign in cookie
                 app.UseCookieAuthentication(new CookieAuthenticationOptions
                 {
                     AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                     LoginPath = new PathString("/Account/Login"),
                     Provider = new CookieAuthenticationProvider
                     {
                         // Enables the application to validate the security stamp when the user logs in.
                         // This is a security feature which is used when you change a password or add an external login to your account.  
                         OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, CustomerUser>(
                             validateInterval: TimeSpan.FromMinutes(30),
                             regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                     }
                 });

         */
            OpenIdConnectAuthenticationOptions options = new OpenIdConnectAuthenticationOptions
            {
                // These are standard OpenID Connect parameters, with values pulled from web.config
                ClientId = clientId,
                RedirectUri = redirectUri,
                PostLogoutRedirectUri = redirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = AuthenticationFailed,
                    RedirectToIdentityProvider = OnRedirectToIdentityProvider,
                    MessageReceived = message =>
                    {
                        IdToken = message.ProtocolMessage.IdToken;
                        
                        return Task.FromResult(IdToken);
                    },
              //      SecurityTokenValidated = SecurityTokenValidated,
              //      SecurityTokenReceived = SecurityTokenReceived,
              //      AuthorizationCodeReceived = AuthorizationCodeReceived
                },
                Scope = "openid",
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
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            /*
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            */
        }

        private Task AuthorizationCodeReceived(AuthorizationCodeReceivedNotification authorizationCodeReceivedNotification) 
        {
            throw new NotImplementedException();
        }

        private Task MessageReceived(MessageReceivedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> messageReceivedNotification)
        {
            throw new NotImplementedException();
        }

        private Task SecurityTokenReceived(SecurityTokenReceivedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> securityTokenReceivedNotification)
        {
            throw new NotImplementedException();
        }

        private Task SecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> securityTokenValidatedNotification)
        {
            var token = TokenHelper.DecodeAndWrite(securityTokenValidatedNotification.ProtocolMessage.IdToken);
            
            return Task.CompletedTask;
        }

        // This notification can be used to manipulate the OIDC request before it is sent.  Here we use it to send the correct policy.
        private async Task OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            PolicyConfigurationManager mgr = notification.Options.ConfigurationManager as PolicyConfigurationManager;
            if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
            {
                OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, notification.OwinContext.Authentication.AuthenticationResponseRevoke.Properties.Dictionary[Startup.PolicyKey]);
                notification.ProtocolMessage.IssuerAddress = config.EndSessionEndpoint;
            }
            else
            {
                OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, SignInPolicyId);//notification.OwinContext.Authentication.AuthenticationResponseChallenge.Properties.Dictionary[SignInPolicyId]);
                notification.ProtocolMessage.IssuerAddress = config.AuthorizationEndpoint;
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