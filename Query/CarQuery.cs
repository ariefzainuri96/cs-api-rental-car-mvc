using System.Linq;
using cs_api_rental_car_mvc.Data;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Entities;

namespace cs_api_rental_car_mvc.Query
{
    public static class CarQuery
    {
        public static IQueryable<CarEntity> GetQuery(RentalCarDbContext context, PaginationRequestDto request)
        {
            IQueryable<CarEntity> query = context.Cars;

            if (!string.IsNullOrWhiteSpace(request.SearchAll))
            {
                query = query.Where(c =>
                    c.Brand.ToLower().Contains(request.SearchAll) ||
                    c.Model.ToLower().Contains(request.SearchAll) ||
                    c.PlateNumber.ToLower().Contains(request.SearchAll)
                );

                if (int.TryParse(request.SearchAll, out _))
                {
                    query = query.Where(c => c.Year == int.Parse(request.SearchAll));
                }

                if (decimal.TryParse(request.SearchAll, out _))
                {
                    query = query.Where(c => c.RentalRatePerDay == int.Parse(request.SearchAll));
                }
            }
            else
            {
                query = query.ApplyDynamicFilter(request);
            }

            // sort
            query = query.ApplyOrdering(request);

            return query;
        }
    }
}