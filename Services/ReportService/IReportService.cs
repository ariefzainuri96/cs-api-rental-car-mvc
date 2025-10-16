using System;
using System.Threading.Tasks;

namespace cs_api_rental_car_mvc.Services.ReportService
{
    public interface IReportService
    {
        Task<byte[]> GenerateExcelReport();
        Task<byte[]> GeneratePdfReport();
    }
}