namespace PedidosMvc.Domain.Interfaces.Strategy;
public interface ISorterStrategy<TResult, TMember>
{
    IEnumerable<TResult> OrderAscending(IEnumerable<TResult> results, Func<TResult, TMember> getMember);
    IEnumerable<TResult> OrderDescending(IEnumerable<TResult> results, Func<TResult, TMember> getMember);
}