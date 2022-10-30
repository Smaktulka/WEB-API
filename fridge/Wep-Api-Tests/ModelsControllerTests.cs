using AutoMapper;
using Contracts;
using FakeItEasy;
using fridge.Controllers;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Wep_Api_Tests
{
    public class ModelsControllerTests : ControllerBase
    {
        private ModelsController _modelsController;
        private IRepositoryManager _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public ModelsControllerTests()
        {
            _repository = A.Fake<IRepositoryManager>();
            _logger = A.Fake<ILoggerManager>();
            _mapper = A.Fake<IMapper>();

            _modelsController = new ModelsController(_repository, _logger, _mapper);
        }

        [Fact]
        public async Task ModelsController_GetAllModels_ReturnOkSuccess()
        {
            var okResult = await _modelsController.GetAllModels();

            Assert.IsAssignableFrom<OkObjectResult>(okResult);
        }
    }
}
