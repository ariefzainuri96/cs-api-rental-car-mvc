using AutoMapper;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Entities;

namespace cs_api_rental_car_mvc.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CarEntity, CarRequestDto>();
            CreateMap<CarRequestDto, CarEntity>();   
            CreateMap<RentEntity, RentRequestDto>();
            CreateMap<RentRequestDto, RentEntity>();       
        }
    }
}
