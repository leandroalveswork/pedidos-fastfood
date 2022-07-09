using PedidosMvc.Domain.Interfaces.Strategy;

namespace PedidosMvc.Domain.Model.Strategy;
public class StringHasFilterStrategy<TResult> : IFilterStrategy<TResult, string>
{
    public IEnumerable<TResult> Apply(IEnumerable<TResult> results, Func<TResult, string> getMember, string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return results;
        }
        var filtered = new List<TResult>(results.Where(x => getMember(x).IndexOf(filter) != -1));
        return filtered;
    }
}