using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.DrugDictionary.Models;
using System;

namespace NCI.OCPL.Api.DrugDictionary.Services
{
    /// <summary>
    /// Elasticsearch implementation of the service for retrieveing suggestions for
    /// GlossaryTerm objects.
    /// </summary>
    public class ESAutosuggestQueryService : IAutosuggestQueryService
    {

        /// <summary>
        /// The elasticsearch client
        /// </summary>
        private IElasticClient _elasticClient;

        /// <summary>
        /// The API options.
        /// </summary>
        protected readonly DrugDictionaryAPIOptions _apiOptions;

        /// <summary>
        /// A logger to use for logging
        /// </summary>
        private readonly ILogger<ESAutosuggestQueryService> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ESAutosuggestQueryService(IElasticClient client,
            IOptions<DrugDictionaryAPIOptions> apiOptionsAccessor,
            ILogger<ESAutosuggestQueryService> logger)
        {
            _elasticClient = client;
            _apiOptions = apiOptionsAccessor.Value;
            _logger = logger;
        }

        /// <summary>
        /// Search for Terms based on the search criteria.
        /// </summary>
        /// <param name="searchText">The text to search for.</param>
        /// <param name="matchType">Set to true to allow search to find terms which contain the query string instead of explicitly starting with it.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>An array of Suggestion objects</returns>
        public async Task<Suggestion[]> GetSuggestions(string searchText, MatchType matchType, int size,
            DrugResourceType[] includeResourceTypes,
                TermNameType[] includeNameTypes,
                TermNameType[] excludeNameTypes
        )
        {
            // Set up the SearchRequest to send to elasticsearch.
            Indices index = Indices.Index(new string[] { this._apiOptions.AliasName });

            ISearchResponse<Suggestion> response = null;

            try
            {
                SearchRequest request;
                switch (matchType)
                {
                    default:
                    case MatchType.Begins:
                        request = BuildBeginRequest(index, searchText, size, includeResourceTypes, includeNameTypes, excludeNameTypes);
                        break;
                    case MatchType.Contains:
                        request = BuildContainsRequest(index, searchText, size, includeResourceTypes, includeNameTypes, excludeNameTypes);
                        break;
                }

                response = await _elasticClient.SearchAsync<Suggestion>(request);
            }
            catch (Exception ex)
            {
                string msg = "Could not search drug dictionary.";
                _logger.LogError($"Error searching index: '{this._apiOptions.AliasName}'.");
                _logger.LogError(ex, msg);
                throw new APIErrorException(500, msg);
            }

            if (!response.IsValid)
            {
                _logger.LogError($"Invalid response when searching for query '{searchText}', contains '{matchType}', size '{size}'.");
                throw new APIErrorException(500, "errors occured");
            }

            List<Suggestion> retVal = new List<Suggestion>(response.Documents);

            return retVal.ToArray();
        }

        /// <summary>
        /// Builds the SearchRequest for terms beginning with the search text.
        /// </summary>
        /// <param name="index">The index which will be searched against.</param>
        /// <param name="query">The text to search for.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        private SearchRequest BuildBeginRequest(Indices index, string query, int size,
                DrugResourceType[] includeResourceTypes,
                    TermNameType[] includeNameTypes,
                    TermNameType[] excludeNameTypes
        )
        {
            SearchRequest request = new SearchRequest(index)
            {
                Query = new BoolQuery
                {
                    Must = new QueryContainer[]
                    {
                        new PrefixQuery {Field = "name", Value = query },
                        new TermsQuery { Field = "type", Terms = includeResourceTypes.Select(p => p.ToString()) },
                        new TermsQuery { Field = "term_name_type", Terms = includeNameTypes.Select(p => p.ToString()) }
                    },
                    MustNot = new QueryContainer[]
                    {
                        new TermsQuery { Field = "term_name_type", Terms = excludeNameTypes.Select(p => p.ToString()) }
                    },
                    Filter = new QueryContainer[]
                    {
                        new ScriptQuery
                        {
                            Script = new InlineScript($"doc['name'].value.length() <= {_apiOptions.Autosuggest.MaxSuggestionLength}")
                        }
                    }
                },
                Sort = new List<ISort>
                {
                    new FieldSort { Field = "name" }
                },
                Source = new SourceFilter
                {
                    Includes = new string[]{"term_id", "name"}
                },
                Size = size
            };

            return request;
        }

        /// <summary>
        /// Builds the SearchRequest for terms containing with the search text.
        /// </summary>
        /// <param name="index">The index which will be searched against.</param>
        /// <param name="query">The text to search for.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        private SearchRequest BuildContainsRequest(Indices index, string query, int size,
                DrugResourceType[] includeResourceTypes,
                    TermNameType[] includeNameTypes,
                    TermNameType[] excludeNameTypes
        )
        {
            SearchRequest request = new SearchRequest(index)
            {
                Query = new BoolQuery
                {
                    Must = new QueryContainer[]
                    {
                        new MatchPhraseQuery { Field = "name._autocomplete", Query = query.ToString() },
                        new MatchQuery { Field = "name._contain", Query = query.ToString() },
                        new TermsQuery { Field = "type", Terms = includeResourceTypes.Select(p => p.ToString()) },
                        new TermsQuery {Field = "term_name_type", Terms = includeNameTypes.Select(p => p.ToString()) }
                    },
                    MustNot = new QueryContainer[]
                    {
                        new PrefixQuery { Field = "name", Value = query },
                        new TermsQuery { Field = "term_name_type", Terms = excludeNameTypes.Select(p => p.ToString())}
                    },
                    Filter = new QueryContainer[]
                    {
                        new ScriptQuery
                        {
                            Script = new InlineScript($"doc['name'].value.length() <= {_apiOptions.Autosuggest.MaxSuggestionLength}")
                        }
                    }
                }
                ,
                Sort = new List<ISort>
                {
                    new FieldSort { Field = "name" }
                },
                Source = new SourceFilter
                {
                    Includes = new string[] { "term_id", "name" }
                },
                Size = size
            };

            return request;
        }

        /// <summary>
        /// Checks whether the underlying data service is in a healthy condition.
        /// </summary>
        /// <returns>True if the data store is operational, false otherwise.</returns>
        public async Task<bool> GetIsHealthy()
        {
            // Use the cluster health API to verify that the index is functioning.
            // Maps to https://<SERVER_NAME>/_cluster/health/<INDEX_NAME>?pretty (or other server)
            //
            // References:
            // https://www.elastic.co/guide/en/elasticsearch/reference/master/cluster-health.html
            // https://github.com/elastic/elasticsearch/blob/master/rest-api-spec/src/main/resources/rest-api-spec/api/cluster.health.json#L20

            ClusterHealthResponse response;
            try
            {
                Indices index = Indices.Index(new string[] { _apiOptions.AliasName });
                response = await _elasticClient.Cluster.HealthAsync(index);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking ElasticSearch health for index '{_apiOptions.AliasName}'.");
                return false;
            }

            if (!response.IsValid)
            {
                _logger.LogError($"Error checking ElasticSearch health for index '{_apiOptions.AliasName}'.");
                _logger.LogError($"Returned debug info: {response.DebugInformation}.");
                return false;
            }

            if (response.Status != Health.Green
                && response.Status != Health.Yellow)
            {
                _logger.LogError($"Elasticsearch not healthy. Index status is '{response.Status}'.");
                return false;
            }

            return true;
        }
    }
}