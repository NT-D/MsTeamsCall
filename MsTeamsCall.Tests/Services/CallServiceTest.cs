using System.Collections.Generic;
using CseSample.Services;
using Microsoft.Graph;
using Moq;
using Xunit;

namespace CseSample.Tests
{
    public class CallServiceTest
    {
        private string[] _userIds = new string[] { "id1", "id2" };
        private string _tenantId = "tenantId";
        private string _accessToken = "accessToken";

        [Fact]
        public async void StartGroupCall_ExpectedInput_ReturnTrue()
        {
            // Arrange
            var graphClientMock = new Mock<IGraphServiceClient>();
            graphClientMock.Setup(g => g.Communications.Calls.Request(It.IsAny<List<HeaderOption>>()).AddAsync(It.IsAny<Call>()))
                .ReturnsAsync(new Mock<Call>().Object);

            var callServiceMock = new CallService(graphClientMock.Object);

            // Act
            var result = await callServiceMock.StartGroupCallWithSpecificMembers(_userIds, _tenantId, _accessToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void StartGroupCall_ExpectedInput_ThrowsException()
        {
            // Arrange
            var graphClientMock = new Mock<IGraphServiceClient>();
            graphClientMock.Setup(g => g.Communications.Calls.Request(It.IsAny<List<HeaderOption>>()).AddAsync(It.IsAny<Call>()))
                .ThrowsAsync(new ServiceException(new Mock<Error>().Object));

            var callServiceMock = new CallService(graphClientMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(async () => await callServiceMock.StartGroupCallWithSpecificMembers(_userIds, _tenantId, _accessToken));
        }
    }
}