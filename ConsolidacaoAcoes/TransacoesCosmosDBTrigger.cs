using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ConsolidacaoAcoes.Data;

namespace ConsolidacaoAcoes
{
    public static class TransacoesCosmosDBTrigger
    {
        [FunctionName("TransacoesCosmosDBTrigger")]
        public static void Run([CosmosDBTrigger(
            databaseName: "DBAcoes",
            collectionName: "Transacoes",
            ConnectionStringSetting = "DBAcoesConnectionString",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                foreach (var doc in input)
                {
                    var transacao = AcoesRepository.GetTransacao(doc.Id);
                    log.LogInformation($"Dados: {JsonSerializer.Serialize(transacao)}");

                    if (transacao.id.StartsWith("COMPRA"))
                    {
                        AcoesRepository.SaveCompra(transacao);
                        log.LogInformation("Compra registrada com sucesso");
                    }
                    else
                    {
                        AcoesRepository.SaveVenda(transacao);
                        log.LogInformation("Venda registrada com sucesso");
                    }
                }
            }
        }
    }
}