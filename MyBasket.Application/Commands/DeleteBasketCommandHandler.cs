using MediatR;
using MyBasket.Domain.BasketAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace MyBasket.Application.Commands
{
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand>
    {
        #region Fields

        private readonly IBasketRepository _basketRepository;

        #endregion Fields

        #region Ctor

        public DeleteBasketCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        #endregion Ctor

        #region Methods

        public async Task<Unit> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            await _basketRepository.DeleteBasketAsync(request.CustomerId);

            return Unit.Value;
        }

        #endregion Methods
    }
}