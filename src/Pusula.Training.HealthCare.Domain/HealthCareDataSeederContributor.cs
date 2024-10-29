using System.Threading.Tasks;
using Pusula.Training.HealthCare.Employees;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Pusula.Training.HealthCare
{
    public class HealthCareDataSeederContributor(
        IEmployeeRepository employeeRepository,
        IGuidGenerator guidGenerator) : IDataSeedContributor, ITransientDependency
    {
        public async Task SeedAsync(DataSeedContext context)
        {
            
            if (await employeeRepository.GetCountAsync() > 0) return;

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
    }
}
