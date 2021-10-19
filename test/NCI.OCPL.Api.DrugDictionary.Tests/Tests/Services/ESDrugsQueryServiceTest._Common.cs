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

using NCI.OCPL.Api.Common.Testing;
using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    ///  Tests to verify the structure of requests to Elasticsearch.
    /// </summary>
    public class ESDrugsQueryServiceTest_Common
    {

        /// <summary>
        /// Mock Elasticsearch configuraiton options.
        /// </summary>
        protected IOptions<DrugDictionaryAPIOptions> GetMockOptions()
        {
            Mock<IOptions<DrugDictionaryAPIOptions>> clientOptions = new Mock<IOptions<DrugDictionaryAPIOptions>>();
            clientOptions
                .SetupGet(opt => opt.Value)
                .Returns(new DrugDictionaryAPIOptions()
                {
                    AliasName = "drugv1"
                }
            );

            return clientOptions.Object;
        }
    }
}