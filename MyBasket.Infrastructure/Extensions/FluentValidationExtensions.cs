using FluentValidation;
using System.Text.RegularExpressions;

namespace MyBasket.Infrastructure.Extensions
{
    public static class FluentValidationExtensions
    {
        #region Fields

        private static readonly Regex IsGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

        #endregion Fields

        #region Methods

        public static IRuleBuilderOptions<T, string> MustGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => IsGuid.IsMatch(x)).WithMessage("This is not global unique identifier");
        }

        public static IRuleBuilderOptions<T, int> GreaterThanDefault<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(default(int));
        }

        public static IRuleBuilderOptions<T, decimal> MustPositive<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder.Must(x => x == default(decimal) || x > default(decimal)).WithMessage("Value cannot be negative");
        }

        public static IRuleBuilderOptions<T, TProperty> MustNotNullOrEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.NotNull().NotEmpty();
        }

        #endregion Methods
    }
}