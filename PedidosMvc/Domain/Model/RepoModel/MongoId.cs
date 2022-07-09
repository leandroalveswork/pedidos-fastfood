namespace PedidosMvc.Domain.Model.RepoModel;
public class MongoId
{
    public MongoId()
    {
        Id = Guid.NewGuid().ToString()
            .Replace("-", "")
            .Substring(0, 24)
            .ToLower();
    }
    public MongoId(string id)
    {
        Id = id;
    }
    public string Id { get; set; }
}