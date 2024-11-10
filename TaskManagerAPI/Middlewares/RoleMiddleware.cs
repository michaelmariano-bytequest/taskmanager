using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerAPI.Middlewares
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _requiredRole;
        private readonly string[] _ignoredPaths = { "/swagger", "/api-docs" }; // Rotas ignoradas

        public RoleMiddleware(RequestDelegate next, string requiredRole)
        {
            _next = next;
            _requiredRole = requiredRole;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Ignora o middleware para as rotas especificadas
            if (_ignoredPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
            {
                await _next(context);
                return;
            }

            // Obtém o token JWT do cabeçalho Authorization
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                // Extrai as claims do token JWT
                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                var roles = jwtToken?.Claims.Where(c => c.Type == "role").Select(c => c.Value);

                // Verifica se o usuário possui a função necessária
                if (roles != null && roles.Contains(_requiredRole))
                {
                    await _next(context); // Permite a continuação da requisição
                    return;
                }
            }

            // Retorna Forbidden se o usuário não tiver a função necessária
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access denied. You do not have the required role.");
        }
    }
}