using MongoDB.Driver;
using PedidosMvc.Domain.Interfaces.Configuration;
using PedidosMvc.Domain.Interfaces.ScopeData;

namespace PedidosMvc.ScopeData;
public class MongoClientScopeData : IMongoClientScopeData
{
    private readonly IDatabaseConfiguration _databaseConfiguration;
    public MongoClientScopeData(IDatabaseConfiguration databaseConfiguration)
    {
        _databaseConfiguration = databaseConfiguration;
    }

    private MongoClient? _client;

    public MongoClient? GetMongoClient()
    {
        return _client;
    }

    public void SetMongoClient(MongoClient? client)
    {
        _client = client;
    }

    public MongoClient GetMongoClientConnectIfNull()
    {
        if (_client != null)
        {
            return _client;
        }
        var newClient = new MongoClient(_databaseConfiguration.GetConexaoBD());
        _client = newClient;
        return newClient;
    }
}