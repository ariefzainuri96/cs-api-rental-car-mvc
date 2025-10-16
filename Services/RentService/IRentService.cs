using System.Collections.Generic;
using System.Threading.Tasks;
using cs_api_rental_car_mvc.Dtos;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Entities;

namespace cs_api_rental_car_mvc.Services.RentService
{
    public interface IRentService
    {
        Task<(HttpError?, RentEntity)> PostRent(RentRequestDto request);
        Task<(HttpError?, RentEntity)> PatchRent(int id, Dictionary<string, object> updates);
    }
}