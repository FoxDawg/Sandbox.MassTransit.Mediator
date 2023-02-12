using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Mediator;

namespace WebApi.Abstractions;

public abstract class RequestHandler<TRequest, TResponse> : IConsumer<TRequest>
    where TRequest : class, Request<TResponse>
    where TResponse : class
{
    public async Task Consume(ConsumeContext<TRequest> context)
    {
        var validationResult = await Validate(context.Message, context.CancellationToken);
        if (validationResult.Errors.Any())
        {
            await context.RespondAsync(validationResult);
            return;
        }
        
        var response = await Handle(context.Message, context.CancellationToken).ConfigureAwait(false);
        await context.RespondAsync(response);
    }

    protected virtual Task<ValidationErrors> Validate(TRequest request, CancellationToken token)
    {
        return Task.FromResult(new ValidationErrors());
    }

    protected abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}