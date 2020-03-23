using MyBasket.Shared.Exception;

namespace MyBasket.Domain.Exception
{
    public class MyBasketDomainException : BaseException
    {
        #region Ctor

        public MyBasketDomainException(string message) : base("Domain", message)
        {
        }

        #endregion Ctor
    }
}