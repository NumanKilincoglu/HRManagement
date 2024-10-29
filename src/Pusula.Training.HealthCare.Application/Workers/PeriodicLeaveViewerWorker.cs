using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Pusula.Training.HealthCare.Workers;

public class PeriodicLeaveViewerWorker : AsyncPeriodicBackgroundWorkerBase
{
    public PeriodicLeaveViewerWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) 
        : base( timer, serviceScopeFactory)
    {
        Timer.Period = 60000;
    }

    [UnitOfWork]
    protected async override Task DoWorkAsync(
        PeriodicBackgroundWorkerContext workerContext)
    {

        Logger.LogInformation("Completed: PeriodicLeaveViewerWorker...");
    }
}
