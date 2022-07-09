using MongoDB.Driver;
using PedidosMvc.Domain.Interfaces.ScopeData;

namespace PedidosMvc.ScopeData;
public class IClientSessionHandleScopeData : IIClientSessionHandleScopeData
{
    private IClientSessionHandle? _session;
    
    public IClientSessionHandle? GetIClientSessionHandle()
    {
        return _session;
    }

    public void SetIClientSessionHandle(IClientSessionHandle? session)
    {
        _session = session;
    }
}