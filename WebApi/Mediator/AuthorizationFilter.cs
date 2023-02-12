using System.Threading.Tasks;
using MassTransit;

namespace WebApi.Mediator;

/// <summary>
///     Domain authorization could be made here.
///     It would, however, require throwing exceptions to control the flow.
/// </summary>
public class AuthorizationFilter<T> : IFilter<SendContext<T>>
    where T : class
{
    public void Probe(ProbeContext context)
    {
    }

    public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        await next.Send(context);
    }
}