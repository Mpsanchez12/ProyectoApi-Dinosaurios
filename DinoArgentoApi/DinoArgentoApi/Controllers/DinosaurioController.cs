using DinoArgentoApi.Enums;
using DinoArgentoApi.Models.Dinosaurio;
using DinoArgentoApi.Models.Dinosaurio.Dto;
using DinoArgentoApi.Services;
using DinoArgentoApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DinoArgentoApi.Controllers
{
    [Route("api/dinosaurios")]
    [ApiController]
    [Authorize(Roles = $"{ROLES.Admin}, {ROLES.Mod}")]
    [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
    public class DinosaurioController : ControllerBase
    {
        private readonly DinosaurioService _dinoService;

        public DinosaurioController(DinosaurioService dinoService)
        {
            _dinoService = dinoService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(List<DinosaurioDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<DinosaurioDTO>>> GetAll()
        {
            var dinos = await _dinoService.GetAll();
            return Ok(dinos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DinosaurioDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DinosaurioDTO>> GetOneById(int id)
        {
            try
            {
                var dino = await _dinoService.GetOneById(id);
                return Ok(dino);
            }
            catch (ErrorResponse ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                ResponseMessage msg = new ResponseMessage(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Dinosaurio), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseValidation), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dinosaurio>> CreateOne([FromBody] CreateDinosaurioDTO createDino)
        {
            try
            {
                var dino = await _dinoService.CreateOne(createDino);
                return Created("POST api/dinosaurios", dino);
            }
            catch (ErrorResponse ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                ResponseMessage msg = new ResponseMessage(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = ROLES.Admin)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Dinosaurio), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseValidation), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Dinosaurio>> UpdateOneById(int id, [FromBody] UpdateDinosaurioDTO updateDinosaurio)
        {
            try
            {
                var dino = await _dinoService.UpdateOneById(id, updateDinosaurio);
                return Ok(dino);
            }
            catch (ErrorResponse ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                ResponseMessage msg = new ResponseMessage(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ROLES.Admin)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOneById(int id)
        {
            try
            {
                await _dinoService.DeleteOneById(id);
                ResponseMessage msg = new ResponseMessage($"Espécimen con id = {id} ha sido eliminado de los registros");

                return Ok(msg);
            }
            catch (ErrorResponse ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                ResponseMessage msg = new ResponseMessage(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
    }
}
