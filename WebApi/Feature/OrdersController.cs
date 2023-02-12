using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using WebApi.Feature.Orders;
using WebApi.Model;

namespace WebApi.Feature;

[Route("[controller]")]
public class OrdersController : Controller
{
    private readonly IRequestBus requestBus;

    public OrdersController(IRequestBus requestBus)
    {
        this.requestBus = requestBus;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int orderId)
    {
        var response = await requestBus.GetResponse<GetOrderStatus, OrderStatusResult>(new GetOrderStatus(orderId));

        if (response.IsValid)
        {
            return new OkObjectResult(response.Result);
        }

        return new BadRequestObjectResult(response.ValidationErrors);
    }
}