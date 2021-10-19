using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging.Testing;
using Moq;
using Xunit;

using NCI.OCPL.Api.DrugDictionary.Controllers;
using NCI.OCPL.Api.Common;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// Tests for the DrugsController's GetById method.
    /// </summary>
    public partial class DrugsControllerTests
    {
        /// <summary>
        /// Verify correct handling of invalid IDs.
        /// </summary>
        [Theory]
        [InlineData(new object[] { 0 })]
        [InlineData(new object[] { -65537 })]
        [InlineData(new object[] { long.MinValue })]
        public async void GetById_InvalidId(long id)
        {
            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.GetById(
                    It.IsAny<long>()
                )
            )
            .Returns(Task.FromResult(new DrugTerm()));

            // Call the controller, we don't care about the actual return value.
            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);

            var exception = await Assert.ThrowsAsync<APIErrorException>(
                () => controller.GetById(id)
            );

            Assert.Equal(DrugsController.INVALID_ID_MESSAGE, exception.Message);
            Assert.Equal(400, exception.HttpStatusCode);
        }

        /// <summary>
        /// Verify return of a 404 status on unknown ID.
        /// </summary>
        [Fact]
        public async void GetById_NotFound()
        {
            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.GetById(
                    It.IsAny<long>()
                )
            )
            .Returns(Task.FromResult((DrugTerm)null));

            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);

            APIErrorException ex = await Assert.ThrowsAsync<APIErrorException>(
                () => controller.GetById(12345)
            );
            Assert.Equal(404, ex.HttpStatusCode);
            Assert.Equal(DrugsController.NOT_FOUND_MESSAGE, ex.Message);
        }

        /// <summary>
        /// Verify the controller gracefully handles errors in the service layer.
        /// </summary>
        [Theory]
        //[InlineData(typeof(APIInternalException))]
        [InlineData(typeof(ArgumentNullException))]
        public async void GetById_ServiceErrors(Type exceptionType)
        {
            // In order to test throwing more than a single exception type, we need to to pass a type
            // and construct it rather than throwing with a new.
            Type[] constructorSignature = { typeof(string) };
            System.Reflection.ConstructorInfo constructor = exceptionType.GetConstructor(constructorSignature);
            Exception exception = (Exception)constructor.Invoke(new object[] { "An error message." });

            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.GetById(
                    It.IsAny<long>()
                )
            )
            .Throws(exception);

            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);

            // No matter what the service throws, the controller should throw APIErrorException with a 500 status
            APIErrorException ex = await Assert.ThrowsAsync<APIErrorException>(
                () => controller.GetById(12345)
            );

            Assert.Equal(500, ex.HttpStatusCode);
            Assert.Equal(DrugsController.INTERNAL_ERROR_MESSAGE, ex.Message);
        }


        /// <summary>
        /// Cerify correct handling of a valid ID.
        /// </summary>
        [Fact]
        public async void GetById_ValidId()
        {
            const int theID = 37780;

            DrugTerm testTerm = new DrugTerm()
            {
                Aliases = new TermAlias[] {
                    new TermAlias()
                    {
                        Type = TermNameType.Synonym,
                        Name = "contrast dye, iodinated"
                    },
                    new TermAlias()
                    {
                        Type=TermNameType.LexicalVariant,
                        Name = "Iodinated Contrast Agent"
                    }
                },
                Definition = new Definition()
                {
                    Html = "A contrast agent containing an iodine-based dye used in many diagnostic imaging examinations, including computed tomography, angiography, and myelography. Check for <a ref=\"https://www.cancer.gov/about-cancer/treatment/clinical-trials/intervention/C28500\">active clinical trials</a> using this agent. (<a ref=\"https://ncit.nci.nih.gov/ncitbrowser/ConceptReport.jsp?dictionary=NCI%20Thesaurus&code=C28500\">NCI Thesaurus</a>)",
                    Text = "A contrast agent containing an iodine-based dye used in many diagnostic imaging examinations, including computed tomography, angiography, and myelography. Check for active clinical trials using this agent. (NCI Thesaurus)"
                },
                DrugInfoSummaryLink = null,
                FirstLetter = 'i',
                Name = "iodinated contrast dye",
                NCIConceptId = "C28500",
                NCIConceptName = "Iodinated Contrast Agent",
                PrettyUrlName = "iodinated-contrast-agent",
                TermId = 37780,
                TermNameType = TermNameType.PreferredName,
                Type = DrugResourceType.DrugTerm
            };

            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.GetById(
                    It.IsAny<long>()
                )
            )
            .Returns(Task.FromResult(testTerm));

            // Call the controller, we don't care about the actual return value.
            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);
            DrugTerm actual = await controller.GetById(theID);

            Assert.Equal(testTerm, actual);

            // Verify that the query layer is called:
            //  a) with the ID value.
            //  b) exactly once.
            querySvc.Verify(
                svc => svc.GetById(theID),
                Times.Once,
                $"ITermsQueryService::GetById() should be called once, with id = '{theID}"
            );
        }
    }
}