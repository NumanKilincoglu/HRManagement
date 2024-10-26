using Pusula.Training.HealthCare.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Pusula.Training.HealthCare.Permissions;

public class HealthCarePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(HealthCarePermissions.GroupName);

        myGroup.AddPermission(HealthCarePermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(HealthCarePermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(HealthCarePermissions.MyPermission1, L("Permission:MyPermission1"));

        var employeePermission = myGroup.AddPermission(HealthCarePermissions.Employees.Default, L("Permission:Employees"));
        employeePermission.AddChild(HealthCarePermissions.Employees.Create, L("Permission:Create"));
        employeePermission.AddChild(HealthCarePermissions.Employees.Edit, L("Permission:Edit"));
        employeePermission.AddChild(HealthCarePermissions.Employees.Delete, L("Permission:Delete"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<HealthCareResource>(name);
    }
}