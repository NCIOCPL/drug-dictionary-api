using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Elasticsearch.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json.Linq;
using Xunit;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.Common.Testing;
using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    ///  Tests to verify the structure of requests to Elasticsearch.
    /// </summary>
    public class ESDrugsQueryServiceTest_GetById : ESDrugsQueryServiceTest_Common
    {
        /// <summary>
        /// Test failures to successfully connect to Elasticsearch
        /// </summary>
        /// <param name="returnStatus"></param>
        [Theory]
        [InlineData(401)]
        [InlineData(403)]
        [InlineData(500)]
        [InlineData(502)]
        [InlineData(503)]
        public async void BadConnection(int returnStatus)
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes("An error message"),
                statusCode: returnStatus,
                exception: null,
                contentType: "text/plain"
            );

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            Exception ex = await Assert.ThrowsAsync<APIInternalException>(
                () => drugClient.GetById(43234)
            );
            Assert.Equal(ESDrugsQueryService.INTERNAL_ERRORS_MESSAGE, ex.Message);
        }

        /// <summary>
        /// Test receiving an invalid response from ES in GetById.
        /// </summary>
        [Theory]
        [InlineData("Not the server you were looking for")] // Bad server
        [InlineData("{")] // Interrupted connection
        public async void InvalidResponse(string responseBody)
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes(responseBody),
                statusCode: 200,
                exception: null,
                contentType: "text/plain"
            );

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            Exception ex = await Assert.ThrowsAsync<APIInternalException>(
                () => drugClient.GetById(43234)
            );
            Assert.Equal(ESDrugsQueryService.INTERNAL_ERRORS_MESSAGE, ex.Message);
        }

        [Fact]
        public async void TestRequestSetup()
        {
            string expectedUrl = "http://localhost:9200/drugv1/_doc/801844";
            HttpMethod expectedMethod = HttpMethod.GET;
            string expectedContentType = "application/json";
            JToken expectedBody = null;

            Uri esURI = null;
            string esContentType = String.Empty;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.

            JToken requestBody = null;

            #region Mock response string
            string MockResponse = @"{
    ""_index"": ""drugv1-20211018224500"",
    ""_type"": ""_doc"",
    ""_id"": ""801844"",
    ""_version"": 1,
    ""_seq_no"": 28171,
    ""_primary_term"": 1,
    ""found"": true,
    ""_source"": {
        ""term_id"": ""801844"",
        ""name"": ""autologous anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T cells AIC100"",
        ""first_letter"": ""a"",
        ""type"": ""DrugTerm"",
        ""term_name_type"": ""PreferredName"",
        ""pretty_url_name"": ""autologous-anti-icam-1-car-cd28-4-1bb-cd3zeta-expressing-t-cells-aic100"",
        ""aliases"": [
            {
                ""type"": ""CodeName"",
                ""name"": ""AIC100""
            },
            {
                ""type"": ""Synonym"",
                ""name"": ""autologous ICAM-1-targeted CAR-T cells AIC100""
            },
            {
                ""type"": ""Synonym"",
                ""name"": ""autologous ICAM-1-targeted CAR T cells AIC100""
            },
            {
                ""type"": ""Synonym"",
                ""name"": ""autologous ICAM-1-targeted CAR-T lymphocytes AIC100""
            },
            {
                ""type"": ""Synonym"",
                ""name"": ""CAR-T cells AIC100""
            },
            {
                ""type"": ""CodeName"",
                ""name"": ""AIC-100""
            },
            {
                ""type"": ""CodeName"",
                ""name"": ""AIC 100""
            }
        ],
        ""definition"": {
            ""text"": ""A preparation of autologous T lymphocytes that have been transduced with a lentiviral vector to express a chimeric antigen receptor (CAR) containing the Inserted (I) domain variant of lymphocyte function-associated antigen-1 (LFA-1) which targets intercellular adhesion molecule-1 (ICAM-1 or CD54), and the co-stimulatory signaling domains of CD28, 4-1BB (CD137) and CD3zeta, with potential immunostimulating and antineoplastic activities. Upon administration, autologous anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T cells AIC100 recognize and kill ICAM-1-expressing tumor cells. ICAM-1, normally expressed on leukocytes and endothelial cells, may be overexpressed in a variety of cancers. CAR-T cells AIC100 are also engineered to express somatostatin receptor subtype 2 (SSTR2), allowing the imaging of the CAR-T cells in patients."",
            ""html"": ""A preparation of autologous T lymphocytes that have been transduced with a lentiviral vector to express a chimeric antigen receptor (CAR) containing the Inserted (I) domain variant of lymphocyte function-associated antigen-1 (LFA-1) which targets intercellular adhesion molecule-1 (ICAM-1 or CD54), and the co-stimulatory signaling domains of CD28, 4-1BB (CD137) and CD3zeta, with potential immunostimulating and antineoplastic activities. Upon administration, autologous anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T cells AIC100 recognize and kill ICAM-1-expressing tumor cells. ICAM-1, normally expressed on leukocytes and endothelial cells, may be overexpressed in a variety of cancers. CAR-T cells AIC100 are also engineered to express somatostatin receptor subtype 2 (SSTR2), allowing the imaging of the CAR-T cells in patients.""
        },
        ""nci_concept_id"": ""C173378"",
        ""nci_concept_name"": ""Autologous Anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T-cells AIC100""
    }
}";
            #endregion

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.GetResponse<DrugTerm>>((req, res) =>
            {
                // We don't really care about the response, we just need it to be an actual DrugTerm structure.
                res.Stream = new MemoryStream(Encoding.UTF8.GetBytes(MockResponse));
                res.StatusCode = 200;

                esURI = req.Uri;
                esContentType = req.RequestMimeType;
                esMethod = req.Method;
                requestBody = conn.GetRequestPost(req);
            });

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> clientOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, clientOptions, new NullLogger<ESDrugsQueryService>());

            DrugTerm result = await drugClient.GetById(801844);

            Assert.Equal(expectedUrl, esURI.AbsoluteUri);
            Assert.Equal(expectedMethod, esMethod);
            Assert.Equal(expectedContentType, esContentType);
            Assert.Equal(expectedBody, requestBody);
        }

        /// <summary>
        /// Verify that GetById returns in the expected manner when Elasticsearch reports that the
        /// term doesn't exist.
        /// </summary>
        [Fact]
        public async void TermNotFound()
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes(
                    @"{
                        ""_index"": ""drugv1"",
                        ""_type"": ""_doc"",
                        ""_id"": ""1"",
                        ""found"": false
                    }"
                ),
                statusCode: 404,
                exception: null,
                contentType: "application/json"
            );

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            DrugTerm term = await drugClient.GetById(801844);

            Assert.Null(term);
        }

        public static IEnumerable<object[]> DataLoadingData => new[] {
            new  object[] { new  ID_NoDrugInfoSummary_TestData() },
            new object[] { new ID_WithDrugInfoSummary_TestData() }
        };

        /// <summary>
        /// Test loading variations of the DrugTerm structure.
        /// </summary>
        [Theory, MemberData(nameof(DataLoadingData))]
        public async void DataLoading(BaseGetByIdTestData data)
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes(data.ResponseBody),
                statusCode: 200,
                exception: null,
                contentType: "application/json"
            );

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            DrugTerm actualTerm = await drugClient.GetById(data.TermID);

            Assert.Equal(data.ExpectedData.Definition, actualTerm.Definition, new DefinitionComparer());
            Assert.Equal(data.ExpectedData.DrugInfoSummaryLink, actualTerm.DrugInfoSummaryLink, new DrugInfoSummaryLinkComparer());
            Assert.Equal(data.ExpectedData.Aliases, actualTerm.Aliases, new ArrayComparer<TermAlias, TermAliasComparer>());
            Assert.Equal(data.ExpectedData, actualTerm, new DrugTermComparer());
        }
    }
}