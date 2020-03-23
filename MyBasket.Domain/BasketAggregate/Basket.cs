using MyBasket.Domain.Exception;
using MyBasket.Shared;
using System.Collections.Generic;

namespace MyBasket.Domain.BasketAggregate
{
    public class Basket
    {
        #region Ctor

        public Basket(string customerId)
        {
            Check.That(customerId.IsNullOrEmpty(), () => throw new MyBasketDomainException("Customer Id is null"));
            CustomerId = customerId;
            BasketItems = new List<BasketItem>();
        }

        #endregion Ctor

        #region Properties

        public string CustomerId { get; private set; }

        public List<BasketItem> BasketItems { get; private set; }

        #endregion Properties

        #region Methods

        public void AddBasketItems(List<BasketItem> basketItems)
        {
            Check.That(basketItems.Count.Equals(default(int)), () => throw new MyBasketDomainException("Basket items is empty"));

            foreach (var item in basketItems)
            {
                item.CheckQuantity();
            }

            BasketItems.AddRange(basketItems);
        }

        #endregion Methods
    }
}