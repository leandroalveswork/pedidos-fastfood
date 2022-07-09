namespace PedidosMvc.Domain.Interfaces.Configuration;
public interface IDatabaseConfiguration
{
    string GetConexaoBD();
    string GetNomeBD();
    string GetNomeColecBebidas();
    string GetNomeColecItemBebidas();
    string GetNomeColecLanches();
    string GetNomeColecItemLanches();
    string GetNomeColecPedidos();
}