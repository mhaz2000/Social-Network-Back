using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Social.Network.Activators.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationException exception)
            {
                await ConfigureResponse(context, HttpStatusCode.BadRequest, exception.Message,
                    (long)HttpStatusCode.BadRequest, "ValidationError");
            }
            catch (CommandValidationException exception)
            {
                await ConfigureResponse(context, HttpStatusCode.BadRequest, exception.Message);
            }

            catch (ManagedException exception)
            {
                await ConfigureResponse(context, HttpStatusCode.BadRequest, exception.Message);
            }
            catch (Exception exception)
            {
                await ConfigureResponse(context, HttpStatusCode.InternalServerError, "Internal Error, Contact with support team.");
                Console.WriteLine(exception.Message);
            }
        }

        private static async Task ConfigureResponse(HttpContext context, HttpStatusCode statusCode, string message,
            long? exceptionCode = null, string exceptionReason = "")
        {
            exceptionCode ??= (long)statusCode;
            exceptionReason = !string.IsNullOrEmpty(exceptionReason) ? exceptionReason : statusCode.ToString();

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                new FailedResponseMessage(message).ToString());
        }
    }
}
