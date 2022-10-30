using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using FakeItEasy;
using fridge.Controllers;
using fridge.Models;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
//using Xunit;
using FluentAssertions;
//using System.Web.Http.Results;
//using OkResult = System.Web.Http.Results.OkResult;

namespace fridgeTests.ControllersTests
{
    [TestClass]
    public class FridgeControllerTests
    {
        private FridgesController _fridgesController;
        private IRepositoryManager _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        public FridgeControllerTests()
        {
            //Dependencies
            _repository = A.Fake<IRepositoryManager>();
            _logger = A.Fake<ILoggerManager>();
            _mapper = A.Fake<IMapper>();

            //SUT
            _fridgesController = new FridgesController(_repository, _logger, _mapper);
        }

        [TestMethod]
        public void FridgeController_GetAllFridges_ReturnSuccess()
        {
            //Arrange
            var fridges = A.Fake<IEnumerable<Fridges>>();
            A.CallTo(() => _repository.Fridges.GetAllFridgesAsync(false)).Returns(fridges);
            var fridgesDto = A.CallTo(() => _mapper.Map<IEnumerable<FridgesDto>>(fridges));
            
            //Act 
            var resultOk = (_fridgesController.GetAllFridges().GetAwaiter().GetResult()) as OkObjectResult;

            //Assert
            Assert.AreEqual(200, resultOk.StatusCode);
        }

        [TestMethod]
        public void FridgeController_GetFridge_ReturnSuccess()
        {
            var result = _fridgesController.GetFridge(Guid.NewGuid());
            var resultOk = (_fridgesController.GetFridge(Guid.NewGuid()).GetAwaiter().GetResult()) as OkObjectResult;
            
            Assert.AreEqual(200, resultOk.StatusCode);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FridgeController_GetFridgesCollectionAsync_ReturnSuccess()
        {
            var ids = new List<Guid>();
            
            var result = _fridgesController.GetFridgesCollectionAsync(ids);        
            var resultOk = (_fridgesController.GetFridgesCollectionAsync(ids).GetAwaiter().GetResult()) as OkObjectResult;
           
            Assert.AreEqual(200, resultOk.StatusCode);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FridgeController_CreateFridges_ReturnsSuccess()
        {
            var expectedFridge = new FridgesForCreationDto() {
                Name = "Kolia", ModelId = Guid.NewGuid(), Owner_Name = "QWR"
            };
            
            var result = _fridgesController.CreateFridges(expectedFridge);            
        }
    }
}
