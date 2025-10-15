using System.Threading.Tasks;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Dtos.Response;
using cs_api_rental_car_mvc.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace cs_api_rental_car_mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse<string>>> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (result, response) = await _authService.LoginAsync(request);

            if (result != null)
            {
                return result;
            }

            return Ok(new BaseResponse<LoginResponseDto>() { Status = 200, Message = "Success Login", Data = response });
        }

        [HttpPost("register")]
        public async Task<ActionResult<BaseResponse<string>>> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns validation errors
            }

            var error = await _authService.RegisterAsync(request);

            if (error != null)
            {
                return error;
            }

            return Ok(new BaseResponse<string>() { Status = 200, Message = "Success Register" });
        }
    }
}
