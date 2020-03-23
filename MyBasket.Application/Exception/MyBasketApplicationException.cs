using MyBasket.Shared.Exception;

namespace MyBasket.Application.Exception
{
    public class MyBasketApplicationException : BaseException
    {
        #region Ctor

        public MyBasketApplicationException(string message) : base("Application", message)
        {
        }

        #endregion Ctor
    }
}