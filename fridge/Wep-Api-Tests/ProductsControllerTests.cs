using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using FakeItEasy;
using fridge.Controllers;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wep_Api_Tests
{
    public class ProductsControllerTests
    {
        private ProductsController _productsController;
        private IRepositoryManager _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        public ProductsControllerTests()
        {
            //Dependencies
            _repository = A.Fake<IRepositoryManager>();
            _logger = A.Fake<ILoggerManager>();
            _mapper = A.Fake<IMapper>();

            //SUT
            _productsController = new ProductsController(_repository, _logger, _mapper);
        }

        // Test for GetAllProducts
        [Fact]
        public async Task ProductsController_GetAllProducts_ReturnOkSuccess()
        {
            var okResult = await _productsController.GetAllProducts();

            Assert.IsAssignableFrom<OkObjectResult>(okResult);
        }

        //Test for UpdateProduct
        [Fact]
        public async Task ProductsController_UpdateProduct_ReturnOkSuccess()
        {
            var testGuid = new Guid("16ad116a-8289-4ce9-aa96-4d367dec6dd2");

            var product = new ProductsForUpdateDto
            { 
                Name = "Vania",
                Default_Quantity = 2,
            };

            var okResult = await _productsController.UpdateProduct(testGuid, product);

            Assert.IsAssignableFrom<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task ProductsController_UpdateProduct_ReturnBadRequest()
        {
            var testGuid = new Guid("16ad116a-8289-4ce9-aa96-4d367dec6dd2");

            ProductsForUpdateDto? products = null;

            var badRequest = await _productsController.UpdateProduct(testGuid, products);

            Assert.IsAssignableFrom<BadRequestObjectResult>(badRequest);
        }

        [Fact]
        public async Task ProductsController_UpdateProduct_ReturnUnproccessableData()
        {
            var testGuid = Guid.NewGuid();

            var product = new ProductsForUpdateDto
            {
                Name = "Vania",
            };

            _productsController.ModelState.AddModelError("Default_Quantity", "Required");

            var badResponse = await _productsController.UpdateProduct(testGuid, product);

            Assert.IsAssignableFrom<UnprocessableEntityObjectResult>(badResponse);
        }
    }
}
