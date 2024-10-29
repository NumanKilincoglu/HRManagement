using System;
namespace Pusula.Training.HealthCare.Leaves

{
    public class LeaveExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;
        public Guid? EmployeeId { get; set; }
        public string? LeaveType { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LeaveExcelDownloadDto() { }
    }
}
