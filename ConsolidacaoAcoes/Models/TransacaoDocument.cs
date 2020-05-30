namespace ConsolidacaoAcoes.Models
{
    public class TransacaoDocument
    {
        public string id { get; set; }
        public string Codigo { get; set; }
        public string Data { get; set; }
        public double Valor { get; set; }
        public bool? Compra { get; set; }
        public bool? Venda { get; set; }
    }
}