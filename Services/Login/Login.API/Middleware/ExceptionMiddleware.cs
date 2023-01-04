using Login.API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Login.API.Middleware
{
    public class ExceptionMiddleware
    {

        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        /**
         * This piece of middleware is in the top of our middlewares, every middleware below
         * this will throw the exception up until the exception come to a middleware that can 
         * handle the exception. Our ExceptionMiddleware class is on the top of the middlewares 
         * tree.
         * */

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            //here we catch the exception - top of the middlewares tree
            catch(Exception ex)
            {
                
                _logger.LogError(ex,ex.Message);

                //The access to the HttpContext is for writing the response to it
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //ex.StackTrace? the question mark checks if it is null
                //in production we dont provide stack trace to the client, just a message about an error
                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");


                var options = new JsonSerializerOptions
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }

    }
}
