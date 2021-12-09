namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class Expand_DataLoading_NoResults : Expand_DataLoading_Base
    {
        public override char Letter => 'f';

        public override int From => 0;

        public override int Size =>  100;

        public override string ResponseBody =>
            @"{
                ""took"": 2,
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

        public override DrugTermResults ExpectedResult => new
            DrugTermResults
            {
                Meta = new ResultsMetadata
                {
                    From = 0,
                    TotalResults = 0
                },
                Results = new IDrugResource[0]
            };
    }
}