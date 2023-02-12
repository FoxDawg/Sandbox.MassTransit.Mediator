using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Abstractions;

namespace WebApi.Mediator;

public class MassTransitRequestBus : IRequestBus
{
    private readonly IServiceScopeFactory scopeFactory;

    public MassTransitRequestBus(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    public async Task<RequestResult<TResult>> GetResponse<TRequest, TResult>(TRequest message)
        where TRequest : class
        where TResult : class
    {
        using var scope = scopeFactory.CreateScope();
        var clientType = typeof(IRequestClient<>).MakeGenericType(typeof(TRequest));
        var client = scope.ServiceProvider.GetService(clientType);

        if (client is not IRequestClient<TRequest> requestClient)
        {
            throw new InvalidOperationException($"Could not create client for type {clientType}");
        }

        // If the request handler will throw an exception, it will be propagated to the caller inside
        // an RequestException. Custom wrapping/unwrapping could be placed here.
        var response = await requestClient.GetResponse<TResult, ValidationErrors>(message);

        if (response.Is(out Response<ValidationErrors>? validationResult) && validationResult != null)
        {
            return new RequestResult<TResult>(validationResult.Message);
        }

        if (response.Message is TResult requestResult)
        {
            return new RequestResult<TResult>(requestResult);
        }
        
        throw new InvalidOperationException($"Response is not of the expected type {typeof(TResult)}");
    }
}