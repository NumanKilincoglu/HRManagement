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
    
    public virtual async Task<long> GetCountAsync(Guid EmployeeId,
        DateTime? startDate,
        DateTime? endDate,
        string? LeaveType,
        string? Status,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), startDate, endDate, LeaveType, Status, EmployeeId);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Leave>> GetListAsync(
        Guid EmployeeId,
        DateTime? startDate,
        DateTime? endDate,
        string? LeaveType,
        string? Status,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), startDate, endDate, LeaveType, Status, EmployeeId);
        
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? LeaveConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public Task DeleteAllAsync(DateTime? startDate, DateTime? endDate, string? LeaveType, string? Status,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #region ApplyFilter and Queryable

    protected virtual IQueryable<Leave> ApplyFilter(
        IQueryable<Leave> query,
        DateTime? startDate,
        DateTime? endDate,
        string? LeaveType,
        string? Status,
        Guid employeeId) =>
        query
            .WhereIf(!string.IsNullOrWhiteSpace(Status), e => e.Status!.Contains(Status!))
            .WhereIf(!string.IsNullOrWhiteSpace(LeaveType), e => e.LeaveType!.Contains(LeaveType!))
            .WhereIf(startDate.HasValue, e => e.StartDate >= startDate!.Value)
            .WhereIf(endDate.HasValue, e => e.EndDate <= endDate!.Value)
            .Where(e => e.EmployeeId == employeeId);

    
    #endregion
    
}