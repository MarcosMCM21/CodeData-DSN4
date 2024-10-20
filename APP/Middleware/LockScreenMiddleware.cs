using System.IO;
using System.Text.Encodings.Web;

namespace CodeData_Connection.Middleware
{
    public class LockScreenMiddleware : IMiddleware
    {
        public LockScreenMiddleware() {}

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Verificar se a sessão está marcada como "bloqueada"
            var isLocked = context.Session.GetString("IsLocked");

            if (isLocked == "true")
            {
                // Evitar loop infinito se já estiver na tela de LockScreen
                if (!context.Request.Path.Value.Contains("/Account/LockScreen") && !context.Request.Path.Value.Contains("/Identity/Account/Login") && !context.Request.Path.Value.Contains("/Identity/Account/Register"))
                {
                   
                    context.Response.Redirect($"/Account/LockScreen?returnUrl={UrlEncoder.Default.Encode(context.Request.Path.Value)}");
                }
            }

            return next(context);
        }
    }

    public static class LockScreenMiddlewareExtensions
    {
        public static IApplicationBuilder UseLockScreenMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LockScreenMiddleware>();
        }
    }
}
