using AutoMapper;
using ECPAPI.Data;
using ECPAPI.Models;
using ECPAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;
        private readonly ICategoryRepository<Category> _ECPAPIRepository;

        public CategoriesController(ECPDbContext dbContext, IMapper mapper, IProductRepository productRepository, ILogger<ProductsController> logger, ICategoryRepository<Category> ECPAPIRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _logger = logger;
            _ECPAPIRepository = ECPAPIRepository;
        }

        [HttpGet("All Categories", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("GetAllCategories method started");
            var Categories = await _ECPAPIRepository.GetAllAsync();
            List<CategoryDTO> CategoriesDTOData = _mapper.Map<List<CategoryDTO>>(Categories);
            return Ok(Categories);
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryDTO?>> CreateCategoryAsync(CategoryDTO model)
        {
            if (model == null)
                return BadRequest();
            Category category = _mapper.Map<Category>(model);
            Category categoryAfterCreation = await _ECPAPIRepository.CreateAsync(category);
            model.CategoryId = categoryAfterCreation.CategoryId;
            return Created("",model);   
            //return CreatedAtRoute("GetCategoryById", new { CategoryId = model.CategoryId }, category);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryDTO>> UpdateCategoryAsync(CategoryDTO model)
        {
            if (model == null || model.CategoryId <= 0)
                return BadRequest("Invalid Category data.");
            var existingCategory = await _ECPAPIRepository.GetAsync(x => x.CategoryId == model.CategoryId);

            if (existingCategory == null)
                return NotFound($"Category with ID {model.CategoryId} not found.");

            _mapper.Map(model, existingCategory);

            await _ECPAPIRepository.UpdateAsync(existingCategory);
            return Ok(model);
        }


        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteCategoryAsync(int id)
        {
            if (id <= 0)
                return BadRequest();
            Category? category = await _ECPAPIRepository.GetAsync(x => x.CategoryId == id);
            if (category == null)
                return NotFound();

            await _ECPAPIRepository.DeleteAsync(category);
            return Ok(true);
        }

    }
}
