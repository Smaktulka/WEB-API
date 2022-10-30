using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using fridge.Models;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;

namespace fridge.Controllers
{
    [Route("api/fridges/{FridgeId}/fridgeProducts")]
    [ApiController]
    public class FridgeProductsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public FridgeProductsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFridgeProductForFridge(Guid fridgeId,
            [FromBody] FridgeProductsForCreationDto fridgeProducts)
        {
            if (fridgeProducts == null)
            {
                _logger.LogError("FridgeProductsForCreationDto object sent from client is null.");
                return BadRequest("FridgeProductsForCreationDto object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the FridgeProductsForCreationDto object.");
                return UnprocessableEntity(ModelState);
            }

            var product = await _repository.Products.GetProductAsync(fridgeProducts.ProductId, trackChanges: false);
            if (product == null)
            {
                _logger.LogError($"Product with id: {fridgeProducts.ProductId} do not exists.");
                return NotFound();
            }

            var fridge = _repository.Fridges.GetFridgeAsync(fridgeId, trackChanges: false);
            if (fridge == null)
            {
                _logger.LogInfo($"Fridge with id: {fridgeId} doesn't exist in the database.");
                return NotFound();
            }

            var fridgeProductsEntity = _mapper.Map<FridgeProducts>(fridgeProducts);

            _repository.FridgeProducts.CreateFridgeProductForFridge(fridgeId, fridgeProductsEntity);
            await _repository.SaveAsync();

            var fridgeProductsToReturn = _mapper.Map<FridgeProductsDto>(fridgeProductsEntity);

            return CreatedAtRoute("GetFridgeProducts", new { fridgeId, id = fridgeProductsToReturn.Id },
                fridgeProductsToReturn);
        }

        [HttpPost("fpcollection")]
        public async Task<IActionResult> CreateFridgeProductsCollection(Guid fridgeId,
            [FromBody] IEnumerable<FridgeProductsForCreationDto> fridgeProductsCollection)
        {
            if (fridgeProductsCollection == null)
            {
                _logger.LogError("Fridges collection sent from client is null.");
                return BadRequest("Fridges collecton is null.");
            }

            var fridgeProductsEntities = _mapper.Map<IEnumerable<FridgeProducts>>(fridgeProductsCollection);
            foreach (var fridgeProduct in fridgeProductsEntities)
            {
                if (fridgeProduct == null)
                {
                    _logger.LogError("FridgeProductsForCreationDto object sent from client is null.");
                    return BadRequest("FridgeProductsForCreationDto object is null.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the FridgeProductsForCreationDto object.");
                    return UnprocessableEntity(ModelState);
                }

                var product = await _repository.Products.GetProductAsync(fridgeProduct.ProductId, trackChanges: false);
                if (product == null)
                {
                    _logger.LogError($"Product with id: {fridgeProduct.ProductId} do not exists.");
                    return NotFound();
                }

                var fridge = _repository.Fridges.GetFridgeAsync(fridgeId, trackChanges: false);
                if (fridge == null)
                {
                    _logger.LogInfo($"Fridge with id: {fridgeId} doesn't exist in the database.");
                    return NotFound();
                }

                _repository.FridgeProducts.CreateFridgeProductForFridge(fridgeId, fridgeProduct);
            }

            await _repository.SaveAsync();

            var fridgeProductsCollectionToReturn = _mapper.Map<IEnumerable<FridgeProductsDto>>(fridgeProductsEntities);
            var ids = string.Join(",", fridgeProductsCollectionToReturn.Select(c => c.Id));

            return CreatedAtAction("FridgeProductsCollection", routeValues: new { ids }, fridgeProductsCollectionToReturn);
        }

        [HttpGet("fpcollection/{ids}", Name = "FridgeProductsCollection")]
        public async Task<IActionResult> GetFridgeProductsCollection(IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter id is null.");
                return BadRequest("Parameter id is null.");
            }

            var fridgeProductsEntities = await _repository.FridgeProducts.
                GetFridgeProductsByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != fridgeProductsEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var fridgeProductsEntitiesToReturn = _mapper.Map<IEnumerable<FridgeProductsDto>>(fridgeProductsEntities);

