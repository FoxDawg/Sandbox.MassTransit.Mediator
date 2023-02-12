namespace WebApi.Model;

public class Order
{
    public int Id { get; init; }
    public OrderState State { get; init; }
}