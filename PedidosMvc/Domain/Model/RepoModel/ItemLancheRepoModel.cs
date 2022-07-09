using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PedidosMvc.Domain.Model.RepoModel;
public class ItemLancheRepoModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdPedido { get; set; }
    public string? IdLanche { get; set; }
    public int Quantidade { get; set; }
    public double ValorUnidade { get; set; }
    public int PosicaoLista { get; set; }
}