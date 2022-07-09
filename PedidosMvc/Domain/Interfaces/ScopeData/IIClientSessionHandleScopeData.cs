using MongoDB.Driver;

namespace PedidosMvc.Domain.Interfaces.ScopeData;
public interface IIClientSessionHandleScopeData
{
    IClientSessionHandle? GetIClientSessionHandle();
    void SetIClientSessionHandle(IClientSessionHandle? session);
}