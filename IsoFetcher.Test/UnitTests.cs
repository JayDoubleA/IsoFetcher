using IsoFetcher.Enums;
using IsoFetcher.Models;
using IsoFetcher.Services;
using IsoFetcher.Services.Interfaces;
using IsoFetcher.Test.DummyData;
using Moq;

namespace IsoFetcher.Test
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void ValidIsoCode_ReturnsExpectedModel()
        {
            var clientServiceMoq = new Mock<IWorldBankIsoRequestClientService>();
            var response = Task.FromResult(Response.ValidReseponse);
            clientServiceMoq.Setup(x => x.SendIsoRequest("nl")).Returns(response);
            clientServiceMoq.Setup(x => x.IsIsoCodeValid("nl")).Returns(true);

            var responseService = new WorldBankIsoResponseService();
            var loggingService = new DummyLoggingService();

            var processingService = new WorldBankIsoProcessingService(responseService, clientServiceMoq.Object, loggingService);

            var responseModel = processingService.ProcessIso("nl").Result; // yes, were calling it sync - VS2022 doesn't like async test methods

            Assert.AreEqual(Status.SuccessResponse, responseModel.ProcessingStatus);

            var data = responseModel.Data.First();

            Assert.AreEqual("Netherlands", data.Name);
            Assert.AreEqual("Amsterdam", data.CapitalCity);
            Assert.AreEqual("Europe & Central Asia", data.Region.Value);
            Assert.AreEqual("4.89095", data.Longitude);
            Assert.AreEqual("52.3738", data.Latitude);
        }

        [TestMethod]
        public void NotUsedIsoCode_ReturnsInvalidValueResponseStatus()
        {
            var clientServiceMoq = new Mock<IWorldBankIsoRequestClientService>();
            var response = Task.FromResult(Response.InvalidResponse);
            clientServiceMoq.Setup(x => x.SendIsoRequest("xx")).Returns(response);
            clientServiceMoq.Setup(x => x.IsIsoCodeValid("xx")).Returns(true);

            var responseService = new WorldBankIsoResponseService();
            var loggingService = new DummyLoggingService();

            var processingService = new WorldBankIsoProcessingService(responseService, clientServiceMoq.Object, loggingService);

            var responseModel = processingService.ProcessIso("xx").Result; // yes, were calling it sync - VS2022 doesn't like async test methods

            Assert.AreEqual(Status.InvalidValueResponse, responseModel.ProcessingStatus);
        }

        [TestMethod]
        public void InvalidIsoCode_ReturnsFailedValidationStatus()
        {
            var clientServiceMoq = new Mock<IWorldBankIsoRequestClientService>();
            var response = Task.FromResult(Response.InvalidResponse);
            clientServiceMoq.Setup(x => x.SendIsoRequest("xx")).Returns(response);
            clientServiceMoq.Setup(x => x.IsIsoCodeValid("xx")).Returns(false);

            var responseService = new WorldBankIsoResponseService();
            var loggingService = new DummyLoggingService();

            var processingService = new WorldBankIsoProcessingService(responseService, clientServiceMoq.Object, loggingService);

            var responseModel = processingService.ProcessIso("xx").Result; // yes, were calling it sync - VS2022 doesn't like async test methods

            Assert.AreEqual(Status.FailedValidation, responseModel.ProcessingStatus);
        }

        [TestMethod]
        public void RandomError_ReturnsErrorStatus()
        {
            var clientServiceMoq = new Mock<IWorldBankIsoRequestClientService>();
            var response = Task.FromResult(string.Empty);
            clientServiceMoq.Setup(x => x.SendIsoRequest("xx")).Throws(new Exception("Random hideous error"));
            clientServiceMoq.Setup(x => x.IsIsoCodeValid("xx")).Returns(true);

            var responseService = new WorldBankIsoResponseService();
            var loggingService = new DummyLoggingService();

            var processingService = new WorldBankIsoProcessingService(responseService, clientServiceMoq.Object, loggingService);

            var responseModel = processingService.ProcessIso("xx").Result; // yes, were calling it sync - VS2022 doesn't like async test methods

            Assert.AreEqual(Status.Error, responseModel.ProcessingStatus);
        }

        [TestMethod]
        public void EmptyResponse_ReturnsErrorStatus()
        {
            var clientServiceMoq = new Mock<IWorldBankIsoRequestClientService>();
            var response = Task.FromResult(string.Empty);
            clientServiceMoq.Setup(x => x.SendIsoRequest("xx")).Returns(response);
            clientServiceMoq.Setup(x => x.IsIsoCodeValid("xx")).Returns(true);

            var responseService = new WorldBankIsoResponseService();
            var loggingService = new DummyLoggingService();

            var processingService = new WorldBankIsoProcessingService(responseService, clientServiceMoq.Object, loggingService);

            var responseModel = processingService.ProcessIso("xx").Result; // yes, were calling it sync - VS2022 doesn't like async test methods

            Assert.AreEqual(Status.Error, responseModel.ProcessingStatus);
        }

        [TestMethod]
        public void NullResponse_ReturnsSomethingOddStatus()
        {
            var clientServiceMoq = new Mock<IWorldBankIsoRequestClientService>();
            var response = Task.FromResult((string)null);
            clientServiceMoq.Setup(x => x.SendIsoRequest("xx")).Returns(response);
            clientServiceMoq.Setup(x => x.IsIsoCodeValid("xx")).Returns(true);

            var responseService = new WorldBankIsoResponseService();
            var loggingService = new DummyLoggingService();

            var processingService = new WorldBankIsoProcessingService(responseService, clientServiceMoq.Object, loggingService);

            var responseModel = processingService.ProcessIso("xx").Result; // yes, were calling it sync - VS2022 doesn't like async test methods

            Assert.AreEqual(Status.SomethingOdd, responseModel.ProcessingStatus);
        }

        [TestMethod]
        public void IsoInputValidator_Validates()
        {
            var clientService = new WorldBankIsoRequestClientService();

            // valid two letters
            Assert.IsTrue(clientService.IsIsoCodeValid("aa"));
            Assert.IsTrue(clientService.IsIsoCodeValid("Aa"));
            Assert.IsTrue(clientService.IsIsoCodeValid("aA"));
            Assert.IsTrue(clientService.IsIsoCodeValid("AA"));

            // valid three letters
            Assert.IsTrue(clientService.IsIsoCodeValid("aaa"));
            Assert.IsTrue(clientService.IsIsoCodeValid("Aaa"));
            Assert.IsTrue(clientService.IsIsoCodeValid("aAa"));
            Assert.IsTrue(clientService.IsIsoCodeValid("aaA"));
            Assert.IsTrue(clientService.IsIsoCodeValid("AAa"));
            Assert.IsTrue(clientService.IsIsoCodeValid("AaA"));
            Assert.IsTrue(clientService.IsIsoCodeValid("aAA"));
            Assert.IsTrue(clientService.IsIsoCodeValid("AAA"));

            // not valid - too short
            Assert.IsFalse(clientService.IsIsoCodeValid("a"));

            // not valid - too long
            Assert.IsFalse(clientService.IsIsoCodeValid("aaaa"));

            // not valid - non letters
            Assert.IsFalse(clientService.IsIsoCodeValid("a1"));
            Assert.IsFalse(clientService.IsIsoCodeValid("a1a"));
            Assert.IsFalse(clientService.IsIsoCodeValid("a!"));
            Assert.IsFalse(clientService.IsIsoCodeValid("a!a"));
        }
    }
}