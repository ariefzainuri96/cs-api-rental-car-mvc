using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using cs_api_rental_car_mvc.Data;
using cs_api_rental_car_mvc.Dtos;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Entities;
using cs_api_rental_car_mvc.Exceptions;
using cs_api_rental_car_mvc.Utils;
using Microsoft.EntityFrameworkCore;

namespace cs_api_rental_car_mvc.Services.RentService
{
    public class RentService : IRentService
    {
        private readonly RentalCarDbContext context;
        private readonly IMapper mapper;

        public RentService(RentalCarDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<(HttpError?, RentEntity)> PatchRent(int id, Dictionary<string, object> updates)
        {
            var rent = await context.Rents.FirstOrDefaultAsync(x => x.Id == id) ?? throw new EntityNotFoundException($"Rent with ID {id} not found.");

            var invalidPropertyList = EntityUtil.CheckEntityField<CarEntity>(updates);
            if (invalidPropertyList.Count > 0)
            {
                throw new ArgumentException($"Invalid property: {string.Join(", ", invalidPropertyList)}");
            }

            // Update fields dynamically
            EntityUtil.PatchEntity(rent, updates);

            context.Entry(rent).State = EntityState.Modified;

            context.SaveChanges();

            return (null, rent);
        }

        public async Task<(HttpError?, RentEntity)> PostRent(RentRequestDto request)
        {
            var car = await context.Cars.FirstAsync(c => c.Id == request.CarId);

            var totalDay = (request.EndDate - request.StartDate).TotalDays;

            var totalAmount = car.RentalRatePerDay * (decimal)totalDay;

            var rentEntity = mapper.Map<RentEntity>(request);

            rentEntity.TotalAmount = totalAmount;
            rentEntity.Status = "Disewa";

            await context.Rents.AddAsync(rentEntity);
            await context.SaveChangesAsync();

            

            return (null, rentEntity);
        }
    }
}