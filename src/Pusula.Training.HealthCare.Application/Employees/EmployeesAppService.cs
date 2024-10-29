using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using MiniExcelLibs;
using Newtonsoft.Json;
using Pusula.Training.HealthCare.Permissions;
using Pusula.Training.HealthCare.Shared;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.EventBus.Distributed;

namespace Pusula.Training.HealthCare.Employees
{
    [RemoteService(IsEnabled = false)]
    [Authorize(HealthCarePermissions.Employees.Default)]
    public class EmployeesAppService(
        IEmployeeRepository employeeRepository,
        EmployeeManager employeeManager,
        IDistributedCache<EmployeeDownloadTokenCacheItem, string> downloadTokenCache,
        IDistributedCache<EmployeeDto> employeeCache,
        IDistributedEventBus distributedEventBus
    ) : HealthCareAppService, IEmployeesAppService
    {
        public async Task<PagedResultDto<EmployeeDto>> GetListAsync(GetEmployeesInput input)
        {
            var items = await employeeRepository.GetListAsync(input.FilterText, input.FirstName,
                input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.Sorting,
                input.MaxResultCount, input.SkipCount);

            var totalCount = await employeeRepository.GetCountAsync(input.FilterText, input.FirstName, input.LastName,
                input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.Sorting,
                input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<EmployeeDto>
            {
                TotalCount = totalCount,
                Items =
                    ObjectMapper
                        .Map<List<Employee>, List<EmployeeDto>>(items)
            };
        }

        public async Task<EmployeeWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id) =>
            ObjectMapper.Map<EmployeeWithNavigationProperties, EmployeeWithNavigationPropertiesDto>(
                await employeeRepository.GetWithNavigationPropertiesAsync(id));

        public async Task<EmployeeDto> GetAsync(Guid id)
        {
            var cacheKey = $"employee_{id}";
            var employee = await employeeCache.GetAsync(cacheKey);
            if (employee != null)
            {
                return employee;
            }
            
            var employeeDto = ObjectMapper.Map<Employee, EmployeeDto>(await employeeRepository.GetAsync(id));

            await employeeCache.SetAsync(
                cacheKey,
                employeeDto,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(200)
                }
            );
            
            return employeeDto;
        }

        [Authorize(HealthCarePermissions.Employees.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await distributedEventBus.PublishAsync(new EmployeeDeletedEto { Id = id, DeletedAt = Clock.Now },
                onUnitOfWorkComplete: true);
            await employeeRepository.DeleteAsync(id);
        }

        [Authorize(HealthCarePermissions.Employees.Create)]
        public async Task<EmployeeDto> CreateAsync(EmployeeCreateDto input)
        {
            return ObjectMapper.Map<Employee, EmployeeDto>(
                await employeeManager.CreateAsync(input.FirstName,
                    input.LastName,
                    input.BirthDate,
                    input.IdentityNumber,
                    input.EmailAddress,
                    input.MobilePhoneNumber,
                    input.Gender,
                    input.HomePhoneNumber));
        }

        [Authorize(HealthCarePermissions.Employees.Edit)]
        public async Task<EmployeeDto> UpdateAsync(EmployeeUpdateDto input) =>
            ObjectMapper.Map<Employee, EmployeeDto>(
                await employeeManager.UpdateAsync(
                    input.Id,
                    input.FirstName,
                    input.LastName,
                    input.BirthDate,
                    input.IdentityNumber,
                    input.EmailAddress,
                    input.MobilePhoneNumber,
                    input.Gender,
                    input.Salary,
                    input.HomePhoneNumber));


        [AllowAnonymous]
        public async Task<IRemoteStreamContent> GetListAsExcelFileAsync(EmployeeExcelDownloadDto input)
        {
            var downloadToken = await downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var employees = await employeeRepository.GetListAsync(input.FilterText, input.FirstName,
                input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender);

            var items = employees.Select(employee => new EmployeeExcelDto()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PhoneNumber = employee.MobilePhoneNumber,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                Salary = employee.Salary
            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Employees.xlsx",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(HealthCarePermissions.Employees.Delete)]
        public async Task DeleteByIdsAsync(List<Guid> doctorIds) => await employeeRepository.DeleteManyAsync(doctorIds);

        [Authorize(HealthCarePermissions.Employees.Delete)]
        public async Task DeleteAllAsync(GetEmployeesInput input) =>
            await employeeRepository.DeleteAllAsync(
                input.FilterText,
                input.FirstName,
                input.LastName,
                input.PhoneNumber,
                input.BirthDateMin,
                input.BirthDateMax,
                input.Gender
            );

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await downloadTokenCache.SetAsync(
                token,
                new EmployeeDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(200)
                });

            return new Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}