using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CafeVirtual.Pruebas.API.Services.Interfaces;
using CafeVirtual.Pruebas.API.Services.Settings;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Services.Services
{
    public class JwtService: IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(TblUsuario usuario)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.ApellidoPaterno} {usuario.ApellidoMaterno}"),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.IdRolNavigation?.Nombre ?? "SinRol"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }        
    }
}
