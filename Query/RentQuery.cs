using System;
using System.Linq;
using cs_api_rental_car_mvc.Data;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Entities;

namespace cs_api_rental_car_mvc.Query
{
    public class RentQuery
    {
        public static IQueryable<RentEntity> GetQuery(RentalCarDbContext context, PaginationRequestDto request,
            int? userId = null, bool? filterIsFinished = false)
        {
            IQueryable<RentEntity> query = context.Rents.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchAll))
            {
                query = query.Where(c =>
                    (c.Car != null && (
                        c.Car.Brand.ToLower().Contains(request.SearchAll) ||
                        c.Car.Model.ToLower().Contains(request.SearchAll) ||
                        c.Car.PlateNumber.ToLower().Contains(request.SearchAll)                       
                    )) ||
                    (c.User != null && (
                        c.User.Name.ToLower().Contains(request.SearchAll) ||
                        c.User.Email.ToLower().Contains(request.SearchAll)
                    )) ||
                    c.Status.ToLower().Contains(request.SearchAll)
                );

                if (int.TryParse(request.SearchAll, out _))
                {
                    query = query.Where(c => c.Car != null && c.Car.Year == int.Parse(request.SearchAll));
                }

                if (DateTime.TryParse(request.SearchAll, out var parsedDate))
                {
                    query = query.Where(c =>
                        c.StartDate.Date == parsedDate.Date ||
                        c.EndDate.Date == parsedDate.Date
                    );
                }
            }
            else
            {
                query = query.ApplyDynamicFilter(request);
            }

            if (filterIsFinished != null)
            {
                query = query.Where(r => r.Status.ToLower().Equals(filterIsFinished.Value ? "selesai" : "active"));
            }

            if (userId != null)
            {
                query = query.Where(r => r.UserId == userId);
            }

            // sort
            query = query.ApplyOrdering(request);

            return query;
        }
    }
}