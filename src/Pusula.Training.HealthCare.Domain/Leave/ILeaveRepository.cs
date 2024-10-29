using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Pusula.Training.HealthCare.Leaves;

public interface ILeaveRepository : IRepository<Leave, Guid>
{
    Task<List<Leave>> GetListAsync(
        Guid? employeeId,
        DateTime? startDate,
        DateTime? endDate,
        string? laveType,
        string? status,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default);

    Task<Leave?> GetEmployeeLeaveAsync(
        Guid employeeId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        Guid? employeeId,
        DateTime? startDate,
        DateTime? endDate,
        string? leaveType,
        string? status,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default);

    Task DeleteAllAsync(
        DateTime? startDate,
        DateTime? endDate,
        string? leaveType,
        string? status,
        CancellationToken cancellationToken = default);
}