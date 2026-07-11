using DinoArgentoApi.Enums;
using DinoArgentoApi.Models.Dieta;
using DinoArgentoApi.Models.Dieta.Dto;
using DinoArgentoApi.Services;
using DinoArgentoApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DinoArgentoApi.Controllers
{
  
        [Route("api/dietas")]
        [ApiController]
        [Authorize(Roles = $"{ROLES.Admin}, {ROLES.Mod}")]
        public class DietaController : ControllerBase
        {
            private readonly DietaService _service;
            public DietaController(DietaService service) { _service = service; }

            [HttpGet]
            [AllowAnonymous]
            public async Task<ActionResult<List<Dieta>>> GetAll() => Ok(await _service.GetAll());

            [HttpGet("{id}")]
            [AllowAnonymous]
            public async Task<ActionResult<Dieta>> GetOneById(int id)
            {
                try { return Ok(await _service.GetOneById(id)); }
                catch (ErrorResponse ex) { return StatusCode((int)ex.StatusCode, ex.Message); }
                catch (Exception ex) { return StatusCode(500, new ResponseMessage(ex.Message)); }
            }

            [HttpPost]
            public async Task<ActionResult<Dieta>> CreateOne([FromBody] CreateDietaDTO dto)
            {
                try { return Created("api/dietas", await _service.CreateOne(dto)); }
                catch (ErrorResponse ex) { return StatusCode((int)ex.StatusCode, ex.Message); }
                catch (Exception ex) { return StatusCode(500, new ResponseMessage(ex.Message)); }
            }

            [HttpPut("{id}")]
            [Authorize(Roles = ROLES.Admin)]
            public async Task<ActionResult<Dieta>> UpdateOneById(int id, [FromBody] UpdateDietaDTO dto)
            {
                try { return Ok(await _service.UpdateOneById(id, dto)); }
                catch (ErrorResponse ex) { return StatusCode((int)ex.StatusCode, ex.Message); }
                catch (Exception ex) { return StatusCode(500, new ResponseMessage(ex.Message)); }
            }

            [HttpDelete("{id}")]
            [Authorize(Roles = ROLES.Admin)]
            public async Task<ActionResult> DeleteOneById(int id)
            {
                try { await _service.DeleteOneById(id); return Ok(new ResponseMessage($"Dieta {id} eliminada")); }
                catch (ErrorResponse ex) { return StatusCode((int)ex.StatusCode, ex.Message); }
                catch (Exception ex) { return StatusCode(500, new ResponseMessage(ex.Message)); }
            }
        }
    }


