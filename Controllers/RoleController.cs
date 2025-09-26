using AutoMapper;
using ECPAPI.Data;
using ECPAPI.Models;
using ECPAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository<Role> _roleRepository;
        private APIResponse _apiResponse;

        public RoleController(IMapper mapper, ICategoryRepository<Role> roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _apiResponse = new APIResponse();
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateRoleAsync(RoleDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();

                Role roles = _mapper.Map<Role>(dto);
                roles.IsDeleted = false;
                roles.CreatedDate = DateTime.Now;
                roles.ModifiedDate = DateTime.Now;

                var result = await _roleRepository.CreateAsync(roles);

                dto.Id = result.Id;
                _apiResponse.Data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return CreatedAtRoute("GetRoleId", new { id = dto.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("All", Name = "GetAllRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> GetRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();

                _apiResponse.Data = _mapper.Map<List<RoleDTO>>(roles);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRoleAsync(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var roles = await _roleRepository.GetAsync(x => x.Id == id);

                if (roles == null)
                    return NotFound();

                _apiResponse.Data = _mapper.Map<RoleDTO>(roles);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> UpdateRoleAsync(RoleDTO dto)
        {
            try
            {
                if (dto == null || dto.Id == 0)
                    return BadRequest();

                var existingRole = await _roleRepository.GetAsync(x => x.Id == dto.Id, true);

                if (existingRole == null)
                    return BadRequest();

                var newRole = _mapper.Map<Role>(dto);

                await _roleRepository.UpdateAsync(newRole);

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = newRole;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }

        [HttpDelete]
        [Route("Delete/{id}", Name = "DeleteRoleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> DeleteRoleAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var roles = await _roleRepository.GetAsync(x => x.Id == id);

                if (roles == null)
                    return BadRequest();

                await _roleRepository.DeleteAsync(roles);

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = true;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }

    }
}
