

using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.ErrorModels;
using System.Net;

namespace Store.Api.Middlewares
{
    public class GlobalErrorHandlingMiddleware // : IMiddleware
    {
        //public Task InvokeAsync(HttpContext context, RequestDelegate next)
        //{
        //    throw new NotImplementedException();
        //}

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                await _next(httpcontext);
                if (httpcontext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await NotFoundExceptionEndPoint(httpcontext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                await HandleException(httpcontext, ex);
            }
        }




        private async Task NotFoundExceptionEndPoint(HttpContext httpcontext)
        {

            httpcontext.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = $"the end point {httpcontext.Request.Path} is not found !!",
                StatueCode = (int)HttpStatusCode.NotFound
            };

            await httpcontext.Response.WriteAsync(response.ToString());
        }


        private async Task HandleException(HttpContext httpcontext, Exception ex)
        {
            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message
            };
            httpcontext.Response.ContentType = "application/json";
            // match pattern
            httpcontext.Response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                ValidationException validationException => HandleValidationException(validationException, response),
                _ => (int)HttpStatusCode.InternalServerError
            };
            response.StatueCode = httpcontext.Response.StatusCode;

            await httpcontext.Response.WriteAsync(response.ToString());
        }



        /// <summary>
        /// this private function use to passing enumerable of errors list with the validation exceptions  
        /// </summary>
        /// <param name="ex">is a general validation exception</param>
        /// <param name="errorDetails"> model of error details like status code , message , errors</param>
        /// <returns></returns>
        private int HandleValidationException(ValidationException ex, ErrorDetails errorDetails)
        {

            errorDetails.Errors = ex.Errors;
            //return (int)HttpStatusCode.BadRequest;
            return StatusCodes.Status400BadRequest;
        }
    }
}

