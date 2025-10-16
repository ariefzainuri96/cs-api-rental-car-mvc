using System.Linq;
using cs_api_rental_car_mvc.Data;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Entities;
using Microsoft.EntityFrameworkCore;

namespace cs_api_rental_car_mvc.Query
{
    public class RentQuery
    {
        public static IQueryable<RentEntity> GetQuery(RentalCarDbContext context, PaginationRequestDto request, int? userId)
        {
            IQueryable<RentEntity> query = context.Rents.Include(r => r.Car).Include(r => r.User);

            if (!string.IsNullOrWhiteSpace(request.SearchAll))
            {
                query = query.Where(c =>
                c.Car != null && c.Car.Brand.Contains(request.SearchAll) ||
                c.Car != null && c.Car.Model.Contains(request.SearchAll) ||
                c.Car != null && c.Car.Year.ToString() == request.SearchAll ||
                c.User != null && c.User.Name.Contains(request.SearchAll) ||
                c.User != null && c.User.Email.Contains(request.SearchAll) ||
                c.StartDate.ToString() == request.SearchAll ||
                c.EndDate.ToString() == request.SearchAll ||
                c.Status.Contains(request.SearchAll)
                );
            }
            else
            {
                query = query.ApplyDynamicFilter(request);
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