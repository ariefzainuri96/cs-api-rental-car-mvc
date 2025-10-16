namespace cs_api_rental_car_mvc.Dtos.Request
{
    public class PaginationRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchAll { get; set; } = string.Empty;
        public string SearchField { get; set; } = string.Empty;
        public string SearchValue { get; set; } = string.Empty;
        public string OrderBy { get; set; } = "Id"; // Default to Id as a fail-safe
        public string OrderDirection { get; set; } = "asc"; // "asc" or "desc"
    }
}