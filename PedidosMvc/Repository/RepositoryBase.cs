using System.Linq.Expressions;
using MongoDB.Driver;
using PedidosMvc.Domain.Interfaces.Configuration;
using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.ScopeData;

namespace PedidosMvc.Repository;
public class RepositoryBase<T> : IRepositoryBase<T>
{
    protected readonly IMongoClientScopeData _mongoClientScopeData;
    protected readonly IDatabaseConfiguration _databaseConfiguration;
    protected readonly IIClientSessionHandleScopeData _iclientSessionHandleScopeData;
    public RepositoryBase(IMongoClientScopeData mongoClientScopeData, IDatabaseConfiguration databaseConfiguration, IIClientSessionHandleScopeData iclientSessionHandleScopeData)
    {
        _mongoClientScopeData = mongoClientScopeData;
        _databaseConfiguration = databaseConfiguration;
        _iclientSessionHandleScopeData = iclientSessionHandleScopeData;
    }

    protected virtual string GetNomeColecEntidade()
    {
        throw new NotImplementedException();
    }

    protected virtual Expression<Func<T, bool>> GetExpressaoFindById(string id)
    {
        throw new NotImplementedException();
    }

    protected IMongoCollection<T> GetCollection()
    {
        var client = _mongoClientScopeData.GetMongoClientConnectIfNull();
        var database = client.GetDatabase(_databaseConfiguration.GetNomeBD());
        var collection = database.GetCollection<T>(GetNomeColecEntidade());
        return collection;
    }

    public async Task<List<T>> SelectAllAsync()
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            var entities = await GetCollection().Find(_ => true).ToListAsync();
            return entities;
        }
        else
        {
            var entities = await GetCollection().Find(session, _ => true).ToListAsync();
            return entities;
        }
    }

    public async Task<T?> SelectByIdAsync(string id)
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            var entity = await GetCollection().Find(GetExpressaoFindById(id)).FirstOrDefaultAsync();
            return entity;
        }
        else
        {
            var entity = await GetCollection().Find(session, GetExpressaoFindById(id)).FirstOrDefaultAsync();
            return entity;
        }
    }

    public async Task InsertAsync(T entity)
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            await GetCollection().InsertOneAsync(entity);
        }
        else
        {
            await GetCollection().InsertOneAsync(session, entity);
        }
    }

    public async Task UpdateAsync(string id, T entity)
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            await GetCollection().ReplaceOneAsync(GetExpressaoFindById(id), entity);
        }
        else
        {
            await GetCollection().ReplaceOneAsync(session, GetExpressaoFindById(id), entity);
        }
    }

    public async Task DeleteAsync(string id)
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            await GetCollection().DeleteOneAsync(GetExpressaoFindById(id));
        }
        else
        {
            await GetCollection().DeleteOneAsync(session, GetExpressaoFindById(id));
        }
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            return await GetCollection().Find(GetExpressaoFindById(id)).AnyAsync();
        }
        else
        {
            return await GetCollection().Find(session, GetExpressaoFindById(id)).AnyAsync();
        }
    }
}