using MyBasket.Application.Models.Requests;
using MyBasket.Application.Models.Responses;
using MyBasket.Domain.BasketAggregate;
using MyBasket.Infrastructure.Extensions;
using MyBasket.Shared;
using System.Linq;
using System.Threading.Tasks;
using BasketItem = MyBasket.Application.Models.BasketItem;

namespace MyBasket.Application.Queries
{
    public class BasketQueries : IBasketQueries
    {
        #region Fields

        private readonly IBasketRepository _basketRepository;

        #endregion Fields

        #region Ctor

        public BasketQueries(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        #endregion Ctor

        #region Methods

        public async Task<GetBasketViewModel> GetBasket(GetBasketViewRequest request)
        {
            var result = new GetBasketViewModel();

            var basket = await _basketRepository.GetBasketAsync(request.CustomerId);

            if (basket.IsNull())
            {
                return default(GetBasketViewModel);
            }

            result.BasketItems = basket.BasketItems.Select(x => x.Map<BasketItem>()).ToList();

            result.BasketTotalPrice = basket.BasketItems.Sum(x => x.Price * x.Quantity);

            return result;
        }

        #endregion Methods
    }
}