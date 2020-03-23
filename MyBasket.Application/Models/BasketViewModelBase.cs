using System.ComponentModel;

namespace MyBasket.Application.Models
{
    [DisplayName("basketItems")]
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
    }
}