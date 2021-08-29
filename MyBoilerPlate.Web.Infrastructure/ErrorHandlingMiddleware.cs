using Core.Common.Exceptions;
using MyBoilerPlate.Business.Exceptions;
using MyBoilerPlate.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;

namespace MyBoilerPlate.Web.Infrastructure
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _Next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this._Next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _Next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            string messageCode = null;
            var message = exception.Message;

            var responseModel = new ApiErrorResponseModel();

            switch (exception)
            {
                case SecurityException _:
                    message = "Acceso no autorizado.";

                    code = HttpStatusCode.Unauthorized;
                    break;
                case AggregateException _:
                    if (!string.IsNullOrEmpty(exception.Message))
                    {
                        message = "Ocurrieron fallas multiples.";

                        code = HttpStatusCode.BadRequest;
                    }
                    break;
                case SecurityCustomException _:
                    if (!string.IsNullOrEmpty(exception.Message))
                    {
                        message = exception.Message;
                    }

                    code = HttpStatusCode.Unauthorized;
                    break;
                case ArgumentNullException _:
                    if (!string.IsNullOrEmpty(exception.Message))
                    {
                        message = exception.Message;
                    }
                    else
                    {
                        message = "Error en parámetros recibidos.";
                    }

                    code = HttpStatusCode.BadRequest;
                    break;
                case ModelValidationException _:
                    if (string.IsNullOrEmpty(exception.Message))
                    {
                        message = "Error en parámetros recibidos.";
                    }

                    responseModel.Errors = ((ModelValidationException)exception).Errors;

                    code = HttpStatusCode.BadRequest;
                    break;
                case System.InvalidOperationException _:
                    if (!string.IsNullOrEmpty(exception.Message))
                    {
                        message = exception.Message;
                    }
                    else
                    {
                        message = "Operación inválida.";
                    }

                    code = HttpStatusCode.BadRequest;
                    break;
                case ArgumentException _:
                    if (!string.IsNullOrEmpty(exception.Message))
                    {
                        message = exception.Message;
                    }
                    else
                    {
                        message = "Error en parámetros recibidos.";
                    }

                    code = HttpStatusCode.BadRequest;
                    break;
                case ValidationException _:
                    if (!string.IsNullOrEmpty(exception.Message))
                    {
                        message = exception.Message;
                    }
                    else
                    {
                        message = "Operación inválida.";
                    }

                    messageCode = ((ValidationException)exception).Code;
                    code = HttpStatusCode.BadRequest;
                    break;
                case NotFoundException _:
                    if (!string.IsNullOrEmpty(exception.Message))
                    {
                        message = exception.Message;
                    }

                    code = HttpStatusCode.NotFound;

                    Log.Error(exception, exception.Message, exception);
                    break;
                case ProfileNotFoundException _:
                    if (!string.IsNullOrEmpty(exception.Message))
                    {
                        message = exception.Message;
                    }

                    code = HttpStatusCode.BadRequest;

                    Log.Error(exception, exception.Message, exception);
                    break;
                case UriFormatException _:
                    message = "Formato de ruta inválido.";

                    code = HttpStatusCode.BadRequest;

                    Log.Error(exception, exception.Message, exception);
                    break;
                case HttpRequestException _:
                    message = "Error de comunicación.";

                    code = HttpStatusCode.BadRequest;

                    Log.Error(exception, exception.Message, exception);
                    break;
                case EntityValidationException _:
                    code = HttpStatusCode.InternalServerError;
                    break;
                case StorageException _:
                case DatabaseException _:
                    code = HttpStatusCode.InternalServerError;

                    Log.Error(exception, exception.Message, exception);
                    break;
                default:
                    Log.Error(exception, exception.Message, exception);
                    break;
            }

            responseModel.MessageCode = messageCode;
            responseModel.Message = message;

            var _errors = new Dictionary<string, IEnumerable<string>>();

            _errors.Add("errors", new List<string>() {
                message
            });            

            responseModel.Errors = _errors;

#if DEBUG
            Debug.WriteLine($"Message: {exception.Message}, StackTrace: {exception.StackTrace}");
#endif

            var result = JsonConvert.SerializeObject(responseModel);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
