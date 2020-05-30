using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ConsolidacaoAcoes.Data;

namespace ConsolidacaoAcoes
{
    public static class Vendas
    {
        [FunctionName("Vendas")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult(AcoesRepository.ListVendas());
        }
    }
}