using AutoMapper;
using ECPAPI.Data;
using ECPAPI.Models;
using ECPAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace ECPAPI.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    [Authorize(AuthenticationSchemes = "LoginForLocalUsers")]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;
        private APIResponse _apiResponse;

        public ProductsController(ECPDbContext dbContext, IMapper mapper, IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _logger = logger;
            _apiResponse = new APIResponse();
        }

        [HttpGet("Search", Name = "Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> SearchProducts(
          [FromQuery] int pageIndex = 1,
          [FromQuery] int pageSize = 50,
          [FromQuery] string? keywords = null,
          [FromQuery] decimal? priceMin = null,
          [FromQuery] decimal? priceMax = null)
        {
            try
            {
                _logger.LogInformation("SearchProducts started");

                var products = await _productRepository.SearchProductsAsync(pageIndex, pageSize, keywords, priceMin, priceMax);
                var productDTOs = _mapper.Map<List<ProductDTO>>(products);

                _apiResponse.Data = productDTOs;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                return _apiResponse;
            }
        }

        [HttpGet("AllProducts", Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("GetAllProducts Method Started");
                List<Product> products = await _productRepository.GetAllAsync();
                _apiResponse.Data = _mapper.Map<List<ProductDTO>>(products);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;

                return _apiResponse;
            }
        }

        [HttpGet("{Id:int}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse?>> GetProductById(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    _logger.LogWarning("Bad Request");
                    return BadRequest();
                }

                Product? product = await _productRepository.GetAsync(x =>x.Id == Id);
                if (product == null)
                    return NotFound();

                _apiResponse.Data = _mapper.Map<ProductDTO>(product);
                _apiResponse.Status = true;
                _apiResponse.StatusCode= HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;

                return _apiResponse;
            }
        }

        [HttpGet("{name:alpha}", Name = "GetProductByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse?>> GetProductByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest();
                Product? product = await _productRepository.GetAsync(x => x.Name.ToLower().Contains(name.ToLower()));

                if (product == null) 
                    return NotFound();

                _apiResponse.Data = _mapper.Map<ProductDTO>(product);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;

                return _apiResponse;
            }
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProductAsync([FromBody] ProductDTO model)
        {
            try
            {
                if (model == null)
                    return BadRequest();
                Product product = _mapper.Map<Product>(model);
                Product productAfterCreation = await _productRepository.CreateAsync(product);
                model.Id = productAfterCreation.Id;
                _apiResponse.Data = model;
                _apiResponse.Status = true;
                _apiResponse.StatusCode= HttpStatusCode.OK;

                return CreatedAtRoute("GetProductById", new { Id = model.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;

                return _apiResponse;
            }
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateProductAsync([FromBody] ProductDTO model)
        {
            try
            {
                if (model == null || model.Id <=0)
                    return BadRequest();

                Product? existingProduct = await _productRepository.GetAsync(x => x.Id == model.Id, true);

                if (existingProduct == null)
                    return BadRequest();

                Product newRecord = _mapper.Map<Product>(model);

                await _productRepository.UpdateAsync(newRecord);
                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
        }

        [HttpDelete("{Id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteProductAsync(int Id)
        {
            try
            {
                if (Id <= 0)
                    return BadRequest();
                Product? product = await _productRepository.GetAsync(x => x.Id == Id);
                
                if (product == null) 
                    return NotFound();
                await _productRepository.DeleteAsync(product);

                _apiResponse.Data = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Status = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
        }
    }
}
