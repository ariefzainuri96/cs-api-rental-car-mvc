using System.Collections.Generic;
using System.Threading.Tasks;
using cs_api_rental_car_mvc.Dtos;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Dtos.Response;
using cs_api_rental_car_mvc.Entities;

namespace cs_api_rental_car_mvc.Services.CarService
{
    public interface ICarService
    {
        Task<(HttpError?, PaginationBaseResponse<CarEntity>)> GetCar(PaginationRequestDto requestDto);
        Task<(HttpError?, CarEntity)> GetCarById(int id);
        Task<(HttpError?, CarEntity)> PostCar(CarRequestDto request);
        Task<(HttpError?, CarEntity)> PatchCar(int id, Dictionary<string, object> updates);
        Task<HttpError?> DeleteCar(int id);
        Task<(HttpError?, CarEntity)> PutCar(int id, CarRequestDto product);
    }
}