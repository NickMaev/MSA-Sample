using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Shared.Exceptions;
using Shared.Models;

namespace PublicApi.App.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, logger, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, ILogger log, Exception exception)
        {
            Error errorDto = null;

            switch (exception)
            {
                case ValidationException e:
                    errorDto = new Error(
                        HttpStatusCode.BadRequest, 
                        e.Errors.Select(x => x.ErrorMessage).ToArray()
                        );
                    break;
                case EntityNotFoundException e:
                    errorDto = new Error(HttpStatusCode.NotFound, e.Message);
                    break;
            }

            if(errorDto != null)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)errorDto.StatusCode;

                return context.Response.WriteAsync(JsonSerializer.Serialize(errorDto));
            }
            else
            {
                log.LogError(exception, "An unhandled exception has occurred during HTTP request.");
            }

            return Task.CompletedTask;
        }
    }
}