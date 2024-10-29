using System.Threading.Tasks;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Leaves;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace Pusula.Training.HealthCare
{
    public class HealthCareDataSeederContributor(
        IEmployeeRepository employeeRepository,
        ILeaveRepository leaveRepository,
        IdentityRoleManager roleManager,
        IPermissionManager permissionManager,
        IdentityUserManager userManager,
        IGuidGenerator guidGenerator) : IDataSeedContributor, ITransientDependency
    {
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await employeeRepository.GetCountAsync() > 0) return;

            await SetHrManager();

            var employee1 = await employeeRepository.InsertAsync(
                new Employee(guidGenerator.Create(), "Ali", "Yılmaz", new System.DateTime(2020, 01, 01), "49762602911",
                    "ali.yilmaz@gmail.com", "5111000515", 1, "5111000515"), true);
            var employee2 = await employeeRepository.InsertAsync(
                new Employee(guidGenerator.Create(), "Mehmet", "Kaya", new System.DateTime(1990, 05, 20), "18594105779",
                    "mehmet.kaya@hotmail.com", "5091059875", 1, "5111110515"), true);
            var employee3 = await employeeRepository.InsertAsync(
                new Employee(guidGenerator.Create(), "Veli", "Yıldız", new System.DateTime(1995, 04, 15), "75710296501",
                    "veli.yildiz@outlook.com.tr", "5059800099", 1, "5111000525"), true);

            var l1 = new Leave(guidGenerator.Create(), employee1.Id, new System.DateTime(2025, 11, 04),
                new System.DateTime(2026, 11, 08), "Paid", "Approved");
            var l2 = new Leave(guidGenerator.Create(), employee2.Id, new System.DateTime(2025, 11, 06),
                new System.DateTime(2026, 11, 08), "Paid", "Approved");
            var l3 = new Leave(guidGenerator.Create(), employee3.Id, new System.DateTime(2025, 12, 03),
                new System.DateTime(2026, 12, 06), "Paid", "Approved");
            var l4 = new Leave(guidGenerator.Create(), employee3.Id, new System.DateTime(2025, 12, 04),
                new System.DateTime(2025, 12, 09), "Paid", "Approved");
            var l5 = new Leave(guidGenerator.Create(), employee2.Id, new System.DateTime(2025, 11, 05),
                new System.DateTime(2025, 11, 09), "Unpaid", "Approved");
            await leaveRepository.InsertManyAsync([l1, l2, l3, l4, l5]);
        }

        private async Task SetHrManager()
        {
            var role = new IdentityRole(guidGenerator.Create(), "HRManager", null)
            {
                IsPublic = true,
                IsDefault = false
            };
            
            await roleManager.CreateAsync(role);

            await permissionManager.SetForRoleAsync(role.Name, "HealthCare.Employees", true);
            await permissionManager.SetForRoleAsync(role.Name, "HealthCare.Employees.Create", true);
            await permissionManager.SetForRoleAsync(role.Name, "HealthCare.Employees.Delete", true);
            
            await permissionManager.SetForRoleAsync(role.Name, "HealthCare.Leaves", true);
            await permissionManager.SetForRoleAsync(role.Name, "HealthCare.Leaves.Create", true);
            await permissionManager.SetForRoleAsync(role.Name, "HealthCare.Leaves.Delete", true);

            var hrUser = new IdentityUser(guidGenerator.Create(), "hr@pusula", "hr@pusula.com", null);
            
            await userManager.CreateAsync(hrUser, "Pusula*hr1", true);
            await userManager.AddToRoleAsync(hrUser, role.Name);

        }
    }
}