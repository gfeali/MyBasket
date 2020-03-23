using System;

namespace MyBasket.Shared
{
    public static class Check
    {
        #region Methods

        public static bool IsNull(this object value)
        {
            return value == null;
        }

        public static bool NotNull(this object value)
        {
            return value != null;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static void That(bool assertion, Action action)
        {
            if (assertion)
            {
                action.Invoke();
            }
        }

        #endregion Methods
    }
}