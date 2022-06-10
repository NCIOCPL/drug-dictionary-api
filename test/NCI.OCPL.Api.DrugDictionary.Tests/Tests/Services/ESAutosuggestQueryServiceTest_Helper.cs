using Microsoft.Extensions.Options;
using System.IO;
using System.Text;

using Moq;

using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public static class ESQueryServiceTest_Helper
    {
        /// <summary>
        /// Mock Elasticsearch configuration options for autosuggest.
        /// </summary>
        public static IOptions<DrugDictionaryAPIOptions> MockSearchOptions
        {
            get
            {
                Mock<IOptions<DrugDictionaryAPIOptions>> clientOptions = new Mock<IOptions<DrugDictionaryAPIOptions>>();
                clientOptions
                    .SetupGet(opt => opt.Value)
                    .Returns(new DrugDictionaryAPIOptions
                    {
                        AliasName = "drugv1",
                        Autosuggest = new AutosuggestOptions
                        {
                            MaxSuggestionLength = 30
                        }
                    }
                );

                return clientOptions.Object;
            }
        }

        public static Stream GetMockHealthCheckResponse(string color)
        {
            string response = GetMockHealthCheckResponseString(color);
            byte[] byteArray = Encoding.UTF8.GetBytes(response);
            return new MemoryStream(byteArray);
        }

        public static string GetMockHealthCheckResponseString(string color)
        {
            string response =
                @"{
                ""cluster_name"": ""es232_dev"",
                ""status"": ""{color}"",
                ""timed_out"": false,
                ""number_of_nodes"": 1,
                ""number_of_data_nodes"": 1,
                ""active_primary_shards"": 1,
                ""active_shards"": 1,
                ""relocating_shards"": 0,
                ""initializing_shards"": 0,
                ""unassigned_shards"": 0,
                ""delayed_unassigned_shards"": 0,
                ""number_of_pending_tasks"": 0,
                ""number_of_in_flight_fetch"": 0,
                ""task_max_waiting_in_queue_millis"": 0,
                ""active_shards_percent_as_number"": 100.0
                }";

            return response.Replace("{color}", color);
        }

    }
}