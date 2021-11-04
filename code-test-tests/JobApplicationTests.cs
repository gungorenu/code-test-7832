using System;
using CodeTest;
using Moq;
using NUnit.Framework;

namespace CodeTestTests
{
    public class JobApplicationTests
    {

        // NOTE: I dont like below test which tests almost everything about Initialize. it should be divided into multiple small tests
        // it is left here for showcase about how much mocked a test can be 
        // example: Initialize_MustCallApi, Initialize_NextSteps_AfterSuccess, Initialize_UserKeySet ...
        [Test]
        public void Initialize_MustCallApi()
        {
            // arrange
            Mock<IConsole> consoleMock = new Mock<IConsole>();
            Mock<IWebClient> clientMock = new Mock<IWebClient>();
            Mock<IRequest> requestMock = new Mock<IRequest>();

            var model = new Newtonsoft.Json.Linq.JObject();
            var response = new UserKeyApiResponseObject() { UserKey = "123asd" };

            consoleMock.Setup(f => f.InputObject(It.IsAny<object>())).Returns(model);
            clientMock.Setup(f => f.POST(It.IsAny<string>())).Returns(requestMock.Object);
            requestMock.Setup(f => f.AddJsonBodyParameter(model));
            requestMock.Setup(f => f.Execute<UserKeyApiResponseObject>()).Returns(response);

            IJobApplicationInternal jobApp = new JobApplication(consoleMock.Object, clientMock.Object);

            // act
            var nextSteps = jobApp.Initialize();

            // assert
            Assert.IsTrue(jobApp.IsInitialized, "Initialize should have succeeded");
            Assert.AreEqual(nextSteps, Operations.Exit | Operations.None | Operations.Register, "Initialize messed up next steps");
            Assert.AreEqual("123asd", jobApp.UserKey, "Initialize messed up and did not get proper user key");
        }

        [Test]
        public void Initialize_CannotReInitialize_Error()
        {
            // special test case for forcing it throw exception on initialize after first initialize is done
            // use reflection and set field "_userKey"
            // call initialize again, should throw exception
        }

        [Test]
        public void Initialize_CannotInitialize_AfterRegister_Error()
        {
            // special test case for forcing it throw exception on initialize after first register is done
            // use reflection and set field "_userKey"
            // call initialize again, should throw exception
        }

        // MORE TESTS? like above more tests can be added for each operation

    }
}
