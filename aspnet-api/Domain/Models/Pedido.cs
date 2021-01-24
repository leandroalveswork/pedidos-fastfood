namespace AspnetApi.Domain.Models
{
    public class Pedido
    {
        public Pedido(int id, string solicitanteId, int posicao, string lanche, string bebida)
        {
            Id = id;
            SolicitanteId = solicitanteId;
            Posicao = posicao;
            Lanche = lanche;
            Bebida = bebida;
        }
        
        public int Id { get; set; }

        public string SolicitanteId { get; set; }

        public int Posicao { get; set; }

        public string Lanche { get; set; }

        public string Bebida { get; set; }
    }
}