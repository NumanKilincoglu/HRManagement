using Microsoft.EntityFrameworkCore;
using Pusula.Training.HealthCare.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Leaves;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Pusula.Training.HealthCare.Leaves;

public class EfCoreLeaveRepository(IDbContextProvider<HealthCareDbContext> dbContextProvider)
    :  EfCoreRepository<HealthCareDbContext, Leave, Guid>(dbContextProvider), ILeaveRepository
{

    public async Task<Leave?> GetEmployeeLeaveAsync(Guid employeeId, DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), startDate, endDate, employeeId);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(Guid? employeeId,
        DateTime? startDate,
        DateTime? endDate,
        string? leaveType,
        string? status,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), startDate, endDate, leaveType, status, employeeId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Leave>> GetListAsync(
        Guid? employeeId,
        DateTime? startDate,
        DateTime? endDate,
        string? leaveType,
        string? status,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), startDate, endDate, leaveType, status, employeeId);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? LeaveConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public async Task DeleteAllAsync(DateTime? startDate, DateTime? endDate, string? leaveType, string? status,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), startDate, endDate, leaveType, status, null);
        var ids = query.Select(x => x.Id).ToList();
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    #region ApplyFilter and Queryable

    protected virtual IQueryable<Leave> ApplyFilter(
        IQueryable<Leave> query,
        DateTime? startDate,
        DateTime? endDate,
        string? leaveType,
        string? status,
        Guid? employeeId) =>
        query
            .WhereIf(!string.IsNullOrWhiteSpace(status), e => e.Status!.Contains(status!))
            .WhereIf(!string.IsNullOrWhiteSpace(leaveType), e => e.LeaveType!.Contains(leaveType!))
            .WhereIf(startDate.HasValue, e => e.StartDate >= startDate!.Value)
            .WhereIf(endDate.HasValue, e => e.EndDate <= endDate!.Value)
            .WhereIf(employeeId.HasValue, e=> e.EmployeeId == employeeId);

    protected virtual IQueryable<Leave> ApplyFilter(
        IQueryable<Leave> query,
        DateTime startDate,
        DateTime endDate,
        Guid employeeId) =>
        query
            .Where(e => e.StartDate >= startDate && e.EndDate <= endDate)
            .Where(e => e.EmployeeId == employeeId);
    #endregion
    
}