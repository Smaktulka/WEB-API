using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fridge.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ProductsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _repository.Products.GetAllProductsAsync(trackChanges: false);

            var productsDto = _mapper.Map<IEnumerable<ProductsDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _repository.Products.GetProductAsync(id, trackChanges: false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {id} does not exist.");
                return NotFound();
            }

            var productDto = _mapper.Map<ProductsDto>(product);
            return Ok(productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, 
            [FromForm] ProductsForUpdateDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForUpdateDto object sent from client is null.");
                return BadRequest("ProductForUpdateDto object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductForUpdateDto object.");
                return UnprocessableEntity(ModelState);
            }

            var productEntity = await _repository.Products.GetProductAsync(id,
                trackChanges: true);
            if (productEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist.");
                return NotFound();
            }

            _mapper.Map(product, productEntity);
            await _repository.SaveAsync();

            return Ok(productEntity);
        }
    }   
}
