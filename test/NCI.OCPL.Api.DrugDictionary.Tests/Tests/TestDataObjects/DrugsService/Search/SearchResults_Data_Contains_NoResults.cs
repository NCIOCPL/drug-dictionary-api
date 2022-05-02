namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class SearchResults_Data_Contains_NoResults : SearchResults_Data_Base
    {
        public override MatchType MatchType => MatchType.Contains;

        public override string ResponseBody =>
            @"{
                ""took"": 3,
                ""timed_out"": false,
                ""_shards"": {
                    ""total"": 1,
                    ""successful"": 1,
                    ""skipped"": 0,
                    ""failed"": 0
                },
                ""hits"": {
                    ""total"": {
                        ""value"": 0,
                        ""relation"": ""eq""
                    },
                    ""max_score"": null,
                    ""hits"": []
                }
            }";


        public override DrugTermResults ExpectedResult =>
            new DrugTermResults
            {
                Results = new DrugTerm[0],
                Meta = new ResultsMetadata
                {
                    TotalResults = 0,
                    From = 0
                }
            };
    }
}