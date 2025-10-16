using System.Collections.Generic;
using System.Threading.Tasks;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Dtos.Response;
using cs_api_rental_car_mvc.Entities;
using cs_api_rental_car_mvc.Services.RentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cs_api_rental_car_mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly RentService rentService;

        public RentController(RentService rentService)
        {
            this.rentService = rentService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BaseResponse<RentEntity>>> PostRent([FromQuery] RentRequestDto requestDto)
        {
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
    }
}
