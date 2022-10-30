using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using LoggerService;
using Microsoft.AspNetCore.Mvc;

namespace fridge.Controllers
{
    [Route("api/models")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ModelsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllModels()
        {
            var models = await _repository.FridgeModels.GetAllFridgeModelsAsync(trackChanges: false);

            var modelsDto = _mapper.Map<IEnumerable<FridgeModelsDto>>(models);

            return Ok(modelsDto);
        }
    }
}
