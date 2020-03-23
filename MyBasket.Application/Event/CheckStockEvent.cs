using MediatR;
using System.Collections.Generic;

namespace MyBasket.Application.Event
{
    public class CheckStockEvent : INotification
    {
        #region Ctor

        public CheckStockEvent()
        {
            ProductStocks = new List<KeyValuePair<int, int>>();
        }

        public CheckStockEvent(List<KeyValuePair<int, int>> productStocks) : this()
        {
            ProductStocks = productStocks;
        }

        #endregion Ctor

        #region Properties

        public List<KeyValuePair<int, int>> ProductStocks { get; private set; }

        #endregion Properties
    }
}