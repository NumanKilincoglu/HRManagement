using Microsoft.EntityFrameworkCore;
using Pusula.Training.HealthCare.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Pusula.Training.HealthCare.Employees;

public class EfCoreEmployeeRepository(IDbContextProvider<HealthCareDbContext> dbContextProvider)
    :  EfCoreRepository<HealthCareDbContext, Employee, Guid>(dbContextProvider), IEmployeeRepository
{

    public async  Task DeleteAllAsync(string? filterText = null, string? firstName = null, string? lastName = null,
        string? phoneNumber = null, DateTime? birthDateMin = null, DateTime? birthDateMax = null, EnumGender? gender = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, gender);

        var ids = query.Select(x => x.Employee.Id);
        await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<EmployeeWithNavigationProperties> GetWithNavigationPropertiesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        return (await GetDbSetAsync()).Where(b => b.Id == id)
            .Select(employee => new EmployeeWithNavigationProperties
            {
                Employee = employee,
                Leave = dbContext.Set<Leave>().FirstOrDefault(c => c.EmployeeId == employee.Id)!,
            })
            .FirstOrDefault()!;
    }

    public async Task<long> GetCountAsync(string? filterText = null, string? firstName = null, string? lastName = null,
        string? phoneNumber = null, DateTime? birthDateMin = null, DateTime? birthDateMax = null, EnumGender? gender = null,
        string? sorting = null, int maxResultCount = Int32.MaxValue, int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, gender);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<EmployeeWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
        string? filterText = null,
        string? firstName = null,
        string? lastName = null,
        string? phoneNumber = null,
        DateTime? birthDateMin = null,
        DateTime? birthDateMax = null,
        EnumGender? gender = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryForNavigationPropertiesAsync();
        query = ApplyFilter(query, filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, gender);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? EmployeeConsts.GetDefaultSorting(true) : sorting);
        
        
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<Employee>> GetListAsync(
        string? filterText = null,
        string? firstName = null,
        string? lastName = null,
        string? phoneNumber = null,
        DateTime? birthDateMin = null,
        DateTime? birthDateMax = null,
        EnumGender? gender = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter(await GetQueryableAsync(), filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, gender);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? EmployeeConsts.GetDefaultSorting(false) : sorting);
        return await query.Page(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    #region ApplyFilter and Queryable

    protected virtual IQueryable<Employee> ApplyFilter(
        IQueryable<Employee> query,
        string? filterText = null,
        string? firstName = null,
        string? lastName = null,
        string? phoneNumber = null,
        DateTime? birthDateMin = null,
        DateTime? birthDateMax = null,
        EnumGender? gender = null) =>
        query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText),
                e => e.FirstName!.Contains(filterText!) || e.LastName!.Contains(filterText!) ||
                     e.MobilePhoneNumber!.Contains(filterText!))
            .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.FirstName!.Contains(firstName!))
            .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.LastName!.Contains(lastName!))
            .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), e => e.MobilePhoneNumber!.Contains(phoneNumber!))
            .WhereIf(birthDateMin.HasValue, e => e.BirthDate >= birthDateMin!.Value)
            .WhereIf(birthDateMax.HasValue, e => e.BirthDate <= birthDateMax!.Value)
            .WhereIf(gender.HasValue, e => e.Gender == gender);

    protected virtual async Task<IQueryable<EmployeeWithNavigationProperties>> GetQueryForNavigationPropertiesAsync() =>
        from employee in (await GetDbSetAsync())
        join leave in (await GetDbContextAsync()).Set<Leave>() on employee.Id equals leave.EmployeeId into leaves
        from leave in leaves.DefaultIfEmpty()
        select new EmployeeWithNavigationProperties
        {
            Employee = employee,
            Leave = leave
        };
    

    protected virtual IQueryable<EmployeeWithNavigationProperties> ApplyFilter(
        IQueryable<EmployeeWithNavigationProperties> query,
        string? filterText = null,
        string? firstName = null,
        string? lastName = null,
        string? phoneNumber = null,
        DateTime? birthDateMin = null,
        DateTime? birthDateMax = null,
        EnumGender? gender = null) =>
            query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Employee.FirstName!.Contains(filterText!) || e.Employee.LastName!.Contains(filterText!) || e.Employee.MobilePhoneNumber!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.Employee.FirstName!.Contains(firstName!))
                .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.Employee.LastName!.Contains(lastName!))
                .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), e => e.Employee.MobilePhoneNumber!.Contains(phoneNumber!))
                .WhereIf(birthDateMin.HasValue, e => e.Employee.BirthDate >= birthDateMin!.Value)
                .WhereIf(birthDateMax.HasValue, e => e.Employee.BirthDate <= birthDateMax!.Value)
                .WhereIf(gender.HasValue, e => e.Employee.Gender == gender);
    #endregion
}