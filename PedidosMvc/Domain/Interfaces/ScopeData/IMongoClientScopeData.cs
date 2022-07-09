using MongoDB.Driver;

namespace PedidosMvc.Domain.Interfaces.ScopeData;
public interface IMongoClientScopeData
{
    MongoClient? GetMongoClient();
    void SetMongoClient(MongoClient? client);
    MongoClient GetMongoClientConnectIfNull();
}