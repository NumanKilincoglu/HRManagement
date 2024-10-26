using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Pusula.Training.HealthCare.Jobs;

public class LogViewedPatientJob()
{
    public async Task ExecuteAsync()
    {

        await Task.CompletedTask;
    }
}
