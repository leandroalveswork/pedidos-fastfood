using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PedidosMvc.Domain.Model.RepoModel;
public class LancheRepoModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Nome { get; set; }
    public double ValorUnidadeSugerido { get; set; }
    public bool EstaAtivo { get; set; }
}