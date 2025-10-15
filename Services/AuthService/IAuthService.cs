using System.Threading.Tasks;
using cs_api_rental_car_mvc.Dtos;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Dtos.Response;

namespace cs_api_rental_car_mvc.Services.AuthService
{
    public interface IAuthService
    {
        Task<HttpError?> RegisterAsync(RegisterRequestDto request);
        Task<(HttpError?, LoginResponseDto)> LoginAsync(LoginRequestDto request);
    }
}