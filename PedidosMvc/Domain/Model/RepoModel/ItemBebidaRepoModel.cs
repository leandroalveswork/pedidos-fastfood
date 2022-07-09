using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PedidosMvc.Domain.Model.RepoModel;
public class ItemBebidaRepoModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdPedido { get; set; }
    public string? IdBebida { get; set; }
    public int VolumeMl { get; set; }
    public double ValorLitro { get; set; }
    public int PosicaoLista { get; set; }
}