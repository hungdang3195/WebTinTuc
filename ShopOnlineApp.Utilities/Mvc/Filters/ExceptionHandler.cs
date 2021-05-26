using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ShopOnlineApp.Utilities.Mvc.Filters
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ArgumentException argumentException:
                    {
                        if (!string.IsNullOrEmpty(argumentException.ParamName))
                        {
                            context.ModelState.AddModelError(argumentException.ParamName, argumentException.Message);
                        }
                        var problemDetails = new ValidationProblemDetails(context.ModelState);
                        problemDetails.Detail = argumentException.Message;

                        context.Result = new ObjectResult(problemDetails)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        context.ExceptionHandled = true;
                        return;
                    }

                case InvalidOperationException invalidOperationException:
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState);
                        problemDetails.Detail = invalidOperationException.Message;

                        context.Result = new ObjectResult(problemDetails)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        context.ExceptionHandled = true;
                        return;
                    }

                case ValidationException validationException:
                    {
                        foreach (var item in validationException.Errors)
                        {
                            context.ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                        }

                        var problemDetails = new ValidationProblemDetails(context.ModelState);
                        problemDetails.Detail = "Your request cannot pass our validations. Sorry";
                        context.Result = new ObjectResult(problemDetails)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        context.ExceptionHandled = true;
                        return;
                    }

                default:
                    _logger.LogError(context.Exception, "Exception thrown but no one can catch it");
                    break;
            }
        }
    }
}
