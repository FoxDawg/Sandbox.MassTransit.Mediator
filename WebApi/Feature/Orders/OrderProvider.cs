using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Feature.Orders;

public class OrderProvider
{
    private static readonly Order[] Orders =
    {
        new Order
        {
            Id = 1,
            State = OrderState.Shipped
        }
    };

    public Task<Order> GetByIdAsync(int id)
    {
        return Task.FromResult(Orders.Single(o => o.Id == id));
    }
}