            return Ok(fridgeProductsEntitiesToReturn);
        }

        [HttpGet("storedproc")]
        public async Task<IActionResult> CreateFridgeProductsFromSPForFridge(Guid fridgeId)
        {
            List<FridgeProductsSpDto> fridgeProductsSp = new List<FridgeProductsSpDto>();
            IDataReader reader = null;
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection("server=.; database=fridge; Integrated Security=true");
                IDbCommand dbCommand = new SqlCommand();
                dbCommand.Connection = sqlConnection;
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "FindFridgeProductsWithZeroQuantity";

                sqlConnection.Open();

                reader = dbCommand.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        FridgeProductsSpDto fridgeProducts = new FridgeProductsSpDto();
                        fridgeProducts.Id = (Guid)reader["FridgeProductsId"];
                        fridgeProducts.ProductId = (Guid)reader["ProductId"];
                        fridgeProducts.FridgeId = (Guid)reader["FridgeId"];
                        fridgeProducts.Quantity = (int)reader["Quantity"];
                        fridgeProductsSp.Add(fridgeProducts);
                    }
                }
                else
                {
                    _logger.LogInfo("No rows found.");
                    return NotFound();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            foreach (var fridgeProduct in fridgeProductsSp)
            {
                var product = await _repository.Products.GetProductAsync(fridgeProduct.ProductId,
                    trackChanges: false);
                if (product == null)
                {
                    _logger.LogError($"Product with id: {fridgeProduct.ProductId} do not exists.");
                    return NotFound();
                }

                fridgeProduct.Quantity = product.Default_Quantity;
            }

            var fridgeProductsEntities = fridgeProductsSp.Select(c => new FridgeProducts
            {
                Id = c.Id,
                ProductId = c.ProductId,
                FridgeId = c.FridgeId,
                Quantity = c.Quantity
            }).ToList();
            
            foreach (var fridgeProduct in fridgeProductsEntities)
            {
                var fridgeProductForUpdate = new FridgeProductsForUpdateDto
                {
                    ProductId = fridgeProduct.ProductId,
                    Quantity = fridgeProduct.Quantity
                };

                if (fridgeProductForUpdate == null)
                {
                    _logger.LogError("FridgeProductsForUpdateDto object sent from client is null.");
                    return BadRequest("FridgeProductsForUpdateDto object is null.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the FridgeProductsForUpdateDto object.");
                    return UnprocessableEntity(ModelState);
                }

                var fridge = _repository.Fridges.GetFridgeAsync(fridgeProduct.FridgeId, trackChanges: false);
                if (fridge == null)
                {
                    _logger.LogError($"Fridge with id: {fridgeId} doesn't exist.");
                    return NotFound();
                }

                var fridgeProductEntity = await _repository.FridgeProducts.GetFridgeProductAsync(fridgeProduct.FridgeId,
                    fridgeProduct.Id,
                    trackChanges: true);
                if (fridgeProductEntity == null)
                {
                    _logger.LogInfo($"fridgeProduct with id: {fridgeProduct.Id} doesn't exist.");
                    return NotFound();
                }

                _mapper.Map(fridgeProductForUpdate, fridgeProductEntity);
                await _repository.SaveAsync();
            }
            
            await _repository.SaveAsync();

            var fridgeProductsToReturn = _mapper.Map<IEnumerable<FridgeProductsDto>>(fridgeProductsEntities);
            var ids = string.Join(",", fridgeProductsToReturn.Select(c => c.Id));

            return CreatedAtAction("FridgeProductsCollection", new { ids }, fridgeProductsToReturn);
        }

        [HttpPut("updatefridgeProduct/{id}")]
        public async Task<IActionResult> UpdateFridgeProductForFridge(Guid fridgeId, Guid id,
            [FromBody] FridgeProductsForUpdateDto fridgeProducts)
        {
            if (fridgeProducts == null)
            {
                _logger.LogError("FridgeProductsForUpdateDto object sent from client is null.");
                return BadRequest("FridgeProductsForUpdateDto object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the FridgeProductsForUpdateDto object.");
                return UnprocessableEntity(ModelState);
            }

            var fridge = _repository.Fridges.GetFridgeAsync(fridgeId, trackChanges: false);
            if (fridge == null)
            {
                _logger.LogError($"Fridge with id: {fridgeId} doesn't exist.");
                return NotFound();
            }

            var fridgeProductEntity = await _repository.FridgeProducts.GetFridgeProductAsync(fridgeId, id,
                trackChanges: true);
            if (fridgeProductEntity == null)
            {
                _logger.LogInfo($"fridgeProduct with id: {id} doesn't exist.");
                return NotFound();
            }

            _mapper.Map(fridgeProducts, fridgeProductEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFridgeProduct(Guid fridgeId, Guid id)
        {
            var fridge = await _repository.Fridges.GetFridgeAsync(fridgeId, trackChanges: false);
            if (fridge == null)
            {
                _logger.LogError($"Fridge with id: {fridgeId} doesn't exist.");
                return BadRequest($"Fridge with id: {fridgeId} doesn't exist.");
            }

            var fridgeProductDb = await _repository.FridgeProducts.GetFridgeProductAsync(fridgeId, id, 
                trackChanges: false);
            if (fridgeProductDb == null)
            {
                _logger.LogInfo($"fridgeProduct with id: {id} doesn't exist.");
                return NotFound();
            }

            var fridgeProduct = _mapper.Map<FridgeProductsDto>(fridgeProductDb);
            return Ok(fridgeProduct);
        }


        [HttpGet(Name = "GetFridgeProducts")]
        public async Task<IActionResult> GetFridgeProducts(Guid fridgeId)
        {
            var fridge = await _repository.Fridges.GetFridgeAsync(fridgeId, trackChanges: false);
            if (fridge == null)
            {
                _logger.LogInfo($"Fridge with id: {fridgeId} doesn't exists.");
                return NotFound();
            }

            var fridgeProductsFromDb = await _repository.FridgeProducts.GetFridgeProductsAsync(fridgeId, trackChanges: false);

            var fridgeProductsDto = _mapper.Map<IEnumerable<FridgeProductsDto>>(fridgeProductsFromDb);

            return Ok(fridgeProductsDto);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFridgeProducts(Guid fridgeId)
        {
            var fridge = _repository.Fridges.GetFridgeAsync(fridgeId, trackChanges: false);
            if (fridge == null)
            {
                _logger.LogInfo($"Fridge with id: {fridgeId} doesn't exists.");
                return NotFound();
            }

            var fridgeProductsForFridge =
                await _repository.FridgeProducts.GetFridgeProductsAsync(fridgeId, trackChanges: false);

            if (fridgeProductsForFridge == null)
            {
                _logger.LogInfo($"Fridge with id: {fridgeId} doesn't have any products.");
                return NotFound();
            }

            foreach (var fridgeProduct in fridgeProductsForFridge)
            {
                _repository.FridgeProducts.DeleteFridgeProducts(fridgeProduct);
            }

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
