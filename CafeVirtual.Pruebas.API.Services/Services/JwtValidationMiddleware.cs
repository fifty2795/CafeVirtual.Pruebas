using Microsoft.AspNetCore.Http;
using CafeVirtual.Pruebas.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Services.Services
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Middleware para hacer validaciones en escenarios que lo requieran
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jwtService"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                var principal = jwtService.ValidateToken(token);
                if (principal != null)
                    context.User = principal;
            }

            await _next(context);
        }
    }
}
