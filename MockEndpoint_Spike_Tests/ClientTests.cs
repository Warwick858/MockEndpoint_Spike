using MockEndpoint_Spike;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MockEndpoint_Spike_Tests
{
    [TestFixture]
    public class ClientTests
    {
        private Client _testClient = null;
        private HttpClient _httpClient = null;
        private Mock<HttpMessageHandler> _handler = null;

        [Test]
        [TestCase("123456")]
        public void GetData_HappyPath(string id)
        {
            //Arrange
            SetupMockedHttpHandler("{\"success\": true, \"messages\": [\"Success\"]}");
            _httpClient = new HttpClient(_handler.Object);
            _testClient = new Client(_httpClient);

            //Act
            var result = _testClient.GetData(id);

            //Assert
            Assert.IsTrue(Validate(1, $"test/{id}", HttpMethod.Get));
            Assert.IsTrue(result.Success);
        }

        public void SetupMockedHttpHandler(string responseContent)
        {
            _handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _handler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(responseContent),
               }).Verifiable();
        }

        public bool Validate(int invocations, string path, HttpMethod method, string parameter = "")
        {
            var expectedUri = new Uri($"http://www.test.com/{path}{parameter}");

            try
            {
                _handler.Protected().Verify
                (
                   "SendAsync",
                   Times.Exactly(invocations),
                   ItExpr.Is<HttpRequestMessage>
                   (
                       req => req.Method == method && req.RequestUri == expectedUri
                   ),
                   ItExpr.IsAny<CancellationToken>()
                );
            }
            catch (MockException)
            {
                return false;
            }

            return true;
        }
    }
}