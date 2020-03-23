using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBasket.Application.Commands;
using MyBasket.Application.Models.Requests;
using MyBasket.Application.Models.Responses;
using MyBasket.Application.Queries;
using MyBasket.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        #region Fields

        private readonly IBasketQueries _basketQueries;
        private readonly IMediator _mediator;

        #endregion Fields

        #region Ctor

        public BasketController(IBasketQueries basketQueries, IMediator mediator)
        {
            _basketQueries = basketQueries;
            _mediator = mediator;
        }

        #endregion Ctor

        #region Methods

        #region Get

        [Route("get")]
        [HttpGet]
        [ProducesResponseType(typeof(List<GetBasketViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBasket([FromQuery]GetBasketViewRequest request)
        {
            var result = await _basketQueries.GetBasket(request);

            return result.PrepareResult();
        }

        #endregion Get

        #region Put

        [Route("update")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateBasket([FromBody] UpdateBasketCommand request)
        {
            await _mediator.Send(request);

            return Accepted();
        }

        #endregion Put

        #region Delete

        [Route("delete")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteBasket([FromBody] DeleteBasketCommand request)
        {
            await _mediator.Send(request);

            return Accepted();
        }

        #endregion Delete

        #endregion Methods
    }
}