using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using cs_api_rental_car_mvc.Data;
using cs_api_rental_car_mvc.Dtos;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Dtos.Response;
using cs_api_rental_car_mvc.Entities;
using cs_api_rental_car_mvc.Exceptions;
using cs_api_rental_car_mvc.Query;
using cs_api_rental_car_mvc.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace cs_api_rental_car_mvc.Services.CarService
{
    public class CarService : ICarService
    {
        private readonly RentalCarDbContext _context;
        private readonly IMapper _mapper;

        public CarService(RentalCarDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<HttpError?> DeleteCar(int id)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(p => p.Id == id);

            if (car is null)
            {
                return new HttpError("Product that you want to delete is not found!") { StatusCode = StatusCodes.Status404NotFound };
            }

            _context.Cars.Remove(car);

            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<(HttpError?, PaginationBaseResponse<CarEntity>)> GetCar(PaginationRequestDto requestDto)
        {
            var query = CarQuery.GetQuery(_context, requestDto);

            // Calculate the total number of items BEFORE applying skip/take.
            var totalCount = await query.CountAsync();

            // Apply pagination logic (Skip and Take)
            var items = await query
                .Skip((requestDto.Page - 1) * requestDto.PageSize)
                .Take(requestDto.PageSize)
                .ToListAsync();

            return (null, new PaginationBaseResponse<CarEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = requestDto.Page,
                PageSize = requestDto.PageSize,
            });
        }

        public async Task<(HttpError?, CarEntity)> GetCarById(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            return car is null ? (new HttpError("Car with specified ID not found!") { StatusCode = StatusCodes.Status404NotFound }, new CarEntity()) : (null, car);
        }

        public async Task<(HttpError?, CarEntity)> PatchCar(int id, Dictionary<string, object> updates)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(vg => vg.Id == id) ?? throw new EntityNotFoundException($"Car with ID {id} not found.");

            var invalidPropertyList = EntityUtil.CheckEntityField<CarEntity>(updates);
            if (invalidPropertyList.Count > 0)
            {
                throw new ArgumentException($"Invalid property: {string.Join(", ", invalidPropertyList)}");
            }

            // Update fields dynamically
            EntityUtil.PatchEntity(car, updates);

            _context.Entry(car).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return (null, car);
        }

        public async Task<(HttpError?, CarEntity)> PostCar(CarRequestDto request)
        {
            var car = _mapper.Map<CarEntity>(request);

            await _context.Cars.AddAsync(car);

            await _context.SaveChangesAsync();

            return (null, car);
        }

        public async Task<(HttpError?, CarEntity)> PutCar(int id, CarRequestDto request)
        {
            var existingProduct = await _context.Cars
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new EntityNotFoundException($"Car with ID {id} not found.");

            _mapper.Map(request, existingProduct);

            _context.Entry(existingProduct).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return (null, existingProduct);
        }
    }
}