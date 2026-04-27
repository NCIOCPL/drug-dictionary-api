using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.DrugDictionary.Models;

namespace NCI.OCPL.Api.DrugDictionary.Services
{
    /// <summary>
    /// Elasticsearch implementation of the service for retrieving suggestions for
    /// DrugTerm objects.
    /// </summary>
    public class ESAutosuggestQueryService : IAutosuggestQueryService
    {

        /// <summary>
        /// The elasticsearch client
        /// </summary>
        private ElasticsearchClient _elasticClient;

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
        public ESAutosuggestQueryService(ElasticsearchClient client,
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
            SearchResponse<Suggestion> response = null;

            try
            {
                SearchRequest request;
                switch (matchType)
                {
                    default:
                    case MatchType.Begins:
                        request = BuildBeginRequest(searchText, size, includeResourceTypes, includeNameTypes, excludeNameTypes);
                        break;
                    case MatchType.Contains:
                        request = BuildContainsRequest(searchText, size, includeResourceTypes, includeNameTypes, excludeNameTypes);
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

            if (!response.IsValidResponse)
            {
                string msg = $"Invalid response when searching for query '{searchText}', contains '{matchType}', size '{size}'."
                    .Replace(Environment.NewLine, String.Empty);
                _logger.LogError(msg);
                throw new APIErrorException(500, "errors occurred");
            }

            List<Suggestion> retVal = new List<Suggestion>(response.Documents);

            return retVal.ToArray();
        }

        /// <summary>
        /// Builds the SearchRequest for terms beginning with the search text.
        /// </summary>
        /// <param name="query">The text to search for.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        private SearchRequest BuildBeginRequest(string query, int size,
                DrugResourceType[] includeResourceTypes,
                    TermNameType[] includeNameTypes,
                    TermNameType[] excludeNameTypes
        )
        {
            var mustClauses = new List<Query>
            {
                new PrefixQuery { Field = "name", Value = query },
                new TermsQuery { Field = "type", Terms = new TermsQueryField(includeResourceTypes.Select(p => (FieldValue)p.ToString()).ToList()) }
            };
            if (includeNameTypes.Length > 0)
            {
                mustClauses.Add(new TermsQuery { Field = "term_name_type", Terms = new TermsQueryField(includeNameTypes.Select(p => (FieldValue)p.ToString()).ToList()) });
            }

            var mustNotClauses = new List<Query>();
            if (excludeNameTypes.Length > 0)
            {
                mustNotClauses.Add(new TermsQuery { Field = "term_name_type", Terms = new TermsQueryField(excludeNameTypes.Select(p => (FieldValue)p.ToString()).ToList()) });
            }

            SearchRequest request = new SearchRequest(this._apiOptions.AliasName)
            {
                Query = new BoolQuery
                {
                    Must = mustClauses.ToArray(),
                    MustNot = mustNotClauses.Count > 0 ? mustNotClauses.ToArray() : null,
                    Filter = new Query[]
                    {
                        new ScriptQuery
                        {
                            Script = new Script { Source = $"doc['name'].value.length() <= {_apiOptions.Autosuggest.MaxSuggestionLength}" }
                        }
                    }
                },
                Sort = new List<SortOptions>
                {
                    new SortOptions { Field = new FieldSort { Field = "name" } }
                },
                Source = new SourceConfig(new SourceFilter
                {
                    Includes = new Field[] { "term_id", "name" }
                }),
                Size = size
            };

            return request;
        }

        /// <summary>
        /// Builds the SearchRequest for terms containing the search text.
        /// </summary>
        /// <param name="query">The text to search for.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        private SearchRequest BuildContainsRequest(string query, int size,
                DrugResourceType[] includeResourceTypes,
                    TermNameType[] includeNameTypes,
                    TermNameType[] excludeNameTypes
        )
        {
            var mustClauses = new List<Query>
            {
                new MatchPhraseQuery { Field = "name._autocomplete", Query = query.ToString() },
                new MatchQuery { Field = "name._contain", Query = query.ToString() },
                new TermsQuery { Field = "type", Terms = new TermsQueryField(includeResourceTypes.Select(p => (FieldValue)p.ToString()).ToList()) }
            };
            if (includeNameTypes.Length > 0)
            {
                mustClauses.Add(new TermsQuery { Field = "term_name_type", Terms = new TermsQueryField(includeNameTypes.Select(p => (FieldValue)p.ToString()).ToList()) });
            }

            var mustNotClauses = new List<Query>
            {
                new PrefixQuery { Field = "name", Value = query }
            };
            if (excludeNameTypes.Length > 0)
            {
                mustNotClauses.Add(new TermsQuery { Field = "term_name_type", Terms = new TermsQueryField(excludeNameTypes.Select(p => (FieldValue)p.ToString()).ToList()) });
            }

            SearchRequest request = new SearchRequest(this._apiOptions.AliasName)
            {
                Query = new BoolQuery
                {
                    Must = mustClauses.ToArray(),
                    MustNot = mustNotClauses.ToArray(),
                    Filter = new Query[]
                    {
                        new ScriptQuery
                        {
                            Script = new Script { Source = $"doc['name'].value.length() <= {_apiOptions.Autosuggest.MaxSuggestionLength}" }
                        }
                    }
                },
                Sort = new List<SortOptions>
                {
                    new SortOptions { Field = new FieldSort { Field = "name" } }
                },
                Source = new SourceConfig(new SourceFilter
                {
                    Includes = new Field[] { "term_id", "name" }
                }),
                Size = size
            };

            return request;
        }

    }
}