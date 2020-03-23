using MyBasket.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyBasket.Application.Models.Responses
{
    [DisplayName("BasketItem")]
    public class GetBasketViewModel : IViewModel
    {
        #region Properties

        public List<BasketItem> BasketItems { get; set; }
        public decimal BasketTotalPrice { get; set; }

        #endregion Properties
    }
}