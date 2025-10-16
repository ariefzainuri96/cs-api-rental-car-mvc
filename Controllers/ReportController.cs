using System.Security.Claims;
using System.Threading.Tasks;
using cs_api_rental_car_mvc.Dtos.Response;
using cs_api_rental_car_mvc.Services.ReportService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cs_api_rental_car_mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [Authorize]
        [HttpGet("excel")]
        public async Task<IActionResult> GetExcelReport()
        {
            if (!IsAdmin())
            {
                return StatusCode(StatusCodes.Status403Forbidden, new BaseResponse<object>
                {
                    Status = 403,
                    Message = "You are not authorized to perform this action.",
                    Data = null
                });
            }

            var file = await reportService.GenerateExcelReport();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
        }

        [Authorize]
        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdfReport()
        {
            if (!IsAdmin())
            {
                return StatusCode(StatusCodes.Status403Forbidden, new BaseResponse<object>
                {
                    Status = 403,
                    Message = "You are not authorized to perform this action.",
                    Data = null
                });
            }

            var file = await reportService.GeneratePdfReport();
            return File(file, "application/pdf", "report.pdf");
        }

        private bool IsAdmin()
        {
            string? role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (role == null)
            {
                return false;
            }

            return role == "admin";
        }

    }
}
