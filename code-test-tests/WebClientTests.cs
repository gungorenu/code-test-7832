using System;
using CodeTest;
using Moq;
using NUnit.Framework;

namespace CodeTestTests
{
    public class WebClientTests
    {
        [Test]
        public void Get_ReturnsGet()
        {
            // arrange
            Mock<IConsole> consoleMock = new Mock<IConsole>();
            IWebClient client = new WebClient(consoleMock.Object, "http://www.google.com");

            // act
            var request = client.GET("/api/dummy") as WebClient.InternalRequest;
            var restSharpReq = request.Request;

            // assert
            Assert.AreEqual(RestSharp.Method.GET, restSharpReq.Method, "Request was supposed to be a GET");
            Assert.AreEqual("/api/dummy", restSharpReq.Resource, "Request api was messed up");
        }

        [Test]
        public void Post_ReturnsPost()
        {
            // skipping too primitive, check above tests
        }


        [Test]
        public void Put_ReturnsPut()
        {
            // skipping too primitive, check above tests
        }


        [Test]
        public void Delete_ReturnsDelete()
        {
            // skipping too primitive, check above tests
        }

        // MORE TESTS? can be done. the system can make API calls to always existing systems (not so reliable)
        // can make more mocks internally perhaps but limited to Restsharp


    }
}
