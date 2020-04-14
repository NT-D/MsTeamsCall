using System;
using CseSample;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using Moq.Protected;

namespace MsTeamsCall.Tests
{
    public class CallFunctionTest
    {
        string _accessToken = "dummyAccessToken";

        [Fact]
        public async Task CallFunction_request_returns200OK()
        {
            // Arrange
            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(t => t.FetchAccessTokenByTenantId(It.IsAny<string>()))
                .ReturnsAsync(_accessToken);

            var callFunction = new CallFunction(tokenServiceMock.Object);

            // Act
            var result = await callFunction.Run(new Mock<HttpRequest>().Object, new Mock<ILogger>().Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CallFunction_request_mayThrowException()
        {
            // Arrange
            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(t => t.FetchAccessTokenByTenantId(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var callFunction = new CallFunction(tokenServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await callFunction.Run(new Mock<HttpRequest>().Object, new Mock<ILogger>().Object));
        }
    }
}