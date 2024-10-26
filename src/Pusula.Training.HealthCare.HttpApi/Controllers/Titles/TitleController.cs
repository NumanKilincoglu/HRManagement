using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Pusula.Training.HealthCare.Shared;
using Pusula.Training.HealthCare.Titles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Controllers.Titles
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Title")]
    [Route("api/app/titles")]
    public class TitleController(ITitlesAppService titlesAppService) : HealthCareController, ITitlesAppService
    {
        [HttpGet]
        public Task<PagedResultDto<TitleDto>> GetListAsync(GetTitlesInput input) => titlesAppService.GetListAsync(input);

        [HttpGet]
        [Route("{id}")]
        public Task<TitleDto> GetAsync(Guid id) => titlesAppService.GetAsync(id);

        [HttpPost]
        public Task<TitleDto> CreateAsync(TitleCreateDto input) => titlesAppService.CreateAsync(input);

        [HttpPut]
        public Task<TitleDto> UpdateAsync(TitleUpdateDto input) => titlesAppService.UpdateAsync(input);

        [HttpDelete]
        [Route("{id}")]
        public void DeleteAsync(Guid id) => titlesAppService.DeleteAsync(id);

        [HttpDelete]
        [Route("all")]
        public Task DeleteAllAsync(GetTitlesInput input) => titlesAppService.DeleteAllAsync(input);

        [HttpDelete]
        [Route("")]
        public Task DeleteByIdsAsync(List<Guid> titleIds) => titlesAppService.DeleteByIdsAsync(titleIds);

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync() => titlesAppService.GetDownloadTokenAsync();

        [HttpGet]
        [Route("as-excel-file")]
        public Task<IRemoteStreamContent> GetListAsExcelFileAsync(TitleExcelDownloadDto input) => titlesAppService.GetListAsExcelFileAsync(input);
    }
}