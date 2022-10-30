using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using fridge.Models;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace fridge.Controllers
{
    [Route("api/fridges")]
    [ApiController]
    public class FridgesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public FridgesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFridges([FromBody] FridgesForCreationDto fridge)
        {
            if (fridge == null)
            {
                _logger.LogError("FridgesFroCreationDto object sent from client is null.");
                return BadRequest("FridgesFroCreationDto object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the FridgeForCreationDto object.");
                return UnprocessableEntity(ModelState);
            }

            var fridgeEntity = _mapper.Map<Fridges>(fridge);

            _repository.Fridges.CreateFridge(fridgeEntity);
            await _repository.SaveAsync();

            var fridgeToReturn = _mapper.Map<FridgesDto>(fridgeEntity);

            return CreatedAtRoute("FridgeById", new { id = fridgeToReturn.Id }, fridgeToReturn);
        }


        [HttpPost("collection")]
        public async Task<IActionResult> CreateFridgesCollection([FromBody]
            IEnumerable<FridgesForCreationDto> fridgesCollection)
        {
            if (fridgesCollection == null)  
            {
                _logger.LogError("Fridges collection sent from client is null.");
                return BadRequest("Fridges collecton is null.");
            }

            foreach (var fridge in fridgesCollection)
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the FridgeForCreationDto object.");
                    return UnprocessableEntity(ModelState);
                }
            }

            var fridgesEntities = _mapper.Map<IEnumerable<Fridges>>(fridgesCollection);
            foreach (var fridge in fridgesEntities)
            {
                _repository.Fridges.CreateFridge(fridge);
            }

            await _repository.SaveAsync();

            var fridgesCollectionToReturn = _mapper.Map<IEnumerable<FridgesDto>>(fridgesEntities);
            var ids = string.Join(",", fridgesCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("FridgesCollection", new { ids }, fridgesCollectionToReturn);
        }

        [HttpGet("collection/{ids}", Name = "FridgesCollection")]
        public async Task<IActionResult> GetFridgesCollectionAsync(IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter id is null.");
                return BadRequest("Parameter id is null.");
            }

            var fridgeEntities = await _repository.Fridges.GetByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != fridgeEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var fridgesToReturn = _mapper.Map<IEnumerable<FridgesDto>>(fridgeEntities);

            return Ok(fridgesToReturn);
        }

        [HttpGet(Name = "GetAllFridges")]
        public async Task<IActionResult> GetAllFridges()
        {
            var fridges = await _repository.Fridges.GetAllFridgesAsync(trackChanges: false);

            var fridgesDto = _mapper.Map<IEnumerable<FridgesDto>>(fridges);

            return Ok(fridgesDto);
        }

        [HttpGet("{id}", Name = "FridgeById")]
        public async Task<IActionResult> GetFridge(Guid id)
        {    
            var fridges = await _repository.Fridges.GetFridgeAsync(id, trackChanges: false);
            if (fridges == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist the database");
                return NotFound();
            }

            var fridgesDto = _mapper.Map<FridgesDto>(fridges);
            return Ok(fridgesDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFridge(Guid id,
            [FromForm] FridgeForUpdateDto fridge)
        {
            if (fridge == null)
            {
                _logger.LogError("FridgeForUpdateDto object sent from client is null.");
                return BadRequest("FridgeForUpdateDto object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the FridgeForUpdateDto object.");
                return UnprocessableEntity(ModelState);
            }

            var fridgeModel = await _repository.FridgeModels.GetFridgeModelByIdAsync(fridge.ModelId,
                trackChanges: false);
            if (fridgeModel == null)
            {
                _logger.LogError($"Fridge model with {id} does not exist.");
                return NotFound();
            }
            
            var fridgeEntity = await _repository.Fridges.GetFridgeAsync(id,
                trackChanges: true);
            if (fridgeEntity == null)
            {
                _logger.LogInfo($"Fridge with id: {id} doesn't exist.");
                return NotFound();
            }

            _mapper.Map(fridge, fridgeEntity);
            await _repository.SaveAsync();

            return Ok(fridgeEntity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFridge(Guid id)
        {
            var fridge = await _repository.Fridges.GetFridgeAsync(id, trackChanges: false);
            if (fridge == null)
            {
                _logger.LogError($"Fridge with id: {id} doesn't exist.");
                return BadRequest($"Fridge with id: {id} doesn't exist.");
            }

            var fridgeProductsForFridge =
                await _repository.FridgeProducts.GetFridgeProductsAsync(id, trackChanges: false);

            if (fridgeProductsForFridge == null)
            {
                _logger.LogInfo($"Fridge with id: {id} doesn't have any products.");
                return NotFound();
            }

            foreach (var fridgeProduct in fridgeProductsForFridge)
            {
                _repository.FridgeProducts.DeleteFridgeProducts(fridgeProduct);
            }

            _repository.Fridges.DeleteFridge(fridge);

            await _repository.SaveAsync();

            return Ok();
        }
    }
}
