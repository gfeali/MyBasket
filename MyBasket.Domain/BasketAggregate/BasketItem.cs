using MyBasket.Domain.Exception;
using MyBasket.Shared;

namespace MyBasket.Domain.BasketAggregate
{
    public class BasketItem
    {
        #region Properties

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }

        #endregion Properties

        #region Methods

        public void CheckQuantity()
        {
            Check.That(this.Quantity.Equals(default(int)), () => throw new MyBasketDomainException("Item quantity is not valid"));
        }

        #endregion Methods
    }
}