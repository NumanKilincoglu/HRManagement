using Microsoft.AspNetCore.Authorization;
using MiniExcelLibs;
using Pusula.Training.HealthCare.Permissions;
using Pusula.Training.HealthCare.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Titles
{
    [RemoteService(IsEnabled = false)]
    [Authorize(HealthCarePermissions.Titles.Default)]
    public class TitlesAppService(ITitleRepository titleRepository,
        TitleManager titleManager,
        IDistributedCache<TitleDownloadTokenCacheItem, string> downloadTokenCache)
        : HealthCareAppService, ITitlesAppService
    {
        public virtual async Task<PagedResultDto<TitleDto>> GetListAsync(GetTitlesInput input)
        {
            var totalCount = await titleRepository.GetCountAsync(input.FilterText, input.Name);
            var items = await titleRepository.GetListAsync(input.FilterText, input.Name, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<TitleDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Title>, List<TitleDto>>(items)
            };
        }

        public virtual async Task<TitleDto> GetAsync(Guid id) => ObjectMapper.Map<Title, TitleDto>(await titleRepository.GetAsync(id));

        [Authorize(HealthCarePermissions.Titles.Create)]
        public virtual async Task<TitleDto> CreateAsync(TitleCreateDto input)
        {
            var title = await titleManager.CreateAsync(input.Name);

            return ObjectMapper.Map<Title, TitleDto>(title);
        }

        [Authorize(HealthCarePermissions.Titles.Edit)]
        public virtual async Task<TitleDto> UpdateAsync(TitleUpdateDto input)
        {
            var title = await titleManager.UpdateAsync(input.Id, input.Name);

            return ObjectMapper.Map<Title, TitleDto>(title);
        }

        [Authorize(HealthCarePermissions.Titles.Delete)]
        public virtual void DeleteAsync(Guid id) => titleManager.DeleteAsync(id);

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(TitleExcelDownloadDto input)
        {
            var downloadToken = await downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);

            var items = await titleRepository.GetListAsync(input.FilterText, input.Name);
            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Title>, List<TitleExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Title.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(HealthCarePermissions.Titles.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> titleIds) => await titleRepository.DeleteManyAsync(titleIds);
        [Authorize(HealthCarePermissions.Titles.Delete)]
        public virtual async Task DeleteAllAsync(GetTitlesInput input) => await titleRepository.DeleteAllAsync(input.FilterText, input.Name);
        public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await downloadTokenCache.SetAsync(
                token,
                new TitleDownloadTokenCacheItem() { Token = token },
                new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30)
                }
            );

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }

    }
}
