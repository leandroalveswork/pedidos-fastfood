using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PedidosMvc.Domain.Model.RepoModel;
public class BebidaRepoModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Nome { get; set; }
    public double ValorLitroSugerido { get; set; }
    public bool EstaAtivo { get; set; }
}