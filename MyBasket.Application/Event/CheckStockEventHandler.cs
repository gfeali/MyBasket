using MediatR;
using Microsoft.Extensions.Configuration;
using MyBasket.Application.Exception;
using MyBasket.Shared;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyBasket.Application.Event
{
    public class CheckStockEventHandler : INotificationHandler<CheckStockEvent>
    {
        #region Fields

        private readonly IConfiguration _configuration;

        #endregion Fields

        #region Ctor

        public CheckStockEventHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion Ctor

        #region Methods

        public async Task Handle(CheckStockEvent stockEvent, CancellationToken cancellationToken)
        {
            //  Product service address !!!Important Dummy address :):):)
            var url = _configuration.GetSection("Services:ProductCheckQuantity").Value;

            var query = new StringBuilder();

            query.Append("?");

            for (var i = 0; i < stockEvent.ProductStocks.Count; i++)
            {
                if (!default(int).Equals(i))
                {
                    query.Append("&");
                }

                query.Append($"filters[{i}].ProductId = {stockEvent.ProductStocks.ToArray()[i].Key}&filters[{i}].Quantity = {stockEvent.ProductStocks.ToArray()[i].Value}");
            }

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(string.Concat(url, query.ToString()), cancellationToken))
            using (var content = response.Content)
            {
                var result = await content.ReadAsStringAsync();

                Check.That(result != null && result.Length < 50, () => throw new MyBasketApplicationException("There is no stock for the products"));
            }
        }

        #endregion Methods
    }
}