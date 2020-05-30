using Dapper.Contrib.Extensions;

namespace ConsolidacaoAcoes.Models
{
    [Table("dbo.VendaAcoes")]
    public class VendaAcao
    {
        [Key]
        public int Id { get; set; }
        public string CodReferencia { get; set; }
        public string Sigla { get; set; }
        public string DataReferencia { get; set; }
        public double Valor { get; set; }
    }
}