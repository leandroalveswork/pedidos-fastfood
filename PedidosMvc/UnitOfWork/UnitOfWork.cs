using MongoDB.Driver;
using PedidosMvc.Domain.Interfaces.ScopeData;
using PedidosMvc.Domain.Interfaces.UnitOfWork;

namespace PedidosMvc.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoClientScopeData _mongoClientScopeData;
    private readonly IIClientSessionHandleScopeData _iclientSessionHandleScopeData;
    public UnitOfWork(IMongoClientScopeData mongoClientScopeData, IIClientSessionHandleScopeData iclientSessionHandleScopeData)
    {
        _mongoClientScopeData = mongoClientScopeData;
        _iclientSessionHandleScopeData = iclientSessionHandleScopeData;
    }
    public async Task StartTransactionAsync()
    {
        var client = _mongoClientScopeData.GetMongoClientConnectIfNull();
        var session = await client.StartSessionAsync();
        session.StartTransaction();
        _iclientSessionHandleScopeData.SetIClientSessionHandle(session);
    }

    public async Task CommitAsync()
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            throw new ArgumentNullException(Message.ErroTransacaoNaoIniciadaCommitRollback);
        }
        await session.CommitTransactionAsync();
        _iclientSessionHandleScopeData.SetIClientSessionHandle(null);
    }

    public async Task RollbackAsync()
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            throw new ArgumentNullException(Message.ErroTransacaoNaoIniciadaCommitRollback);
        }
        await session.AbortTransactionAsync();
        _iclientSessionHandleScopeData.SetIClientSessionHandle(null);
    }
}