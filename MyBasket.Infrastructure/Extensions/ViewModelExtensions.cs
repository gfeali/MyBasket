using Microsoft.AspNetCore.Mvc;
using MyBasket.Shared;

namespace MyBasket.Infrastructure.Extensions
{
    public static class ViewModelExtensions
    {
        #region Methods

        public static IActionResult PrepareResult<T>(this T value, object obj = null) where T : IViewModel
        {
            if (value.IsNull())
            {
                return new NotFoundObjectResult(obj);
            }
            return new OkObjectResult(value);
        }

        #endregion Methods
    }
}