using MediatR;
using MyBasket.Application.Event;
using MyBasket.Domain.BasketAggregate;
using MyBasket.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BasketItem = MyBasket.Domain.BasketAggregate.BasketItem;

namespace MyBasket.Application.Commands
{
    public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand>
    {
        #region Fields

        private readonly IBasketRepository _basketRepository;

        private readonly IMediator _mediator;

        #endregion Fields

        #region Ctor

        public UpdateBasketCommandHandler(IBasketRepository basketRepository, IMediator mediator)
        {
            _basketRepository = basketRepository;
            _mediator = mediator;
        }

        #endregion Ctor

        #region Methods

        public async Task<Unit> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
        {
            var products = request.Items.Select(x => new KeyValuePair<int, int>(x.ProductId, x.Quantity)).ToList();

            await _mediator.Publish(new CheckStockEvent(products), cancellationToken);

            var basket = new Basket(request.CustomerId);

            basket.AddBasketItems(request.Items.Select(x => x.Map<BasketItem>()).ToList());

            await _basketRepository.UpdateBasketAsync(basket);

            return Unit.Value;
        }

        #endregion Methods
    }
}