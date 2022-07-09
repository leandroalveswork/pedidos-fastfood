namespace PedidosMvc.Domain.Interfaces.UnitOfWork;
public interface IUnitOfWork
{
    Task StartTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}