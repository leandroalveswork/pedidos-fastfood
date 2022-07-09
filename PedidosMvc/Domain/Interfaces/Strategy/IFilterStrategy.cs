namespace PedidosMvc.Domain.Interfaces.Strategy;
public interface IFilterStrategy<TResult, TMember>
{
    IEnumerable<TResult> Apply(IEnumerable<TResult> results, Func<TResult, TMember> getMember, TMember filter);
}