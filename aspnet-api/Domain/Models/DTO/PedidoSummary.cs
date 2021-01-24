namespace AspnetApi.Domain.Models.DTO
{
    public class PedidoSummary
    {
        public PedidoSummary(int id, int posicao, string lanche, string bebida)
        {
            Id = id;
            Posicao = posicao;
            Lanche = lanche;
            Bebida = bebida;
        }

        public int Id { get; set; }
        
        public int Posicao { get; set; }

        public string Lanche { get; set; }

        public string Bebida { get; set; }
    }
}