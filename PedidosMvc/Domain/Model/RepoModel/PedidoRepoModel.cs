using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PedidosMvc.Domain.Model.RepoModel;
public class PedidoRepoModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string NomeCliente { get; set; }
    public DateTime HoraCriacao { get; set; }
    public DateTime? HoraEntrega { get; set; }
    public bool EstaAtivo { get; set; }
}