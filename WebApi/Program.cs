using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Abstractions;
using WebApi.Feature.Orders;
using WebApi.Mediator;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<OrderProvider>();
builder.Services.AddTransient<IRequestBus, MassTransitRequestBus>();
builder.Services.AddControllers();
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.UsingInMemory();
    busConfigurator.AddMediator(x =>
    {
        x.ConfigureMediator((ctx, mcfg) => { mcfg.UseSendFilter(typeof(AuthorizationFilter<>), ctx); });

        x.AddConsumers(typeof(Program).Assembly);
        x.AddRequestClient(typeof(GetOrderStatus));
    });
});

var app = builder.Build();

app.MapControllers();

app.Run();