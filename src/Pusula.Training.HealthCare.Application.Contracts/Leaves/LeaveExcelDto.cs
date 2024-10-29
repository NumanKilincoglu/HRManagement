using System;

namespace Pusula.Training.HealthCare.Leaves
{
    public class LeaveExcelDto
    {
        public string? LeaveType { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
