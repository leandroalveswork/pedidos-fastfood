namespace AspnetApi.Domain.Models.DTO
{
    public class PostPedido
    {
        public PostPedido(string solicitanteId, string lanche, string bebida)
        {
            SolicitanteId = solicitanteId;
            Lanche = lanche;
            Bebida = bebida;
        }
        public string SolicitanteId { get; set; }

        public string Lanche { get; set; }

        public string Bebida { get; set; }
    }
}