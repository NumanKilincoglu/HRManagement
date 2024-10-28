using System;
using System.Threading.Tasks;
using Pusula.Training.HealthCare.Employees;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Services;

namespace Pusula.Training.HealthCare.Leaves;

public class LeaveManager(ILeaveRepository leaveRepository) : DomainService
{
    public virtual async Task<Leave> CreateAsync(
        Guid employeeId,
        DateTime startDate,
        DateTime endDate,
        string leaveType,
        string status)
    {
        Check.NotNull(employeeId, nameof(employeeId));
        Check.NotNullOrWhiteSpace(leaveType, nameof(leaveType));
        Check.NotNull(startDate, nameof(startDate));
        Check.NotNull(endDate, nameof(endDate));
        
        if ((startDate < DateTime.Today) || (endDate < DateTime.Today))
        {
            throw new UserFriendlyException("End date or Start Date cannot be less than Current Time.");
        }
        
        if (endDate < startDate)
        {
            throw new UserFriendlyException("End date cannot be earlier than start date.");
        }
        
        var hasLeaveConflict = await leaveRepository.GetEmployeeLeaveAsync(
            employeeId,
            startDate,
            endDate);
 
        if (hasLeaveConflict != null)
        {
            throw new UserFriendlyException("The employee already has a leave during this period.");
        }
        
        var leave = new Leave(
            GuidGenerator.Create(),
            employeeId,
            startDate,
            endDate,
            leaveType,
            status
        );
        
        return await leaveRepository.InsertAsync(leave);
    }

    public virtual async Task<Leave> UpdateAsync(
        Guid id,
        Guid employeeId,
        DateTime startDate,
        DateTime endDate,
        string leaveType,
        string status
        )
    {
        Check.NotNull(id, nameof(id));
        Check.NotNullOrWhiteSpace(leaveType, nameof(leaveType));
        Check.NotNull(startDate, nameof(startDate));
        Check.NotNull(endDate, nameof(endDate));
        Check.NotNullOrWhiteSpace(status, nameof(status));
        
        if ((startDate < DateTime.Today) || (endDate < DateTime.Today))
        {
            throw new UserFriendlyException("End date or Start Date cannot be less than Current Time.");
        }
        
        if (endDate < startDate)
        {
            throw new UserFriendlyException("End date cannot be earlier than start date.");
        }

        var leave = await leaveRepository.GetAsync(id);

        leave.LeaveType = leaveType;
        leave.Status = status;
        leave.StartDate = startDate;
        leave.EndDate = endDate;
        leave.EmployeeId = employeeId;
        
        return await leaveRepository.UpdateAsync(leave);
    }
}