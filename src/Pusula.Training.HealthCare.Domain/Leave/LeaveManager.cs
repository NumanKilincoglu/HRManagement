using System;
using System.Threading.Tasks;
using Pusula.Training.HealthCare.Employees;
using Volo.Abp.Domain.Services;

namespace Pusula.Training.HealthCare.Leaves;

public class LeaveManager(ILeaveRepository leaveRepository) : DomainService
{
    public virtual async Task<Leave> CreateAsync()
    {

        return null;
    }

    public virtual async Task<Employee> UpdateAsync()

    {
        return null;
    }
}