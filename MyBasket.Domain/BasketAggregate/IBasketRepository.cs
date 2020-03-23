using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBasket.Domain.BasketAggregate
{
    public interface IBasketRepository
    {
        #region Methods

        Basket GetBasket(string customerId);

        Task<Basket> GetBasketAsync(string customerId);

        IReadOnlyList<string> GetUsers();

        void DeleteBasket(string id);

        Task DeleteBasketAsync(string id);

        Basket UpdateBasket(Basket basket);

        Task<Basket> UpdateBasketAsync(Basket basket);

        #endregion Methods
    }
}