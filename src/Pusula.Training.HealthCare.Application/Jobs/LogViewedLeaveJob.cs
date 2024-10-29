using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Pusula.Training.HealthCare.Leaves;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Pusula.Training.HealthCare.Jobs;

public class LogViewedLeaveJob() : AsyncBackgroundJob<LeaveViewLogArgs>, ITransientDependency
{
    public override async Task ExecuteAsync(LeaveViewLogArgs args)
    {
        Logger.LogInformation($" -----> BACKGROUND-JOB -> {args.Name} with Id {args.Id} viewed.");

        await Task.CompletedTask;
    }
}