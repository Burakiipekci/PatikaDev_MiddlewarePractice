using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiddlewarePractice.Middlewares
{
    public class Middleware
    {
        RequestDelegate _next;
        public Middleware(RequestDelegate next)
        {
            _next= next;
        }
        public async Task Invoke (HttpContext context)
        {
             System.Console.WriteLine("Hello Middleware.");
            await _next.Invoke(context);
            System.Console.WriteLine("By Middleware.");
        }
    }
    static public class MiddlewareExtension
    {
        public static IApplicationBuilder UseMiddlewareTest(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Middleware>();
               
        }
    }
}
