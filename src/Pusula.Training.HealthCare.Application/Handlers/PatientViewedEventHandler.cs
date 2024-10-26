using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Pusula.Training.HealthCare.Handlers;

public class PatientViewedEventHandler()
{
    public Task HandleEventAsync()
    {

        return Task.CompletedTask;
    }
}
