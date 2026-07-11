using DinoArgentoApi.Enums;
using DinoArgentoApi.Models.User.Dto;
using DinoArgentoApi.Services;
using DinoArgentoApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DinoArgentoApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseValidation), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO register)
        {
            try
            {
                var created = await _authService.Register(register);
                return Created("/api/auth/register", created);
            }
            catch (ErrorResponse ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage(ex.Message));
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseValidation), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginDTO login)
        {
            try
            {
                var res = await _authService.Login(login, HttpContext);
                return Ok(res);
            }
            catch (ErrorResponse ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage(ex.Message));
            }
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await _authService.Logout(HttpContext);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage(ex.Message));
            }
        }

        [HttpPut("update-roles/{userId}")]
        [Authorize(Roles = ROLES.Admin)]
        public async Task<ActionResult<UserDTO>> UpdateRolesToUser(int userId, [FromBody] UpdateUserRolesDTO rolesDTO)
        {
            try
            {
                var res = await _authService.UpdateRolesToUser(userId, rolesDTO.RoleIds);
                return Ok(res);
            }
            catch (ErrorResponse ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage(ex.Message));
            }
        }

        [HttpGet("health")]
        [AllowAnonymous]
        public ActionResult<ResponseMessage> Health() => Ok(new ResponseMessage("DinosArchive System: Todo ok"));

        [HttpGet("check-auth")]
        [Authorize]
        public ActionResult CheckAuth() => Ok();
    }
}
