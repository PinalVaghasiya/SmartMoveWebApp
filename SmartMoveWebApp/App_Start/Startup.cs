using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SmartMoveWebApp.BusniessLogic;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(SmartMoveWebApp.App_Start.Startup))]
namespace SmartMoveWebApp.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            var myProvider = new AuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                Provider = myProvider,
                RefreshTokenProvider = new RefreshTokenProvider()
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}