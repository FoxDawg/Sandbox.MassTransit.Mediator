using MassTransit.Mediator;
using WebApi.Model;

namespace WebApi.Feature.Orders;

public record GetOrderStatus(int OrderId) : Request<OrderStatusResult>;