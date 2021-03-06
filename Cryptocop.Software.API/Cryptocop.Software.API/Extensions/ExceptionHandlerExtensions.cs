using System.Net;
using Cryptocop.Software.API.Models.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Cryptocop.Software.API.Models.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error => {
                error.Run(async ctx => {
                    var exceptionHandlerFeature = ctx.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionHandlerFeature.Error;
                    
                    // Set the default status code
                    var statusCode = (int) HttpStatusCode.InternalServerError;

                    if (exception is NotFoundException)
                    {
                        statusCode = (int) HttpStatusCode.NotFound;
                    }
                    else if (exception is BadRequestException)
                    {
                        statusCode = (int) HttpStatusCode.BadRequest;
                    }
                    else if (exception is ConflictException)
                    {
                        statusCode = (int) HttpStatusCode.Conflict;
                    }

                    // Setup context
                    ctx.Response.ContentType = "application/json";
                    ctx.Response.StatusCode = statusCode;

                    await ctx.Response.WriteAsync("");
                });
            });
        }
    }
}