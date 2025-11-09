using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Dtos.Response;
using cs_api_rental_car_mvc.Entities;
using cs_api_rental_car_mvc.Services.RentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cs_api_rental_car_mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly ILogger<RentController> logger;
        private readonly IRentService rentService;

        public RentController(ILogger<RentController> logger, IRentService rentService)
        {
            this.logger = logger;
            this.rentService = rentService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BaseResponse<RentEntity>>> PostRent([FromQuery] RentRequestDto requestDto)
        {
            if (IsAdmin())
            {
                return StatusCode(StatusCodes.Status403Forbidden, new BaseResponse<object>
                {
                    Status = 403,
                    Message = "You are not authorized to perform this action.",
                    Data = null
                });
            }

            int userId = int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "", out int id) ? id : -1;

            if (userId == -1)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new BaseResponse<object>
                {
                    Status = 403,
                    Message = "You are not authorized to perform this action.",
                    Data = null
                });
            }            

            requestDto.UserId = userId;

            var (error, rent) = await rentService.PostRent(requestDto);

            if (error != null)
            {
                return error;
            }

            return Ok(new BaseResponse<RentEntity>
            {
                Status = 200,
                Message = "Sukses",
                Data = rent
            });
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<BaseResponse<RentEntity>>> PatchRent(int id, [FromBody] Dictionary<string, object> updates)
        {
            var (error, rent) = await rentService.PatchRent(id, updates);

            if (error != null)
            {
                return error;
            }

            return Ok(new BaseResponse<RentEntity>
            {
                Status = 200,
                Message = "Sukses",
                Data = rent
            });
        }

        [Authorize]
        [HttpDelete("cancel/{id}")]
        public async Task<ActionResult> CancelRent(int id)
        {
            var (error, rent) = await rentService.CancelRent(id);

            if (error != null)
            {
                return error;
            }

            return Ok(new BaseResponse<RentEntity>
            {
                Status = 200,
                Message = "Sukses",
                Data = rent
            });
        }

        [Authorize]
        [HttpPost("return/{id}")]
        public async Task<ActionResult> ReturnCar(int id)
        {
            var (error, rent) = await rentService.ReturnCar(id);

            if (error != null)
            {
                return error;
            }

            return Ok(new BaseResponse<RentEntity>
            {
                Status = 200,
                Message = "Sukses",
                Data = rent
            });
        }

        // for admin
        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<BaseResponse<List<RentEntity>>>> GetAllRent([FromQuery] PaginationRequestDto requestDto)
        {
            if (!IsAdmin())
            {
                return StatusCode(StatusCodes.Status403Forbidden, new BaseResponse<object>
                {
                    Status = 403,
                    Message = "You are not authorized to perform this action.",
                    Data = null
                });
            }

            var (result, rent) = await rentService.GetAllRents(requestDto);

            if (result != null)
            {
                return result;
            }

            return Ok(new BaseResponse<PaginationBaseResponse<RentEntity>>
            {
                Status = 200,
                Message = "Sukses",
                Data = rent
            });
        }

        // for user
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<RentEntity>>>> GetRentSpecificUser([FromQuery] PaginationRequestDto requestDto)
        {
            int userId = int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "", out int id) ? id : -1;

            if (userId == -1)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new BaseResponse<object>
                {
                    Status = 403,
                    Message = "You are not authorized to perform this action.",
                    Data = null
                });
            }

            var (result, rent) = await rentService.GetSpecificUserRent(requestDto, userId);

            if (result != null)
            {
                return result;
            }

            return Ok(new BaseResponse<PaginationBaseResponse<RentEntity>>
            {
                Status = 200,
                Message = "Sukses",
                Data = rent
            });
        }

        private bool IsAdmin()
        {
            string? role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (role == null)
            {
                return false;
            }

            return role == "admin";
        }
    }
}
