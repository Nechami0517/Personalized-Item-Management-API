
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using project.Controllers;
using project.Interfaces;
using project.Models;
using project.Services;

namespace project.middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        var token = context.Request.Cookies["AuthToken"];

        // בדוק אם הטוקן קיים ואם הוא תקף
        if (string.IsNullOrEmpty(token) || !TokenService.IsTokenValid(token))
        {
            // בדוק אם הבקשה היא לדף הכניסה
            if (context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase))
            {
                // אם הבקשה היא לדף הכניסה, המשך לעבד את הבקשה
                await _next(context);
                return;
            }

            // אם הטוקן לא תקף, הפנה לדף הכניסה
           // context.Response.Redirect("/login.html");
            return;
        }
        else
        {
            // קריאה לפונקציה SaveToken

            var claims = TokenService.DecodeToken(token);
            if (claims == null)
            {
                context.Response.Redirect("/login.html");
                return;
            }

            var currentUser = serviceProvider.GetRequiredService<CurrentUserService>();
            currentUser.Id = int.Parse(claims.FindFirst(c => c.Type == "Id").Value);
            currentUser.Role = claims.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            currentUser.Name = claims.FindFirst(c => c.Type == ClaimTypes.Name).Value;

             
            
            
           
        }
        // המשך לעבד את הבקשה
        await _next(context);
    }
    
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder) 
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}

