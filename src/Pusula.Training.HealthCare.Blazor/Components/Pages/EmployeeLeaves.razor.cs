using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Leaves;
using Pusula.Training.HealthCare.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI.Components;

namespace Pusula.Training.HealthCare.Blazor.Components.Pages;

public partial class EmployeeLeaves
{
    protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = [];
    protected PageToolbar Toolbar { get; } = new PageToolbar();
    protected bool ShowAdvancedFilters { get; set; }

    [Parameter] public Guid EmployeeId { get; set; }
    public EmployeeDto Employee { get; set; }
    private GetLeavesInput Filter { get; set; }

    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; } = 1;
    private string CurrentSorting { get; set; } = string.Empty;
    private int TotalCount { get; set; }

    private bool CanCreateLeave { get; set; }
    private bool CanEditLeave { get; set; }
    private bool CanDeleteLeave { get; set; }
    private LeaveCreateDto NewLeave { get; set; }
    private Validations NewLeaveValidations { get; set; } = new();
    private LeaveUpdateDto EditingLeave { get; set; }
    private Validations EditingLeaveValidations { get; set; } = new();
    private Guid EditingLeaveId { get; set; }

    public Modal CreateLeaveModal { get; set; }

    public Modal EditLeaveModal { get; set; }
    private IReadOnlyList<LeaveDto> LeavesList { get; set; }

    private DataGridEntityActionsColumn<LeaveDto> EntityActionsColumn { get; set; } = new();

    protected string SelectedCreateTab = "leave-create-tab";
    protected string SelectedEditTab = "leave-edit-tab";

    private List<LeaveDto> SelectedLeaves { get; set; } = [];
    private bool AllLeavesSelected { get; set; }

    public EmployeeLeaves()
    {
        NewLeave = new LeaveCreateDto();
        EditingLeave = new LeaveUpdateDto();
        Filter = new GetLeavesInput()
        {
            EmployeeId = EmployeeId,
            MaxResultCount = PageSize,
            SkipCount = (CurrentPage - 1) * PageSize,
            Sorting = CurrentSorting
        };
        LeavesList = [];
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await HandleEmployeeDetails();
    }

    public void Dispose()
    {
        AppState.OnChange -= StateHasChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await SetToolbarItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Leaves"]));
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["ExportToExcel"], DownloadAsExcelAsync, IconName.Download);

        Toolbar.AddButton(L["NewLeave"], OpenCreateLeaveModalAsync, IconName.Add,
            requiredPolicyName: HealthCarePermissions.Leaves.Create);

        return ValueTask.CompletedTask;
    }

    private async Task HandleEmployeeDetails()
    {
        if (AppState.SelectedEmployee == null)
        {
            Employee = await EmployeesAppService.GetAsync(EmployeeId);
        }
        else
        {
            Employee = AppState.SelectedEmployee;
        }
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateLeave = await AuthorizationService
            .IsGrantedAsync(HealthCarePermissions.Leaves.Create);
        CanEditLeave = await AuthorizationService
            .IsGrantedAsync(HealthCarePermissions.Leaves.Edit);
        CanDeleteLeave = await AuthorizationService
            .IsGrantedAsync(HealthCarePermissions.Leaves.Delete);
    }


    private async Task DownloadAsExcelAsync()
    {
        var token = (await LeavesAppService.GetDownloadTokenAsync()).Token;
        var remoteService =
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("HealthCare") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
        var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
        if (!culture.IsNullOrEmpty())
        {
            culture = "&culture=" + culture;
        }

        await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
        NavigationManager.NavigateTo(
            $"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/leaves/as-excel-file?" +
            $"DownloadToken={token}" +
            $"&EmployeeId={HttpUtility.UrlEncode(Filter.EmployeeId.ToString())}{culture}" +
            $"&LeaveType={HttpUtility.UrlEncode(Filter.LeaveType)}" +
            $"&Status={HttpUtility.UrlEncode(Filter.Status)}" +
            $"&StartDate={HttpUtility.UrlEncode(Filter.StartDate.ToString())}" +
            $"&StartDate={HttpUtility.UrlEncode(Filter.EndDate.ToString())}",
            forceLoad: true);
    }

    private async Task GetLeavesAsync()
    {
        Filter.MaxResultCount = PageSize;
        Filter.SkipCount = (CurrentPage - 1) * PageSize;
        Filter.Sorting = CurrentSorting;
        Filter.EmployeeId = EmployeeId;

        var result = await LeavesAppService.GetListAsync(Filter);
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

    private async Task OpenCreateLeaveModalAsync()
    {
        NewLeave = new LeaveCreateDto()
        {
            EmployeeId = EmployeeId,
            Status = "Approved"
        };

        SelectedCreateTab = "leave-create-tab";
        await NewLeaveValidations.ClearAll();
        await CreateLeaveModal.Show();
    }

    private async Task CreateLeaveAsync()
    {
        try
        {
            Console.WriteLine(NewLeave);

            if (await NewLeaveValidations.ValidateAll() == false)
            {
                return;
            }

            await LeavesAppService.CreateAsync(NewLeave);
            await GetLeavesAsync();
            await CloseCreateLeaveModalAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task CloseCreateLeaveModalAsync()
    {
        NewLeave = new LeaveCreateDto
        {
        };
        await CreateLeaveModal.Hide();
    }

    private async Task OpenEditLeaveModalAsync(LeaveDto input)
    {
        SelectedEditTab = "leave-edit-tab";

        var leave = await LeavesAppService.GetAsync(input.Id);

        try
        {
            EditingLeaveId = leave.Id;
            EditingLeave = ObjectMapper.Map<LeaveDto, LeaveUpdateDto>(leave);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        await EditingLeaveValidations.ClearAll();
        await EditLeaveModal.Show();
    }

    private async Task UpdateEmployeeAsync()
    {
        try
        {
            if (await EditingLeaveValidations.ValidateAll() == false)
            {
                return;
            }

            await LeavesAppService.UpdateAsync(EditingLeaveId, EditingLeave);

            await GetLeavesAsync();
            await EditLeaveModal.Hide();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task CloseEditEmployeeModalAsync()
    {
        await EditLeaveModal.Hide();
    }

    private async Task DeleteLeaveAsync(LeaveDto input)
    {
        await LeavesAppService.DeleteAsync(input.Id);
        await GetLeavesAsync();
    }

    private Task SelectAllItems()
    {
        AllLeavesSelected = true;

        return Task.CompletedTask;
    }

    private Task ClearSelection()
    {
        AllLeavesSelected = false;
        SelectedLeaves.Clear();

        return Task.CompletedTask;
    }

    private async Task DeleteSelectedLeavesAsync()
    {
        var message = AllLeavesSelected
            ? L["DeleteAllRecords"].Value
            : L["DeleteSelectedRecords", SelectedLeaves.Count].Value;

        if (!await UiMessageService.Confirm(message))
        {
            return;
        }

        await LeavesAppService.DeleteByIdsAsync(SelectedLeaves.Select(x => x.Id).ToList());

        SelectedLeaves.Clear();
        AllLeavesSelected = false;

        await GetLeavesAsync();
    }

    private Task SelectedLeaveRowsChanged()
    {
        if (SelectedLeaves.Count != PageSize)
        {
            AllLeavesSelected = false;
        }

        return Task.CompletedTask;
    }

    protected virtual async Task OnLeaveTypeChangedAsync(string? type)
    {
        Filter.LeaveType = type;
        await GetLeavesAsync();
    }

    protected virtual async Task OnStartDateChangedAsync(DateTime? date)
    {
        Filter.StartDate = date?.Date ?? date;
        await GetLeavesAsync();
    }

    protected virtual async Task OnEndDateChangedAsync(DateTime? date)
    {
        Filter.EndDate = date?.Date ?? date;
        await GetLeavesAsync();
    }

    protected virtual async Task OnStatusChangedAsync(string? status)
    {
        Filter.Status = status;
        await GetLeavesAsync();
    }
}