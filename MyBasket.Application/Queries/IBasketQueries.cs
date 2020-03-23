using MyBasket.Application.Models.Requests;
using MyBasket.Application.Models.Responses;
using System.Threading.Tasks;

namespace MyBasket.Application.Queries
{
    public interface IBasketQueries
    {
        #region Methods

        Task<GetBasketViewModel> GetBasket(GetBasketViewRequest request);

        #endregion Methods
    }
}