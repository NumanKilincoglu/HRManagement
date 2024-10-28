using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Pusula.Training.HealthCare.Workers;

public class PeriodicPatientViewerWorker : AsyncPeriodicBackgroundWorkerBase
{
    public PeriodicPatientViewerWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) 
        : base( timer, serviceScopeFactory)
    {
        Timer.Period = 100000;
    }

    [UnitOfWork]
    protected async override Task DoWorkAsync(
        PeriodicBackgroundWorkerContext workerContext)
    {

        Logger.LogInformation("Completed: PeriodicPatientViewerWorker...");
    }
}
