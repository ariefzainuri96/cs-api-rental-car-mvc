using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Dtos.Response;
using cs_api_rental_car_mvc.Entities;
using cs_api_rental_car_mvc.Services.CarService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cs_api_rental_car_mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService carService;

        public CarController(ICarService carService)
        {
            this.carService = carService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<CarEntity>>>> GetCars([FromQuery] PaginationRequestDto requestDto)
        {
            var (result, cars) = await carService.GetCar(requestDto);

            if (result != null)
            {
                return result;
            }

            return Ok(new BaseResponse<PaginationBaseResponse<CarEntity>>
            {
                Status = 200,
                Message = "Sukses",
                Data = cars
            });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<CarEntity>>> GetCarById(int id)
        {
            var (result, car) = await carService.GetCarById(id);

            if (result != null)
            {
                return result;
            }

            return Ok(new BaseResponse<CarEntity>
            {
                Status = 200,
                Message = "Sukses",
                Data = car
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BaseResponse<CarEntity>>> PostCar([FromBody] CarRequestDto request)
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns validation errors
            }

            var (error, car) = await carService.PostCar(request);

            if (error != null)
            {
                return error;
            }

            return Ok(new BaseResponse<CarEntity>()
            {
                Status = 200,
                Message = "Car is Added!",
                Data = car
            });
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<BaseResponse<CarEntity>>> PatchCar(int id, [FromBody] Dictionary<string, object> updates)
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

            if (updates == null || updates.Count == 0)
            {
                return BadRequest("No fields to update.");
            }

            var (error, car) = await carService.PatchCar(id, updates);

            if (error != null)
            {
                return error;
            }

            return Ok(new BaseResponse<CarEntity>()
            {
                Status = 200,
                Message = "Succesfully update the Video Game!",
                Data = car
            });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(int id)
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

            var error = await carService.DeleteCar(id);

            if (error != null)
            {
                return error;
            }

            return Ok(new BaseResponse<string>() { Status = 200, Message = "Succesfully delete car!" });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCar(int id, [FromBody] CarRequestDto request)
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (error, car) = await carService.PutCar(id, request);

            if (error != null)
            {
                return error;
            }

            return Ok(new BaseResponse<CarEntity>()
            {
                Status = 200,
                Message = "Succesfully update the Car!",
                Data = car
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
