using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Azure.Documents.Client;
using Microsoft.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
using StackExchange.Redis;
using ConsolidacaoAcoes.Models;

namespace ConsolidacaoAcoes.Data
{
    public static class AcoesRepository
    {
        private static readonly ConnectionMultiplexer _CONEXAO_REDIS =
            ConnectionMultiplexer
                .Connect(Environment.GetEnvironmentVariable("RedisConnectionString"));

        public static TransacaoDocument GetTransacao(string id)
        {
            using (var client = new DocumentClient(
                new Uri(Environment.GetEnvironmentVariable("DBAcoesEndpointUri")),
                        Environment.GetEnvironmentVariable("DBAcoesEndpointPrimaryKey")))
            {
                FeedOptions queryOptions =
                    new FeedOptions { MaxItemCount = -1 };

                return client.CreateDocumentQuery<TransacaoDocument>(
                        UriFactory.CreateDocumentCollectionUri(
                            "DBAcoes", "Transacoes"),
                            "SELECT * FROM Transacoes t " +
                           $"WHERE t.id = '{id}'", queryOptions)
                        .ToArray()[0];
            }
        }

        public static void SaveCompra(TransacaoDocument document)
        {
            var dbRedis = _CONEXAO_REDIS.GetDatabase();
            dbRedis.StringSet(
                "COMPRA-ACAO-" + document.Codigo,
                JsonSerializer.Serialize(document, new JsonSerializerOptions()
                {
                    IgnoreNullValues = true
                }),
                expiry: null);
        }

        public static void SaveVenda(TransacaoDocument document)
        {
            using (var conexao = new SqlConnection(
                Environment.GetEnvironmentVariable("BaseIndicadores")))
            {
                VendaAcao venda = new VendaAcao();
                venda.CodReferencia = document.id;
                venda.Sigla = document.Codigo;
                venda.DataReferencia = document.Data;
                venda.Valor = document.Valor;
                conexao.Insert(venda);
            }
        }

        public static IEnumerable<VendaAcao> ListVendas()
        {
            using (var conexao = new SqlConnection(
                Environment.GetEnvironmentVariable("BaseIndicadores")))
            {
                return conexao.Query<VendaAcao>(
                    "SELECT * FROM dbo.VendaAcoes " +
                    "ORDER BY Sigla, DataReferencia DESC");
            }
        }
    }
}