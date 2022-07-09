using PedidosMvc.Domain.Interfaces.Configuration;

namespace PedidosMvc.Configuration;
public class MongoDbConfiguration : IDatabaseConfiguration
{
    private readonly IConfiguration _configuration;
    public MongoDbConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GetConexaoBD()
    {
        return _configuration["CONEXAO_BANCO_DADOS_NOSQL"];
    }
    public string GetNomeBD()
    {
        return _configuration["NOME_BANCO_DADOS_NOSQL"];
    }
    public string GetNomeColecBebidas()
    {
        return _configuration["BancoDadosMongoDB:ColecaoBebidas"];
    }
    public string GetNomeColecItemBebidas()
    {
        return _configuration["BancoDadosMongoDB:ColecaoItemBebidas"];
    }
    public string GetNomeColecLanches()
    {
        return _configuration["BancoDadosMongoDB:ColecaoLanches"];
    }
    public string GetNomeColecItemLanches()
    {
        return _configuration["BancoDadosMongoDB:ColecaoItemLanches"];
    }
    public string GetNomeColecPedidos()
    {
        return _configuration["BancoDadosMongoDB:ColecaoPedidos"];
    }
}