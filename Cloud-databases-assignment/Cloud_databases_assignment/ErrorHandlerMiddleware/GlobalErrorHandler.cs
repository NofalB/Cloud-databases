using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Cloud_databases_assignment.Utils;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Cloud_databases_assignment.ErrorHandlerMiddleware
{
    public class GlobalErrorHandler : IFunctionsWorkerMiddleware
    {
        ILogger Logger { get; }
        public GlobalErrorHandler(ILogger<GlobalErrorHandler> Logger)
        {
            this.Logger = Logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error found in endpoint {context.FunctionDefinition.Name}: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(FunctionContext context, Exception exception)
        {
            var req = context.GetHttpRequestData();
            
            HttpResponseData response = req.CreateResponse();

            var responseData = new
            {
                Status = exception.GetBaseException() switch
                {
                    ArgumentNullException => HttpStatusCode.BadRequest,
                    ArgumentException => HttpStatusCode.BadRequest,
                    InvalidOperationException => HttpStatusCode.BadRequest,
                    NullReferenceException => HttpStatusCode.BadRequest,
                    FileNotFoundException => HttpStatusCode.BadRequest,
                    _ => HttpStatusCode.InternalServerError
                },
                Message = exception.InnerException != null ? exception.InnerException.Message : exception.Message
            };

            await response.WriteAsJsonAsync(responseData);
            
            response.StatusCode = responseData.Status;

            context.InvokeResult(response);
        }
    }
}
