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

        public override bool Equals(object obj)
        {
            var rightObj = (PedidoSummary) obj;

            if (Id != rightObj.Id) {
                return false;
            }
            if (Posicao != rightObj.Posicao) {
                return false;
            }
            if (Lanche != rightObj.Lanche) {
                return false;
            }
            if (Bebida != rightObj.Bebida) {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}