using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Scrum.Core.Exceptions;
using Scrum.Core.Validations.ValidationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Scrum.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;


        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ValidationResultModel validationResult;

            context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //validationResult = new ValidationResultModel("Unhandled Exception", 500);

            if (exception.GetType() == typeof(BusinessException))
            {
                BusinessException ex = exception as BusinessException;
                validationResult = ex.ValidationResult;
                context.Response.StatusCode = (int)ex.ValidationResult.ErrorCode;
            }
            else if (exception.GetType() == typeof(TechnicalException))
            {
                TechnicalException ex = exception as TechnicalException;
                validationResult = ex.Result;
                if (validationResult == null)
                {
                    validationResult = new ValidationResultModel(validationResult.Message, ex.StatusCode.Value);
                }
                if (validationResult.ErrorCode == null)
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                else
                    context.Response.StatusCode = (int)validationResult.ErrorCode;
            }
            else
            {
                validationResult = exception.BuildValidationResult(HttpStatusCode.InternalServerError);
                context.Response.StatusCode = (int)validationResult.ErrorCode;
            }

            return context.Response.WriteAsync(JsonConvert.SerializeObject(validationResult));
        }
    }
}
