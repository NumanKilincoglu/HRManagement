using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using MiniExcelLibs;
using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.Permissions;
using Pusula.Training.HealthCare.Protocols;
using Pusula.Training.HealthCare.Shared;
using Pusula.Training.HealthCare.Titles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Doctors
{
    [RemoteService(IsEnabled = false)]
    [Authorize(HealthCarePermissions.Doctors.Default)]
    public class DoctorsAppService(
        IDoctorRepository doctorRepository,
        DoctorManager doctorManager,
        IDistributedCache<DoctorDownloadTokenCacheItem, string> downloadTokenCache,
        ITitleRepository titleRepository,
        IDepartmentRepository departmentRepository) : HealthCareAppService, IDoctorsAppService
    {

        public virtual async Task<PagedResultDto<DoctorWithNavigationPropertiesDto>> GetListAsync(GetDoctorsInput input)
        {
            var totalCount = await doctorRepository.GetCountAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.DepartmentId, input.TitleId, input.Sorting, input.MaxResultCount, input.SkipCount);
            var items = await doctorRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.DepartmentId, input.TitleId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<DoctorWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<DoctorWithNavigationProperties>, List<DoctorWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<DoctorWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id) =>
            ObjectMapper.Map<DoctorWithNavigationProperties, DoctorWithNavigationPropertiesDto>(
                await doctorRepository.GetWithNavigationPropertiesAsync(id));

        public virtual async Task<DoctorDto> GetAsync(Guid id) => ObjectMapper.Map<Doctor, DoctorDto>(
                await doctorRepository.GetAsync(id));

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetTitleLookupAsync(LookupRequestDto input)
        {
            var query = (await titleRepository.GetQueryableAsync())
                           .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                               x => x.Name != null && x.Name.Contains(input.Filter!));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Title>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Title>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetDepartmentLookupAsync(LookupRequestDto input)
        {
            var query = (await departmentRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null && x.Name.Contains(input.Filter!));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Department>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Department>, List<LookupDto<Guid>>>(lookupData)
            };
        }


        [Authorize(HealthCarePermissions.Doctors.Delete)]
        public virtual async Task DeleteAsync(Guid id) => await doctorRepository.DeleteAsync(id);

        [Authorize(HealthCarePermissions.Doctors.Create)]
        public virtual async Task<DoctorDto> CreateAsync(DoctorCreateDto input) => ObjectMapper.Map<Doctor, DoctorDto>(
            await doctorManager.CreateAsync(input.TitleId, input.DepartmentId, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDate, input.Gender));

        [Authorize(HealthCarePermissions.Doctors.Edit)]
        public virtual async Task<DoctorDto> UpdateAsync(DoctorUpdateDto input) => ObjectMapper.Map<Doctor, DoctorDto>(
            await doctorManager.UpdateAsync(input.Id, input.TitleId, input.DepartmentId, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDate, input.Gender));

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(DoctorExcelDownloadDto input)
        {
            var downloadToken = await downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var doctors = await doctorRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.DepartmentId, input.TitleId);
            var items = doctors.Select(item => new DoctorExcelDto
            {
                FirstName = item.Doctor.FirstName,
                LastName = item.Doctor.LastName,
                PhoneNumber = item.Doctor.PhoneNumber,
                BirthDate = item.Doctor.BirthDate,
                Gender = item.Doctor.Gender,
                Department = item.Department?.Name ?? string.Empty,
                Title = item.Title?.Name ?? string.Empty
            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Doctors.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(HealthCarePermissions.Doctors.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> doctorIds) => await doctorRepository.DeleteManyAsync(doctorIds);

        [Authorize(HealthCarePermissions.Doctors.Delete)]
        public virtual async Task DeleteAllAsync(GetDoctorsInput input) => await doctorRepository.DeleteAllAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.DepartmentId, input.TitleId);

        public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await downloadTokenCache.SetAsync(
                token,
                new DoctorDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new Shared.DownloadTokenResultDto
            {
                Token = token
            };


        }
    }
}