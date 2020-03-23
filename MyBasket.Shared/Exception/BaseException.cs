namespace MyBasket.Shared.Exception
{
    public abstract class BaseException : System.Exception
    {
        #region Ctor

        protected BaseException(string layer, string message) : base(message)
        {
            Layer = layer;
            Message = message;
        }

        #endregion Ctor

        #region Properties

        public string Layer { get; }
        public new string Message { get; }

        #endregion Properties
    }
}