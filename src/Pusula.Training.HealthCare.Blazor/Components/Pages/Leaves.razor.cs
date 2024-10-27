using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Leaves;
using Pusula.Training.HealthCare.Permissions;
using Volo.Abp.Application.Dtos;

namespace Pusula.Training.HealthCare.Blazor.Components.Pages;

public partial class Leaves
{
    [Parameter]
    public Guid EmployeeId { get; set; }
    private GetLeavesInput Filter { get; set; }
    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; } = 1;
    private string CurrentSorting { get; set; } = string.Empty;
    private int TotalCount { get; set; }
    
    
    private bool CanCreateEmployee { get; set; }
    private bool CanEditEmployee { get; set; }
    private bool CanDeleteEmployee { get; set; }
    
    private LeaveCreateDto NewLeave { get; set; }
    private Validations NewLeaveValidations { get; set; } = new();
    private LeaveUpdateDto EditingLeave { get; set; }
    private Validations EditingLeaveValidations { get; set; } = new();
    
    
    private IReadOnlyList<LeaveDto> LeavesList { get; set; }

    
    public Leaves()
    {
        NewLeave = new LeaveCreateDto();
        EditingLeave = new LeaveUpdateDto();
        Filter = new GetLeavesInput()
        {
            MaxResultCount = PageSize,
            SkipCount = (CurrentPage - 1) * PageSize,
            Sorting = CurrentSorting
        };
        LeavesList = [];
        
    }
    
    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
    }
    
    private async Task SetPermissionsAsync()
    {
        CanCreateEmployee = await AuthorizationService
            .IsGrantedAsync(HealthCarePermissions.Leaves.Create);
        CanEditEmployee = await AuthorizationService
            .IsGrantedAsync(HealthCarePermissions.Leaves.Edit);
        CanDeleteEmployee = await AuthorizationService
            .IsGrantedAsync(HealthCarePermissions.Leaves.Delete);
    }
    
    private async Task GetLeavesAsync()
    {
        Filter.MaxResultCount = PageSize;
        Filter.SkipCount = (CurrentPage - 1) * PageSize;
        Filter.Sorting = CurrentSorting;

        var result = await leavesAppService.GetListAsync(Filter);
        LeavesList = result.Items;
        TotalCount = (int)result.TotalCount;
    }
    
    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<LeaveDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;
        await GetLeavesAsync();
        await InvokeAsync(StateHasChanged);
    }
    

}