using CrossCutting.Models;
using CrossCutting.Response;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="_repo">The repo.</param>
        public async Task Invoke(HttpContext context, IRepositoryService _repo /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the exception asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public Task HandleExceptionAsync(HttpContext context, HttpStatusCodeException exception)
        {
            if (exception is HttpStatusCodeException)
            {
                var response = new ResponseDetails()
                {
                    Result = null,
                    Errored = true,
                    ErrorMessage = exception.Message,
                };

                var result = JsonConvert.SerializeObject(response);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)exception.StatusCode;
                return context.Response.WriteAsync(result);
            }
            else
            {
                var response = new ResponseDetails()
                {
                    Result = null,
                    Errored = true,
                    ErrorMessage = "Runtime Error"
                };
                var result = JsonConvert.SerializeObject(response);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(result);
            }
        }

        /// <summary>
        /// Handles the exception asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
           
            var response = new ResponseDetails
            {
                Result = null,
                Errored = true,
                ErrorMessage = ex.Message
            };

            var result = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}