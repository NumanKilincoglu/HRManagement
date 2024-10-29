using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Pusula.Training.HealthCare.Leaves;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Pusula.Training.HealthCare.Handlers;

public class LeaveViewedEventHandler(ILogger<LeaveViewedEventHandler> logger)
    : IDistributedEventHandler<LeaveViewedEto>, ITransientDependency
{
    public Task HandleEventAsync(LeaveViewedEto eventData)
    {
        logger.LogInformation(
            $" -----> HANDLER -> Leave {eventData.Id} viewed as {eventData.ViewedAt.ToLongTimeString()}.");

        return Task.CompletedTask;
    }
}