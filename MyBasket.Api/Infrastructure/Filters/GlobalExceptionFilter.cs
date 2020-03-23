using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyBasket.Application.Exception;
using MyBasket.Domain.Exception;
using MyBasket.Shared.Exception;
using System.Net;

namespace MyBasket.Api.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        #region Fields

        private readonly ILogger<GlobalExceptionFilter> _logger;

        #endregion Fields

        #region Ctor

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this._logger = logger;
        }

        #endregion Ctor

        #region Methods

        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(MyBasketDomainException) || context.Exception.GetType() == typeof(MyBasketApplicationException))
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = "There is at least one error. Please check additional info"
                };

                var baseException = (BaseException)context.Exception;

                problemDetails.Errors.Add(baseException.Layer, new[] { baseException.Message });

                context.Result = new BadRequestObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            }
            else
            {
                context.Result = new InternalServerErrorObjectResult(new JsonResult(new
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status500InternalServerError,
                    Message = context.Exception.Message,
                    Detail = "There is at least one error. Please check additional info"
                }));
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
            _logger.LogError(context.Exception.Message);
        }

        #endregion Methods
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        #region Ctor

        public InternalServerErrorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }

        #endregion Ctor
    }
}