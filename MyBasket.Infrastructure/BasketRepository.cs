using MyBasket.Domain.BasketAggregate;
using MyBasket.Infrastructure.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBasket.Infrastructure
{
    public class BasketRepository : IBasketRepository
    {
        #region Fields

        private readonly IRedisManager _redisManager;

        #endregion Fields

        #region Ctor

        public BasketRepository(IRedisManager redisManager)
        {
            _redisManager = redisManager;
        }

        #endregion Ctor

        #region Methods

        public Basket GetBasket(string customerId)
        {
            return _redisManager.Get<Basket>(customerId);
        }

        public async Task<Basket> GetBasketAsync(string customerId)
        {
            return await _redisManager.GetAsync<Basket>(customerId);
        }

        public IReadOnlyList<string> GetUsers()
        {
            return _redisManager.Keys;
        }

        public void DeleteBasket(string id)
        {
            _redisManager.Delete(id);
        }

        public async Task DeleteBasketAsync(string id)
        {
            await _redisManager.DeleteAsync(id);
        }

        public Basket UpdateBasket(Basket basket)
        {
            _redisManager.Add(basket.CustomerId, basket);

            return basket;
        }

        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            await _redisManager.AddAsync(basket.CustomerId, basket);

            return basket;
        }

        #endregion Methods
    }
}