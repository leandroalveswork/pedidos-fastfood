using PedidosMvc.Domain.Interfaces.Strategy;

namespace PedidosMvc.Domain.Model.Strategy;
public class BaseSorterStrategy<TResult, TMember> : ISorterStrategy<TResult, TMember>
{
    public IEnumerable<TResult> OrderAscending(IEnumerable<TResult> results, Func<TResult, TMember> getMember)
    {
        return new List<TResult>(results.OrderBy(x => getMember(x)));
    }

    public IEnumerable<TResult> OrderDescending(IEnumerable<TResult> results, Func<TResult, TMember> getMember)
    {
        return new List<TResult>(results.OrderByDescending(x => getMember(x)));
    }
}