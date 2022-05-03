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
    public class ESDrugsQueryServiceTest_GetIsHealthy
    {
        /// <summary>
        /// Verify graceful handling of failure to connect to Elasticsearch
        /// </summary>
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
            IElasticClient esClient = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = ESQueryServiceTest_Helper.MockSearchOptions;

            ESDrugsQueryService suggestSvc = new ESDrugsQueryService(esClient, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            // There should be no unhandled exception.
            bool result = await suggestSvc.GetIsHealthy();

            Assert.False(result);
        }

        /// <summary>
        /// Verify graceful handling of an invalid response from Elaticsearch.
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
            IElasticClient esClient = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = ESQueryServiceTest_Helper.MockSearchOptions;

            ESDrugsQueryService suggestSvc = new ESDrugsQueryService(esClient, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            // There should be no unhandled exception.
            bool result = await suggestSvc.GetIsHealthy();

            Assert.False(result);
        }

        /// <summary>
        /// Verify healthcheck requests to ES have the expected structure.
        /// </summary>
        [Fact]
        public async void GetIsHealthy_RequestStructure()
        {
            string expectedMimeType = "application/json";
            string expectedUrl = "http://localhost:9200/_cluster/health/drugv1";
            HttpMethod expectedMethod = HttpMethod.GET;

            Uri actualURI = null;
            string actualMimeType = String.Empty;
            HttpMethod actualMethod = HttpMethod.DELETE; // Something other than the expected value (default is GET).

            JToken actualRequestBody = null;

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.ClusterHealthResponse>((req, res) =>
            {
                res.Stream = ESQueryServiceTest_Helper.GetMockHealthCheckResponse("green");
                res.StatusCode = 200;

                actualURI = req.Uri;
                actualMimeType = req.RequestMimeType;
                actualMethod = req.Method;
                actualRequestBody = conn.GetRequestPost(req);
            });
            // The URL doesn't matter, it won't be used.
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            ESDrugsQueryService autosuggestClient = new ESDrugsQueryService(client, ESQueryServiceTest_Helper.MockSearchOptions, new NullLogger<ESDrugsQueryService>());

            // We don't care about the call's result, only the request.
            await autosuggestClient.GetIsHealthy();
            Assert.Equal(expectedMimeType, actualMimeType);
            Assert.Equal(expectedUrl, actualURI.AbsoluteUri);
            Assert.Equal(expectedMethod, actualMethod);
            Assert.Null(actualRequestBody);
        }

        /// <summary>
        /// Verify the service will return true when Elasticsearch reports itself healthy.
        /// </summary>
        /// <param name="responseColor">Path to a JSON file containing a simulated Elasticsearch
        /// health check response. The path is relative to the test project's TestData
        /// directory.</param>
        [Theory]
        [InlineData("green")]
        [InlineData("yellow")]
        public async void GetStatus_Healthy(string responseColor)
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes(ESQueryServiceTest_Helper.GetMockHealthCheckResponseString(responseColor)),
                statusCode: 200,
                exception: null,
                contentType: "application/json; charset=UTF-8"
            );

            var pool = new SingleNodeConnectionPool(new Uri("http://asdf:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient esClient = new ElasticClient(connectionSettings);
            ESDrugsQueryService autosuggestClient = new ESDrugsQueryService(esClient, ESQueryServiceTest_Helper.MockSearchOptions, new NullLogger<ESDrugsQueryService>());

            bool status = await autosuggestClient.GetIsHealthy();
            Assert.True(status);
        }

        /// <summary>
        /// Verify the service will return false when Elasticsearch reports itself unhealthy.
        /// </summary>
        /// <param name="datafile">Path to a JSON file containing a simulated Elasticsearch response.
        /// The path is relative to the test project's TestData directory.</param>
        [Theory]
        [InlineData("red")]
        //[InlineData("unexpected")]   // i.e. "Unexpected color" - ES will throw an exception, this makes sure we handle it.
        public async void GetStatus_Unhealthy(string responseColor)
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes(ESQueryServiceTest_Helper.GetMockHealthCheckResponseString(responseColor)),
                statusCode: 200,
                exception: null,
                contentType: "application/json; charset=UTF-8"
            );

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient esClient = new ElasticClient(connectionSettings);
            ESDrugsQueryService autosuggestClient = new ESDrugsQueryService(esClient, ESQueryServiceTest_Helper.MockSearchOptions, new NullLogger<ESDrugsQueryService>());

            bool status = await autosuggestClient.GetIsHealthy();
            Assert.False(status);
        }

    }
}