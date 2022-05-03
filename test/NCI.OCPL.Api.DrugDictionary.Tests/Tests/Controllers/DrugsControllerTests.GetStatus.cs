using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Testing;
using System;
using System.Threading.Tasks;

using Moq;
using Xunit;

using NCI.OCPL.Api.Common;

namespace NCI.OCPL.Api.DrugDictionary.Controllers.Tests
{
    public class DrugsControllerTests_GetStatus
    {
        /// <summary>
        /// Verify the controller gracefully handles errors in the service layer.
        /// </summary>
        [Theory]
        [InlineData(typeof(APIInternalException))]
        [InlineData(typeof(ArgumentNullException))]
        public async void GetStatus_ServiceErrors(Type exceptionType)
        {
            // In order to test throwing more than a single exception type, we need to to pass a type
            // and construct it rather than throwing with a new.
            Type[] constructorSignature = { typeof(string) };
            System.Reflection.ConstructorInfo constructor = exceptionType.GetConstructor(constructorSignature);
            Exception exception = (Exception)constructor.Invoke(new object[] { "An error message." });

            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.GetIsHealthy()
            )
            .Throws(exception);


            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);

            // This route is not expected to throw unhandled exceptions.
            ActionResult<string> ar = await controller.GetStatus();

            Assert.Null(ar.Value);
            ObjectResult objr = Assert.IsType<ObjectResult>(ar.Result);
            Assert.Equal(500, objr.StatusCode);
            Assert.Equal(DrugsController.UNHEALTHY_STATUS, objr.Value);
        }

        /// <summary>
        /// Verify the controller responds correctly when the service is unhealthy.
        /// </summary>
        [Fact]
        public async void GetStatus_ReportUnhealthy()
        {
            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.GetIsHealthy()
            )
            .Returns(Task.FromResult(false));


            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);

            // This route is not expected to throw unhandled exceptions.
            ActionResult<string> ar = await controller.GetStatus();

            Assert.Null(ar.Value);
            ObjectResult objr = Assert.IsType<ObjectResult>(ar.Result);
            Assert.Equal(500, objr.StatusCode);
            Assert.Equal(DrugsController.UNHEALTHY_STATUS, objr.Value);
        }

        /// <summary>
        /// Verify the controller responds correctly when the service is healthy.
        /// </summary>
        [Fact]
        public async void GetStatus_ReportHealthy()
        {
            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.GetIsHealthy()
            )
            .Returns(Task.FromResult(true));

            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);

            ActionResult<string> ar = await controller.GetStatus();

            Assert.Null(ar.Result);
            Assert.Equal(DrugsController.HEALTHY_STATUS, ar.Value);
        }

    }
}