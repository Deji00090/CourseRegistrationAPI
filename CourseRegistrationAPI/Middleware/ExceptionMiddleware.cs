using Serilog;
using System.ComponentModel;
using System.Net;
using System.Text.Json;

namespace CourseRegistrationAPI.Middleware
{
    public class ExceptionMiddleware
    {
        //global error hadling with middleware
        private readonly RequestDelegate _next; // a function that can process http request

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "An unhandled  exception occurred.");
                await HandleExceptionsAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionsAsync(HttpContext context,Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                status = context.Response.StatusCode,
                message = "An excepted error occurred.Please try again later."
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
