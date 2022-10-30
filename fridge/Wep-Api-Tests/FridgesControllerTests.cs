using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using FakeItEasy;
using fridge.Controllers;
using fridge.Models;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Diagnostics.Contracts;

namespace Wep_Api_Tests
{
    public class FridgesControllerTests
    {

        private FridgesController _fridgesController;
        private IRepositoryManager _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        public FridgesControllerTests()
        {
            //Dependencies
            _repository = A.Fake<IRepositoryManager>();
            _logger = A.Fake<ILoggerManager>();
            _mapper = A.Fake<IMapper>();

            //SUT
            _fridgesController = new FridgesController(_repository, _logger, _mapper);
        }

        [Fact]
        public async Task FridgesController_GetAllFridges_ReturnsOkResultAsync()
        {
            //Arrange
            
            //Act 
            var okResult = await _fridgesController.GetAllFridges();

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        //Tests for GetFridge
        [Fact]
        public async Task FridgesController_GetFridge_ReturnOkSuccessAsync()
        {
            var testGuid = new Guid("CB574013-622D-442A-D447-08DAB05F727F");

            var okResult = await _fridgesController.GetFridge(testGuid);

            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task FridgesController_GetFridge_NotFoundFridge()
        {
            var testGuid = Guid.NewGuid();
                   
        }
        
        // Test for GetFridgesCollection
        [Fact]
        public async Task FridgesController_GetFridgesCollection_ReturnRigthResponse()
        {
            var testGuids = new List<Guid> { new Guid("CB574013-622D-442A-D447-08DAB05F727F"),
                new Guid("c020d1b2-7187-46c8-cfa7-08dab66f331b") };

            var okResult = await _fridgesController.GetFridgesCollectionAsync(testGuids);

            Assert.IsAssignableFrom<IActionResult>(okResult);
        }

        [Fact]
        public async Task FridgesController_GetFridgesCollection_ReturnNotFound()
        {
            var testGuids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid()};

            var notFound = await _fridgesController.GetFridgesCollectionAsync(testGuids);

            Assert.IsAssignableFrom<NotFoundResult>(notFound);
        }

        [Fact]
        public async Task FridgesController_GetFridgesController_ReturnBadRequest()
        {
            IEnumerable<Guid>? testGuids = null;

            var badRequest = await _fridgesController.GetFridgesCollectionAsync(testGuids);

            Assert.IsAssignableFrom<BadRequestObjectResult>(badRequest);
        }

        // Tests for CreateFridges 
        [Fact]
        public async Task FridgesController_CreateFridges_ReturnUprocessableRequest()
        {
            var nameMissingItem = new FridgesForCreationDto {

                Owner_Name = "Fri",
                ModelId = Guid.NewGuid(),   
            };
            _fridgesController.ModelState.AddModelError("Name", "Required");

            var badResponse = await _fridgesController.CreateFridges(nameMissingItem);

            Assert.IsType<UnprocessableEntityObjectResult>(badResponse); 
        }

        [Fact]
        public async Task FridgesController_CreateFridges_ReturnsCreatedResponse()
        {
            var testItem = new FridgesForCreationDto
            { 
                Name = "Wet",
                Owner_Name = "Nal",
                ModelId = Guid.NewGuid()
            };

            var createdResponse = await _fridgesController.CreateFridges(testItem);

            Assert.IsType<CreatedAtRouteResult>(createdResponse);
        }

        [Fact]
        public async Task FridgesController_CreateFridges_ReturnResponseHasCreatedItem()
        {
            var testItem = new FridgesForCreationDto
            {
                Name = "Wet",
                Owner_Name = "Nal",
                ModelId = Guid.NewGuid()
            };

            var result = await _fridgesController.CreateFridges(testItem);

            Assert.NotNull(result);
            var objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            var model = Assert.IsAssignableFrom<FridgesDto>(objectResult.Value);
            Assert.NotNull(model);
        }

        // Tests for CreateFridgeCollection
        [Fact]
        public async Task FridgesController_CreateFridgesCollection_ReturnResponseHasCreatedItem()
        {
            var testItem = new List<FridgesForCreationDto>
            {
                new FridgesForCreationDto
                {
                     Name = "Wet",
                    Owner_Name = "Nal",
                    ModelId = Guid.NewGuid()
                },
                new FridgesForCreationDto
                {
                    Name = "Cvvt",
                    Owner_Name = "Mag",
                    ModelId = Guid.NewGuid()
                }
            };

            var result = await _fridgesController.CreateFridgesCollection(testItem);

            Assert.NotNull(result);
            var objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<FridgesDto>>(objectResult.Value);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task FridgeControllers_CreateFridgeCollection_ReturnResponseCreatedResponse()
        {
            var testItem = new List<FridgesForCreationDto>
            {
                new FridgesForCreationDto
                {
                     Name = "Wet",
                    Owner_Name = "Nal",
                    ModelId = Guid.NewGuid()
                },
                new FridgesForCreationDto
                {
                    Name = "Cvvt",
                    Owner_Name = "Mag",
                    ModelId = Guid.NewGuid()
                }
            };

            var createdResponse = await _fridgesController.CreateFridgesCollection(testItem);

            Assert.IsType<CreatedAtRouteResult>(createdResponse);
        }

        [Fact]
        public async Task FridgeCotrollers_CreateFridgeCollection_ReturnUnproccessableData()
        {
            var testItem = new List<FridgesForCreationDto>
            {
                new FridgesForCreationDto
                {
                     Name = "Wet"
                },
                new FridgesForCreationDto
                {
                    Name = "Cvvt"
                }
            };

            _fridgesController.ModelState.AddModelError("Owner_Name", "Required");
            _fridgesController.ModelState.AddModelError("ModelId", "Required");

            var badResponse = await _fridgesController.CreateFridgesCollection(testItem);

            Assert.IsType<UnprocessableEntityObjectResult>(badResponse);
        }

        //Test for UpdateFridge
        [Fact]
        public async Task FridgesController_UpdateFridge_ReturnOkSuccess()
        {
            var testGuid = new Guid("CB574013-622D-442A-D447-08DAB05F727F");

            var testItem = new FridgeForUpdateDto()
            {
                Name = "HG",
                Owner_Name = "Vania",
                ModelId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991879")
            };

            var okResult = await _fridgesController.UpdateFridge(testGuid, testItem) as OkObjectResult;

            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task FridgesController_UpdateFridge_ReturnBadRequest()
        {
            var testGuid = new Guid("CB574013-622D-442A-D447-08DAB05F727F");

            FridgeForUpdateDto? testItem = null;

            var badRequest = await _fridgesController.UpdateFridge(testGuid, testItem);

            Assert.IsAssignableFrom<BadRequestObjectResult>(badRequest);
        }

        [Fact]
        public async Task FridgesController_UpdateFridge_ReturnUnproccessableData()
        {
            var testGuid = new Guid("CB574013-622D-442A-D447-08DAB05F727F");

            var testItem = new FridgeForUpdateDto()
            {
                Owner_Name = "Vania",
                ModelId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991879")
            };

            _fridgesController.ModelState.AddModelError("Name", "Required");

            var badRequest = await _fridgesController.UpdateFridge(testGuid, testItem);

            Assert.IsAssignableFrom<UnprocessableEntityObjectResult>(badRequest);
        }

        // Test for DeleteFridge
        [Fact]
        public async Task FridgesController_DeleteFridge_ReturnOkSuccess()
        {
            var testGuid = new Guid("CB574013-622D-442A-D447-08DAB05F727F");

            var okResult = await _fridgesController.DeleteFridge(testGuid);

            Assert.IsType<OkResult>(okResult);
        }
    }
}
