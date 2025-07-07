using CrediAPI.Common.Helpers;
using CrediAPI.Domain.Models;
using CrediAPI.Web.DTOs.Response;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrediAPI.Domain.Services.Impl
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly string _jwtSecret;
        private readonly int _jwtExpiresInMinutes;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthServiceImpl()
        {
            _jwtSecret = System.Configuration.ConfigurationManager.AppSettings["JwtSecret"] ?? "ByrSc,XSWYqWknc3Md;z1w[MJqGehCCZ!a+S/:vY]f.6A3KNx%.t-=#?";
            _jwtExpiresInMinutes = int.Parse(System.Configuration.ConfigurationManager.AppSettings["JwtExpiresInMinutes"] ?? "60");
            _issuer = System.Configuration.ConfigurationManager.AppSettings["JwtIssuer"] ?? "http://credi-api.com";
            _audience = System.Configuration.ConfigurationManager.AppSettings["JwtAudience"] ?? "CrediWinFormsApp";
            if (Encoding.UTF8.GetBytes(_jwtSecret).Length < 32)
            {
                throw new InvalidOperationException("JwtSecret must be at least 32 bytes long for SHA256.");
            }
        }

        public LoginResponse Authenticate(UsuarioModel usuario, string password)
        {
            bool isPasswordValid = PasswordHasherHelper.VerifyPassword(password, usuario.PasswordHash, usuario.PasswordSalt);

            if (!isPasswordValid)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username)
            });

            var param = DateTime.UtcNow.AddMinutes(_jwtExpiresInMinutes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpiresInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _issuer,
                Audience = _audience,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string encodedToken = tokenHandler.WriteToken(token);

            return new LoginResponse
            {
                AccessToken = encodedToken,
                TokenType = "Bearer",
                ExpiresInMinutes = _jwtExpiresInMinutes * 60
            };
        }
    }
}