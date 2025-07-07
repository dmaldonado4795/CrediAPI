using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(CrediAPI.Startup))]
namespace CrediAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            UnityConfig.RegisterComponents(config);

            var jwtSecret = ConfigurationManager.AppSettings["JwtSecret"] ?? "ByrSc,XSWYqWknc3Md;z1w[MJqGehCCZ!a+S/:vY]f.6A3KNx%.t-=#?";
            var issuer = ConfigurationManager.AppSettings["JwtIssuer"] ?? "http://credi-api.com";
            var audience = ConfigurationManager.AppSettings["JwtAudience"] ?? "CrediWinFormsApp";

            int jwtExpiresInMinutes;
            if (!int.TryParse(ConfigurationManager.AppSettings["JwtExpiresInMinutes"], out jwtExpiresInMinutes))
            {
                jwtExpiresInMinutes = 60;
            }

            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JwtSecret key is missing or empty in Web.config AppSettings.");
            }
            if (Encoding.UTF8.GetBytes(jwtSecret).Length < 32)
            {
                throw new InvalidOperationException("JwtSecret must be at least 32 bytes long for SHA256.");
            }

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }
                });

            app.UseWebApi(config);
        }
    }
}