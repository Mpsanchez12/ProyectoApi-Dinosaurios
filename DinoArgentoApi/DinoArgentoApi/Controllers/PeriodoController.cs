using DinoArgentoApi.Enums;
using DinoArgentoApi.Models.Periodo;
using DinoArgentoApi.Models.Periodo.Dto;
using DinoArgentoApi.Services;
using DinoArgentoApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DinoArgentoApi.Controllers
{
    [Route("api/periodos")]
    [ApiController]
    [Authorize(Roles = $"{ROLES.Admin}, {ROLES.Mod}")]
    [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
    public class PeriodoController : ControllerBase
    {
        private readonly PeriodoService _service;
        public PeriodoController(PeriodoService service) { _service = service; }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PeriodoDTO>), StatusCodes.Status200OK)] // Etiqueta del GET
        public async Task<ActionResult<List<Periodo>>> GetAll() => Ok(await _service.GetAll());

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PeriodoDTO), StatusCodes.Status200OK)] // Etiqueta del GET por ID
        public async Task<ActionResult<Periodo>> GetOneById(int id)
        {
            try { return Ok(await _service.GetOneById(id)); }
            catch (ErrorResponse ex) { return StatusCode((int)ex.StatusCode, ex.Message); }
            catch (Exception ex) { return StatusCode(500, new ResponseMessage(ex.Message)); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(PeriodoDTO), StatusCodes.Status201Created)] // Etiqueta del POST (Creado)
        public async Task<ActionResult<Periodo>> CreateOne([FromBody] PeriodoDTO dto)
        {
            try { return Created("api/periodos", await _service.CreateOne(dto)); }
            catch (ErrorResponse ex) { return StatusCode((int)ex.StatusCode, ex.Message); }
            catch (Exception ex) { return StatusCode(500, new ResponseMessage(ex.Message)); }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = ROLES.Admin)]
        [ProducesResponseType(typeof(PeriodoDTO), StatusCodes.Status200OK)] // Etiqueta del PUT (Actualizar)
        public async Task<ActionResult<Periodo>> UpdateOneById(int id, [FromBody] PeriodoDTO dto)
        {
            try { return Ok(await _service.UpdateOneById(id, dto)); }
            catch (ErrorResponse ex) { return StatusCode((int)ex.StatusCode, ex.Message); }
            catch (Exception ex) { return StatusCode(500, new ResponseMessage(ex.Message)); }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ROLES.Admin)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)] // Etiqueta del DELETE
        public async Task<ActionResult> DeleteOneById(int id)
        {
            try { await _service.DeleteOneById(id); return Ok(new ResponseMessage($"Periodo {id} eliminado")); }
            catch (ErrorResponse ex) { return StatusCode((int)ex.StatusCode, ex.Message); }
            catch (Exception ex) { return StatusCode(500, new ResponseMessage(ex.Message)); }
        }
    }
}