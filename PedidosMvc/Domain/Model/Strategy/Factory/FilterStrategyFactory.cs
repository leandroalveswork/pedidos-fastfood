using PedidosMvc.Domain.Interfaces.Strategy;

namespace PedidosMvc.Domain.Model.Strategy.Factory;
public class FilterStrategyFactory
{
    public static IFilterStrategy<TResult, string> GetStringHasFilter<TResult>()
    {
        return new StringHasFilterStrategy<TResult>();
    }

    public static IFilterStrategy<TResult, IComparable?> GetIsExactBaseFilter<TResult>()
    {
        return new IsExactBaseFilterStrategy<TResult>();
    }

    public static IFilterStrategy<TResult, IComparable?> GetIsLessEqualThanBaseFilter<TResult>()
    {
        return new IsLessEqualThanBaseFilterStrategy<TResult>();
    }

    public static IFilterStrategy<TResult, IComparable?> GetIsGreaterEqualThanBaseFilter<TResult>()
    {
        return new IsGreaterEqualThanBaseFilterStrategy<TResult>();
    }
}