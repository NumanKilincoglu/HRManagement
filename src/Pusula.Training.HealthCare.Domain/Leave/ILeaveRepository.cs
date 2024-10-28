using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Pusula.Training.HealthCare.Leaves;

public interface ILeaveRepository : IRepository<Leave, Guid>
{
    Task<List<Leave>> GetListAsync(
        Guid EmployeeId,
        DateTime? startDate,
        DateTime? endDate,
        string? LeaveType,
        string? Status,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
    
    Task<Leave?> GetEmployeeLeaveAsync(
        Guid EmployeeId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        Guid EmployeeId,
        DateTime? startDate,
        DateTime? endDate,
        string? LeaveType,
        string? Status,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default);

    Task DeleteAllAsync(
        DateTime? startDate,
        DateTime? endDate,
        string? LeaveType,
        string? Status,
        CancellationToken cancellationToken = default);
}