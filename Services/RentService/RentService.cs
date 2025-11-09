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
using Microsoft.EntityFrameworkCore;

namespace cs_api_rental_car_mvc.Services.RentService
{
    public class RentService : IRentService
    {
        private readonly RentalCarDbContext _context;
        private readonly IMapper _mapper;

        public RentService(RentalCarDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(HttpError?, RentEntity)> CancelRent(int id)
        {
            return await ChangeStatus(id, "Dibatalkan");
        }

        public async Task<(HttpError?, PaginationBaseResponse<RentEntity>)> GetAllRents(PaginationRequestDto requestDto)
        {
            IQueryable<RentEntity> query = RentQuery.GetQuery(_context, requestDto);

            // Calculate the total number of items BEFORE applying skip/take.
            int totalCount = await query.CountAsync();

            // Apply pagination logic (Skip and Take)
            List<RentEntity> items = await query
                .Include(x => x.Car)
                .Include(x => x.User)
                .Skip((requestDto.Page - 1) * requestDto.PageSize)
                .Take(requestDto.PageSize)
                .ToListAsync();

            return (null, new PaginationBaseResponse<RentEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = requestDto.Page,
                PageSize = requestDto.PageSize,
            });
        }

        public async Task<(HttpError?, PaginationBaseResponse<RentEntity>)> GetSpecificUserRent(
            PaginationRequestDto requestDto, int userId)
        {
            IQueryable<RentEntity> query = RentQuery.GetQuery(_context, requestDto, userId: userId);

            // Calculate the total number of items BEFORE applying skip/take.
            int totalCount = await query.CountAsync();

            // Apply pagination logic (Skip and Take)
            List<RentEntity> items = await query
                .Skip((requestDto.Page - 1) * requestDto.PageSize)
                .Take(requestDto.PageSize)
                .ToListAsync();

            return (null, new PaginationBaseResponse<RentEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = requestDto.Page,
                PageSize = requestDto.PageSize,
            });
        }

        public async Task<(HttpError?, RentEntity)> PatchRent(int id, Dictionary<string, object> updates)
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(x => x.Id == id) ??
                       throw new EntityNotFoundException($"Rent with ID {id} not found.");

            var invalidPropertyList = EntityUtil.CheckEntityField<CarEntity>(updates);
            if (invalidPropertyList.Count > 0)
            {
                throw new ArgumentException($"Invalid property: {string.Join(", ", invalidPropertyList)}");
            }

            // Update fields dynamically
            EntityUtil.PatchEntity(rent, updates);

            _context.Entry(rent).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return (null, rent);
        }

        public async Task<(HttpError?, RentEntity)> PostRent(RentRequestDto request)
        {
            var car = await _context.Cars.FirstAsync(c => c.Id == request.CarId);

            var totalDay = (request.EndDate - request.StartDate).TotalDays + 1;

            var totalAmount = car.RentalRatePerDay * (decimal)totalDay;

            var rentEntity = _mapper.Map<RentEntity>(request);

            rentEntity.TotalAmount = totalAmount;
            rentEntity.Status = "Active";

            await _context.Rents.AddAsync(rentEntity);

            car.Status = "Disewa";
            _context.Entry(car).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return (null, rentEntity);
        }

        public async Task<(HttpError?, RentEntity)> ReturnCar(int id)
        {
            return await ChangeStatus(id, "Selesai");
        }

        private async Task<(HttpError?, RentEntity)> ChangeStatus(int id, string status)
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(x => x.Id == id) ??
                       throw new EntityNotFoundException($"Rent with ID {id} not found.");

            rent.ActualEndDate = DateTime.Now;
            rent.Status = status;

            var car = await _context.Cars.FirstAsync(x => x.Id == rent.CarId);

            car.Status = "Tersedia";

            _context.Entry(rent).State = EntityState.Modified;
            _context.Entry(car).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return (null, rent);
        }
    }
}