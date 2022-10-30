using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using FakeItEasy;
using fridge.Controllers;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Wep_Api_Tests
{
    public class FridgeProductsControllerTests
    {
        private FridgeProductsController _fridgeProductsController;
        private IRepositoryManager _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        public FridgeProductsControllerTests()
        {
            //Dependencies
            _repository = A.Fake<IRepositoryManager>();
            _logger = A.Fake<ILoggerManager>();
            _mapper = A.Fake<IMapper>();

            //SUT
            _fridgeProductsController = new FridgeProductsController(_repository, _logger, _mapper);
        }

        // Test for GetFridgeProducts
        [Fact]
        public async Task FridgeProductsController_GetFridgeProducts_ReturnOkSuccess()
        {
            var testGuid = new Guid("CB574013-622D-442A-D447-08DAB05F727F");

            var okResult = await _fridgeProductsController.GetFridgeProducts(testGuid) as OkObjectResult;

            Assert.IsAssignableFrom<OkObjectResult>(okResult);
        }

        //Test for GetFridgeProduct
        [Fact]
        public async Task FridgeProductsController_GetFridgeProduct_ReturnOkSuccess()
        {
            var testGuidForFridge = new Guid("eb8f5b00-0583-49e9-c4b3-08dab978f38a");

            var testGuidForFridgeProduct = new Guid("65954913-d651-4c94-642f-08dab978f3b3");

            var okResult = await _fridgeProductsController.GetFridgeProduct(testGuidForFridge, testGuidForFridgeProduct);

            Assert.IsAssignableFrom<OkObjectResult>(okResult);
        }

        //Test for DeleteFridgeProducts
        [Fact]
        public async Task FridgeProductsController_DeleteFridgeProducts_ReturnNoContent()
        {
            var testGuidForFridge = new Guid("eb8f5b00-0583-49e9-c4b3-08dab978f38a");
            
            await _fridgeProductsController.DeleteFridgeProducts(testGuidForFridge);

            var noContent = await _fridgeProductsController.DeleteFridgeProducts(testGuidForFridge);

            Assert.IsAssignableFrom<NoContentResult>(noContent);
        }

        //Test for UPdateFridgeProductForFridge
        [Fact]
        public async Task FridgeProductsController_UpdateFridgeProductForFridge_ReturnNoContent()
        {
            var testGuidForFridge = new Guid("eb8f5b00-0583-49e9-c4b3-08dab978f38a");

            var testGuidForFridgeProduct = new Guid("65954913-d651-4c94-642f-08dab978f3b3");

            var fridgeProduct = new FridgeProductsForUpdateDto()
            {
                ProductId = new Guid("16ad116a-8289-4ce9-aa96-4d367dec6dd4"),
                Quantity = 20
            };

            var noContent = await _fridgeProductsController.UpdateFridgeProductForFridge(testGuidForFridge, testGuidForFridgeProduct, fridgeProduct);

            Assert.IsAssignableFrom<NoContentResult>(noContent);
        }

        //Test for CreateFridgeProductsFromSPForFridge 
        [Fact]
        public async Task FridgeProductsController_CreateFridgeProductsFromSPForFridge_ReturnRightResponse()
        {
            var testGuidForFridge =  new Guid("e8741532-ee29-4ff1-3579-08dab97d4ff7");

            var createdResponse = await _fridgeProductsController.CreateFridgeProductsFromSPForFridge(testGuidForFridge);

            Assert.IsAssignableFrom<CreatedAtActionResult>(createdResponse);
        }


        //Test for GetFridgeProductsCollection
        [Fact]
        public async Task FridgeProductsController_GetFridgeProductsCollection_ReturnRightResponse()
        {
            List<Guid> testGuids = new List<Guid>
            {
                new Guid("65954913-d651-4c94-642f-08dab978f3b3"), new Guid("7fd3cad8-c802-4df1-3c42-08dab97d504b")
            };

            var okResult = await _fridgeProductsController.GetFridgeProductsCollection(testGuids);

            Assert.IsAssignableFrom<IActionResult>(okResult);
        }

        // Test for CreateFridgeProductsCollection
        [Fact]
        public async Task FridgeProductsController_CreateFridgeProductsCollection_ReturnRightResponse()
        {
            var testGuidForFridge = new Guid("e8741532-ee29-4ff1-3579-08dab97d4ff7");

            var fridgeProductsCollection = new List<FridgeProductsForCreationDto>
            {
                new FridgeProductsForCreationDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 0,
                },
                new FridgeProductsForCreationDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 0
                }
            };

            var createdResponse = await _fridgeProductsController.CreateFridgeProductsCollection(testGuidForFridge, fridgeProductsCollection);

            Assert.IsAssignableFrom<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public async Task FridgeProductsController_CreateFridgeProductsCollection_ReturnBadRequest()
        {
            var testGuidForFridge = new Guid("e8741532-ee29-4ff1-3579-08dab97d4ff7");

            List<FridgeProductsForCreationDto>? fridgeProductsCollection = null;

            var badRequest = await _fridgeProductsController.CreateFridgeProductsCollection(testGuidForFridge, fridgeProductsCollection);

            Assert.IsAssignableFrom<BadRequestObjectResult>(badRequest);
        }


        // Test for CreateFridgeProductForFridge
        [Fact]
        public async Task FridgeProductsController_CreateFridgeProductForFridge_ReturnRightResponse()
        {
            var testGuid = new Guid("e8741532-ee29-4ff1-3579-08dab97d4ff7");

            var fridgeProduct = new FridgeProductsForCreationDto
            {
                ProductId = Guid.NewGuid(),
                Quantity = 0
            };

            var createdResponse = await _fridgeProductsController.CreateFridgeProductForFridge(testGuid, fridgeProduct);

            Assert.IsAssignableFrom<CreatedAtRouteResult>(createdResponse);           
        }

        [Fact]
        public async Task FridgeProductsController_CreateFridgeProductForFridge_ReturnBadRequest()
        {
            var testGuid = Guid.NewGuid();

            FridgeProductsForCreationDto? fridgeProduct = null;

            var badRequest = await _fridgeProductsController.CreateFridgeProductForFridge(testGuid, fridgeProduct);

            Assert.IsAssignableFrom<BadRequestObjectResult>(badRequest);
        }

        [Fact]
        public async Task FridgeProductsController_CreateFridgeProductForFridge_ReturnUnproccessableData()
        {
            var testGuid = new Guid("e8741532-ee29-4ff1-3579-08dab97d4ff7");

            var fridgeProduct = new FridgeProductsForCreationDto
            {
                Quantity = 0
            };

            _fridgeProductsController.ModelState.AddModelError("ProductId", "Required");

            var createdResponse = await _fridgeProductsController.CreateFridgeProductForFridge(testGuid, fridgeProduct);

            Assert.IsAssignableFrom<UnprocessableEntityObjectResult>(createdResponse);
        }
    }
}
