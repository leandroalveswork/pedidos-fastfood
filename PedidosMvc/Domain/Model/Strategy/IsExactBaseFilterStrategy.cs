using PedidosMvc.Domain.Interfaces.Strategy;

namespace PedidosMvc.Domain.Model.Strategy;
public class IsExactBaseFilterStrategy<TResult> : IFilterStrategy<TResult, IComparable?>
{
    public IEnumerable<TResult> Apply(IEnumerable<TResult> results, Func<TResult, IComparable?> getMember, IComparable? filter)
    {
        if (filter == null)
        {
            return results;
        }
        var filtered = new List<TResult>(results.Where(x => new NullableComparable(getMember(x)).CompareTo(filter) == 0));
        return filtered;
    }
}